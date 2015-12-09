using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public class GeoCodeResult
    {
        public bool Success { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Message { get; set; }
    }
}
