using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADLibrary
{
    public class Util
    {
        public static double ToStationDouble(string station)
        {
            station = station.Replace("+", "");
            if (station.Contains("-")) station = "-" + station.Replace("-", "");

            return Convert.ToDouble(station);
        }

        public static string ToStationString(double station)
        {
            return String.Format("{0}{1}{2:000.0000}", (int)(station / 1000), (station < 0 ? "" : "+"), station - ((int)(station / 1000) * 1000));
        }

        public static double D2R(double degree)
        {
            return degree * (Math.PI / 180);
        }
    }
}
