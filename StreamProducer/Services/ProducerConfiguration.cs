using Confluent.Kafka;
using StreamProducer.Services.Interfaces;

namespace StreamProducer.Services
{
    public class ProducerConfiguration: IProducerConfiguration
    {   
        private IProducer<string, string> producer = null;
        private ProducerConfig producerConfig = null;
        public ProducerConfiguration()
        {

        }

        public ProducerConfig CreateConfig()
        {
            producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            
            return producerConfig;
        }

        public IProducer<string, string> CreateProducer(ProducerConfig producerConfig)
        {
            var pb = new ProducerBuilder<string, string>(producerConfig);
            producer = pb.Build();
            return producer;
        }
    }
}
