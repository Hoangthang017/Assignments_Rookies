using ECommerce.Utilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ECommerce.ApiItegration;

public class BaseApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public BaseApiClient(IHttpClientFactory httpClientFactory,
                IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TResponse> GetAsync<TResponse>(string url)
    {
        //var sessions = _httpContextAccessor
        //    .HttpContext
        //    .Session
        //    .GetString(SystemConstants.AppSettings.Token);

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(SystemConstants.AppSettings.BackendApiAddress);
        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
        var response = await client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            TResponse myDeserializedObjList = (TResponse)JsonConvert.DeserializeObject(body,
                typeof(TResponse));

            return myDeserializedObjList;
        }
        return JsonConvert.DeserializeObject<TResponse>(body);
    }

    public async Task<List<T>> GetListAsync<T>(string url, bool requiredLogin = false)
    {
        //var sessions = _httpContextAccessor
        //   .HttpContext
        //   .Session
        //   .GetString(SystemConstants.AppSettings.Token);
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(SystemConstants.AppSettings.BackendApiAddress);
        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

        var response = await client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var data = (List<T>)JsonConvert.DeserializeObject(body, typeof(List<T>));
            return data;
        }
        throw new Exception(body);
    }
}