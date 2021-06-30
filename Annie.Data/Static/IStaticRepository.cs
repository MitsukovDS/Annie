using Annie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annie.Data.Static
{
    /// <summary>
    /// Справочники БД
    /// </summary>
    public interface IStaticRepository
    {
        public List<Department> Departments { get; }
        public List<Diploma> Diplomas { get; }
        public List<DiplomaElement> DiplomaElements { get; }
        public List<DiplomaType> DiplomaTypes { get; }
        public List<Discipline> Disciplines { get; }
        public List<DisciplineAlias> DisciplineAliases { get; }
        public List<OlympiadSubtype> OlympiadSubtypes { get; }
        public List<OlympiadType> OlympiadTypes { get; }
        public List<Product> Products { get; }
        public List<PaymentType> PaymentTypes { get; }
        public List<ParticipantStatus> ParticipantStatuses { get; }
        public List<Role> Roles { get; }
        public List<RoleLevel> RoleLevels { get; }
        public List<Session> Sessions { get; }
        public List<StudyYear> StudyYears { get; }
        public List<QuestionCategory> QuestionCategories { get; }
        public List<UploadedFileType> UploadedFileTypes { get; }

        public void ActualizeDepartments();
        public void ActualizeDiplomas();
        public void ActualizeDiplomaElements();
        public void ActualizeDiplomaTypes();
        public void ActualizeDisciplines();
        public void ActualizeDisciplineAliases();
        public void ActualizeOlympiadSubtypes();
        public void ActualizeOlympiadTypes();
        public void ActualizeProducts();
        public void ActualizePaymentTypes();
        public void ActualizeParticipantStatuses();
        public void ActualizeRoles();
        public void ActualizeRoleLevels();
        public void ActualizeSessions();
        public void ActualizeStudyYears();
        public void ActualizeQuestionCategories();
        public void ActualizeUploadedFileTypes();
    }
}
