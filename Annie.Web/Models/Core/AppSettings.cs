using Annie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Models.Core
{
    /// <summary>
    /// Used IOptions pattern for configuration from appsettings.json
    /// </summary>
    public class AppSettings
    {
        public PostgreSQLSettings PostgreSQLSettings { get; set; }
        public MongoDbSettings MongoDbSettings { get; set; }
        public EmailSettings EmailSettings { get; set; }
        public RobokassaSettings RobokassaSettings { get; set; }
        public DaDataSettings DaDataSettings { get; set; }
        public PathDirectory PathDirectory { get; set; }
        public DiplomaSettings DiplomaSettings { get; set; }
        public QuestionsFormSettings QuestionsFormSettings { get; set; }
        public YandexDiskSettings YandexDiskSettings { get; set; }
        public FileServer FileServer { get; set; }
    }

    public class PostgreSQLSettings
    {
        public string ConnectionString { get; set; }
        public string TestConnectionString { get; set; }
    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }

    public class EmailSettings
    {
        public string Sender { get; set; }
        public string HostForConnect { get; set; }
        public string PortForConnect { get; set; }
        public string SecureSocketOption { get; set; }
        public string AuthenticateUserName { get; set; }
        public string AuthenticatePassword { get; set; }
        public string MailboxName { get; set; }
    }

    public class GrpcSettings
    {
        public string HostForConnect { get; set; }
    }

    public class RobokassaSettings
    {
        private bool IsProduction { get; set; }
        private string LinkPayment { get; set; }
        public string GetLinkPayment(int paymentId, string email, decimal amount, string signature)
        {
            string result = $"{this.LinkPayment}?MerchantLogin={this.ShopIdentity}&InvoiceID={paymentId}&Description={this.Description}&Culture=ru&Encoding=utf-8&Email={email}&OutSum={amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}&SignatureValue={signature}";

            if (!this.IsProduction)
                result += "&IsTest=1";

            return result;
        }
        public string ShopIdentity { get; set; }
        public string Description { get; set; }
        public string Password1 => IsProduction ? Password1Production : Password1Test;
        public string Password2 => IsProduction ? Password2Production : Password2Test;
        private string Password1Production { get; set; }
        private string Password2Production { get; set; }
        private string Password1Test { get; set; }
        private string Password2Test { get; set; }
        public string LinkStatePayment { get; set; }
    }

    public class DaDataSettings
    {
        public string Token { get; set; }
        public string SecretToken { get; set; }
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

    public class DiplomaSettings
    {
        public string LightFontPath { get; set; }
        public string BoldFontPath { get; set; }
    }

    public class QuestionsFormSettings
    {
        public string LogoPath { get; set; }
        public string FontPath { get; set; }
        private string DisciplineLink { get; set; }
        public string GetDisciplineLink(int disciplineId, int? olympiadTypeId = null)
        {
            return $"{DisciplineLink}/{disciplineId}/{olympiadTypeId}";
        }
        private string AgreementPaymentLink { get; set; }
        public string GetAgreementPaymentLink(Guid externalId, Products product)
        {
            return $"{AgreementPaymentLink}/{externalId}/{(int)product}";
        }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
    }

    public class YandexDiskSettings
    {
        public string AccessToken { get; set; }
        public string ClientId { get; set; }
        public string Password { get; set; }
        private string OauthAuthorizeLink { get; set; }
        public string GetOauthAuthorizeLink()
        {
            return $"{OauthAuthorizeLink}?response_type=token&client_id={ClientId}";
        }
    }

    public class FileServer
    {
        private bool IsProduction { get; set; }
        private string ServerLink { get; set; }
        private string LocalhostServerLink { get; set; }
        private UploadFileLink UploadLink { get; set; }

        public string GetUploadOlympiadFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{UploadLink.OlympiadFileLink}";
        }

        public string GetUploadQuestionFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{UploadLink.QuestionFileLink}";
        }

        public string GetUploadAnswerFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{UploadLink.AnswerFileLink}";
        }

        public string GetUploadDisciplineFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{UploadLink.DisciplineFileLink}";
        }

        public string GetUploadDiplomaFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{UploadLink.DisciplineFileLink}";
        }

        public string GetUploadAvatarFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{UploadLink.AvatarFileLink}";
        }

        public string GetUploadStaticFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{UploadLink.StaticFileLink}";
        }

        private class UploadFileLink
        {
            public string OlympiadFileLink { get; set; }
            public string QuestionFileLink { get; set; }
            public string AnswerFileLink { get; set; }
            public string DisciplineFileLink { get; set; }
            public string DiplomaFileLink { get; set; }
            public string AvatarFileLink { get; set; }
            public string StaticFileLink { get; set; }
        }

        private DownloadFileLink DownloadLink { get; set; }

        public string GetDownloadOlympiadFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{DownloadLink.OlympiadFileLink}";
        }

        public string GetDownloadQuestionFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{DownloadLink.QuestionFileLink}";
        }

        public string GetDownloadAnswerFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{DownloadLink.AnswerFileLink}";
        }

        public string GetDownloadDisciplineFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{DownloadLink.DisciplineFileLink}";
        }

        public string GetDownloadDiplomaFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{DownloadLink.DisciplineFileLink}";
        }

        public string GetDownloadAvatarFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{DownloadLink.AvatarFileLink}";
        }

        public string GetDownloadStaticFileLink()
        {
            string serverLink = IsProduction ? ServerLink : LocalhostServerLink;
            return $"{serverLink}{DownloadLink.StaticFileLink}";
        }

        private class DownloadFileLink
        {
            public string OlympiadFileLink { get; set; }
            public string QuestionFileLink { get; set; }
            public string AnswerFileLink { get; set; }
            public string DisciplineFileLink { get; set; }
            public string DiplomaFileLink { get; set; }
            public string AvatarFileLink { get; set; }
            public string StaticFileLink { get; set; }
        }
    }
}
