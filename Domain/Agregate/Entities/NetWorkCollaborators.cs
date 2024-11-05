using app.Domain.DTO.NetWorkCollaborator;
using System.Xml;

namespace app.Domain.Agregate.Entities
{
    public class NetWorkCollaborators
    {
        private string _idNetWorkCollaborators;
        private string _idUserPrimary;
        private List<string> _idCollaborators;
        private int _numberMaxOfCollaborators;
        private int _numberOfCollaborators;
        public string iduserprimary { get { return _idUserPrimary; } }
        public int numbermaxcollaborator { get { return _numberMaxOfCollaborators;} }
        public List<string> idcollaborator { get { return _idCollaborators;} }
        public int numberofcollaborators { get { return _numberMaxOfCollaborators; } }
        private NetWorkCollaborators() { }
        public static NetWorkCollaborators create(string iduserprimary,string categoryuser)
        {
            NetWorkCollaborators network = new NetWorkCollaborators();
            if (categoryuser != "Clinic")
                throw new Exception("this user not permission for create a network");
            network._idUserPrimary = iduserprimary==null?throw new ArgumentNullException("Id user invalid"):iduserprimary;
            network._idCollaborators = new List<string>();
            network._idCollaborators.Add(iduserprimary);
            network._numberMaxOfCollaborators = 15;
            network._numberOfCollaborators = 1;
            return network;
        }
        public static NetWorkCollaborators restore(NetWorkDbDTO net)
        {
            NetWorkCollaborators network = new NetWorkCollaborators();
            network._idUserPrimary = net.iduserprimary;
            network._idCollaborators = net.idcollaborator;
            //network._idNetWorkCollaborators = net.idNetWorkCollaborators;
            network._numberOfCollaborators = net.idcollaborator.Count();
            network._numberMaxOfCollaborators = net.numbermaxuser;
            return network;
        }
        public bool conttainsCollaborator(string id)
        {
            bool conttains = _idCollaborators.Contains(id);
            return conttains;
        }
        public void addCollaborator(string idCollaborator,string categoryUser)
        {
            if(!_idCollaborators.Contains(idCollaborator) && categoryUser.ToString() == "Collaborator")
            {
                if (_numberMaxOfCollaborators == _idCollaborators.Count())
                    throw new Exception("Number max of users");
                _idCollaborators.Add(idCollaborator);
                _numberOfCollaborators = _idCollaborators.Count();
                return;
            }
            throw new Exception("Not allowed to add this user to the network");
        }
        public void removeCollaborator(string idCollaborator)
        {
            if (!conttainsCollaborator(idCollaborator))
            {
                throw new Exception("User not found on the network");
            }
            _idCollaborators.Remove(idCollaborator);
            _numberOfCollaborators = _idCollaborators.Count();
        }
        public void setNetWorkToPremium()
        {
            _numberMaxOfCollaborators = 30;
        }
        public void setNetWorkToBase()
        {
            _numberMaxOfCollaborators = 15;
        }
    }
}
