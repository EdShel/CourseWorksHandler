using CourseWorksHandler.WEB.Models;
using CourseWorksHandler.WEB.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CourseWorksHandler.WEB.Repositories
{

    public sealed class StudentRepository : BasicAsyncRepository<Student>
    {
        public StudentRepository(SqlConnection sqlConnection) : base(sqlConnection)
        {
        }

        public async Task<IEnumerable<StudentsGeneralInfo>> GetStudentsGeneralInfoPaginated(int pageIndex, int pageSize)
        {
            var functionCallCommand = db.CreateCommand();
            functionCallCommand.CommandText = "SELECT * FROM GetStudentsGeneralInfoPaginated(@pageIndex, @pageSize)";
            functionCallCommand.Parameters.AddWithValue("@pageIndex", pageIndex);
            functionCallCommand.Parameters.AddWithValue("@pageSize", pageSize);
            using (var r = await functionCallCommand.ExecuteReaderAsync())
            {
                var results = new List<StudentsGeneralInfo>();
                while (await r.ReadAsync())
                {
                    results.Add(new StudentsGeneralInfo
                    {
                        GroupName = r.GetString(0),
                        StudentName = r.GetString(1),
                        Theme = await r.IsDBNullAsync(2) ? null : r.GetString(2),
                        Mark = r.GetInt32(3),
                        StudentId = r.GetInt32(4)
                    });
                }
                return results;
            }
        }

        public async Task<int> GetStudentsCount()
        {
            var selectCount = db.CreateCommand();
            selectCount.CommandText = "SELECT COUNT(*) FROM Student";
            return (int)await selectCount.ExecuteScalarAsync();
        }


        public async Task<StudentInfo> GetStudentInfo(int studentId)
        {
            var selectCommand = db.CreateCommand();
            selectCommand.CommandText = "SELECT * FROM GetStudentFullInfo(@id)";
            selectCommand.Parameters.AddWithValue("@id", studentId);
            using (var r = await selectCommand.ExecuteReaderAsync())
            {
                if (await r.ReadAsync())
                {
                    return new StudentInfo
                    {
                        Student = new Student
                        {
                            Id = studentId,
                            FullName = r.GetString(0),
                            Mark = r.GetInt32(1)
                        },
                        Group = new AcademicGroup
                        {
                            GroupName = r.GetString(2)
                        },
                        Teacher = r.GetInt32(3) == -1
                            ? null : new Teacher
                            {
                                Id = r.GetInt32(3),
                                FullName = r.GetString(4)
                            },
                        CourseWork = String.IsNullOrEmpty(r.GetString(5))
                            ? null : new CourseWork
                            {
                                Id = studentId,
                                Theme = r.GetString(5),
                                Task = r.GetString(6),
                                SubmissionTime = r.GetDateTime(7)
                            }
                    };
                }
            }

            return null;
        }

        public async Task<IEnumerable<CourseWorkHistoryEntry>> GetCourseWorkHistory(int studentId)
        {
            var getCommand = db.CreateCommand();
            getCommand.CommandText = "EXEC GetCourseWorkHistory @id";
            getCommand.Parameters.AddWithValue("@id", studentId);

            var history = new List<CourseWorkHistoryEntry>();
            using (var r = await getCommand.ExecuteReaderAsync())
            {
                while (await r.ReadAsync())
                {
                    history.Add(new CourseWorkHistoryEntry
                    {
                        Theme = r.GetString(0),
                        Task = r.GetString(1),
                        ChangeTime = r.GetDateTime(2)
                    });
                }
            }
            return history;
        }

        #region Default implementation

        protected override Func<SqlDataReader, Student> SelectMapper
        {
            get => r => new Student
            {
                Id = r.GetInt32(0),
                FullName = r.GetString(1),
                GroupId = r.GetInt32(2),
                Mark = r.GetInt32(3)
            };
        }

        protected override Func<Student, object[]> InsertValues
        {
            get => student => new object[]
            {
                student.FullName, student.GroupId, student.Mark
            };
        }

        protected override Func<Student, TablePropertyValuePair[]> UpdatePropertiesAndValuesExtractor
        {
            get => student => new TablePropertyValuePair[]
            {
                new TablePropertyValuePair("FullName", student.FullName),
                new TablePropertyValuePair("Mark", student.Mark)
            };
        }

        protected override TablePropertyExtractor UpdatePredicatePropertyEqualsValue
        {
            get => new TablePropertyExtractor("Id", st => st.Id);
        }

        #endregion
    }
}
