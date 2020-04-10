using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using StreamConsumer.Services;
using StreamConsumer.Services.Interfaces;
using System;
using System.Threading;

namespace StreamConsumer
{
    class Program
    {
        private static CancellationTokenSource cts = new CancellationTokenSource();
        static void Main(string[] args)
        {
            var services = ConfigureServiceDependency();
            var serviceProvider = services.BuildServiceProvider();
            var configService = serviceProvider.GetService<IConsumerServiceConfig>();
            var consumerConfig = configService.CreateConfig();

            Console.WriteLine("Press Ctrl+C to exit");
            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);

            const string topic = "cars";
            var consumerService = serviceProvider.GetService<IConsumerService>();
            var result = consumerService.CreateConsumerAndConsume(consumerConfig, topic, cts.Token);
            
            foreach (var item in result)
            {
                Console.WriteLine($"Message '{item.Message}' at: '{item.TopicOffset}'.");
            }

            Console.WriteLine("Hello World!");
        }

        private static IServiceCollection ConfigureServiceDependency()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IConsumerServiceConfig, ConsumerServiceConfig>();
            services.AddScoped<IConsumerService, ConsumerService>();

            return services;
        }

        private static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            Console.WriteLine("In OnExit");
            cts.Cancel();

        }
    }
}
