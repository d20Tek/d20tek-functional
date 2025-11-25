using D20Tek.Functional;
using System.Net.Http.Json;

namespace Games.Common;

internal class WebApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Result<T>> Fetch<T>(string url) where T : notnull
    {
        try
        {
            var response = await this._httpClient.GetAsync(url);
            return response.IsSuccessStatusCode
                ? Result<T>.Success(await ReadJson<T>(response))
                : Result<T>.Failure(Error.Invalid("Fetch.Error", response.ReasonPhrase!));
        }
        catch (Exception e)
        {
            return Result<T>.Failure(e);
        }
    }

    private static async Task<T> ReadJson<T>(HttpResponseMessage response)
        where T : notnull =>
        (await response.Content.ReadFromJsonAsync<T>())!;
}
