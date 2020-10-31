using CourseWorksHandler.WEB.Models;
using CourseWorksHandler.WEB.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CourseWorksHandler.WEB.Repositories
{
    public sealed class AppUserRepository : BasicAsyncRepository<AppUser>
    {
        public AppUserRepository(SqlConnection db) : base(db)
        {
        }

        public async Task RegisterTeacherAsync(RegisterTeacherModel request)
        {
            var registerCommand = db.CreateCommand();
            registerCommand.CommandText = "EXEC RegisterTeacher @email, @passwordHash, @fullName";
            registerCommand.Parameters.AddWithValue("@email", request.Email);
            registerCommand.Parameters.AddWithValue("@passwordHash", 
                new PasswordHasher<AppUser>().HashPassword(null, request.Password));
            registerCommand.Parameters.AddWithValue("@fullName", request.FullName);
            await registerCommand.ExecuteNonQueryAsync();
        }

        public bool VerifyPassword(AppUser user, string password)
        {
            return new PasswordHasher<AppUser>()
                .VerifyHashedPassword(null, user.PasswordHashed, password) == PasswordVerificationResult.Success;
        }

        public async Task<AppUser> GetByEmailAsync(string email)
        {
            var selectCommand = db.CreateCommand();
            selectCommand.CommandText = $"SELECT * FROM AppUser WHERE Email = @email";
            selectCommand.Parameters.AddWithValue("@email", email);
            var reader = await selectCommand.ExecuteReaderAsync();
            if (!(await reader.ReadAsync()))
            {
                return null;
            }
            var user = SelectMapper(reader);
            reader.Close();
            return user;
        }

        protected override Func<SqlDataReader, AppUser> SelectMapper
        {
            get => r => new AppUser
            {
                Id = r.GetInt32(0),
                Email = r.GetString(1),
                PasswordHashed = r.GetString(2),
                RoleName = r.GetString(3)
            };
        }

        protected override Func<AppUser, object[]> InsertValues
        {
            get => throw new InvalidOperationException();
        }

        protected override Func<AppUser, TablePropertyValuePair[]> UpdatePropertiesAndValuesExtractor
        {
            get => throw new InvalidOperationException();
        }

        protected override TablePropertyExtractor UpdatePredicatePropertyEqualsValue
        {
            get => throw new InvalidOperationException();
        }
    }
}
