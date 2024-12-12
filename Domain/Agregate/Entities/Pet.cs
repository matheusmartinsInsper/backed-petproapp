using app.Domain.DTO.Pet;

namespace app.Domain.Agregate.Entities
{
    public class Pet
    { 
        private string _idusertutor;
        private string _idpet;
        private DateTime _dateborn;
        private int _years;
        private int _months;
        private string _age;
        private string _petname;
        private float _weight;
        private string _race;
        private string _species;
        private string _sex;
        private bool _castrated;
        private List<Contraindication> _Contraindications;
        private List<ContraindicationDTO> _contraindications;
        public string IdUserTutor { get { return _idusertutor; } }
        public string IdPet { get { return _idpet; } }
        public DateTime DateBorn { get { return _dateborn; } }
        public string Age { get { return _age; } }
        public string PetName { get { return _petname; } }
        public float Weight { get { return _weight; } }
        public string Race { get { return _race; } }
        public string Species { get { return _species; } }
        public string Sex { get { return _sex; } }
        public bool Castrated { get { return _castrated; } }
        public List<Contraindication> ContraIndications { get { return _Contraindications; } }
        public List<ContraindicationDTO> contraindications { get { return _contraindications; } }
        private Pet() { }
        public static Pet create(PetDTOInputUseCase petdto)
        {
            Pet pet = new Pet();
            pet._idusertutor = petdto.idusertutor.ToString();
            pet._petname = petdto.petname.ToString();
            pet._idpet = Guid.NewGuid().ToString("N");
            pet._dateborn = petdto.dateborn.Day > DateTime.Now.Day
                                   && petdto.dateborn.Month == DateTime.Now.Month
                                   && petdto.dateborn.Year >= DateTime.Now.Year
                                   ? throw new Exception("Date invalid") : petdto.dateborn;
            pet._weight = petdto.weight;
            pet._sex = petdto.sex=="Macho"||petdto.sex=="Femea"?petdto.sex: throw new Exception("Sexo invalido - validos (Macho,Femea)");
            pet._race = petdto.race;
            pet._castrated = petdto.castrated;
            pet._species = petdto.species == "Canina" || petdto.species == "Felina" ? petdto.species: throw new Exception("Especie nao existente");
            return pet;
        }
        public static Pet create(PetDTOInputUseCase petdto,List<ContraindicationDTO> contraindicatiosdto)
        {
            Pet pet = new Pet();
            List<Contraindication> contraindications = new List<Contraindication>();
            pet._idusertutor = petdto.idusertutor.ToString();
            pet._petname = petdto.petname.ToString();
            pet._idpet = Guid.NewGuid().ToString("N");
            pet._dateborn = petdto.dateborn.Day > DateTime.Now.Day
                                   && petdto.dateborn.Month == DateTime.Now.Month
                                   && petdto.dateborn.Year >= DateTime.Now.Year
                                   ? throw new Exception("Date invalid") : petdto.dateborn;
            pet._weight = petdto.weight;
            pet._sex = petdto.sex;
            pet._race = petdto.race;
            pet._castrated = petdto.castrated;
            pet._species = petdto.species == "Canina" || petdto.species == "Felina" ? petdto.species : throw new Exception("Especie nao existente");
            foreach(ContraindicationDTO contr in contraindicatiosdto)
            {
                Contraindication contraindication = Contraindication.create(contr.description, pet._idpet, contr.categoria);
                contraindications.Add(contraindication);
            }
            pet._Contraindications = contraindications;
            return pet;
        }
        public static Pet restore(PetDbDTO petdto,List<ContraindicationDTODb> contraindications)
        {
            Pet pet = new Pet();
            pet._Contraindications = new List<Contraindication>();
            pet._contraindications = new List<ContraindicationDTO>();
            int mounths = DateTime.Now.Month - petdto.dateborn.Month;
            int year = DateTime.Now.Year - petdto.dateborn.Year;
            pet._idusertutor = petdto.idusertutor;
            pet._idpet = petdto.idpet.ToString();
            pet._petname= petdto.petname.ToString();
            pet._weight= petdto.weight;
            pet._years = DateTime.Now.Year - petdto.dateborn.Year;
            pet._months = DateTime.Now.Month - petdto.dateborn.Month;
            pet._age = year < 1 ? $"{pet._months} meses" : $"{pet._years} Anos e {pet._months} meses";
            pet._sex = petdto.sex;
            pet._species= petdto.species;
            pet._race= petdto.race;
            pet._dateborn = petdto.dateborn;
            pet._castrated= petdto.castrated;
            foreach(ContraindicationDTODb contr in contraindications)
            {
                Contraindication contraindication = new Contraindication();
                ContraindicationDTO contraindicationdto = new ContraindicationDTO();

                contraindicationdto.categoria = contr.categoria;
                contraindicationdto.description = contr.description;
                contraindicationdto.idcontraindication = contr.idcontraindication;

                contraindication._idcontraindication = contr.idcontraindication;
                contraindication.Type = contr.categoria;
                contraindication._idPet = contr.idpet;
                contraindication.Description = contr.description;
                pet._Contraindications.Add(contraindication);
                pet._contraindications.Add(contraindicationdto);
            }
            return pet;
        }
        public void addContraindication(ContraindicationDTOInput contraindicationdto)
        {
            Contraindication contraindication = Contraindication.create(contraindicationdto.description, _idpet, contraindicationdto.categoria);
            _Contraindications.Add(contraindication);
            return;
        }
        public void removeContraindication(string idcontraindication)
        {
            Contraindication contr = _Contraindications.Where((x)=>x.idcontraindication == idcontraindication).FirstOrDefault();    
            _Contraindications.Remove(contr);
            return;
        }
    }
    public class Contraindication
    {
        public string Description { get; set; }
        public string _idcontraindication {  get; set; }
        public string _idPet { get; set; }
        public string idcontraindication { get { return _idcontraindication; } }
        public enum ContraindicationType
        {
            Alergia,
            Convívio,
            Medicamentos,
            Cirurgias,
            Vacinas,
            Alimentação,
            AtividadeFisica,
            Comportamento,
            Outros
        }
        public ContraindicationType _typeEnum;
        public string Type
        {
            get => _typeEnum.ToString();
            set
            {
                if (Enum.TryParse<ContraindicationType>(value, true, out var parsedType))
                {
                    _typeEnum = parsedType;
                }
                else
                {
                    throw new ArgumentException($"Invalid type: {value}");
                }
            }
        }

        public static Contraindication create(string description, string idPet, string type)
        {
            Contraindication contraindication = new Contraindication();
            contraindication._idcontraindication = Guid.NewGuid().ToString("N");
            contraindication.Type = type;
            contraindication._idPet = idPet;
            contraindication.Description = description;
            return contraindication;
        }
    }

}
