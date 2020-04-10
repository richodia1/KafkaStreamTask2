using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StreamConsumer.Services;
using StreamProducer.Model;
using StreamProducer.Services;
using StreamProducer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamProducer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Kafka peoducer.");

            var services = ConfigureServiceDependency();
            var serviceProvider = services.BuildServiceProvider();

            var producerConfigService = serviceProvider.GetService<IProducerConfiguration>();
            var producerService = serviceProvider.GetService<IProducerService>();
            var producerConfig = producerConfigService.CreateConfig();
            var producer = producerConfigService.CreateProducer(producerConfig);

            Console.WriteLine("Ready to send Car Messages .");
            const string topic = "cars";
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                Console.WriteLine("Enter Brand Name:");
                string brandName = Console.ReadLine();
                Console.WriteLine("Enter Model:");
                string model = Console.ReadLine();
                Console.WriteLine("Enter Number Of Doors:");
                int.TryParse(Console.ReadLine(), out int numberOfDoors);
                Console.WriteLine("Press Y if the Car Is Sport Car");
                string value = Console.ReadLine();
                bool IsSportCar = (value.ToLower() == "Y".ToLower()) ? true : false;

                var car = new Car
                {
                    BrandName = brandName,
                    Model = model,
                    NumberOfDoors = numberOfDoors,
                    IsSportCar = IsSportCar
                };

                //Serilialize the message 
                var message = JsonConvert.SerializeObject(car);
                var deleveredResult = await producerService.SendMessage(producer, topic, message);

                Console.WriteLine("---------Delevered Result after:---------");
                Console.WriteLine($"Delivered '{deleveredResult.Result}' to: {deleveredResult.TopicOffset}");
                Console.WriteLine();
                Console.WriteLine("Add another Car!");
            }
        }

        private static IServiceCollection ConfigureServiceDependency()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IProducerConfiguration, ProducerConfiguration>();
            services.AddScoped<IProducerService, ProducerService>();

            return services;
        }
    }
}
