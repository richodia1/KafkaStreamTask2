using Confluent.Kafka;
using StreamConsumer.Model;
using StreamConsumer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StreamConsumer.Services
{
    public class ConsumerService : IConsumerService
    {
        public IEnumerable<ConsumerResult> CreateConsumerAndConsume(ConsumerConfig consumerConfig, string topic, CancellationToken cts)
        {
            var result = new List<ConsumerResult>();

            var cb = new ConsumerBuilder<string, string>(consumerConfig);
            using (var consumer = cb.Build())
            {
                consumer.Subscribe(topic);
                try
                {
                    while (!cts.IsCancellationRequested)
                    {
                        var cr = consumer.Consume(cts);
                        var offset = cr.TopicPartitionOffset;
                        result.Add(new ConsumerResult { Message = cr.Message.Value, TopicOffset = offset });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    consumer.Close();
                }
            }

            return result;
        }
    }
}
