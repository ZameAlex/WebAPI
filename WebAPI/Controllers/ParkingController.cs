using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Parking")]
    public class ParkingController : Controller
    {
        // GET: api/Parking
        [HttpGet("[action]")]

        public JsonResult FreePlaces()
        {
            return Json(Parking.Core.Parking.Instanse.FreePlaces());
        }

        [HttpGet("[action]")]
        public JsonResult OccupiedPlaces()
        {
            return Json(Parking.Core.Parking.Instanse.OccupiedPlaces());
        }

        [HttpGet("[action]")]
        public JsonResult Balance()
        {
            return Json(Parking.Core.Parking.Instanse.Balance);
        }
    }
}
