﻿namespace MWMS.Messaging.Infrastructure
{
    public interface IMessageHandler
    {
        void Start(IMessageHandlerCallback callback);
        void Stop();
    }
}
