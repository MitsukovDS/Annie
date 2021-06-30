using Annie.Data.DatabaseProvider;
using Annie.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Data
{
    /// <summary>
    /// Класс содержит свойства, заполняющиеся справочниками БД
    /// </summary>
    public class StaticRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public List<Department> Departments { get; private set; }
        public List<Diploma> Diplomas { get; private set; }
        public List<DiplomaElement> DiplomaElements { get; private set; }
        public List<DiplomaType> DiplomaTypes { get; private set; }
        public List<Discipline> Disciplines { get; private set; }
        public List<DisciplineAlias> DisciplineAliases { get; private set; }
        public List<OlympiadSubtype> OlympiadSubtypes { get; private set; }
        public List<OlympiadType> OlympiadTypes { get; private set; }
        public List<Product> Products { get; private set; }
        public List<PaymentType> PaymentTypes { get; private set; }
        public List<ParticipantStatus> ParticipantStatuses { get; private set; }
        public List<Role> Roles { get; private set; }
        public List<RoleLevel> RoleLevels { get; private set; }
        public List<Session> Sessions { get; private set; }
        public List<StudyYear> StudyYears { get; private set; }
        public List<QuestionCategory> QuestionCategories { get; private set; }
        public List<UploadedFileType> UploadedFileTypes { get; private set; }

        public StaticRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;

            Departments = GetAllDepartments();
            Diplomas = GetAllDiplomas();
            DiplomaElements = GetAllDiplomaElements();
            DiplomaTypes = GetAllDiplomaTypes();
            Disciplines = GetAllDisciplines();
            DisciplineAliases = GetAllDisciplineAliases();
            OlympiadSubtypes = GetAllOlympiadSubtypes();
            OlympiadTypes = GetAllOlympiadTypes();
            Products = GetAllProducts();
            PaymentTypes = GetAllPaymentTypes();
            ParticipantStatuses = GetAllParticipantStatuses();
            Roles = GetAllRoles();
            RoleLevels = GetAllRoleLevels();
            Sessions = GetAllSessions();
            StudyYears = GetAllStudyYears();
            QuestionCategories = GetAllQuestionCategories();
            UploadedFileTypes = GetAllUploadedFileTypes();
        }

        private List<Department> GetAllDepartments()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""Department"";";
                return dbConnection.Query<Department>(query).ToList();
                #endregion
            }
        }

        public void ActualizeDepartments() => this.Departments = GetAllDepartments();

        private List<Diploma> GetAllDiplomas()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""Diploma"";";
                return dbConnection.Query<Diploma>(query).ToList();
                #endregion
            }
        }

        public void ActualizeDiplomas() => this.Diplomas = GetAllDiplomas();

        private List<DiplomaElement> GetAllDiplomaElements()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""DiplomaElement"";";
                return dbConnection.Query<DiplomaElement>(query).ToList();
                #endregion
            }
        }

        public void ActualizeDiplomaElements() => this.DiplomaElements = GetAllDiplomaElements();

        private List<DiplomaType> GetAllDiplomaTypes()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""DiplomaType"";";
                return dbConnection.Query<DiplomaType>(query).ToList();
                #endregion
            }
        }

        public void ActualizeDiplomaTypes() => this.DiplomaTypes = GetAllDiplomaTypes();

        private List<Discipline> GetAllDisciplines()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT d.""Id"" AS ""DisciplineId""
                                       ,d.*
                                       ,duf.""Id"" AS ""DisciplineUploadedFileId""
                                       ,duf.*
                                       ,da.""Id"" AS ""DisciplineAliasId""
                                       ,da.*
                                       ,dauf.""Id"" AS ""DisciplineAliasUploadedFileId""
                                       ,dauf.*
                                 FROM public.""Discipline"" d
                                 LEFT JOIN public.""UploadedFile"" duf ON duf.""Id"" = d.""ImageId""
                                 LEFT JOIN public.""DisciplineAlias"" da ON da.""DisciplineId"" = d.""Id""
                                 LEFT JOIN public.""UploadedFile"" dauf ON dauf.""Id"" = da.""ImageId""
                                 ORDER BY d.""Title"";";

                Dictionary<int, Discipline> disciplineDictionary = new Dictionary<int, Discipline>();

                return dbConnection.Query<Discipline>(
                    query,
                    types: new[]
                    {
                        typeof(Discipline),
                        typeof(UploadedFile),
                        typeof(DisciplineAlias),
                        typeof(UploadedFile)
                    },
                    map: objects =>
                    {
                        Discipline discipline = objects[0] as Discipline;
                        UploadedFile disciplineUploadedFile = objects[1] as UploadedFile;
                        DisciplineAlias disciplineAlias = objects[2] as DisciplineAlias;
                        UploadedFile disciplineAliasUploadedFile = objects[3] as UploadedFile;

                        discipline.UploadedFile = disciplineUploadedFile;
                        disciplineAlias.UploadedFile = disciplineAliasUploadedFile;

                        if (!disciplineDictionary.TryGetValue(discipline.Id, out Discipline disciplineEntry))
                        {
                            disciplineEntry = discipline;
                            disciplineEntry.DisciplineAliases = new List<DisciplineAlias>();
                            disciplineDictionary.Add(disciplineEntry.Id, disciplineEntry);
                        }

                        disciplineEntry.DisciplineAliases.Add(disciplineAlias);

                        return disciplineEntry;
                    },
                    splitOn: "DisciplineUploadedFileId, DisciplineAliasId, DisciplineAliasUploadedFileId").Distinct().ToList();
                #endregion
            }
        }

        public void ActualizeDisciplines() => this.Disciplines = GetAllDisciplines();
        
        private List<DisciplineAlias> GetAllDisciplineAliases()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT da.""Id"" AS ""DisciplineAliasId""
                                       ,da.*
                                       ,dauf.""Id"" AS ""DisciplineAliasUploadedFileId""
                                       ,dauf.*
                                       ,di.""Id"" AS ""DisciplineId""
                                       ,di.*
                                       ,diuf.""Id"" AS ""DisciplineUploadedFileId""
                                       ,diuf.*
                                       ,ot.""Id"" AS ""OlympiadTypeId""
                                       ,ot.*
                                 FROM public.""DisciplineAlias"" da
                                 LEFT JOIN public.""UploadedFile"" dauf ON dauf.""Id"" = da.""ImageId""
                                 INNER JOIN public.""Discipline"" di ON di.""Id"" = da.""DisciplineId""
                                 LEFT JOIN public.""UploadedFile"" diuf ON diuf.""Id"" = di.""ImageId""
                                 INNER JOIN public.""OlympiadType"" ot ON ot.""Id"" = da.""OlympiadTypeId""
                                 ORDER BY da.""Title"";";
                return dbConnection.Query<DisciplineAlias>(
                    query,
                    types: new[]
                    {
                        typeof(DisciplineAlias),
                        typeof(UploadedFile),
                        typeof(Discipline),
                        typeof(UploadedFile),
                        typeof(OlympiadType)
                    },
                    map: objects =>
                    {
                        DisciplineAlias disciplineAlias = objects[0] as DisciplineAlias;
                        UploadedFile disciplineAliasUploadedFile = objects[1] as UploadedFile;
                        Discipline discipline = objects[2] as Discipline;
                        UploadedFile disciplineUploadedFile = objects[3] as UploadedFile;
                        OlympiadType olympiadType = objects[4] as OlympiadType;

                        if (disciplineAliasUploadedFile.Id != default)
                            disciplineAlias.UploadedFile = disciplineAliasUploadedFile;

                        if (disciplineUploadedFile.Id != default)
                            discipline.UploadedFile = disciplineUploadedFile;

                        disciplineAlias.Discipline = discipline;
                        disciplineAlias.OlympiadType = olympiadType;

                        return disciplineAlias;
                    },
                    splitOn: "DisciplineAliasUploadedFileId, DisciplineId, DisciplineUploadedFileId, OlympiadTypeId").Distinct().ToList();
                #endregion
            }
        }

        public void ActualizeDisciplineAliases() => this.DisciplineAliases = GetAllDisciplineAliases();

        private List<OlympiadSubtype> GetAllOlympiadSubtypes()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""OlympiadSubtype"";";
                return dbConnection.Query<OlympiadSubtype>(query).ToList();
                #endregion
            }
        }

        public void ActualizeOlympiadSubtypes() => this.OlympiadSubtypes = GetAllOlympiadSubtypes();

        private List<OlympiadType> GetAllOlympiadTypes()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""OlympiadType"";";
                return dbConnection.Query<OlympiadType>(query).ToList();
                #endregion
            }
        }

        public void ActualizeOlympiadTypes() => this.OlympiadTypes = GetAllOlympiadTypes();

        private List<Product> GetAllProducts()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""Product"";";
                return dbConnection.Query<Product>(query).ToList();
                #endregion
            }
        }

        public void ActualizeProducts() => this.Products = GetAllProducts();

        private List<PaymentType> GetAllPaymentTypes()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""PaymentType"";";
                return dbConnection.Query<PaymentType>(query).ToList();
                #endregion
            }
        }

        public void ActualizePaymentTypes() => this.PaymentTypes = GetAllPaymentTypes();

        private List<ParticipantStatus> GetAllParticipantStatuses()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""ParticipantStatus"";";
                return dbConnection.Query<ParticipantStatus>(query).ToList();
                #endregion
            }
        }

        public void ActualizeParticipantStatuses() => this.ParticipantStatuses = GetAllParticipantStatuses();

        private List<Role> GetAllRoles()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""Role"";";
                return dbConnection.Query<Role>(query).ToList();
                #endregion
            }
        }

        public void ActualizeRoles() => this.Roles = GetAllRoles();

        private List<RoleLevel> GetAllRoleLevels()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""RoleLevel"";";
                return dbConnection.Query<RoleLevel>(query).ToList();
                #endregion
            }
        }

        public void ActualizeRoleLevels() => this.RoleLevels = GetAllRoleLevels();

        private List<Session> GetAllSessions()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""Session"";";
                return dbConnection.Query<Session>(query).ToList();
                #endregion
            }
        }

        public void ActualizeSessions() => this.Sessions = GetAllSessions();

        private List<StudyYear> GetAllStudyYears()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""StudyYear"";";
                return dbConnection.Query<StudyYear>(query).ToList();
                #endregion
            }
        }

        public void ActualizeStudyYears() => this.StudyYears = GetAllStudyYears();

        private List<QuestionCategory> GetAllQuestionCategories()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""QuestionCategory"";";
                return dbConnection.Query<QuestionCategory>(query).ToList();
                #endregion
            }
        }

        public void ActualizeQuestionCategories() => this.QuestionCategories = GetAllQuestionCategories();

        private List<UploadedFileType> GetAllUploadedFileTypes()
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT * FROM public.""UploadedFileType"";";
                return dbConnection.Query<UploadedFileType>(query).ToList();
                #endregion
            }
        }

        public void ActualizeUploadedFileTypes() => this.UploadedFileTypes = GetAllUploadedFileTypes();
    }
}
