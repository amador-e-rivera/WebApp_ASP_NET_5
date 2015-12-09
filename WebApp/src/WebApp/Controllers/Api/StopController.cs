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
    [Route("api/stop")]
    public class StopController : Controller
    {
        private ILogger _logger;
        private IWebAppRepository _repository;

        public StopController(IWebAppRepository repository, ILogger<StopController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public JsonResult Get()
        {
            return Json(new { name = "Implement Get in StopController" });
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]StopViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = AutoMapper.Mapper.Map<Stop>(viewModel);

                    //Add Stop to database
                    _logger.LogInformation("Attempting to add new stop to db.");
                    _repository.AddStop(newStop);

                    if(_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(newStop);
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
            return Json(new { name = "Failed!" });
        }
    }
}
