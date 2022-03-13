namespace ECommerce.ApiItegration
{
    public interface IBaseApiClinet
    {
        Task<TResponse> GetAsync<TResponse>(string url);

        Task<List<T>> GetListAsync<T>(string url, bool requiredLogin = false);
    }
}