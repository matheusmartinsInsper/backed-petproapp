namespace app.Domain.DTO.Pet
{
    public class PetDTOInputUseCase
    {
        public string idusertutor {  get; set; }
        public DateTime dateborn { get; set; }
        public string petname { get; set; }
        public float weight { get; set; }
        public string race { get; set; }
        public string species { get; set; }
        public string sex { get; set; }
        public bool castrated { get; set; }
        public List<ContraindicationDTO>? ContraindicationDTO { get; set; }
    }
}
