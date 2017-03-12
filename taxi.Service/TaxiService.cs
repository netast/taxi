using System;
using System.Threading.Tasks;
using taxi.Model;
using System.Collections.Generic;
using System.Globalization;

namespace taxi.Service
{
	public class TaxiService : ITaxiService
	{
		IRestService _restService;
		const string baseUrl= "http://client.taxi-omsk.com:81/";

		public TaxiService(IRestService restService)
		{
			_restService = restService;
		}

		public async Task<TaxiRequestResult> ActivateClientBySMSAsync(string phoneNumber)
		{
			var url = baseUrl+"api/account/activateClientBySMS?phoneNumber="+phoneNumber;
			var result = await _restService.GetAsync<MessageResult>(url);
			return new TaxiRequestResult{ result = true, message = result.Message};
		}

		public Task<TaxiRequestResult> AddOrderAsync(string sourceStreetOrPlace, string sourceHouse, string[] interimAddress, string[] interimHomeNumber, string comments, bool isBarter, string pinCode, bool specifiedDateTime, string dateTime, int classID)
		{
			throw new NotImplementedException();
		}

		public Task<TaxiRequestResult> CancelOrderAsync(int orderID, string phoneNumber)
		{
			throw new NotImplementedException();
		}

		public async Task<WebOrderInfo> GetOrderInfoAsync(WebOrder webOrder)
		{
			var url = baseUrl + "api/webOrders/getOrderInfo";
			var result = await _restService.PostAsync<WebOrder,WebOrderInfo>(url,webOrder);
			return result;

		}

		public async Task<string[]> GetStreetsOrPlacesAsync(string term)
		{
			var url = baseUrl + "api/navigation/getStreetsOrPlaces)?term="+term;
			var result = await _restService.GetAsync<string[]>(url);
			return result;
		}

		public async Task<TaxiRequestResult> LoginMobileAsync(string phoneNumber, string password)
		{
			var url = baseUrl + $"api/account/login?phoneNumber={phoneNumber}&password={password}";

			try
			{

				var loginResult = await _restService.LoginAsync(url);

				return loginResult;
			}
			catch (Exception ex) {

				var faultResult = new TaxiRequestResult();

				faultResult.result = false;
				faultResult.message = ex.Message;

				return faultResult;
			}
		}


		public async Task<WebOrderAddress> GetAddressByCoord(double lat,double lon)
		{
			var url = baseUrl + $"api/navigation/getAddressByCoordinates?lat={lat.ToString("G",CultureInfo.InvariantCulture)}&lon={lon.ToString("G",CultureInfo.InvariantCulture)}";

			try
			{
				var webAddress = await _restService.GetAsync<WebOrderAddress>(url);
				return webAddress;
				
			}catch (Exception ex)
			{
				return null;
			}
		}
	}
}
