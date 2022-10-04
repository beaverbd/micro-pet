using Convey.MessageBrokers;

namespace Common.MessageBroker;

public class ApplicationBusPublisher : IApplicationBusPublisher
{
    private readonly IBusPublisher _publisher;

    public ApplicationBusPublisher(IBusPublisher publisher)
    {
        _publisher = publisher;
    }

    public Task PublishMessageAsync<T>(T message) where T : class => _publisher.PublishAsync(message);
}
