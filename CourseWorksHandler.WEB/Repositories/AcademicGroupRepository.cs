using CourseWorksHandler.WEB.Models;
using System;
using System.Data.SqlClient;

namespace CourseWorksHandler.WEB.Repositories
{
    public sealed class AcademicGroupRepository : BasicAsyncRepository<AcademicGroup>
    {
        public AcademicGroupRepository(SqlConnection sqlConnection) : base(sqlConnection)
        {
        }

        protected override Func<SqlDataReader, AcademicGroup> SelectMapper
        {
            get => r => new AcademicGroup
            {
                Id = r.GetInt32(0),
                GroupName = r.GetString(1),
                TeacherId = r.GetInt32(2)
            };
        }

        protected override Func<AcademicGroup, object[]> InsertValues
        {
            get => group => new object[]
            {
                group.GroupName, group.TeacherId
            };
        }

        protected override Func<AcademicGroup, TablePropertyValuePair[]> UpdatePropertiesAndValuesExtractor
        {
            get => grop => new TablePropertyValuePair[]{
                new TablePropertyValuePair("GroupName", grop.GroupName),
                new TablePropertyValuePair("TeacherId", grop.TeacherId)
            };
        }

        protected override TablePropertyExtractor UpdatePredicatePropertyEqualsValue
        {
            get => new TablePropertyExtractor("Id", t => t.Id);
        }
    }
}
