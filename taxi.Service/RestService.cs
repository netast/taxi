using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using taxi.Model;

namespace taxi.Service
{
	public class RestService : IRestService
	{
		private CookieContainer authCookieContainer = new CookieContainer();

		public async Task<LoginResult> LoginAsync(string url)
		{
			var tempCookieContainer = new CookieContainer();
			var httpClientHandler = new HttpClientHandler { UseCookies = true, CookieContainer = tempCookieContainer };
			using (var httpClient = new HttpClient(httpClientHandler))
			{


				var response = await httpClient.GetAsync(url);

				var content = await response.Content.ReadAsStringAsync();
				var loginResult = JsonConvert.DeserializeObject<LoginResult>(content);
				if (loginResult.result)
				{

					authCookieContainer = tempCookieContainer;
				}
				return loginResult;
			}


		}

		public async Task<bool> LogOut(string url)
		{
			var result = await GetAsync<bool>(url);
			if(result){
				authCookieContainer = null;
				return true;
 			}
			return false;
		}

		public async Task<T> PostAsync<T>(string url, List<KeyValuePair<string, string>> pairs)
		{
			var httpClientHandler = new HttpClientHandler();
			var cookkieContainer = authCookieContainer;
			if(authCookieContainer != null)
			{
			httpClientHandler.CookieContainer = cookkieContainer;
			}

			using (var httpClient = new HttpClient(httpClientHandler))
			{
				try
				{
					var content = new FormUrlEncodedContent(pairs);
					var response = await httpClient.PostAsync(url, content);
					if (!response.IsSuccessStatusCode)
					{
						throw new RestException(response.ReasonPhrase, response.StatusCode);
					}
					var resultContent = await response.Content.ReadAsStringAsync();
					var result = JsonConvert.DeserializeObject<T>(resultContent);
					return result;
				}

				catch (Exception ex)
				{
					var restException = ex as RestException;
					var statusCode = restException != null ? restException.HttpStatusCode : HttpStatusCode.NoContent;

					throw new RestException(ex.Message, statusCode);
				}

			}
		}

		public async Task<T> GetAsync<T>(string url)
		{
			var httpClientHandler = new HttpClientHandler();
			if(authCookieContainer != null){
				httpClientHandler.CookieContainer = authCookieContainer;
			}
			using (var httpClient = new HttpClient(httpClientHandler))
			{
				try
				{
					var response = await httpClient.GetAsync(url);
					if (!response.IsSuccessStatusCode)
					{
						throw new RestException(response.ReasonPhrase, response.StatusCode);
					}
					var content = await response.Content.ReadAsStringAsync();
					var result = JsonConvert.DeserializeObject<T>(content);
					return result;
				}

				catch (Exception ex)
				{
					var restException = ex as RestException;
					var statusCode = restException != null ? restException.HttpStatusCode : HttpStatusCode.NoContent;

					throw new RestException(ex.Message, statusCode);
				}

			}
		}
	}
}
