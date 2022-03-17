namespace ECommerce.ApiItegration
{
    public interface IBaseApiClinet
    {
        Task<TResponse> GetAsync<TResponse>(string url, bool isAuthenticate = false);

        Task<List<T>> GetListAsync<T>(string url, bool requiredLogin = false);

        Task<ReponseType> PostAsync<ReponseType, RequestType>(string url, RequestType values, bool isAuthenticate = false);

        Task<bool> PostAsync(string url, Dictionary<string, object> values, bool isAuthenticate = false);
    }
}