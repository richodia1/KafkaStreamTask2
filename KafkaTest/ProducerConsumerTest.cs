using Confluent.Kafka;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using StreamConsumer.Model;
using StreamConsumer.Services;
using StreamConsumer.Services.Interfaces;
using StreamProducer.Model;
using StreamProducer.Services;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaTest
{
    public class Tests
    {
        private Mock<IProducer<string, string>> producerMock;

        [SetUp]
        public void Setup()
        {
            producerMock = new Mock<IProducer<string, string>>();
        }

        [Test]
        [Category("Unit Test")]
        public async Task SendMessage_WithValidParameter_ShouldSucceed()
        {
            //arrange
            var car = new Car
            {
                BrandName = "Jeep",
                Model = "2017",
                IsSportCar = true,
                NumberOfDoors = 2
            };

            var message = JsonConvert.SerializeObject(car);
            var deliveryResult = new DeliveryResult<string, string>
            {
                Message = new Message<string, string>
                {
                    Value = message,
                }
            };

            producerMock.Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>(), CancellationToken.None))
                        .Returns(Task.FromResult(deliveryResult));
            var producer = new ProducerService();
            //Act
            var deleveredMessage = await producer.SendMessage(producerMock.Object, "cars", message);
            var carResponse = JsonConvert.DeserializeObject<Car>(deleveredMessage.Result);
            //Asssert
            Assert.IsNotNull(deleveredMessage);
            Assert.AreEqual(deleveredMessage.Result, message);
            Assert.AreEqual(car.BrandName, carResponse.BrandName);
            Assert.AreEqual(car.Model, carResponse.Model);
            Assert.AreEqual(car.IsSportCar, carResponse.IsSportCar);
            Assert.AreEqual(car.NumberOfDoors, carResponse.NumberOfDoors);
        }

        [Test]
        [Category("Integration Test")]
        public async Task ProducerConsumerIntegrationTest()
        {
            //arrange
            var car = new Car
            {
                BrandName = "Jeep",
                Model = "2017",
                IsSportCar = true,
                NumberOfDoors = 2
            };

            var message = JsonConvert.SerializeObject(car);

            //producer
            var config = new ProducerConfiguration();
            var producerConfig = config.CreateConfig();
            var producer = config.CreateProducer(producerConfig);
            var producerService = new ProducerService();
            var deleveredResponse = await producerService.SendMessage(producer, "cars", message);
            Assert.IsNotNull(deleveredResponse);
            Assert.IsNotNull(deleveredResponse.TopicOffset);
            Assert.AreEqual(message, deleveredResponse.Result);
            var carResponse = JsonConvert.DeserializeObject<Car>(deleveredResponse.Result);
            Assert.AreEqual(car.BrandName, carResponse.BrandName);
            Assert.AreEqual(car.Model, carResponse.Model);
            Assert.AreEqual(car.IsSportCar, carResponse.IsSportCar);
            Assert.AreEqual(car.NumberOfDoors, carResponse.NumberOfDoors);

            //Consumer
            var consumerServiceConfig = new ConsumerServiceConfig();
            var consumerConfig = consumerServiceConfig.CreateConfig();
            var consumerService = new ConsumerService();
            // CancellationTokenSource cts = new CancellationTokenSource();
            var consumerResult = consumerService.CreateConsumerAndConsumeSingleMessage(consumerConfig, "cars", CancellationToken.None);
            Assert.IsNotNull(consumerResult);

            Assert.IsNotNull(consumerResult.TopicOffset);
            Assert.AreEqual(message, consumerResult.Message);
            var carConsumerResponse = JsonConvert.DeserializeObject<Car>(deleveredResponse.Result);
            Assert.IsNotNull(carConsumerResponse);
            Assert.AreEqual(car.BrandName, carConsumerResponse.BrandName);
            Assert.AreEqual(car.Model, carConsumerResponse.Model);
            Assert.AreEqual(car.IsSportCar, carConsumerResponse.IsSportCar);
            Assert.AreEqual(car.NumberOfDoors, carConsumerResponse.NumberOfDoors);
        }
    }
}