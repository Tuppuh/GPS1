using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GPS1
{
    class LiikennePaikka
    {
        public async static Task<string> asemaNimi(string lyhenne)
        {
            string url = @"https://rata.digitraffic.fi/api/v1/metadata/stations";
            var http = new HttpClient();
            var response = await http.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            JArray array = JArray.Parse(result);

            string asema = "notFound";
            string code;

            foreach(JObject obj in array.Children<JObject>()){
                code = obj.Property("stationShortCode").Value.ToObject<String>();
                if (code == lyhenne)
                {
                    asema = obj.Property("stationName").Value.ToObject<String>();
                }
            }
            return asema;
        }

        public async static Task<string> etsiPaikka(double lat, double lon)
        {
            string url = @"https://rata.digitraffic.fi/api/v1/metadata/stations";

            var http = new HttpClient();
            var response = await http.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            JArray array = JArray.Parse(result);
            //var lativ;
            double lati;
            //var longv;
            double longi;
            double latDiff = 0;
            double lonDiff = 0;

            double mindist = 100000;
            double dist;
            string asema = "ERRORHORROR";
            bool passenger = false;

            foreach(JObject obj in array.Children<JObject>())
            {
                lati = obj.Property("latitude").Value.ToObject<Double>();
                longi = obj.Property("longitude").Value.ToObject<Double>();

                latDiff = Math.Abs(lat - lati);
                lonDiff = Math.Abs(lon - longi);

                dist = latDiff * latDiff + lonDiff * lonDiff;
                if(dist < mindist)
                {
                    passenger = obj.Property("passengerTraffic").Value.ToObject<Boolean>();
                    if (passenger)
                    {
                        mindist = dist;
                        asema = obj.Property("stationShortCode").Value.ToString();
                    }

                }
            }

            return asema;
        }
    }
}
