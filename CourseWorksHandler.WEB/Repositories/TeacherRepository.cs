using CourseWorksHandler.WEB.Models;
using System;
using System.Data.SqlClient;

namespace CourseWorksHandler.WEB.Repositories
{
    public class TeacherRepository : BasicAsyncRepository<Teacher>
    {
        public TeacherRepository(SqlConnection sqlConnection) : base(sqlConnection)
        {
        }

        public override Func<SqlDataReader, Teacher> SelectMapper
        {
            get => r => new Teacher
            {
                Id = r.GetInt32(0),
                FullName = r.GetString(1)
            };
        }

        public override Func<Teacher, object[]> InsertValues
        {
            get => teacher => new object[]
            {
                teacher.FullName
            };
        }

        public override Func<Teacher, (string, object)[]> UpdateFieldsAndValues
        {
            get => teacher => new (string, object)[]{
                ("FullName", teacher.FullName)
            };
        }

        public override (string, Func<Teacher, object>) UpdatePredicatePropertyEqualsValue
        {
            get => ("Id", teacher => teacher.Id);
        }
    }
}
