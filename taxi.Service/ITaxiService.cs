using System;
using System.Threading.Tasks;
using taxi.Model;
namespace taxi.Service
{
	public interface ITaxiService
	{
		
		Task<TaxiRequestResult> LoginMobileAsync(string login, string password);
		Task<TaxiRequestResult> ActivateClientBySMSAsync(string phoneNumber);
		Task<string[]> GetStreetsOrPlacesAsync(string term);
		Task<WebOrderAddress> GetAddressByCoord(double lat,double lon);
		Task<WebOrderInfo> GetOrderInfoAsync(WebOrder webOrder);
		Task<MessageResult> AddOrderAsync(WebOrder webOrder);
		Task<TaxiRequestResult> CancelOrderAsync(int orderID, string phoneNumber);
	}
}
