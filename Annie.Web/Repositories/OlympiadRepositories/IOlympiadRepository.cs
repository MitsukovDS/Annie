using Annie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Repositories.OlympiadRepositories
{
    public interface IOlympiadRepository
    {
        /// <summary>
        /// Поиск олимпиад, которые были созданы после <paramref name="startDate"/> включительно
        /// </summary>
        /// <param name="startDate">Дата, после которой созданы олимпиады</param>
        /// <returns></returns>
        public Task<List<Olympiad>> GetOlympiadsAsync(DateTime startDate);

        /// <summary>
        /// Поиск олимпиад с типом <paramref name="olympiadType"/>, которые были созданы после <paramref name="startDate"/> включительно
        /// </summary>
        /// <param name="olympiadType">Тип олимпиады</param>
        /// <param name="startDate">Дата, после которой созданы олимпиады</param>
        /// <returns></returns>
        public Task<List<Olympiad>> GetOlympiadsAsync(OlympiadTypes olympiadType, DateTime startDate);
    }
}
