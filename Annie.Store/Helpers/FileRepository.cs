using Dapper;
using Annie.Data.DatabaseProvider;
using Annie.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Mime;
using Annie.Store.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Annie.Store.Helpers
{
    public class FileRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly DomainModelContext _context;
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileRepository(IDbConnectionFactory dbConnectionFactory, DomainModelContext context, IOptions<AppSettings> appSettings, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _context = context;
            _appSettings = appSettings.Value;
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UploadedFile> LoadFileAsync(byte[] streamedFileContent, long length, string sourceFileName, string contentType, UploadedFileTypes fileType)
        {
            string pathToDirectory = _appSettings.PathDirectory.GetPathDirectory(fileType);
            string extension = Path.GetExtension(sourceFileName);
            string fileName = Guid.NewGuid() + extension;
            string pathToFile = pathToDirectory + fileName;

            var file = new UploadedFile()
            {
                OriginalName = sourceFileName,
                Name = fileName,
                Hash = GetHashFile(length, sourceFileName, contentType, (int)fileType),
                Path = pathToFile,
                UploadedFileTypeId = (int)fileType
            };

            UploadedFile dbFile = await GetFileByHashAsync(hash: file.Hash);

            if (dbFile != null)
            {
                return dbFile;
            }

            using (var targetStream = System.IO.File.Create(Path.Combine(pathToFile)))
            {
                await targetStream.WriteAsync(streamedFileContent);
            }

            file.Id = await SetFileAsync(file);
            return file;
        }

        public byte[] GetFileByteArray(string fileNameWithExtension, UploadedFileTypes fileType)
        {
            string pathToDirectory = _appSettings.PathDirectory.GetPathDirectory(fileType);
            return File.ReadAllBytes(pathToDirectory + fileNameWithExtension);
        }

        public MemoryStream GetFileMemoryStream(string fileNameWithExtension, UploadedFileTypes fileType)
        {
            string pathToDirectory = _appSettings.PathDirectory.GetPathDirectory(fileType);

            using (FileStream fileStream = File.OpenRead(pathToDirectory + fileNameWithExtension))
            {
                var memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                return memStream;
            }
        }

        private async Task<int> SetFileAsync(UploadedFile file)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region query
                string query = @"INSERT INTO public.""UploadedFile"" 
                                           (""CreatedDate"", 
                                            ""Name"", 
                                            ""OriginalName"", 
                                            ""Hash"", 
                                            ""Path"",
                                            ""UploadedFileTypeId"")
	                                    VALUES 
                                           (CURRENT_TIMESTAMP, 
                                            @Name, 
                                            @OriginalName, 
                                            @Hash, 
                                            @Path,
                                            @UploadedFileTypeId)
	                             RETURNING ""Id"";";
                return await dbConnection.QueryFirstAsync<int>(query, file);
                #endregion
            }
        }

        private async Task<UploadedFile> GetFileByHashAsync(string hash)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region query
                string query = @"SELECT uf.*
                                 FROM public.""UploadedFile"" uf
                                 WHERE uf.""Hash"" = @hash;";

                return await dbConnection.QueryFirstOrDefaultAsync<UploadedFile>(query, new { hash });
                #endregion
            }
        }

        private string GetHashFile(long length, string fileName, string contentType, int uploadedFileTypeId)
        {
            var inputHash = $"{length}{fileName}{contentType}{uploadedFileTypeId}";
            return GetMd5(inputHash);
        }

        private string GetMd5(string input)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] bSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                var sbSignature = new StringBuilder();
                foreach (byte b in bSignature)
                    sbSignature.AppendFormat("{0:x2}", b);
                return sbSignature.ToString();
            }
        }


        private async Task SendFile(MemoryStream fileMemoryStream, string fileName, UploadedFileTypes fileType)
        {
            var client = _clientFactory.CreateClient();

            string boundary = "------WebKitFormBoundaryFoxUxCRayQhs5eNN";
            var fContent = new MultipartFormDataContent(boundary);
            fContent.Headers.Remove("Content-Type");
            fContent.Headers.TryAddWithoutValidation("Content-Type", $"multipart/form-data; boundary={boundary}");

            fileMemoryStream.Position = 0;

            using var streamContent = new StreamContent(fileMemoryStream);
            var contentType = new ContentType();

            streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
            streamContent.Headers.ContentDisposition.Name = "\"file\"";
            streamContent.Headers.ContentDisposition.FileName = "\"" + fileName + "\"";
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            fContent.Add(streamContent);

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:12937/file/upload"),
                Content = fContent
            };
            //request.Headers.Add("Authorization", $"OAuth {_appSettings.YandexDiskSettings.AccessToken}");

            var responce = await client.SendAsync(request);

            if (responce.IsSuccessStatusCode)
            {
                //using var responceStream = await responce.Content.ReadAsStreamAsync();
                //linkForUpload = await JsonSerializer.DeserializeAsync<LinkForUpload>(responceStream);
            }
        }


        public async Task<List<UploadedFile>> Upload(UploadedFileTypes uploadedFileType)
        {
            var uploadedFiles = new List<UploadedFile>();

            if (!MultipartRequestHelper.IsMultipartContentType(_httpContextAccessor.HttpContext.Request.ContentType))
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                //resultUpload.ModelError = "The request couldn't be processed (Error 1).";
                return null;
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(_httpContextAccessor.HttpContext.Request.ContentType));
            var reader = new MultipartReader(boundary, _httpContextAccessor.HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    // This check assumes that there's a file
                    // present without form data. If form data
                    // is present, this method immediately fails
                    // and returns the model error.
                    if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        //resultUpload.ModelError = "The request couldn't be processed (Error 2).";
                        return null;
                    }
                    else
                    {
                        // Don't trust the file name sent by the client. To display
                        // the file name, HTML-encode the value.
                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                contentDisposition.FileName.Value);
                        var trustedFileNameForFileStorage = Path.GetRandomFileName();

                        // **WARNING!**
                        // In the following example, the file is saved without
                        // scanning the file's contents. In most production
                        // scenarios, an anti-virus/anti-malware scanner API
                        // is used on the file before making the file available
                        // for download or for use by other systems. 
                        // For more information, see the topic that accompanies 
                        // this sample.

                        var streamedFileContent = await ProcessStreamedFile(
                            section, contentDisposition, _appSettings.GetPermittedExtensions(), _appSettings.MaxFileSizeByte);

                        if (_httpContextAccessor.HttpContext.Response.StatusCode != StatusCodes.Status200OK)
                        {
                            return null;
                        }

                        var file = await LoadFileAsync(
                            streamedFileContent,
                            streamedFileContent.Length,
                            contentDisposition.FileName.Value,
                            section.ContentType,
                            uploadedFileType);

                        uploadedFiles.Add(file);
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            return uploadedFiles;
        }



















        // If you require a check on specific characters in the IsValidFileExtensionAndSignature
        // method, supply the characters in the _allowedChars field.
        private static readonly byte[] _allowedChars = { };
        // For more file signatures, see the File Signatures Database (https://www.filesignatures.net/)
        // and the official specifications for the file types you wish to add.
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },
            { ".zip", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
                    new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
                    new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                    new byte[] { 0x50, 0x4B, 0x07, 0x08 },
                    new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 },
                }
            },
        };

        private async Task<byte[]> ProcessStreamedFile(
            MultipartSection section, ContentDispositionHeaderValue contentDisposition,
            string[] permittedExtensions, long sizeLimit)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await section.Body.CopyToAsync(memoryStream);

                    // Check if the file is empty or exceeds the size limit.
                    if (memoryStream.Length == 0)
                    {
                        _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        //resultUpload.ModelError = "The file is empty.";
                    }
                    else if (memoryStream.Length > sizeLimit)
                    {
                        var megabyteSizeLimit = sizeLimit / 1048576;
                        _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        //resultUpload.ModelError = $"The file exceeds {megabyteSizeLimit:N1} MB.";
                    }
                    //else if (!IsValidFileExtensionAndSignature(
                    //    contentDisposition.FileName.Value, memoryStream,
                    //    permittedExtensions))
                    //{
                    //    _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    //    //resultUpload.ModelError = "The file type isn't permitted or the file's " +
                    //    //    "signature doesn't match the file's extension.";
                    //}
                    else
                    {
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                //resultUpload.ModelError = "The upload failed. Please contact the Help Desk " +
                //    $" for support. Error: {ex.HResult}";
            }

            return Array.Empty<byte>();
        }


        private static bool IsValidFileExtensionAndSignature(string fileName, Stream data, string[] permittedExtensions)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            data.Position = 0;

            using (var reader = new BinaryReader(data))
            {
                if (ext.Equals(".txt") || ext.Equals(".csv") || ext.Equals(".prn"))
                {
                    if (_allowedChars.Length == 0)
                    {
                        // Limits characters to ASCII encoding.
                        for (var i = 0; i < data.Length; i++)
                        {
                            if (reader.ReadByte() > sbyte.MaxValue)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // Limits characters to ASCII encoding and
                        // values of the _allowedChars array.
                        for (var i = 0; i < data.Length; i++)
                        {
                            var b = reader.ReadByte();
                            if (b > sbyte.MaxValue || !_allowedChars.Contains(b))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }

                // Uncomment the following code block if you must permit
                // files whose signature isn't provided in the _fileSignature
                // dictionary. We recommend that you add file signatures
                // for files (when possible) for all file types you intend
                // to allow on the system and perform the file signature
                // check.
                /*
                if (!_fileSignature.ContainsKey(ext))
                {
                    return true;
                }
                */

                // File signature check
                // --------------------
                // With the file signatures provided in the _fileSignature
                // dictionary, the following code tests the input content's
                // file signature.
                var signatures = _fileSignature[ext];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }
    }
}
