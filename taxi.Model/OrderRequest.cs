using System;
namespace taxi
{
	public class OrderRequest
	{
		public string FromStreet { get; set; }
		public string FromHouse { get; set; }
		public string ToStreet { get; set; }
		public string ToHouse { get; set; }
		public int ClassId { get; set; }
		public DateTime Time { get; set; }
	}
}
