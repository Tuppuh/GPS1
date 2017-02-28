using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS1
{
    public class TrainReady
    {
        public string source { get; set; }
        public bool accepted { get; set; }
        public string timestamp { get; set; }
    }

    public class TimeTableRow
    {
        public string stationShortCode { get; set; }
        public int stationUICCode { get; set; }
        public string countryCode { get; set; }
        public string type { get; set; }
        public bool trainStopping { get; set; }
        public bool commercialStop { get; set; }
        public string commercialTrack { get; set; }
        public bool cancelled { get; set; }
        public string scheduledTime { get; set; }
        public string actualTime { get; set; }
        public int differenceInMinutes { get; set; }
        public List<object> causes { get; set; }
        public TrainReady trainReady { get; set; }
        public string liveEstimateTime { get; set; }
    }

    public class RootObject
    {
        public int trainNumber { get; set; }
        public string departureDate { get; set; }
        public int operatorUICCode { get; set; }
        public string operatorShortCode { get; set; }
        public string trainType { get; set; }
        public string trainCategory { get; set; }
        public string commuterLineID { get; set; }
        public bool runningCurrently { get; set; }
        public bool cancelled { get; set; }
        public object version { get; set; }
        public string timetableType { get; set; }
        public string timetableAcceptanceDate { get; set; }
        public List<TimeTableRow> timeTableRows { get; set; }
    }
}
