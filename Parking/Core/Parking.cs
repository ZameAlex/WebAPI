
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Parking.IO;

namespace Parking.Core
{
    public sealed class Parking: IDisposable
    {
        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());
        public static Parking Instanse { get { return lazy.Value; } }

        private readonly Settings settings;

        public List<Car> Cars { get; private set; }
        private Dictionary<Car, Timer> carTimers;

        public List<Transaction> Transactions { get; private set; }

        private List<Transaction> transactionsForLogging;
        private Timer logTimer;

        private Timer deleteOldTransactionsTimer;

        public double Balance { get; private set; }

        private Parking()
        {
            settings = new Settings();
            Cars = new List<Car>();
            Transactions = new List<Transaction>();
            transactionsForLogging = new List<Transaction>();
            carTimers = new Dictionary<Car, Timer>();
            logTimer = new Timer(Log, null, settings.logTimeout*1000, settings.logTimeout*1000);
            deleteOldTransactionsTimer = new Timer(DeleteOldTransaction, null, (settings.logTimeout+1)*1000, 1000);
        }



        public bool AddCar(CarType type, double balance, out string Guid)
        {
            try
            {
                if (settings.ParkingSpace > Cars.Count)
                {
                    var tempCar = new Car(type);
                    tempCar.AddMoney(balance);
                    Guid = tempCar.ID;
                    Cars.Add(tempCar);
                    var timer = new Timer(Payment, tempCar, settings.Timeout * 1000, settings.Timeout * 1000);
                    carTimers.Add(tempCar, timer);
                    return true;
                }
                Guid = null;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool RemoveCar(string Guid)
        {
            try
            {
                var carForRemove = Cars.Find(car => car.ID == Guid);
                if (carForRemove.Balance > 0)
                {
                    Cars.Remove(carForRemove);
                    carTimers[carForRemove].Dispose();
                    carTimers.Remove(carForRemove);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void AddBalance(string Guid, double money)
        {
            try
            {
                var carForAddMoney = Cars.Find(car => car.ID == Guid);
                if (carForAddMoney.Fine > 0)
                {
                    Transaction transaction;
                    carForAddMoney.PayFine(money, out transaction);
                    Balance += transaction.Tax;
                    Transactions.Add(transaction);
                }
                else
                    carForAddMoney.AddMoney(money);
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        private double CalculateTax(Car car)
        {
            try
            { if (car.Balance < 0 || car.Balance < settings.TaxesForCarType[car.Type.ToString()])
                    return settings.Fine * settings.TaxesForCarType[car.Type.ToString()];
                return settings.TaxesForCarType[car.Type.ToString()];
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        #region ShowMethods

        public List<Transaction> ShowTransactions()
        {
            return Transactions;
        }

        public int TotalPlaces()
        {
            return settings.ParkingSpace;
        }

        public int FreePlaces()
        {
            return settings.ParkingSpace - Cars.Count;
        }

        public int OccupiedPlaces()
        {
            return Cars.Count;
        }

        public double ShowIncome(bool minute)
        {
            if (minute)
                return MinuteIncomeCalculate();
            else
                return Balance;
        }

        private double MinuteIncomeCalculate()
        {
            double income = 0;
            foreach (var transaction in Transactions)
            {
                income += transaction.Tax;
            }
            return income;
        }

        public string ShowLog()
        {
            return Logger.ReadLog(settings.logPath);
        }

        public string ShowCar(string Guid)
        {
            try
            {
                var carForDisplay = Cars.Find(car => car.ID == Guid);
                return carForDisplay.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion ShowMethods


        private void Payment(object obj)
        {
            try
            {
                var car = obj as Car;
                var tax = CalculateTax(car);
                if (tax > car.Balance)
                {
                    car.Fine += tax;
                }
                else
                {
                    Transaction transaction;
                    car.Pay(CalculateTax(car), out transaction);
                    Balance += transaction.Tax;
                    Transactions.Add(transaction);
                }
                
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex.Message, "Parking.log");
            }
        }

        private void Log(object obj)
        { 
            double balance = 0;
            foreach (var t in Transactions)
                balance += t.Tax;
           Logger.WriteLog(balance, settings.logPath);
        }

        private void DeleteOldTransaction(object obj)
        {
            try
            {
                for (int counter = 0; counter < Transactions.Count; counter++)
                {
                    if (Transactions[counter].TransactionTime < DateTime.Now.AddSeconds(-settings.logTimeout))
                        Transactions.RemoveAt(counter);
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex.Message, "Parking.log");
            }
        }

        public void Dispose()
        {
            foreach (var item in carTimers)
                item.Value.Dispose();
            deleteOldTransactionsTimer.Dispose();
            logTimer.Dispose();
        }
    }

}

