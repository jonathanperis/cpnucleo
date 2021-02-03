// using Cpnucleo.RazorPages.Services.Interfaces;
// using System.Net;
// using System.Net.Http;
// using System.Text;
// using System.Text.Json;
// using System.Threading.Tasks;

// namespace TSystems.BR.TEntrego.Web.Admin.Services
// {
//     public class HttpService : IHttpService
//     {
//         private readonly HttpClient _httpClient;

//         public HttpService(HttpClient httpClient)
//         {
//             _httpClient = httpClient;
//         }

//         public async Task<(T response, bool sucess, HttpStatusCode code, string message)> Get<T>(string uri)
//         {
//             var request = new HttpRequestMessage(HttpMethod.Get, uri);
//             return await SendRequest<T>(request);
//         }

//         public async Task<(T response, bool sucess, HttpStatusCode code, string message)> Post<T>(string uri, object value)
//         {
//             var request = new HttpRequestMessage(HttpMethod.Post, uri);
//             request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
//             return await SendRequest<T>(request);
//         }

//         public async Task<(T response, bool sucess, HttpStatusCode code, string message)> Put<T>(string uri, object value)
//         {
//             var request = new HttpRequestMessage(HttpMethod.Put, uri);
//             request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
//             return await SendRequest<T>(request);
//         }

//         private async Task<(T response, bool sucess, HttpStatusCode code, string message)> SendRequest<T>(HttpRequestMessage request)
//         {
//             //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "user.Token");

//             using var response = await _httpClient.SendAsync(request);

//             if (response.StatusCode == HttpStatusCode.Unauthorized)
//                 return (default, false, response.StatusCode, string.Empty);

//             if (!response.IsSuccessStatusCode)
//                 return (default, false, response.StatusCode, await response.Content.ReadAsStringAsync());

//             if (response.StatusCode == HttpStatusCode.NoContent)
//                 return (default, true, response.StatusCode, string.Empty);

//             return (await response.Content.ReadFromJsonAsync<T>(), true, response.StatusCode, string.Empty);
//         }
//     }
// }