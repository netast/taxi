using System;
using System.Collections.Generic;
using System.Text;

namespace taxi
{
	internal static class GooglePolylineCoder
	{
		private static int GooglePolylineMinASCII = 63;
		private static byte GooglePolylineBinaryChunkSize = 5;

		/// <summary>
		/// encoded a list of latlon objects into a string
		/// </summary>
		/// <param name="points">the list of latlon objects to encode</param>
		/// <returns>the encoded string</returns>
		public static string EncodeGooglePolyline(List<RoutePoint> points)
		{
			int plat = 0;
			int plng = 0;
			int len = points.Count;

			StringBuilder encoded_points = new StringBuilder();

			for (int i = 0; i < len; ++i)
			{
				//Round to 5 decimal places and drop the decimal
				//int lateR = (int)(points[i].Lat * 1e5);
				//int lngeR = (int)(points[i].Lon * 1e5);

				//Round to 6 decimal places and drop the decimal
				int lateR = (int)(points[i].Lat * 1e6);
				int lngeR = (int)(points[i].Lon * 1e6);

				//encode the differences between the points
				encoded_points.Append(encodeSignedNumber(lateR - plat));
				encoded_points.Append(encodeSignedNumber(lngeR - plng));

				//store the current point
				plat = lateR;
				plng = lngeR;
			}

			return encoded_points.ToString();
		}

		/// <summary>
		/// Encode a signed number in the encode format.
		/// </summary>
		/// <param name="num">the signed number</param>
		/// <returns>the encoded string</returns>
		private static string encodeSignedNumber(int num)
		{
			int sgn_num = num << 1; //shift the binary value
			if (num < 0) //if negative invert
			{
				sgn_num = ~(sgn_num);
			}
			return (encodeNumber(sgn_num));
		}

		/// <summary>
		/// Encode an unsigned number in the encode format.
		/// </summary>
		/// <param name="num">the unsigned number</param>
		/// <returns>the encoded string</returns>
		private static string encodeNumber(int num)
		{
			StringBuilder encodeString = new StringBuilder();
			while (num >= 0x20)
			{
				//while another chunk follows
				encodeString.Append((char)((0x20 | (num & 0x1f)) + GooglePolylineMinASCII));
				//OR value with 0x20, convert to decimal and add 63
				num >>= GooglePolylineBinaryChunkSize; //shift to next chunk
			}
			encodeString.Append((char)(num + GooglePolylineMinASCII));
			return encodeString.ToString();
		}

		public static List<RoutePoint> DecodeGooglePolyline_Alternative(string encodedPoints)
		{
			if (encodedPoints == null || encodedPoints == "") return null;
			List<RoutePoint> poly = new List<RoutePoint>();
			char[] polylinechars = encodedPoints.ToCharArray();
			int index = 0;

			int currentLat = 0;
			int currentLng = 0;
			int next5bits;
			int sum;
			int shifter;

			try
			{
				while (index < polylinechars.Length)
				{
					// calculate next latitude
					sum = 0;
					shifter = 0;
					do
					{
						next5bits = (int)polylinechars[index++] - 63;
						sum |= (next5bits & 31) << shifter;
						shifter += 5;
					} while (next5bits >= 32 && index < polylinechars.Length);

					if (index >= polylinechars.Length)
						break;

					currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

					//calculate next longitude
					sum = 0;
					shifter = 0;
					do
					{
						next5bits = (int)polylinechars[index++] - 63;
						sum |= (next5bits & 31) << shifter;
						shifter += 5;
					} while (next5bits >= 32 && index < polylinechars.Length);

					if (index >= polylinechars.Length && next5bits >= 32)
						break;

					currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
					RoutePoint p = new RoutePoint();

					p.Lat = Convert.ToDouble(currentLat) / 1000000.0;
					p.Lon = Convert.ToDouble(currentLng) / 1000000.0;
					poly.Add(p);
				}
			}
			catch (Exception ex)
			{
				// logo it
			}
			return poly;
		}

		/// <summary>
		/// decodes a string into a list of latlon objects
		/// </summary>
		/// <param name="encoded">encoded string</param>
		/// <returns>list of latlon</returns>
		public static List<RoutePoint> DecodeGooglePolyline(string encodedPoints)
		{
			List<RoutePoint> locs = new List<RoutePoint>();

			int index = 0;
			int lat = 0;
			int lng = 0;

			int len = encodedPoints.Length;
			while (index < len)
			{
				lat += decodePoint(encodedPoints, index, out index);
				lng += decodePoint(encodedPoints, index, out index);

				//locs.Add(new RoutePoint((lat * 1e-5), (lng * 1e-5)));
				locs.Add(new RoutePoint((lat * 1e-6), (lng * 1e-6)));
			}

			return locs;
		}

		/// <summary>
		/// decodes the cuurent chunk into a single integer value
		/// </summary>
		/// <param name="encoded">the complete encodered string</param>
		/// <param name="startindex">the current position in that string</param>
		/// <param name="finishindex">output - the position we end up in that string</param>
		/// <returns>the decoded integer</returns>
		private static int decodePoint(string encoded, int startindex, out int finishindex)
		{
			int b;
			int shift = 0;
			int result = 0;
			do
			{
				//get binary encoding
				b = Convert.ToInt32(encoded[startindex++]) - GooglePolylineMinASCII;
				//binary shift
				result |= (b & 0x1f) << shift;
				//move to next chunk
				shift += GooglePolylineBinaryChunkSize;
			} while (b >= 0x20); //see if another binary value
								 //if negivite flip
			int dlat = (((result & 1) > 0) ? ~(result >> 1) : (result >> 1));
			//set output index
			finishindex = startindex;
			return dlat;
		}

	}
}
