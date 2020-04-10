using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamConsumer.Model
{
    public class ConsumerResult
    {
        public string Message { get; set; }
        public TopicPartitionOffset TopicOffset { get; set; }
    }
}
