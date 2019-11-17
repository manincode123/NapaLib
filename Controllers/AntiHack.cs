using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NAPASTUDENT.Controllers
{
    namespace AntiHack
    {
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class ApiValidateAntiForgeryToken : FilterAttribute, IAuthorizationFilter
        {
            public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
            {
                var headerToken = actionContext
                    .Request
                    .Headers
                    .GetValues("__RequestVerificationToken")
                    .FirstOrDefault(); ;

                var cookie = actionContext
                    .Request
                    .Headers
                    .GetCookies()
                    .Select(c => c[AntiForgeryConfig.CookieName])
                    .FirstOrDefault() ;

                try
                {
                    AntiForgery.Validate(cookie != null ? cookie.Value : null, headerToken);
                }
                catch
                {
                    actionContext.Response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        RequestMessage = actionContext.ControllerContext.Request
                    };
                    return FromResult(actionContext.Response);
                }


                return continuation();
            }

            private Task<HttpResponseMessage> FromResult(HttpResponseMessage result)
            {
                var source = new TaskCompletionSource<HttpResponseMessage>();
                source.SetResult(result);
                return source.Task;
            }
        }

}
}