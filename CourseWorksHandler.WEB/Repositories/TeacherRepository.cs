using CourseWorksHandler.WEB.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CourseWorksHandler.WEB.Repositories
{
    public sealed class TeacherRepository : BasicAsyncRepository<Teacher>
    {
        public TeacherRepository(SqlConnection sqlConnection) : base(sqlConnection)
        {
        }

        public async Task PutMarkToStudent(int studentId, int mark)
        {
            var putCommand = db.CreateCommand();
            putCommand.CommandText = "EXEC PutMark @studentId, @mark";
            putCommand.Parameters.AddWithValue("@studentId", studentId);
            putCommand.Parameters.AddWithValue("@mark", mark);
            await putCommand.ExecuteNonQueryAsync();
        }

        protected override Func<SqlDataReader, Teacher> SelectMapper
        {
            get => r => new Teacher
            {
                Id = r.GetInt32(0),
                FullName = r.GetString(1)
            };
        }

        protected override Func<Teacher, object[]> InsertValues
        {
            get => teacher => new object[]
            {
                teacher.FullName
            };
        }

        protected override Func<Teacher, TablePropertyValuePair[]> UpdatePropertiesAndValuesExtractor
        {
            get => teacher => new TablePropertyValuePair[]{
                new TablePropertyValuePair("FullName", teacher.FullName)
            };
        }

        protected override TablePropertyExtractor UpdatePredicatePropertyEqualsValue
        {
            get => new TablePropertyExtractor("Id", t => t.Id);
        }
    }
}
