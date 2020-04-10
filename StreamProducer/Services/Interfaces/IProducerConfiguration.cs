using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamProducer.Services.Interfaces
{
    public interface IProducerConfiguration
    {
        ProducerConfig CreateConfig();
        IProducer<string, string> CreateProducer(ProducerConfig producerConfig);
    }
}
