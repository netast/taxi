using System;
using System.Threading.Tasks;
using taxi.Model;
namespace taxi.Service
{
	public interface ITaxiService
	{
		
		Task<bool> LoginMobileAsync(string login, string password);
		Task<TaxiRequestResult> ActivateClientBySMSAsync(string phoneNumber);
		Task<string[]> GetStreetsOrPlacesAsync(string term);
		Task<WebOrder> GetOrderInfoAsync(string streetOrPlace, 
		                                 string house, 
		                                 string[] dstStreetOrPlaces, 
		                                 string[] dstHouses, 
		                                 int classID, 
		                                 bool isBarter, 
		                                 bool specifiedDateTime, 
		                                 string dateTime);
		Task<TaxiRequestResult> AddOrderAsync(string sourceStreetOrPlace, 
		                                string sourceHouse, 
		                                string[] interimAddress, 
		                                string[] interimHomeNumber, 
		                                string comments, 
		                                bool isBarter, 
		                                string pinCode, 
		                                bool specifiedDateTime, 
		                                string dateTime, 
		                                int classID);
		Task<TaxiRequestResult> CancelOrderAsync(int orderID, string phoneNumber);
	}
}
