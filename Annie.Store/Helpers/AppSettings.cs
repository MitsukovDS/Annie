using Annie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Store.Helpers
{
    public class AppSettings
    {
        public PostgreSQLSettings PostgreSQLSettings { get; set; }
        public PathDirectory PathDirectory { get; set; }
        public string[] GetPermittedExtensions() => PermittedExtensionsString.Split(',').Select(p => p.Trim()).ToArray();
        private string PermittedExtensionsString { get; set; }
        public long MaxFileSizeByte { get; set; }
    }

    public class PostgreSQLSettings
    {
        public string ConnectionString { get; set; }
        public string TestConnectionString { get; set; }
    }

    public class PathDirectory
    {
        public string Olympiad { get; set; }
        public string Discipline { get; set; }
        public string Diploma { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Avatar { get; set; }
        public string AvatarDefault { get; set; }
        public string Static { get; set; }

        public string GetPathDirectory(UploadedFileTypes fileType)
        {
            return ((int)fileType) switch
            {
                (int)UploadedFileTypes.OlympiadFile => Olympiad,
                (int)UploadedFileTypes.DisciplineFile => Discipline,
                (int)UploadedFileTypes.DiplomaFile => Diploma,
                (int)UploadedFileTypes.QuestionFile => Question,
                (int)UploadedFileTypes.AnswerFile => Answer,
                (int)UploadedFileTypes.AvatarFile => Avatar,
                (int)UploadedFileTypes.StaticFile => Static,
                _ => throw new Exception("Unknown FileType"),
            };
        }
    }
}
