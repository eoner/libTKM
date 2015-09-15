using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libTKM
{
    public class Coordinate
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public class RoadSegment
    {
        public int ID { get; set; }
        public List<Coordinate> Nodes { get; set; }
        public string Semt { get; set; }
        public bool IsTunnel { get; set; }

        public RoadSegment()
        {
            Nodes = new List<Coordinate>();
        }

        public static List<RoadSegment> ParseRoadData(string roadDataString)
        {
            List<RoadSegment> roads = new List<RoadSegment>();
            char[] seps = new char[] { ';' };
            int lastID = -1;
            RoadSegment lastRoad = null;

            string[] lines = roadDataString.Replace('\0', ' ').Trim().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(seps);
                int ID = Convert.ToInt32(tokens[0]);
                if (ID != lastID)
                {
                    if (lastRoad != null) roads.Add(lastRoad);
                    lastRoad = new RoadSegment();
                    lastID = ID;
                    lastRoad.ID = ID;

                    // is Tunnel
                    lastRoad.IsTunnel = (!String.IsNullOrEmpty(tokens[tokens.Length - 1]));

                    // semt
                    if (!String.IsNullOrEmpty(tokens[tokens.Length - 2]))
                    {
                        lastRoad.Semt = tokens[tokens.Length - 2];
                    }
                }

                lastRoad.Nodes.Add(new Coordinate() { Lon = Convert.ToDouble(tokens[2]), Lat = Convert.ToDouble(tokens[3]) });
            }

            if (lastRoad != null) roads.Add(lastRoad);

            return roads;
        }
    
    }
}
