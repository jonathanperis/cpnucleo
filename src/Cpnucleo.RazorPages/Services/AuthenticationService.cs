// using Microsoft.AspNetCore.Components;
// using System.Collections.Generic;
// using System.Net;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using TSystems.BR.TEntrego.Web.Admin.Models;
// using TSystems.BR.TEntrego.Web.Admin.Services.Interfaces;

// namespace TSystems.BR.TEntrego.Web.Admin.Services
// {
//     public class AuthenticationService : IAuthenticationService
//     {
//         private readonly IHttpService _httpService;

//         public AuthenticationService(IHttpService httpService)
//         {
//             _httpService = httpService;
//         }

//         public async Task<(SessionViewModel response, bool sucess, HttpStatusCode code, string message)> Login(string username, string password)
//         {
//             return await _httpService.Post<SessionViewModel>($"User/Auth", new { Username = username, Password = password });
//         }
//     }
// }