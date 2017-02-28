using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GPS1
{
    class AsemaTiedot
    {
        public async static Task<List<JunaTiedot>> haeTiedot(string shortCode)
        {
            string url = @"https://rata.digitraffic.fi/api/v1/live-trains?station=";
            url = url + shortCode;
            url = url + @"&minutes_before_departure=60&minutes_after_departure=0&minutes_before_arrival=0&minutes_after_arrival=0";

            var http = new HttpClient();
            var response = await http.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            JArray junat = JArray.Parse(result);
            JArray plcArray;
            //JObject viimeinen;
            //int i = 0;

            List<JunaTiedot> junalista = new List<JunaTiedot>();
            JunaTiedot juna = new JunaTiedot();
           

            string asema = "";
            string type = "";
            string kohde = "";
            string kohdeA = "";
            string aikastring;
            DateTime lahtoAika;
            //int raide = 0;

            foreach (JObject trainobj in junat.Children<JObject>())
            {
                juna = new JunaTiedot();

                var trainVar = trainobj.Property("trainNumber").Value;
                juna.trainNumber = trainVar.ToObject<int>();
                Debug.WriteLine("Junanumero: " + juna.trainNumber);
                var typeVar = trainobj.Property("trainType").Value;
                juna.trainType = typeVar.ToObject<String>();

                plcArray = trainobj.Property("timeTableRows").Value.ToObject<JArray>();


                foreach(JObject plc in plcArray.Children<JObject>())
                {
                    asema = plc.Property("stationShortCode").Value.ToObject<String>();
                    type = plc.Property("type").Value.ToObject<String>();
                    if ((asema == shortCode)&& (type == "DEPARTURE"))
                    {
                        var raide = plc.Property("commercialTrack").Value;
                        juna.commercialTrack = raide.ToObject<String>();

                        var aika = plc.Property("scheduledTime").Value;
                        lahtoAika = aika.ToObject<DateTime>();
                        juna.timeStamp = lahtoAika;
                        lahtoAika = lahtoAika.ToLocalTime();
                        //Debug.WriteLine(aikastring);
                        //lahtoAika = DateTime.ParseExact(aikastring,"MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        juna.time = lahtoAika.ToString("HH:mm");
                        try
                        {
                            var estimate = plc.Property("liveEstimateTime").Value;
                            if (estimate != null)
                            {
                                lahtoAika = estimate.ToObject<DateTime>();
                                lahtoAika = lahtoAika.ToLocalTime();
                                if(lahtoAika.ToString("HH:mm") != juna.time)
                                {
                                    juna.time = juna.time + "->" + lahtoAika.ToString("HH:mm");

                                }
                            }
                        }
                        catch (System.NullReferenceException)
                        {
                            continue;
                        }
                    }
                    else if(type == "ARRIVAL")
                    {
                        var kohdeVar = plc.Property("stationShortCode").Value;
                        kohde = kohdeVar.ToObject<String>();
                        juna.destination = kohde;
                        //kohdeA = await LiikennePaikka.asemaNimi(kohde);
                        //juna.destination = kohdeA;
                    }
                }
                junalista.Add(juna);

            }

            return junalista;
        }
    }
}
