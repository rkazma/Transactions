namespace Transactions.Common
{
    public interface IQueueMessageService
    {
        Task QueueMessage<T>(T request, string queueType, bool throwException = false);
    }
}
