using app.Domain.Agregate.Entities;

namespace app.Application.IRepository
{
    public interface IRepositoryPayment
    {
        Task save(Payment payment);
        Task<Payment> get(string id);
        Task<List<Payment>> getpayments(string iduser);
        Task update(Payment payment);
    }
}
