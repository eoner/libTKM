using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
namespace libTKM
{
    public class TKM
    {
        public static string DownloadTrafficIndex()
        {
            // trafic index value
            using (WebClient wc = new WebClient())
            {
                string encData = wc.DownloadString("http://tkm.ibb.gov.tr/data/IntensityMap/TrafficIndex.aspx");
                string trafficIndexStr = TKMDecrypt.Decrypt0(encData, "60413275");
                return trafficIndexStr;
            }
        }

        public static string DownloadTrafficData()
        {
            // speed sensors
            using (WebClient wc = new WebClient())
            {
                string encData = wc.DownloadString("http://tkm.ibb.gov.tr/data/IntensityMap/TrafficDataNew.aspx");
                string speedDataStr = TKMDecrypt.Decrypt0(encData, "62403715");
                return speedDataStr;
            }
        }

        public static string DownloadParkingData()
        {
            // parking data
            using(WebClient wc = new WebClient())
            {
                string encData = wc.DownloadString("http://tkm.ibb.gov.tr/data/IntensityMap/ParkingLotData.aspx");
                string parkingDataStr = TKMDecrypt.Decrypt0(encData, "74205136");
                return parkingDataStr;
            }
        }

        public static string DownloadAnnouncements()
        {
            // announcement data
            using(WebClient wc = new WebClient())
            {
                string encData = wc.DownloadString("http://tkm.ibb.gov.tr/data/IntensityMap/AnnouncementData.aspx");
                string announceDataStr = TKMDecrypt.Decrypt0(encData, "50614732").Replace('\r',' ').Replace('\n',' '); // replace new lines
                return announceDataStr;
            }
        }

        public static string DownloadWeatherData()
        {
            // weather data
            using (WebClient wc = new WebClient())
            {
                string encData = wc.DownloadString("http://tkm.ibb.gov.tr/data/IntensityMap/WeatherData.aspx");
                string weatherDataStr = TKMDecrypt.Decrypt0(encData, "26107354");
                return weatherDataStr;
            }
        }

        public static void DownloadStaticFiles(string saveDir)
        {
            string staticBase = "http://tkm.ibb.gov.tr/YHarita/res/";

            for (int i = 1; i <= 8; i++)
            {
                string address = String.Format("{0}d{1:00}.txt", staticBase, i);
                Console.WriteLine(address);
                using (WebClient wc = new WebClient())
                {
                    var encResult = wc.DownloadString(address);
                    var res = TKMDecrypt.Decrypt2(encResult);
                    File.WriteAllText(Path.Combine(saveDir,String.Format("d{0:00}.txt", i)), res);
                    wc.Dispose();
                }
            }

            for (int i = 0; i <= 4; i++)
            {
                string address = String.Format("{0}r{1}.txt", staticBase, i);
                Console.WriteLine(address);
                using (WebClient wc = new WebClient())
                {
                    var encResult = wc.DownloadString(address);
                    var res = TKMDecrypt.Decrypt2(encResult);
                    File.WriteAllText(Path.Combine(saveDir,String.Format("r{0}.txt", i)), res);
                    wc.Dispose();
                }
            }
        }
    }
}
