using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace CreatioAutoPackageLinkerBlazor.Data.Rest;

public class CreatioRestService: ICreatioService
{
    private CookieCollection? CookieCollection { get; set; }
    
    protected virtual async Task<RestResponse> CreatioRequest(string domain, string urlMethod, string body, CookieCollection? cookies = null)
    {
        var csrf = cookies?["BPMCSRF"]?.Value;
        var options = new RestClientOptions(domain);
        var client = new RestClient(options);
        var request = new RestRequest(urlMethod, Method.Post)
        {
            Timeout = int.MaxValue
        };
        if (csrf != null) request.AddHeader("BPMCSRF", csrf);
        request.AddHeader("Content-Type", "application/json");
        if (cookies != null)
        {
            var cookiesStr = cookies.Cast<object>().Aggregate("", (current, item) => current + item.ToString() + ";");
            request.AddHeader("Cookie", cookiesStr);
        }
        request.AddStringBody(body, DataFormat.Json);
        return await client.ExecuteAsync(request);
    }
    
    public virtual async Task CreatioLogin(string url, string userName, string userPassword)
    {
        const string urlMethod = "/ServiceModel/AuthService.svc/Login";
        var body = $"{{\"UserName\": \"{userName}\",\"UserPassword\": \"{userPassword}\"}}";
        var response = await CreatioRequest(url, urlMethod, body);
        CookieCollection = response.Cookies;
    }
    
    public virtual async Task<ListInPackages?> GetPackages(string url, string userName, string userPassword, string body = "")
    {
        var urlMethod = "0/ServiceModel/PackageService.svc/GetPackages";
        var response = await CreatioRequest(url, urlMethod, "", CookieCollection);
        if (response.StatusCode != HttpStatusCode.OK || CookieCollection == null)
        {
            response = await RestReconnection(url, body, userName, userPassword, urlMethod, true);
        }
        return response is { StatusCode: HttpStatusCode.OK, Content: { } } ? JsonConvert.DeserializeObject<ListInPackages>(response.Content) : null;
    }
    
    public virtual async Task<PackageProperties?> GetPackageProperties(string url, string body, string userName, string userPassword)
    {
        var urlMethod = "0/ServiceModel/PackageService.svc/GetPackageProperties";
        var response = await CreatioRequest(url, urlMethod, body, CookieCollection);
        if (response.StatusCode != HttpStatusCode.OK || CookieCollection == null)
        {
            response = await RestReconnection(url, body, userName, userPassword, urlMethod, true);
        }
        return response is { StatusCode: HttpStatusCode.OK, Content: { } } ? JsonConvert.DeserializeObject<PackageProperties>(response.Content) : null;
    }
    
    public virtual async Task<bool> DoesPackageHaveParentById(string url, string body, string userName, string userPassword)
    {
        var urlMethod = "0/rest/TsiPackageHierarchyService/DoesPackageHaveParentById";
        var response = await CreatioRequest(url, urlMethod, body, CookieCollection);
        if (response.StatusCode != HttpStatusCode.OK || CookieCollection == null)
        {
            response = await RestReconnection(url, body, userName, userPassword, urlMethod, true);
        }
        if (response.StatusCode != HttpStatusCode.OK || response.Content is null) return false;
        var result = JsonConvert.DeserializeObject<DoesPackageHaveParentResult>(response.Content);
        return result?.DoesPackageHaveParentByIdResult ?? false;
    }
    
    public virtual async Task<ValidateWorkspaceRoot?> ValidateWorkspace(string url, string userName, string userPassword, string body = "")
    {
        var urlMethod = "0/ServiceModel/WorkspaceExplorerService.svc/ValidateWorkspace";
        var response = await CreatioRequest(url, urlMethod, "", CookieCollection);
        if (response.StatusCode != HttpStatusCode.OK || CookieCollection == null)
        {
            response = await RestReconnection(url, body, userName, userPassword, urlMethod, true);
            
        }
        return response is { StatusCode: HttpStatusCode.OK, Content: { } } ? JsonConvert.DeserializeObject<ValidateWorkspaceRoot>(response.Content) : null;
    }
    
    public virtual async Task<ResponseSavePackageProperties?> SavePackageProperties(string url, string body, string userName, string userPassword)
    {
        var urlMethod = "0/ServiceModel/PackageService.svc/SavePackageProperties";
        var response = await CreatioRequest(url, urlMethod, body, CookieCollection);
        if (response.StatusCode != HttpStatusCode.OK || CookieCollection == null)
        {
            response = await RestReconnection(url, body, userName, userPassword, urlMethod, false);
        }
        ResponseSavePackageProperties? result = new();
        if (response is { StatusCode: HttpStatusCode.OK, Content: not null })
        {
            result = JsonConvert.DeserializeObject<ResponseSavePackageProperties>(response.Content);
            if (result != null) result.StatesCode = (int)response.StatusCode;
            return result;
        }
        result.StatesCode = (int)response.StatusCode;
        return result;
    }

    protected virtual async Task<RestResponse> RestReconnection(string url, string body, string userName, string userPassword, string urlMethod, bool getData)
    {
        RestResponse response;
        var counter = 100;
        do
        {
            response = await CreatioRequest(url, urlMethod, body, CookieCollection);
            if (getData)
            {
                switch ((int)response.StatusCode)
                {
                    case (int)HttpStatusCode.OK :
                        break;
                    case (int)HttpStatusCode.Unauthorized when counter > 1: 
                        await CreatioLogin(url, userName, userPassword);
                        break;
                    case (int)HttpStatusCode.Unauthorized when counter == 1:
                        throw new Exception("Ошибка авторизации!");
                    case 0 when counter == 1:
                        throw new Exception("Ошибка " +  response.StatusCode);
                    case 0:
                        await Task.Delay(TimeSpan.FromMinutes(2));
                        break;
                    default:
                        throw new Exception("Ошибка " + response.StatusCode);
                }
            }
            else
            {
                switch ((int)response.StatusCode)
                {
                    case (int)HttpStatusCode.OK :
                        break;
                    case (int)HttpStatusCode.Unauthorized when counter > 1: 
                        await CreatioLogin(url, userName, userPassword);
                        break;
                    case (int)HttpStatusCode.Unauthorized when counter == 1:
                        throw new Exception("Ошибка авторизации!");
                }
            }
            counter--;
        } while (response.StatusCode != HttpStatusCode.OK && counter != 0);

        return response;
    }
}