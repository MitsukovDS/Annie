using Annie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.ViewModels.Olympiad
{
    public class OlympiadsForDisciplines
    {
        public OlympiadTypes OlympiadType { get; set; }
        public List<DepartmentsForDiscipline> DepartmentsForDisciplines { get; set; } 
        public class DepartmentsForDiscipline
        {
            public Discipline Discipline { get; set; }
            public List<Department> Departments { get; set; }
        }
    }

}
