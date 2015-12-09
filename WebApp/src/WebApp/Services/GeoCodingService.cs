using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApp.Services
{
    public class GeoCodingService
    {
        private ILogger<GeoCodingService> _logger;

        public GeoCodingService(ILogger<GeoCodingService> logger)
        {
            _logger = logger;
        }

        public async Task<GeoCodeResult> lookUp (string location)
        {
            var result = new GeoCodeResult()
            {
                Success = false,
                Message = $"Could not get geocoding for location: {location}"
            };

            var encodedLocation = WebUtility.UrlEncode(location);
            var key = Startup.Configuration["AppSettings:GoogleGeoCodingKey"];
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedLocation}&key={key}";

            var client = new HttpClient();
            var response = await client.GetStringAsync(url);

            JObject responseJson = JObject.Parse(response);

            if(responseJson["status"].ToString() == "OK")
            {
                result.Latitude = Double.Parse(responseJson["results"][0]["geometry"]["location"]["lat"].ToString());
                result.Longitude = Double.Parse(responseJson["results"][0]["geometry"]["location"]["lng"].ToString());
                result.Success = true;
                result.Message = $"Success: Found geolocation for {location}";
            }

            return result;
        }
    }
}
