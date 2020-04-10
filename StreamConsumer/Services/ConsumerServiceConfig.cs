using Confluent.Kafka;
using StreamConsumer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamConsumer.Services
{
    public class ConsumerServiceConfig : IConsumerServiceConfig
    {
        public ConsumerServiceConfig()
        {

        }

        public ConsumerConfig CreateConfig()
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            return consumerConfig;
        }
    }
}
