using CourseWorksHandler.WEB.Models;
using CourseWorksHandler.WEB.ViewModels;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CourseWorksHandler.WEB.Repositories
{
    public sealed class CourseWorkRepository : BasicAsyncRepository<CourseWork>
    {
        public CourseWorkRepository(SqlConnection sqlConnection) : base(sqlConnection)
        {
        }

        public async Task SubmitCourseWork(CourseWorkSubmissionModel model)
        {
            var submitCommand = db.CreateCommand();
            submitCommand.CommandText = "EXEC SubmitCourseWork @studentId, @theme, @task";
            submitCommand.Parameters.AddWithValue("@studentId", model.StudentId);
            submitCommand.Parameters.AddWithValue("@theme", model.Theme);
            submitCommand.Parameters.AddWithValue("@task", model.Task ?? string.Empty);
            await submitCommand.ExecuteNonQueryAsync();
        }

        protected override Func<SqlDataReader, CourseWork> SelectMapper
        {
            get => r => new CourseWork
            {
                Id = r.GetInt32(0),
                Theme = r.GetString(1),
                Task = r.GetString(2),
                SubmissionTime = r.GetDateTime(3)
            };
        }

        protected override Func<CourseWork, object[]> InsertValues
        {
            get => work => new object[]
            {
                work.Theme, work.Task
            };
        }

        protected override Func<CourseWork, TablePropertyValuePair[]> UpdatePropertiesAndValuesExtractor
        {
            get => work => new TablePropertyValuePair[]{
                new TablePropertyValuePair("Theme", work.Theme),
                new TablePropertyValuePair("Task", work.Task),
            };
        }

        protected override TablePropertyExtractor UpdatePredicatePropertyEqualsValue
        {
            get => new TablePropertyExtractor("Id", t => t.Id);
        }
    }
}
