using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Model
{
    public class CarModel
    {
        public double Balance { get; set; }
        public Parking.Core.CarType Type { get; set; }
    }
}
