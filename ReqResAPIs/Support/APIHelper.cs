using RestSharp;

namespace ReqResAPIs.Support;
public class APIHelper<T> where T : class
{
    private string _baseUrl;

    public APIHelper(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public RestClient SetUrl(string endpoint)
    {
        var url = Path.Combine(_baseUrl, endpoint);
        var restClient = new RestClient(url);

        return restClient;
    }

    public RestRequest CreatePostRequest(object payload)
    {
        var restRequest = new RestRequest(string.Empty, Method.Post);
        restRequest.AddHeader("Accept", "application/json");
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(payload);

        return restRequest;
    }

    public RestRequest CreatePutRequest(object payload)
    {
        var restRequest = new RestRequest(string.Empty, Method.Put);
        restRequest.AddHeader("Accept", "application/json");
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(payload);

        return restRequest;
    }

    public RestRequest CreateGetRequest()
    {
        var restRequest = new RestRequest(string.Empty, Method.Get);
        restRequest.AddHeader("Accept", "application/json");

        return restRequest;
    }

    public RestRequest CreatePatchRequest(object payload)
    {
        var restRequest = new RestRequest(string.Empty, Method.Patch);
        restRequest.AddHeader("Accept", "application/json");
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(payload);

        return restRequest;
    }

    public RestResponse GetResponse(string endpoint, RestRequest request)
    {
        var client = SetUrl(endpoint);
        return client.Execute(request);
    }

    public RestResponse GetMethod(string endpoint)
    {
        var request = CreateGetRequest();
        var response = GetResponse(endpoint, request);

        return response;
    }

    public RestResponse PostMethod(string endpoint, object content)
    {
        var request = CreatePostRequest(content);
        var response = GetResponse(endpoint, request);

        return response;
    }

    public RestResponse PutMethod(string endpoint, object content)
    {
        var request = CreatePutRequest(content);
        var response = GetResponse(endpoint, request);

        return response;
    }
    public RestResponse PatchMethod(string endpoint, object content)
    {
        var request = CreatePatchRequest(content);
        var response = GetResponse(endpoint, request);

        return response;
    }
}
