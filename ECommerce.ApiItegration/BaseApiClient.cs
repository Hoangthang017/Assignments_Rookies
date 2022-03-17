using ECommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ECommerce.ApiItegration;

public class BaseApiClient : IBaseApiClinet
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BaseApiClient(
                IHttpClientFactory httpClientFactory,
                IConfiguration configuration,
                IHttpContextAccessor httpContextAccessor
        )
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> GetAsync<TResponse>(string url, bool isAuthenticate = false)
    {
        // create clinet
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(SystemConstants.AppSettings.BackendApiAddress);

        // check has authenticate
        if (isAuthenticate)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
        }

        // method get asynce
        var response = await client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();

        // handle response
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

    public async Task<ReponseType> PostAsync<ReponseType, RequestType>(string url, RequestType values, bool isAuthenticate = false)
    {
        var content = JsonConvert.SerializeObject(values);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(SystemConstants.AppSettings.BackendApiAddress);

        // check has authenticate
        if (isAuthenticate)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString("token");
            if (!string.IsNullOrEmpty(sessions))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
        }

        var repsone = await client.PostAsync(url, httpContent);
        var body = await repsone.Content.ReadAsStringAsync();
        if (repsone.StatusCode == HttpStatusCode.Created && body.Contains("value"))
        {
            Dictionary<string, object> data = (Dictionary<string, object>)JsonConvert.DeserializeObject(body,
                typeof(Dictionary<string, object>));
            body = data["value"].ToString();
        }
        if (repsone.IsSuccessStatusCode)
        {
            ReponseType myDeserializedObjList = (ReponseType)JsonConvert.DeserializeObject(body,
                typeof(ReponseType));
            return myDeserializedObjList;
        };
        return (ReponseType)Activator.CreateInstance(typeof(ReponseType));
    }

    public async Task<bool> PostAsync(string url, Dictionary<string, object> values, bool isAuthenticate = false)
    {
        var content = JsonConvert.SerializeObject(values);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(SystemConstants.AppSettings.BackendApiAddress);

        // check has authenticate
        if (isAuthenticate)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
        }

        var repsone = await client.PostAsync(url, httpContent);
        if (repsone.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }
}