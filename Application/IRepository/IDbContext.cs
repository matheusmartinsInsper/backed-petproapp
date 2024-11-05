using System.Text.Json.Nodes;

namespace app.Application.IRepository
{
    public interface IDbContext
    {
        public Task connect(string connectString);
        public void close();
        public Task command(string command, Dictionary<string, object> parameters);
        Task<List<JsonObject>> read(string command, Dictionary<string, object> parameters);
    }
}
