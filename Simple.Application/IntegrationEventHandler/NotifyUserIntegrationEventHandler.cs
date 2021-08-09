using MediatR;
using Simple.Application.IntegrationEvent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Application.IntegrationEventHandler
{
    public class NotifyUserIntegrationEventHandler : INotificationHandler<NotifyUserIntegrationEvent>
    {
        public NotifyUserIntegrationEventHandler()
        {

        }

        public Task Handle(NotifyUserIntegrationEvent notification, CancellationToken cancellationToken)
        {
            //send notification: api call.
            throw new NotImplementedException();
        }
    }
}
