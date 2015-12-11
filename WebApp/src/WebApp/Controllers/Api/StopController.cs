using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers.Api
{
    [Authorize]
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private GeoCodingService _geoCodingService;
        private ILogger _logger;
        private IWebAppRepository _repository;

        public StopController(IWebAppRepository repository, ILogger<StopController> logger, GeoCodingService geoCodingService)
        {
            _repository = repository;
            _logger = logger;
            _geoCodingService = geoCodingService;
        }

        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var results = _repository.getTripByName(tripName);

                if (results != null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Found;
                    return Json(AutoMapper.Mapper.Map<IEnumerable<StopViewModel>>(results.Stops.OrderBy(c => c.Order)));
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                _logger.LogError($"Failed to get stops for trip {tripName}", ex);
                return Json(new { error = "Failed to get stops for trip" });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { error = "Trip not found" });
        }

        [HttpPost("")]
        public async Task<JsonResult> Post(string tripName, [FromBody]StopViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Do Mapping
                    var newStop = AutoMapper.Mapper.Map<Stop>(viewModel);

                    //Get GeoCoordinates
                    GeoCodeResult geoResult = await _geoCodingService.lookUp(newStop.Name);

                    if(!geoResult.Success)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(geoResult.Message);
                    }

                    newStop.Latitude = geoResult.Latitude;
                    newStop.Longitude = geoResult.Longitude;

                    //Add Stop to database
                    _logger.LogInformation("Attempting to add new stop to db.");
                    _repository.AddStop(newStop, tripName);

                    if(_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(AutoMapper.Mapper.Map<StopViewModel>(newStop));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not add Stop.", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { name = "Validation Failed!" });
        }
    }
}
