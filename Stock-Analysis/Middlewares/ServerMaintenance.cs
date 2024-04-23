using Microsoft.AspNetCore.Http;

namespace Stock_Analysis.Middlewares
{
    public class ServerMaintenance
    {
        private RequestDelegate _next;

        public ServerMaintenance(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 서버점검 시간에 해당된다면 점검페이지로 Redirect합니다.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            DateTime dt = DateTime.Now;
            int hour = dt.Hour;
            int minute = dt.Minute;

            if (hour == 0 && minute <= 10 && !context.Request.Path.StartsWithSegments("/maintenance"))
            {
                // '/maintenance' 페이지로 리디렉트
                context.Response.Redirect("/maintenance");
                return;
            }

            await _next(context);
        }
    }
}