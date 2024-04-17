using BusinessLayer.DTO;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Analysis.Controllers
{
    public class LoginController : Controller
    {

        private ILoginService loginService;
        public LoginController(ILoginService service)
        {
            loginService = service;
        }
        public IActionResult NewUser()
        {
            return View();
        }

        public async Task<IActionResult> GetAllUser()
        {
            return View(await loginService.GetAllUser());
        }

        public IActionResult SearchUserId()
        {
            return View();
        }

        public async Task<IActionResult> GetUser(GetUserDTO getUserDTO)
        {
            if(ModelState.IsValid)
            {
                //return Redirect("/home/index");
            }
            else
            {
                return Redirect("/login/SearchUserId");
            }
            GetUserResponseDTO dto = await loginService.GetUser(getUserDTO);
            HttpContext.Session.Set("LoginUser", dto);
            return Redirect("/home/index");
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO createUserDTO)
        {
            await loginService.CreateUser(createUserDTO);
            return Redirect("/home/index");
        }

    }
}
