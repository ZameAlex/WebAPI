using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Parking.Core
{
    class Settings
    {
        public int Timeout { get; private set; }
        public Dictionary<string,double> TaxesForCarType { get; private set; }
        public int ParkingSpace { get; private set; }
        public double Fine { get; private set; }
        public int logTimeout { get; private set; }
        public string logPath { get; set; }

        /// <summary>
        /// Using App.config for storing settings and ConfigurationManager for reading
        /// </summary>
        public Settings()
        {
            SetDefaultSettings();                
        }

        private void SetDefaultSettings()
        {
            Timeout = 3;
            ParkingSpace = 25;
            Fine = 2.5;
            logPath = "Transactions.log";
            logTimeout = 60;
            TaxesForCarType = new Dictionary<string, double>();
            TaxesForCarType.Add(CarType.Passenger.ToString(), 3);
            TaxesForCarType.Add(CarType.Truck.ToString(), 5);
            TaxesForCarType.Add(CarType.Bus.ToString(), 2);
            TaxesForCarType.Add(CarType.Motorcycle.ToString(), 1);
        }

        
    }
}
