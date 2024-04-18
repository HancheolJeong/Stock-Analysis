using BusinessLayer.DTO;

namespace Stock_Analysis.Controllers
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // 특정 경로에 대한 예외 처리
            if (!context.Request.Path.StartsWithSegments("/login") && !context.Request.Path.StartsWithSegments("/public"))
            {
                var userDto = context.Session.Get<GetUserDTO>("LoginUser");
                if (userDto == null)
                {
                    // 사용자가 로그인하지 않았다면 로그인 페이지로 리디렉션
                    context.Response.Redirect("/login");
                    return; // 중요: 미들웨어 체인을 더 이상 진행하지 않음
                }
            }

            // 다음 미들웨어로 진행
            await _next(context);
        }

    }
}
