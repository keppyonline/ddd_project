using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Application.IntegrationEvent
{
    public class NotifyUserIntegrationEvent: INotification
    {
        public NotifyUserIntegrationEvent(string emailid, string template)
        {
            this.Email = emailid;
            this.Template = template;
        }
        public string Email { get; set; }
        public string Template { get; set; }
    }
}
