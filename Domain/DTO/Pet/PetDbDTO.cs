namespace app.Domain.DTO.Pet
{
    public class PetDbDTO
    {
        public string idpet { get; set; }
        public string idusertutor { get; set; }
        public DateTime dateborn { get; set; }
        public string petname { get; set; }
        public float weight { get; set; }
        public string race { get; set; }
        public string species { get; set; }
        public string sex { get; set; }
        public bool castrated { get; set; }
    }
}
