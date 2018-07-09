using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace TourManagement.API.Helpers
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class RequestHeaderMatchedMediaTypeAttribute : Attribute,IActionConstraint
    {
        private readonly string _requesteHeaderToMatch;
        private readonly string[] _mediaTypes;

        public RequestHeaderMatchedMediaTypeAttribute(string requesteHeaderToMatch, string[] mediaTypes)
        {
            _requesteHeaderToMatch = requesteHeaderToMatch;
            _mediaTypes = mediaTypes;
        }
        public bool Accept(ActionConstraintContext context)
        {
            var requesteHeaders = context.RouteContext.HttpContext.Request.Headers;

            if (!requesteHeaders.ContainsKey(_requesteHeaderToMatch))
            {
                return false;
            }

            // If one of the media types matches, return true. 
            foreach (var mediaType in _mediaTypes)
            {
                var headerValues = requesteHeaders[_requesteHeaderToMatch]
                    .ToString().Split(',').ToList();

                foreach (var headerValue in headerValues)
                {
                    if (string.Equals(headerValue, mediaType, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int Order => 0;
    }
}
