using Annie.Model;
using Annie.Web.Services.OlympiadServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Controllers
{
    public class OlympiadController : Controller
    {
        private readonly IOlympiadService _olympiadService;

        public OlympiadController(IOlympiadService olympiadService)
        {
            _olympiadService = olympiadService;
        }

        [HttpGet]
        [Route("/", Name = "HomePage")]
        [Route("olympiads/{olympiadType?}")]
        public async Task<IActionResult> Index(OlympiadTypes olympiadType = OlympiadTypes.School)
        {
            var olympiadsForDisciplines = await _olympiadService.GetOlympiadsForDisciplines(olympiadType);
            return View(olympiadsForDisciplines);
        }
    }
}
