using Confluent.Kafka;
using StreamConsumer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StreamConsumer.Services.Interfaces
{
    public interface IConsumerService
    {
        IEnumerable<ConsumerResult> CreateConsumerAndConsume(ConsumerConfig consumerConfig, string topic, CancellationToken cts);
    }
}
