namespace app.Domain.DTO.NetWorkCollaborator
{
    public class NetWorkDbDTO
    {
        //public string idNetWorkCollaborators;
        public string iduserprimary {  get; set; }
        public List<string> idcollaborator { get; set; }
        public int numbermaxuser { get; set; }
        //public int numberOfCollaborators;
    }
    public class NetWorkMetaData
    {
        public string iduserprimary { get; set; }
        public int numbermaxuser { get; set; }
    }
    public class NetWorkMetaDataCollaborators
    {
        public string idcollaborator { get; set; }
    }
}
