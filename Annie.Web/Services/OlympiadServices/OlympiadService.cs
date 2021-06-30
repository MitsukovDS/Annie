using Annie.Data.Static;
using Annie.Model;
using Annie.Web.Models.Core.Cache;
using Annie.Web.Repositories.OlympiadRepositories;
using Annie.Web.ViewModels.Olympiad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Services.OlympiadServices
{
    public class OlympiadService : IOlympiadService
    {
        private readonly IStaticRepository _staticRepository;
        private readonly IOlympiadRepository _olympiadRepository;
        private readonly ICache _cache;

        public OlympiadService(IStaticRepository staticRepository, IOlympiadRepository olympiadRepository, ICache cache)
        {
            _staticRepository = staticRepository;
            _olympiadRepository = olympiadRepository;
            _cache = cache;
        }

        public async Task<OlympiadsForDisciplines> GetOlympiadsForDisciplines(OlympiadTypes olympiadType)
        {
            if (!_cache.TryGetValue(CachedObject.Olympiads, out List<Olympiad> olympiads))
            {
                var startDateForVisibleOlympiads = new DateTime(2020, 1, 1);
                olympiads = await _olympiadRepository.GetOlympiadsAsync(startDateForVisibleOlympiads);
                _cache.Set(CachedObject.Olympiads, olympiads, TimeSpan.FromMinutes(30));
            }

            var olympiadsGrouped = olympiads.Where(o => o.OlympiadTypeId == (int)olympiadType).
                GroupBy(o => o.DisciplineId,
                o => o.DepartmentId,
                (key, departmentIds) => new { disciplineId = key, departmentIds = departmentIds.Distinct() });

            var departmentsForDisciplines = new List<OlympiadsForDisciplines.DepartmentsForDiscipline>();

            foreach (var olympiad in olympiadsGrouped)
            {
                departmentsForDisciplines.Add(new OlympiadsForDisciplines.DepartmentsForDiscipline()
                {
                    Discipline = _staticRepository.Disciplines.First(d => d.Id == olympiad.disciplineId),
                    Departments = _staticRepository.Departments.Where(d => olympiad.departmentIds.Contains(d.Id)).OrderBy(d => d.Id).ToList()
                });
            }

            return new OlympiadsForDisciplines
            {
                OlympiadType = olympiadType,
                DepartmentsForDisciplines = departmentsForDisciplines
            };
        }
    }
}
