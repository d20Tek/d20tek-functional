using D20Tek.Minimal.Functional;
using System.Net.Http.Json;

namespace Games.Common;

internal class WebApiClient
{
    private readonly HttpClient _httpClient;

    public WebApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Maybe<T>> Fetch<T>(string url)
    {
        try
        {
            var response = await this._httpClient.GetAsync(url);
            return response.IsSuccessStatusCode
                ? (await response.Content.ReadFromJsonAsync<T>()).ToMaybeIfNull()
                : new Nothing<T>();
        }
        catch (Exception e)
        {
            return new Error<T>(e);
        }
    }
}
