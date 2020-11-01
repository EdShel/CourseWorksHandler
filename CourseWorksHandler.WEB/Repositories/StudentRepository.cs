using CourseWorksHandler.WEB.Models;
using CourseWorksHandler.WEB.ViewModels;
using System;
using System.Collections;
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
            var r = await functionCallCommand.ExecuteReaderAsync();
            var results = new List<StudentsGeneralInfo>();
            while(await r.ReadAsync())
            {
                results.Add(new StudentsGeneralInfo
                {
                    GroupName = r.GetString(0),
                    StudentName = r.GetString(1),
                    Theme = await r.IsDBNullAsync(2) ? null : r.GetString(2),
                    Mark = r.GetInt32(3)
                });
            }

            return results;
        }

        public async Task<int> GetStudentsCount()
        {
            var selectCount = db.CreateCommand();
            selectCount.CommandText = "SELECT COUNT(*) FROM Student";
            return (int)await selectCount.ExecuteScalarAsync();
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
