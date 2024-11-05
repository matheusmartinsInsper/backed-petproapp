using System.ComponentModel;

namespace app.Domain.Agregate.Entities
{
    public class ClientPortfolio
    {
        private string _idowner;
        private List<Client> _idclients;
        public string idowner { get { return _idowner; } }
        public List<Client> clients { get { return _idclients; } }  
        private ClientPortfolio() { }
        public static ClientPortfolio create(string id)
        {
            ClientPortfolio clientPortfolio = new ClientPortfolio();
            clientPortfolio._idowner = id;
            return clientPortfolio;
        }
        public static ClientPortfolio restore(List<Client> idclients,string idowner)
        {
            ClientPortfolio clientPortfolio= new ClientPortfolio();
            clientPortfolio._idclients = idclients;
            clientPortfolio._idowner = idowner;
            return clientPortfolio;
        }
        public void add(string idclient)
        {
            if (!containsClient(idclient))
            {
                Client client = new Client();
                client.idclient = idclient;
                client.dateadd = DateTime.Now;
                _idclients.Add(client);
                return;
            }
            throw new Exception("Cliente ja está na sua carteira de clientes");
        }
        public void remove(string idclient)
        {
            if (containsClient(idclient))
            {
                Client client = _idclients.Find(x => x.idclient == idclient);
                _idclients.Remove(client);
                return;
            }
            throw new Exception("Cliente não registrado em sua carteira");
        }
        public bool containsClient(string idclient)
        {
            bool value = _idclients.Where(x => x.idclient == idclient).Count() != 0;
            return value;
        }
    }
    public class Client
    {
        public string idclient {  get; set; }
        public DateTime dateadd { get; set; }
    }
}
