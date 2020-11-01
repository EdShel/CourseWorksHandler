using CourseWorksHandler.WEB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CourseWorksHandler.WEB.Repositories
{
    public sealed class GroupsRepository : BasicAsyncRepository<AcademicGroup>
    {
        public GroupsRepository(SqlConnection sqlConnection) : base(sqlConnection)
        {
        }

        public async Task<IEnumerable<GroupInfo>> GetGroupsInfoAsync()
        {
            var selectCommand = db.CreateCommand();
            selectCommand.CommandText = "SELECT * FROM GroupsInfo ORDER BY GroupName";
            using(var r = await selectCommand.ExecuteReaderAsync())
            {
                var groups = new List<GroupInfo>();
                while(await r.ReadAsync())
                {
                    groups.Add(new GroupInfo
                    {
                        Id = r.GetInt32(0),
                        GroupName = r.GetString(1),
                        TeacherId = r.IsDBNull(3) ? -1 : r.GetInt32(2),
                        TeacherName = r.IsDBNull(3) ? null : r.GetString(3),
                        StudentsCount = r.GetInt32(4),
                        AverageMark = r.GetInt32(5)
                    });
                }
                return groups;
            }
        }

        public async Task RemoveTeacherFormGroup(int id)
        {
            var removeCommand = db.CreateCommand();
            removeCommand.CommandText = "EXEC RemoveTeacherForGroup @groupId";
            removeCommand.Parameters.AddWithValue("@groupId", id);
            await removeCommand.ExecuteNonQueryAsync();
        }

        protected override Func<SqlDataReader, AcademicGroup> SelectMapper
        {
            get => r => new AcademicGroup
            {
                Id = r.GetInt32(0),
                GroupName = r.GetString(1),
                TeacherId = r.IsDBNull(2) ? -1 : r.GetInt32(2)
            };
        }

        protected override Func<AcademicGroup, object[]> InsertValues
        {
            get => gr => new object[] { gr.GroupName, gr.TeacherId };
        }

        protected override Func<AcademicGroup, TablePropertyValuePair[]> UpdatePropertiesAndValuesExtractor
        {
            get => gr => new TablePropertyValuePair[]
            {
                new TablePropertyValuePair("GroupName", gr.GroupName),
                new TablePropertyValuePair("TeacherId", gr.TeacherId)
            };
        }

        protected override TablePropertyExtractor UpdatePredicatePropertyEqualsValue
        {
            get => new TablePropertyExtractor("Id", gr => gr.Id);
        }
    }
}
