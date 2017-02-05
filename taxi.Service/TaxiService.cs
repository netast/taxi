using System;
using System.Threading.Tasks;
using taxi.Model;
using System.Collections.Generic;

namespace taxi.Service
{
	public class TaxiService : ITaxiService
	{
		IRestService _restService;
		const string baseUrl= "http://client.taxi-omsk.com/";

		public TaxiService(IRestService restService)
		{
			_restService = restService;
		}

		public async Task<TaxiRequestResult> ActivateClientBySMSAsync(string phoneNumber)
		{
			var url = baseUrl+"Login/ActivateClientBySMSAsync?"+phoneNumber;
			var result = await _restService.GetAsync<TaxiRequestResult>(url);
			return result;
		}

		public Task<TaxiRequestResult> AddOrderAsync(string sourceStreetOrPlace, string sourceHouse, string[] interimAddress, string[] interimHomeNumber, string comments, bool isBarter, string pinCode, bool specifiedDateTime, string dateTime, int classID)
		{
			throw new NotImplementedException();
		}

		public Task<TaxiRequestResult> CancelOrderAsync(int orderID, string phoneNumber)
		{
			throw new NotImplementedException();
		}

		public Task<WebOrder> GetOrderInfoAsync(string streetOrPlace, string house, string[] dstStreetOrPlaces, string[] dstHouses, int classID, bool isBarter, bool specifiedDateTime, string dateTime)
		{
			throw new NotImplementedException();
		}

		public Task<string[]> GetStreetsOrPlacesAsync(string term)
		{
			var url = baseUrl + "Order/GetStreetsOrPlaces?"+term;
			var result = _restService.GetAsync<string[]>(url);
			return result;
		}

		public async Task<bool> LoginMobileAsync(string phoneNumber, string password)
		{
			var url = baseUrl + "Login/LoginMobile";

			var requestParams = new List<KeyValuePair<string,string>>();
			requestParams.Add(new KeyValuePair<string, string>(nameof(phoneNumber),phoneNumber));
			requestParams.Add(new KeyValuePair<string, string>(nameof(password),password));

			var result =  await _restService.PostAsync<TaxiRequestResult>(url,requestParams);

			return result != null && result.result;

		}
	}
}
