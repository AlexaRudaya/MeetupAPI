namespace Meetup.ApplicationCore.Interfaces.RabbitMQ
{
    public interface IProducerService
    {
        public void SendEventMessage<T>(T message) where T : class;

        public void SendSponsorMessage<T>(T message) where T : class;

        public void SendSpeakerMessage<T>(T message) where T : class;
    }
}
