using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using taxi.Model;

namespace taxi.Service
{
	public interface IRestService
	{
		Task<TaxiRequestResult>  LoginAsync(string url);
		Task<bool> LogOut(string url);
		Task<T> GetAsync<T>(string url);
		Task<T> PostAsync<T>(string url, List<KeyValuePair<string,string>> pairs);
		Task<T> PostAsync<T> (string url);
		Task<Tresult> PostAsync<T,Tresult>(string url,T requestObject);
	}
}
