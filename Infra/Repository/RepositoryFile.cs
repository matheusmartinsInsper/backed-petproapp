using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Infra.Repository
{
    public class RepositoryFile:IRepositoryFile
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryFile(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }

        public Task<Attendance> get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task save(List<FileAttachment> files)
        {
            await _context.connect(_connectString);
            string command = "INSERT INTO \"file\" (idfile,arquivobase64,nomearquivo,idattendance,categoryfile) VALUES " +
                "(@idfile,@arquivobase64,@nomearquivo,@idattendance,@categoryfile)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
             {
                {"@idfile",""},
                {"@arquivobase64", "" },
                {"@nomearquivo",""},
                {"@idattendance",""},
                {"@categoryfile",""},
             };
            foreach (FileAttachment file in files)
            {
                parameters["@idfile"] = file.id;
                parameters["@arquivobase64"] = file.FileBase64;
                parameters["@nomearquivo"] = file.FileName;
                parameters["@idattendance"] = file.idattendance;
                parameters["@categoryfile"] = file.category;
                await _context.command(command, parameters);
            }
            _context.close();
        }

        public Task update(Attendance attendance)
        {
            throw new NotImplementedException();
        }
    }
}
