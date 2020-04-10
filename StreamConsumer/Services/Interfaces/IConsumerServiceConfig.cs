using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamConsumer.Services.Interfaces
{
    public interface IConsumerServiceConfig
    {
        ConsumerConfig CreateConfig();
    }
}
