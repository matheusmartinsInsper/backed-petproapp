using app.Application.IRepository;
using Npgsql;
using System.Text.Json.Nodes;

namespace app.Infra.Repository.FactoryContext
{
    public class PGSQLContext : IDbContext, IDisposable
    {
        private NpgsqlConnection _conn;
        public NpgsqlConnection getConn { get { return _conn; } }
        public async Task connect(string connectString)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectString);
            _conn = conn;
            await _conn.OpenAsync();
        }
        public void close()
        {
            _conn.Close();
        }

        public async Task command(string command, Dictionary<string, object> parameters)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(command, _conn))
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<JsonObject>> read(string command, Dictionary<string, object> parameters)
        {
            List<JsonObject> resultOfQuery = new List<JsonObject>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(command, _conn))
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        JsonObject lineOfResult = new JsonObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            lineOfResult.Add(reader.GetName(i), JsonValue.Create(reader.GetValue(i)));
                        }
                        resultOfQuery.Add(lineOfResult);
                    }
                }
            }
            return resultOfQuery;
        }
        public void Dispose()
        {
            if (_conn != null)
            {
                _conn.Dispose();
                _conn = null;
            }
        }
    }
}
