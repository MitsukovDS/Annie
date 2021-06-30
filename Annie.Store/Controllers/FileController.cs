using Annie.Authorization.Filter;
using Annie.Model;
using Annie.Store.Core;
using Annie.Store.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Annie.Store.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileRepository _fileRepository;

        public FileController(FileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }


        [HttpGet]
        [Route("/")]
        public string Get()
        {
            return "Follow the white rabbit.";
        }


        // POST file/GetStaticFile/{fileNameWithExtension}
        [Route("[Action]")]
        [HttpPost]
        [DisableFormValueModelBinding]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles.Global, Roles.Admin)]
        public byte[] DownloadStaticFile([FromBody] string fileNameWithExtension)
        {
            var fileType = UploadedFileTypes.StaticFile;
            return _fileRepository.GetFileByteArray(fileNameWithExtension, fileType);
        }



        // POST file/UploadOlympiadFile
        [Route("[Action]")]
        [HttpPost]
        [DisableFormValueModelBinding]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles.Global, Roles.Admin)]
        public async Task<JsonResult> UploadOlympiadFile()
        {
            var fileType = UploadedFileTypes.OlympiadFile;
            var uploadedFiles = await _fileRepository.Upload(fileType);
            return new JsonResult(uploadedFiles);
        }

        // POST file/UploadQuestionFile
        [Route("[Action]")]
        [HttpPost]
        [DisableFormValueModelBinding]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles.Global, Roles.Admin)]
        public async Task<JsonResult> UploadQuestionFile()
        {
            var fileType = UploadedFileTypes.QuestionFile;
            var uploadedFiles = await _fileRepository.Upload(fileType);
            return new JsonResult(uploadedFiles);
        }

        // POST file/UploadAnswerFile
        [Route("[Action]")]
        [HttpPost]
        [DisableFormValueModelBinding]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles.Global, Roles.Admin)]
        public async Task<JsonResult> UploadAnswerFile()
        {
            var fileType = UploadedFileTypes.AnswerFile;
            var uploadedFiles = await _fileRepository.Upload(fileType);
            return new JsonResult(uploadedFiles);
        }

        // POST file/UploadDisciplineFile
        [Route("[Action]")]
        [HttpPost]
        [DisableFormValueModelBinding]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles.Global, Roles.Admin)]
        public async Task<JsonResult> UploadDisciplineFile()
        {
            var fileType = UploadedFileTypes.DisciplineFile;
            var uploadedFiles = await _fileRepository.Upload(fileType);
            return new JsonResult(uploadedFiles);
        }

        // POST file/UploadDiplomaFile
        [Route("[Action]")]
        [HttpPost]
        [DisableFormValueModelBinding]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles.Global, Roles.Admin)]
        public async Task<JsonResult> UploadDiplomaFile()
        {
            var fileType = UploadedFileTypes.DiplomaFile;
            var uploadedFiles = await _fileRepository.Upload(fileType);
            return new JsonResult(uploadedFiles);
        }

        // POST file/UploadAvatarFile
        [Route("[Action]")]
        [HttpPost]
        [DisableFormValueModelBinding]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles.Global, Roles.Admin)]
        public async Task<JsonResult> UploadAvatarFile()
        {
            var fileType = UploadedFileTypes.AvatarFile;
            var uploadedFiles = await _fileRepository.Upload(fileType);
            return new JsonResult(uploadedFiles);
        }

        // POST file/UploadStaticFile
        [Route("[Action]")]
        [HttpPost]
        [DisableFormValueModelBinding]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles.Global, Roles.Admin)]
        public async Task<JsonResult> UploadStaticFile()
        {
            var fileType = UploadedFileTypes.StaticFile;
            var uploadedFiles = await _fileRepository.Upload(fileType);
            return new JsonResult(uploadedFiles);
        }
    }
}
