using System;
using System.Collections.Generic;

namespace taxi.Model
{
	public class WebOrder
	{
		public string PhoneNumber { get; set; }

		// Fields expected from Client

		public WebOrderAddress SrcAddress { get; set; }

		public List<WebOrderAddress> DstAddresses { get; set; }

		public string Comments { get; set; }

		public bool SpecifiedDateTime { get; set; }

		public DateTime? OrderDateTime { get; set; }

		public int OrderClassID { get; set; }

		public bool IsBarter { get; set; }

		public string PinCode { get; set; }


		// Fields populated by the Taxi system

		public int OrderID { get; set; }

		public string Title { get; set; }

		public DateTime CallDateTime { get; set; }

		public string OrderDateTimeInfo { get; set; }

		public string OrderClassInfo { get; set; }

		public int OrderStatus { get; set; }

		public string StatusName { get; set; }

		public string CarDescription { get; set; }

		public string DriverPhoneNumber { get; set; }

		public double CarLat { get; set; }

		public double CarLon { get; set; }

		public bool BarterAvailable { get; set; }

		public int? BarterClientID { get; set; }

		public string BarterClientName { get; set; }

		public decimal OrderAmount { get; set; }

		public bool OrderAmountOK { get; set; }

		public string OrderAmountComment { get; set; }

		public bool DistanceOK { get; set; }

		public decimal DistanceKM { get; set; }

		public string RouteMetrics { get; set; }

		// public string RouteDescription { get; set; }

		public string ActualRouteMetrics { get; set; }


	}
}
