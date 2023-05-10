using RabbitMQProducerLib;
using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text.Json;


var uri = new Uri("amqp://guest:guest@localhost:5672");
Producer producer = new(uri, "testqueue");


Console.WriteLine("Czesc! To aplikacja z RabbitMQ do wysylania roznych obiektow do innej aplikacji.");
ShowMenu();

Console.WriteLine("Wpisz cyfre aby przejsc dalej");
var input = Console.ReadLine();


while(input != "0")
{
    if(input == "1")
    {
        var test = EmailCreator();
        if (test)
        {
            Console.WriteLine("wyslano wiadomosc");
        }
        else
        {
            Console.WriteLine("nie udalo sie wyslac wiadomosci");
        }
        
    }

    if (input == "2")
    {
        var test = CarCreator();
        if (test)
        {
            Console.WriteLine("wyslano wiadomosc");
        }
        else
        {
            Console.WriteLine("nie udalo sie wyslac wiadomosci");
        }

    }

    Console.WriteLine("Wybierz jaki obiekt chcesz stworzyc:");
    Console.WriteLine("1 - Email");
    Console.WriteLine("2 - Samochod");
    Console.WriteLine("0 - Opuscic program");

    input = Console.ReadLine();
}










void ShowMenu()
{
    Console.WriteLine("Wybierz jaki obiekt chcesz stworzyc:");
    Console.WriteLine("1 - Email");
    Console.WriteLine("2 - Samochod");
    Console.WriteLine("0 - Opuscic program");
}

bool EmailCreator()
{
    Console.WriteLine("Do kogo chcesz wyslac email?");
    string? to = Console.ReadLine();
    Console.WriteLine("Jaki tytul wiadomosci?");
    string? title = Console.ReadLine();
    Console.WriteLine("Jaka tresc wiadomosci?");
    string? body = Console.ReadLine();
    if(to != null && title != null && body != null)
    {
        var email = new Email(to, title, body);
        producer.SendToQueue(email);
        return true;
    }
    return false;
}

bool CarCreator()
{
    Console.WriteLine("Jaka marka samochodu?");
    string? brand = Console.ReadLine();
    Console.WriteLine("Jaki model?");
    string? model = Console.ReadLine();
    Console.WriteLine("Pojemnosc silnika?");
    string? enginePower = Console.ReadLine();
    if (brand != null && model != null && enginePower != null)
    {
        var car = new Car(brand, model, enginePower);
        producer.SendToQueue(car);
        return true;
    }
    return false;
}