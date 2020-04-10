using Confluent.Kafka;
using StreamProducer.Model;
using StreamProducer.Services;
using StreamProducer.Services.Interfaces;
using System.Threading.Tasks;

namespace StreamConsumer.Services
{
    public class ProducerService: IProducerService
    {
        public ProducerService()
        {
        }

        public async Task<DeleveredResult> SendMessage(IProducer<string, string> producer, string topic, string message)
        {
            var msg = new Message<string, string>
            {
                Key = null,
                Value = message
            };

            var delRep = await producer.ProduceAsync(topic, msg);
            var topicOffset = delRep.TopicPartitionOffset;

            return new DeleveredResult
            {
                Result = delRep.Value,
                TopicOffset = topicOffset
            };
        }
    }
}
