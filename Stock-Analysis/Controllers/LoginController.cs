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
        public LoginController(ILoginService service)
        {
            loginService = service;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NewUser()
        {
            return View();
        }

        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse", new { ReturnUrl = returnUrl })
            };
            return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return Redirect("/login"); // 인증 실패 시 로그인 페이지로 리디렉션
            }

            GetUserDTO dto = new GetUserDTO();
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            dto.email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            dto.name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var res = await loginService.Login(dto);
            if(res) // 로그인 성공
            {
                HttpContext.Session.Set("LoginUser", dto);
            }

            return Redirect("/login/SearchUserId");
        }


        public IActionResult SearchUserId()
        {
            return View();
        }

        public IActionResult AuthenticateSession()
        {
            return View();
        }

        public IActionResult Auth()
        {
            // 세션에서 사용자 정보 가져오기
            GetUserDTO? userDto = HttpContext.Session.Get<GetUserDTO>("LoginUser");

            if (userDto != null)
            {
                // 사용자 정보가 세션에 있으면, 이를 사용하여 로직 처리
                return View(userDto); // 예를 들어, 사용자 정보를 뷰로 전달
            }
            else
            {
                // 사용자 정보가 세션에 없으면 로그인 페이지로 리디렉션
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
