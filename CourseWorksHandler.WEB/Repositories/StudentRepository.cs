using CourseWorksHandler.WEB.Models;
using System;
using System.Data.SqlClient;

namespace CourseWorksHandler.WEB.Repositories
{
    public sealed class StudentRepository : BasicAsyncRepository<Student>
    {
        public StudentRepository(SqlConnection sqlConnection) : base(sqlConnection)
        {
        }

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
    }
}
