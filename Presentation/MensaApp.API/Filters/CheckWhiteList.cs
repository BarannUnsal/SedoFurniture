using MensaApp.API.Middlewares;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace MensaApp.API.Filters
{
    public class CheckWhiteList : ActionFilterAttribute
    {
        readonly IPList _ipList;

        public CheckWhiteList(IOptions<IPList> ipList)
        {
            _ipList = ipList.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestIPAdress = context.HttpContext.Connection.RemoteIpAddress;

            var isWhiteList = this._ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(requestIPAdress)).Any();

            if (!isWhiteList)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
