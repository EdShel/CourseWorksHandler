using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorksHandler.WEB.Repositories
{
    public abstract class BasicAsyncRepository<T> : IRepositoryAsync<T>
        where T : class
    {
        private static string tableName;

        protected SqlConnection db;

        static BasicAsyncRepository()
        {
            tableName = typeof(T).Name;
        }

        public BasicAsyncRepository(SqlConnection sqlConnection)
        {
            db = sqlConnection;
        }

        public abstract Func<SqlDataReader, T> SelectMapper { get; }

        public abstract Func<T, object[]> InsertValues { get; }

        public abstract Func<T, (string, object)[]> UpdateFieldsAndValues { get; }

        public abstract (string, Func<T, object>) UpdatePredicatePropertyEqualsValue { get; }

        public async Task DeleteAsync(int id)
        {
            var deleteCommand = db.CreateCommand();
            deleteCommand.CommandText = $"DELETE FROM {tableName} WHERE Id = @id";
            deleteCommand.Parameters.AddWithValue("@id", id);
            await deleteCommand.ExecuteNonQueryAsync();
        }

        public async Task<T> SelectAsync(int id)
        {
            var selectCommand = db.CreateCommand();
            selectCommand.CommandText = $"SELECT * FROM {tableName} WHERE Id = @id";
            selectCommand.Parameters.AddWithValue("@id", id);
            var reader = await selectCommand.ExecuteReaderAsync();
            if (!reader.HasRows)
            {
                throw new ArgumentException($"Not found {tableName} with Id = {id}");
            }
            return SelectMapper(reader);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var selectCommand = db.CreateCommand();
            selectCommand.CommandText = $"SELECT * FROM {tableName}";
            var reader = await selectCommand.ExecuteReaderAsync();
            var items = new List<T>();
            while (await reader.ReadAsync())
            {
                items.Add(SelectMapper(reader));
            }
            return items;
        }

        public async Task InsertAsync(T item)
        {
            var values = InsertValues(item);
            var insertCommand = db.CreateCommand();
            insertCommand.CommandText = GenerateInsertCommandOfValues(values);

            for (int i = 0; i < values.Length; i++)
            {
                insertCommand.Parameters.AddWithValue($"@{i}", values[i]);
            }

            await insertCommand.ExecuteNonQueryAsync();
        }

        private static string GenerateInsertCommandOfValues(object[] values)
        {
            var sb = new StringBuilder($"INSERT INTO {tableName} VALUES(");
            for (int i = 0; i < values.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append(',');
                }
                sb.Append('@');
                sb.Append(i);
            }
            sb.Append(')');
            return sb.ToString();
        }

        public async Task UpdateAsync(T item)
        {
            var fieldsAndValues = UpdateFieldsAndValues(item);
            var updateCommand = db.CreateCommand();
            updateCommand.CommandText = GenerateUpdateCommandOfPropertiesAndValues(fieldsAndValues);

            for (int i = 0; i < fieldsAndValues.Length; i++)
            {
                updateCommand.Parameters.AddWithValue($"@{i}", fieldsAndValues[i].Item2);
            }

            updateCommand.Parameters.AddWithValue($"@id", UpdatePredicatePropertyEqualsValue.Item2(item));

            await updateCommand.ExecuteNonQueryAsync();
        }

        private string GenerateUpdateCommandOfPropertiesAndValues((string, object)[] fieldsAndValues)
        {
            var sb = new StringBuilder($"UPDATE {tableName} SET ");
            for (int i = 0; i < fieldsAndValues.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append(',');
                }
                sb.Append(fieldsAndValues[i].Item1);
                sb.Append('=');
                sb.Append($"@{i}");
            }
            sb.Append("WHERE ");
            sb.Append(UpdatePredicatePropertyEqualsValue.Item1);
            sb.Append("=@id");
            return sb.ToString();
        }
    }
}
