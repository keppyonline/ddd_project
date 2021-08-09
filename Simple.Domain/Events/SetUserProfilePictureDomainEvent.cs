using MediatR;
using Simple.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Domain.Events
{
    public class SetUserProfilePictureDomainEvent : IDomainEvent
    {
        public SetUserProfilePictureDomainEvent(Guid userID)
        {
            UserID = userID;
        }

        public Guid UserID { get; set; }

        public DateTime OccurredOn => DateTime.Now;
    }
}
