using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamProducer.Model
{
    public class DeleveredResult
    {
        public string Result { get; set; }
        public TopicPartitionOffset TopicOffset { get; set; }
    }
}
