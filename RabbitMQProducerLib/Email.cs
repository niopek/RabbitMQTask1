using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQProducerLib
{
    public class Email : ISend
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public Email(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }

        public void Send()
        {
            Console.WriteLine($" tytul: {Subject}");
            Console.WriteLine($" body: {Body}");
            Console.WriteLine($" do: {To}");
        }
    }
}
