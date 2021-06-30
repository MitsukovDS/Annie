using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annie.Web.Services.EmailServices;

namespace Annie.Web.Services.EmailServices
{
    public interface IEmailService
    {
        public Task SendMailAsync(Message emailMessage);
    }
}
