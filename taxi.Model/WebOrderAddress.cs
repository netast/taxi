namespace taxi.Model
{
	public class WebOrderAddress
	{
		public WebOrderAddress()
		{

		}

		public WebOrderAddress(string streetOrPlace, string house)
		{
			StreetOrPlace = streetOrPlace;
			House = house;
		}

		// Fields expected from Client

		public string StreetOrPlace { get; set; }

		public string House { get; set; }

		// Fields populated by the Taxi system

		public bool IsPlace { get; set; }

		private double? lat;
		public double? Lat
		{
			get { return lat; }
			set
			{
				if (lat == value) return;
				lat = value;
				CoordinatesChanges = true;
			}
		}

		private double? lon;
		public double? Lon
		{
			get { return lon; }
			set
			{
				if (lon == value) return;
				lon = value;
				CoordinatesChanges = true;
			}
		}

		public string CoordinatesComment { get; set; }

		public bool CoordinatesChanges { get; set; }

		public override string ToString()
		{
			return string.Format("{0} {1}", StreetOrPlace, House, lat, lon);
		}


		public bool HasCoordinates
		{
			get { return lat.HasValue && lat.Value > 0 && lon.HasValue && lon.Value > 0; }
		}

		public void Trim()
		{
			StreetOrPlace = StreetOrPlace.Trim();
			House = House.Trim();
		}



	}
}
