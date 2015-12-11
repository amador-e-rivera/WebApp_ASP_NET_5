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
using WebApp.ViewModels;

namespace WebApp.Controllers.Api
{
    [Authorize]
    [Route("api/trips")] //Allows to create a route for Api
    public class TripController : Controller
    {
        private ILogger<TripController> _logger;
        private IWebAppRepository _repository;

        public TripController(IWebAppRepository repository, ILogger<TripController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")] //Empty string allows method to use route at class level.
        public JsonResult Get()
        {
            return Json(AutoMapper.Mapper.Map<IEnumerable<TripViewModel>>(_repository.getAllTripsWithStops()));
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]TripViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Mapping is initialized in Startup.cs
                    var newTrip = AutoMapper.Mapper.Map<Trip>(viewModel);

                    //Save to Database
                    _logger.LogInformation("Attempting to save new trip");
                    _repository.AddTrip(newTrip);

                    if(_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(AutoMapper.Mapper.Map<TripViewModel>(newTrip));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new trip", ex);
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { message = "failed!", model = ModelState });
        }
    }
}
