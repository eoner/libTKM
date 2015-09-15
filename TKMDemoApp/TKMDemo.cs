using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using libTKM;
using Newtonsoft.Json;

namespace TKMDemoApp
{
    public class TKMDemo
    {
        protected DateTime acqTimeUTC;
        protected DateTime acqTimeLocal;
        protected TimeSpan utcOffset;

        protected List<SpeedSensorData> sensorData;
        protected string trafficIndex;

        protected string dataDir, aggDir;

        public TKMDemo(string workingDir)
        {
               dataDir=Path.Combine(workingDir, "data");
               aggDir = Path.Combine(dataDir, "agg");
        }

        public void DownloadRoadDatabase()
        {
            string staticBase = "http://tkm.ibb.gov.tr/YHarita/res/";
            string address = String.Format("{0}r0.txt", staticBase);
            using (WebClient wc = new WebClient())
            {
                var encResult = wc.DownloadString(address);
                var res = TKMDecrypt.Decrypt2(encResult);
                File.WriteAllText(Path.Combine(dataDir, "r0.txt"), res);
                wc.Dispose();
            }
        }

        public void DownloadData()
        {
            acqTimeUTC = DateTime.UtcNow;
            
            // get local acq time
            TimeZoneInfo turkeyTZ;
            try
            {
                turkeyTZ = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            }
            catch
            {
                turkeyTZ = TimeZoneInfo.FindSystemTimeZoneById("Turkey");
            }

            // TimeZoneInfo 
            utcOffset = turkeyTZ.GetUtcOffset(acqTimeUTC);
            acqTimeLocal = acqTimeUTC.Add(utcOffset);

            sensorData = SpeedSensorData.ParseSpeedData(TKM.DownloadTrafficData());
            trafficIndex = TKM.DownloadTrafficIndex();
        }

        public void GenerateMapData()
        {
            // init road data
            var roads = RoadSegment.ParseRoadData(File.ReadAllText(Path.Combine(dataDir, "r0.txt")));

            // serialize as geojson
            FeatureCollection trafficData = new FeatureCollection();
            foreach (RoadSegment road in roads)
            {
                // find the sensor and determine color
                var sensor = sensorData.Where(s => s.ID == road.ID).FirstOrDefault();
                List<IPosition> segments = new List<IPosition>();
                for (int i = 0; i < road.Nodes.Count; i++)
                {
                    segments.Add(new GeographicPosition(road.Nodes[i].Lat, road.Nodes[i].Lon));
                }

                var featureProperties = new Dictionary<string, object> { { "id", road.ID }, { "l", road.Semt }, { "s", sensor == null ? -1 : sensor.Speed } };
                var feat = new Feature(new LineString(segments), featureProperties);
                trafficData.Features.Add(feat);
            }
            var serializedData = JsonConvert.SerializeObject(trafficData);
            var dateTime = String.Format("{2:00}/{3:00}/{4} {0:00}:{1:00}", acqTimeLocal.Hour, acqTimeLocal.Minute,acqTimeLocal.Day,acqTimeLocal.Month,acqTimeLocal.Year);

            StringBuilder sb = new StringBuilder();
            sb.Append("var mapData=" + serializedData + ";\r\n");
            sb.Append("var acqTime='" + dateTime + "';\r\n");
            sb.Append("var trafficIndex=" + trafficIndex + ";\r\n");

            File.WriteAllText(Path.Combine(dataDir, "tkmData.js"), sb.ToString());
        }

        public void UpdateAggregates()
        {
            int index = Convert.ToInt32(trafficIndex);
            if (index == 255) return;

            if (!Directory.Exists(aggDir)) Directory.CreateDirectory(aggDir);

            string acqKey = String.Format("{0:00}:{1:00}", acqTimeUTC.Hour, acqTimeUTC.Minute);
            int acqDayIndex = (int)(acqTimeUTC.DayOfWeek + 6) % 7; // monday is 0, sunday is 6
            string aggFile = Path.Combine(aggDir, String.Format("aggregates{0}.csv", acqDayIndex));

            // create the bins
            var aggBins = createAggBins();

            // init with initial values
            if (!File.Exists(aggFile))
            {
                aggBins[acqKey][0] = index;
                aggBins[acqKey][1] = 1;
            }
            else
            {
                // parse the file and init aggBins
                string[] lines = File.ReadAllLines(aggFile);
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] tokens = lines[i].Split(',');
                    aggBins[tokens[0]][0] = Convert.ToDouble(tokens[1]);
                    aggBins[tokens[0]][1] = Convert.ToDouble(tokens[2]);
                }

                // update values
                aggBins[acqKey][0] += index;
                aggBins[acqKey][1] += 1;
            }

            // write agg. file
            using (StreamWriter file = File.CreateText(aggFile))
            {
                file.WriteLine("time,sum,count");
                foreach (string key in aggBins.Keys)
                {
                    file.WriteLine("{0},{1},{2}", key, aggBins[key][0], aggBins[key][1]);
                }
            }


        }

        public void WritePlotCSV()
        {
            // write csv file for the plot in local time
            string csvFile = Path.Combine(Path.Combine(dataDir, "trafficIndex.csv"));

            string acqKey = String.Format("{0:00}:{1:00}", acqTimeLocal.Hour, acqTimeLocal.Minute);

            DateTime startTimeUTC = new DateTime(acqTimeLocal.Year, acqTimeLocal.Month, acqTimeLocal.Day, 0, 0, 0).Subtract(utcOffset);
            DateTime endTimeUTC = new DateTime(acqTimeLocal.Year, acqTimeLocal.Month, acqTimeLocal.Day, 23, 59, 0).Subtract(utcOffset);

            int startDayIndexUTC = (int)(startTimeUTC.DayOfWeek + 6) % 7; // monday is 0, sunday is 6
            int endDayIndexUTC = (int)(endTimeUTC.DayOfWeek + 6) % 7; // monday is 0, sunday is 6

            // create the bins
            var aggBins=createAggBins();
            var keys=aggBins.Keys.ToArray();

            // read the first file until the end
            bool readFlag = false;
            string aggFile = Path.Combine(aggDir, String.Format("aggregates{0}.csv", startDayIndexUTC));
            string startKey=String.Format("{0:00}:{1:00}", startTimeUTC.Hour, startTimeUTC.Minute);
            string[] lines = File.ReadAllLines(aggFile);
            int k=0;
            for (int i = 1; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(',');
                if (!readFlag && tokens[0] == startKey)
                {
                    readFlag = true;
                }
                if (readFlag)
                {
                    aggBins[keys[k]][0] = Convert.ToDouble(tokens[1]);
                    aggBins[keys[k]][1] = Convert.ToDouble(tokens[2]);
                    k++;
                }
            }

            //read the second file
            aggFile = Path.Combine(aggDir, String.Format("aggregates{0}.csv", endDayIndexUTC));
            string endKey = String.Format("{0:00}:{1:00}", endTimeUTC.Hour, endTimeUTC.Minute);
            lines = File.ReadAllLines(aggFile);
            for (int i = 1; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(',');
                if (tokens[0] != endKey)
                {
                    aggBins[keys[k]][0] = Convert.ToDouble(tokens[1]);
                    aggBins[keys[k]][1] = Convert.ToDouble(tokens[2]);
                    k++;
                }
                else
                { break; }
            }

            using (StreamWriter file = File.CreateText(csvFile))
            {
                file.WriteLine("time,Ort. Yoğunluk,Anlık Değer");
                foreach (string key in aggBins.Keys)
                {
                    file.WriteLine("{3}/{4}/{5} {0}:00,{1},{2}", key, aggBins[key][1] == 0 ? Double.NaN : aggBins[key][0] / aggBins[key][1],
                        key.Equals(acqKey) ? trafficIndex.ToString() : string.Empty, acqTimeLocal.Year, acqTimeLocal.Month,acqTimeLocal.Day);
                }
            }

        }

        protected Dictionary<string, double[]> createAggBins()
        {
            // create the bins
            Dictionary<string, double[]> aggBins = new Dictionary<string, double[]>();
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    string key = String.Format("{0:00}:{1:00}", i, j);
                    aggBins.Add(key, new double[2]);
                }
            }

            return aggBins;
        }

       

    }
}
