using app.Domain.Agregate.Entities;

namespace app.Domain.DomainService
{
    public class ClientOnboarding
    {
        private User _owner;
        private User _client;
        private ClientPortfolio _portfolio;
        public ClientOnboarding(User owner,User client,ClientPortfolio portfolio)
        {
            _owner = owner;
            _client = client;
            _portfolio = portfolio;
        }
        public ClientPortfolio Integrate()
        {
            if (_owner != null)
            {
                if (_owner.categoryCode == "Clinic" || _owner.categoryCode == "Collaborator")
                {
                    if (_client.categoryCode != "Tutor")
                        throw new Exception("Ñão é possivel adicionar esse usuario a sua rede");
                    _portfolio.add(_client.id);
                    return _portfolio;
                }
            }
            throw new Exception("Usuario sem permissão para acessar esse recurso");
        }
        public ClientPortfolio Remove()
        {
            if (_owner != null)
            {
                if (_owner.categoryCode == "Clinic" || _owner.categoryCode == "Collaborator")
                {
                    if (_client.categoryCode != "Tutor")
                        throw new Exception("Ñão é possivel Remover esse usuario a sua rede");
                    _portfolio.remove(_client.id);
                    return _portfolio;
                }
            }
            throw new Exception("Usuario sem permissão para acessar esse recurso");
        }
    }
}
