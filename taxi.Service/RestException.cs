using System;
using System.Net;

namespace taxi.Service
{
	
		public class RestException : Exception
		{
			public RestException(string message, HttpStatusCode statusCode): base(message)
			{
				HttpStatusCode = statusCode;
			}
			public HttpStatusCode HttpStatusCode { get;private set; }
		}
}

