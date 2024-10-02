using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace payment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bill = new paymentBill()
            {
                DailyPayment = 100,
                DaysCount = 5,
                PenaltyPerDay = 50,
                LastDayCount = 2
            };
            string serializedData = bill.Serialize();
            Console.WriteLine("Сериализованные данные: " + serializedData);

            File.WriteAllText("payment_bill.json", serializedData);

            string jsonData = File.ReadAllText("payment_bill.json");

            var newBill = paymentBill.Deserialize(jsonData);
            Console.WriteLine("Общая сумма к оплате после десериализации: " + newBill.TotalAmountDue);
        }
    }
    public class 
        paymentBill
    {
        public static bool SerializeComputedFields = true;
        public decimal DailyPayment { get; set; }
        public int DaysCount { get; set; }
        public decimal PenaltyPerDay { get; set; }
        public int LastDayCount { get; set; }
        public decimal AmountDueWithoutPenalty => DailyPayment * DaysCount;
        public decimal Penalty => PenaltyPerDay * LastDayCount;
        public decimal TotalAmountDue => AmountDueWithoutPenalty + Penalty;
        public string Serialize()
        {
            var data = new
            {
                DailyPayment,
                DaysCount,
                PenaltyPerDay,
                LastDayCount,
                AmountDueWithoutPenalty = SerializeComputedFields ? AmountDueWithoutPenalty : (decimal?)null,
                Penalty = SerializeComputedFields ? PenaltyPerDay : (decimal?)null,
                TotalAmountDue = SerializeComputedFields ? TotalAmountDue : (decimal?)null

            
            };
            return JsonConvert.SerializeObject(data);
        }
        public static paymentBill Deserialize(string jsonData)
        {
            dynamic data = JsonConvert.DeserializeObject(jsonData);
            return new paymentBill
            {
                DailyPayment = data.DailyPayment,
                DaysCount = data.DaysCount,
                PenaltyPerDay = data.PenaltyPerDay,
                LastDayCount = data.LastDayCount,
            };

        }
    }

}
