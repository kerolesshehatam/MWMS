using System.Threading.Tasks;

namespace MWMS.Messaging.Infrastructure
{
    public interface IMessageHandlerCallback
    {
        Task<bool> HandleMessageAsync(string messageType, string message);
    }
}
