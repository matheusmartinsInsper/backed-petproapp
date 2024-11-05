using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Form;
using app.Domain.DTO.InviteCollaborator;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace app.Infra.Repository
{
    public class RepositoryForm : IRepositoryForm
    {
        private IFactoryDbContext _factoryDbContext;
        private IDbContext _context;
        private string _connectString = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=petprodb";
        public RepositoryForm(IFactoryDbContext factorycontext)
        {
            _factoryDbContext = factorycontext;
            _context = _factoryDbContext.psqlContext();
        }
        public Task<Form> get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Form> getByIdAttendance(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Form>> getByIdUser(string id)
        {
            await _context.connect(_connectString);
            string command = "select  f.formid,f.color,f.formname,a.attributeid,a.attributename,a.attributetype,o.optionid,o.optionvalue from \"form\" f"
                + " join \"attribute\" a on f.formid = a.formid left join \"option\" o ON a.attributeid = o.attributeid where f.iduser = @iduser";
            
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
              {"@iduser", id}
            };

            // Executa a query com o parâmetro iduser
            List<JsonObject> result = await _context.read(command, parameters);
            foreach (var jsonObject in result)
            {
                if (jsonObject.TryGetPropertyValue("optionid", out var optionid))
                {
                    if (optionid.ToString() == "{}")
                    {
                        jsonObject["optionid"] = null;
                    }
                }
                if (jsonObject.TryGetPropertyValue("optionvalue", out var optionvalue))
                {
                    if (optionvalue.ToString() == "{}")
                    {
                        jsonObject["optionvalue"] = null;
                    }
                }
            }
            List<FormDbOutput> dtos = result.Select(jsonObject => JsonSerializer.Deserialize<FormDbOutput>(jsonObject.ToJsonString()))
                                           .ToList();
            // Dicionário para evitar duplicação de formulários e atributos
            Dictionary<string, Form> formsDictionary = new Dictionary<string, Form>();
            Dictionary<string, attribute> attributesDictionary = new Dictionary<string, attribute>();

            foreach (FormDbOutput row in dtos)
            {
                // Pega o formid do formulário atual
                string formId = row.formid.ToString();

                // Verifica se o formulário já foi adicionado à lista
                if (!formsDictionary.ContainsKey(formId))
                {
                    FormDbDTO formdb = new FormDbDTO
                    {
                        idform = formId,
                        color = row.color,
                        nameform = row.formname.ToString(),
                        attributes = new List<attribute>()
                    };
                    Form form = Form.restore(formdb);
                    formsDictionary[formId] = form;
                }

                // Verifica se o atributo já foi adicionado ao formulário
                string attributeId = row.attributeid.ToString();
                if (!attributesDictionary.ContainsKey(attributeId))
                {
                    attribute attribute = new attribute
                    {
                        idattribute = attributeId,
                        idform = formId,
                        label = row.attributename.ToString(),
                        typeattribute = row.attributetype.ToString(),
                        options = new List<Option>()
                    };

                    formsDictionary[formId].attributes.Add(attribute);
                    attributesDictionary[attributeId] = attribute;
                }

                // Adiciona a opção ao atributo se existir
                if (row.optionid!=null)
                {
                    Option option = new Option
                    {
                        idoption = row.optionid.ToString(),
                        idattribute = attributeId,
                        value = row.optionvalue.ToString()
                    };

                    attributesDictionary[attributeId].options.Add(option);
                }
            }

            _context.close();
            // Retorna a lista de formulários
            return formsDictionary.Values.ToList();
        }

        public async Task save(Form form)
        {
            await _context.connect(_connectString);
            string command = "INSERT INTO \"form\" (formid,iduser,formname,color) VALUES " +
                "(@idform,@iduser,@nameform,@color)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
             {
                {"@iduser",form.iduser},
                {"@idform", form.idform},
                {"@nameform",form.nameform},
                {"@color",form.color}
             };
            await _context.command(command, parameters);
            foreach (attribute att in form.attributes)
            {
                string commandatt = "INSERT INTO \"attribute\" (attributeid,attributename,attributetype,formid) VALUES " +
               "(@idattribute,@attributename,@type,@formid)";

                parameters["@idattribute"] = att.idattribute;
                parameters["@formid"] = att.idform;
                parameters["@attributename"] = att.label;
                parameters["@type"] = att.typeattribute;
                await _context.command(commandatt, parameters);
                if (att.typeattribute == "checkbox"||att.typeattribute == "select")
                {
                    string commandoption = "INSERT INTO \"option\" (optionid,attributeid,optionvalue) VALUES " +
                                             "(@optionid,@idattribute,@optionvalue)";
                    foreach (Option option in att.options)
                    {
                        parameters["@optionid"] = option.idoption;
                        parameters["@idattribute"] = option.idattribute;
                        parameters["@optionvalue"] = option.value;
                        await _context.command(commandoption, parameters);
                    }
                }
            }
            _context.close();
        }

        public Task update(Form form)
        {
            throw new NotImplementedException();
        }
    }
}
