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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

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
        client = AddTokenToClient(client, isAuthenticate);

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

    public async Task<List<T>> GetListAsync<T>(string url, bool requiredLogin = false, bool isAuthenticate = false)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(SystemConstants.AppSettings.BackendApiAddress);

        // check has authenticate
        client = AddTokenToClient(client, isAuthenticate);

        var response = await client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var data = (List<T>)JsonConvert.DeserializeObject(body, typeof(List<T>));
            return data;
        }
        throw new ECommerceException(body);
    }

    public async Task<ReponseType> PostAsync<ReponseType, RequestType>(string url, RequestType values, bool isAuthenticate = false)
    {
        var content = JsonConvert.SerializeObject(values);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(SystemConstants.AppSettings.BackendApiAddress);

        // check has authenticate
        client = AddTokenToClient(client, isAuthenticate);

        var response = await client.PostAsync(url, httpContent);

        var body = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            _httpContextAccessor.HttpContext.Session.SetString(SystemConstants.AppSettings.ErrorResponseSessionKey, body);
            return (ReponseType)Activator.CreateInstance(typeof(ReponseType));
        }

        if (response.StatusCode == HttpStatusCode.Created && body.Contains("value"))
        {
            Dictionary<string, object> data = (Dictionary<string, object>)JsonConvert.DeserializeObject(body,
                typeof(Dictionary<string, object>));
            body = data["value"].ToString();
        }
        if (response.IsSuccessStatusCode)
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
        client = AddTokenToClient(client, isAuthenticate);

        var repsone = await client.PostAsync(url, httpContent);
        if (repsone.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }

    public async Task<ReponseType> PatchAsync<ReponseType, RequestType>(string url, RequestType values, bool isAuthenticate = false)
    {
        var content = JsonConvert.SerializeObject(values);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(SystemConstants.AppSettings.BackendApiAddress);

        // check has authenticate
        client = AddTokenToClient(client, isAuthenticate);

        var response = await client.PatchAsync(url, httpContent);

        var body = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            _httpContextAccessor.HttpContext.Session.SetString(SystemConstants.AppSettings.ErrorResponseSessionKey, body);
            return (ReponseType)Activator.CreateInstance(typeof(ReponseType));
        }

        if (response.IsSuccessStatusCode)
        {
            ReponseType myDeserializedObjList = (ReponseType)JsonConvert.DeserializeObject(body,
                typeof(ReponseType));
            return myDeserializedObjList;
        };
        return (ReponseType)Activator.CreateInstance(typeof(ReponseType));
    }

    private HttpClient AddTokenToClient(HttpClient client, bool isAuthenticate)
    {
        // check has authenticate
        if (isAuthenticate)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
        }
        return client;
    }
}