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
    class Program
    {
        static void Main(string[] args)
        {
            TKMDemo demo = new TKMDemo(@"D:\Projects\libTKM\TKMDemoApp\demo");
          
            if (args.Length == 1 && args[0] == "dl-roads") demo.DownloadRoadDatabase();

            demo.DownloadData();
            demo.UpdateAggregates();

            demo.GenerateMapData();
            demo.WritePlotCSV();
        }


    }
}
