namespace gps_service.Core.HttpV2
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Routing;

    public class HttpViewAttribute : HttpMethodAttribute
    {
        public HttpViewAttribute() : base(new [] {"VIEW"})
        {
        }

        public HttpViewAttribute(string template) : base(new[] { "VIEW" }, template)
        {
        }
    }
}