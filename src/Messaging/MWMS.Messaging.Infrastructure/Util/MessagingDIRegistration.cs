using Microsoft.Extensions.DependencyInjection;
using MWMS.Messaging.Infrastructure.RabbitMQ;

namespace MWMS.Messaging.Infrastructure
{
    public static class MessagingDIRegistration
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IMessageHandler, RabbitMQMessageHandler>();
            //services.AddTransient<IMessageHandlerCallback, RabbitMQMessageHandler>();
            services.AddTransient<IMessagePublisher, RabbitMQMessagePublisher>();

        }
    }
}
