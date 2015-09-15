using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libTKM
{
    public class SpeedSensorData
    {
        public int ID { get; set; }
        public int Speed { get; set; }
        //public int Color { get; set; }

        public static List<SpeedSensorData> ParseSpeedData(string sensorDataString)
        {
            List<SpeedSensorData> data = new List<SpeedSensorData>();
            char[] seps1 = new char[] { '&' };
            char[] seps2 = new char[] { '|' };

            string[] lines = sensorDataString.Split(seps1, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in lines)
            {
                var tokens = s.Split(seps2, StringSplitOptions.RemoveEmptyEntries);
                if (tokens == null || tokens.Length < 3) continue;

                SpeedSensorData sensor = new SpeedSensorData()
                {
                    ID = Convert.ToInt32(tokens[0]),
                    Speed = Convert.ToInt32(tokens[1]),
                    //Color = Convert.ToInt32(tokens[2])
                };
                data.Add(sensor);
            }
            return data;
        }
    }
}
