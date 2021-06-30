using Annie.Model;
using Annie.Web.ViewModels.Olympiad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Services.OlympiadServices
{
    public interface IOlympiadService
    {
        public Task<OlympiadsForDisciplines> GetOlympiadsForDisciplines(OlympiadTypes olympiadType);
    }
}
