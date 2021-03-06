﻿using System;
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

		public async Task<TaxiRequestResult> LoginAsync(string url)
		{
			var tempCookieContainer = new CookieContainer();
			var httpClientHandler = new HttpClientHandler { UseCookies = true, CookieContainer = tempCookieContainer };
			using (var httpClient = new HttpClient(httpClientHandler))
			{


				var response = await httpClient.GetAsync(url);
				if (response.IsSuccessStatusCode)
				{

					var content = await response.Content.ReadAsStringAsync();
					var loginResult = JsonConvert.DeserializeObject<TaxiRequestResult>(content);
					loginResult.result = true;					
					authCookieContainer = tempCookieContainer;
					return loginResult;
				}
				else {
					return new TaxiRequestResult { result = false, message = "Login or password incorrect" };
				
				}

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

		public async Task<T> PostAsync<T>(string url)
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
					var response = await httpClient.PostAsync(url,null);
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


		public async Task<TResult> PostAsync<T,TResult>(string url, T requestObject)
		{
			var httpClientHandler = new HttpClientHandler();
			var cookkieContainer = authCookieContainer;
			if (authCookieContainer != null)
			{
				httpClientHandler.CookieContainer = cookkieContainer;
			}

			using (var httpClient = new HttpClient(httpClientHandler))
			{
				try
				{
					var jsonString = JsonConvert.SerializeObject(requestObject);
					var stringContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
					var response = await httpClient.PostAsync(url, stringContent);
					if (!response.IsSuccessStatusCode)
					{
						throw new RestException(response.ReasonPhrase, response.StatusCode);
					}
					var resultContent = await response.Content.ReadAsStringAsync();
					var result = JsonConvert.DeserializeObject<TResult>(resultContent);
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
