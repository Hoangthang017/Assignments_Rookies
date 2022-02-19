namespace ECommerce.DataAccess.Respository.Common
{
    public interface IUnitOfWork
    {
        Task Save();
    }
}