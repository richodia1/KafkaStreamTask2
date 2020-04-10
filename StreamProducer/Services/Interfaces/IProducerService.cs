using Confluent.Kafka;
using StreamProducer.Model;
using System.Threading.Tasks;

namespace StreamProducer.Services
{
    public interface IProducerService
    {
        Task<DeleveredResult> SendMessage(IProducer<string, string> producer, string topic, string message);
    }
}
