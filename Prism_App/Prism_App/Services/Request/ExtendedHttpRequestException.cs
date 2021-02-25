using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Prism_App.Services.Request
{
    class ExtendedHttpRequestException : HttpRequestException
    {
        public System.Net.HttpStatusCode HttpCode { get; }
        public ExtendedHttpRequestException(System.Net.HttpStatusCode code) : this(code, null, null)
        {
        }

        public ExtendedHttpRequestException(System.Net.HttpStatusCode code, string message) : this(code, message, null)
        {
        }

        public ExtendedHttpRequestException(System.Net.HttpStatusCode code, string message, Exception inner) : base(message,
            inner)
        {
            HttpCode = code;
        }
    }
}
