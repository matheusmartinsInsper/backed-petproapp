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
            pet._sex = petdto.sex;
            pet._race = petdto.race;
            pet._castrated = petdto.castrated;
            pet._species = petdto.species == "Canina" || petdto.species == "Felina" ? petdto.species: throw new Exception("Especie nao existente");
            return pet;
        }
        public static Pet restore(PetDbDTO petdto)
        {
            Pet pet = new Pet();
            pet._idusertutor = petdto.idusertutor;
            pet._idpet = petdto.idpet.ToString();
            pet._petname= petdto.petname.ToString();
            pet._weight= petdto.weight;
            pet._years = DateTime.Now.Year - petdto.dateborn.Year;
            pet._months = DateTime.Now.Month - petdto.dateborn.Month;
            pet._age = $"{pet._years},{pet._months} Anos";
            pet._sex = petdto.sex;
            pet._species= petdto.species;
            pet._race= petdto.race;
            pet._dateborn = petdto.dateborn;
            pet._castrated= petdto.castrated;
            return pet;
        }
    }
}
