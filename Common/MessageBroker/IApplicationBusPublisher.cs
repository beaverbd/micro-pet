namespace Common.MessageBroker;

public interface IApplicationBusPublisher
{
    Task PublishMessageAsync<T>(T message) where T : class;
}