using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Payment;
using app.Infra.Repository;

namespace app.Application.UseCase
{
    public class CreatePayment
    {
        private IRepositoryUserClinic _repouser;
        private IRepositoryPayment _repopay;
        private IRepositoryOrderService _repoorder;
        public async Task execute(PaymentDTO payment,string iduser)
        {
            Payment pay = Payment.create(payment,iduser);
            User user = await _repouser.getUserBase(iduser);
            OrderService os = await _repoorder.get(payment.idos);
            if (user.categoryCode == "Tutor")
                throw new Exception("Usuario sem permissão a esse recurso");
            if (os == null)
                throw new Exception("Os não existente");
            await _repopay.save(pay);
            return;
        }
    }
}
