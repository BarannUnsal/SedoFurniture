using Microsoft.Extensions.Options;
using System.Net;

namespace MensaApp.API.Middlewares
{
    public class IPSafeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPList _ipList;

        public IPSafeMiddleware(RequestDelegate next, IOptions<IPList> ipList)
        {
            this._next = next;

            this._ipList = ipList.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestIpAdress = context.Connection.RemoteIpAddress;

            var isWhiteList = _ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(requestIpAdress)).Any();

            if (!isWhiteList)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next(context);

            return;
        }
    }
}
