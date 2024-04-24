using BusinessLayer.DTO;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Stock_Analysis.Controllers
{
    public class LoginController : Controller
    {

        private ILoginService loginService;
        private readonly ILogger<LoginController> _logger;
        public LoginController(ILoginService service, ILogger<LoginController> logger)
        {
            loginService = service;
            _logger = logger;
        }


        /// <summary>
        /// GET /login/login
        /// 구글 로그인 서비스를 연결하는 요청
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse", new { ReturnUrl = returnUrl })
            };
            return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// GET/ login/logout
        /// 로그아웃 세션 삭제 요청
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("LoginUser");
            return Redirect("/");
        }

        /// <summary>
        /// 세션을 생성
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return Redirect("/");
            }

            GetUserDTO dto = new GetUserDTO();
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            dto.email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            dto.name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            (var res, string? err) = await loginService.Login(dto);
            if (err != null)
            {
                _logger.LogError(err);
                return Redirect("/Exception");
            }
            if (res) // 로그인 성공
            {
                HttpContext.Session.Set("LoginUser", dto);
            }

            return Redirect("/");
        }


    }
}
