using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQProducerLib
{
    public class Car : ISend
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string EnginePower { get; set; }
        public Car(string brand, string model, string enginePower) 
        {
            Brand= brand;
            Model= model;
            EnginePower= enginePower;
        }

        public void Send()
        {
            Console.WriteLine($"Marka: {Brand}");
            Console.WriteLine($"Model: {Model}");
            Console.WriteLine($"Pojemnosc silnika: {EnginePower}");
        }
    }
}
