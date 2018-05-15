using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Transactions")]
    public class TransactionsController : Controller
    {
        // GET: api/Transactions
        [HttpGet("[action]")]
        public JsonResult Log()
        {
            return Json(Parking.Core.Parking.Instanse.ShowLog());
        }

        [HttpGet("[action]")]
        public JsonResult MinuteTransaction()
        {
            return Json(Parking.Core.Parking.Instanse.ShowTransactions());
        }


        [HttpGet("[action]/{id}")]
        public JsonResult MinuteTransaction(string id)
        {
            try
            {
                return Json(Parking.Core.Parking.Instanse.ShowTransactions().Where(x=>x.CarID==id));
            }
            catch(Exception)
            {
                return Json(BadRequest());
            }
        }

        [HttpPut("[action]/{id}")]
        public JsonResult AddBalance(string id, double money)
        {
            try
            {
                Parking.Core.Parking.Instanse.AddBalance(id, money);
                return Json(Ok());
            }
            catch (Exception)
            {
                return Json(BadRequest());
            }
        }




    }
}
