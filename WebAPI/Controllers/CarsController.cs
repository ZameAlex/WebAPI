using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        // GET api/values
        [HttpGet("[action]")]
        public JsonResult Cars()
        {
            try
            {
                string fuid;
                Parking.Core.Parking.Instanse.AddCar(Parking.Core.CarType.Bus, 100, out fuid);
                Parking.Core.Parking.Instanse.AddCar(Parking.Core.CarType.Truck, 100, out fuid);
                return Json(Parking.Core.Parking.Instanse.Cars.Select(x => new { x.ID, type = x.Type.ToString() }));
            }
            catch(Exception)
            {
                return Json(NoContent());
            }
        }

        // GET api/values/5
        [HttpGet("[action]/{id}")]
        public JsonResult Car(string id)
        {
            try
            {
                return Json(Parking.Core.Parking.Instanse.ShowCar(id));
            }
            catch(Exception)
            {
                return Json(NoContent());
            }
        }

        // POST api/values
        [HttpPost]
        public JsonResult AddCar([FromBody]Model.CarModel value)
        {
            try
            {
                string guid;
                Parking.Core.Parking.Instanse.AddCar(value.Type, value.Balance, out guid);
                if (guid != null)
                    return Json(Ok());
                else
                    return Json(BadRequest());
            }
            catch(Exception)
            {
                return Json(BadRequest());
            }
        }


        // DELETE api/values/5
        [HttpDelete("[action]/{id}")]
        public JsonResult Delete(string id)
        {
            try
            {
                if (Parking.Core.Parking.Instanse.RemoveCar(id))
                {
                    return Json(Ok());
                }
                else
                    return Json(BadRequest());
            }
            catch(Exception)
            {
                return Json(NoContent());
            }
        }
    }
}
