using MediatR;
using Simple.Domain.Events;
using Simple.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Application.DomainEventHandler
{
    public class SetUserProfilePictureDomainEventHandler : INotificationHandler<SetUserProfilePictureDomainEvent>
    {
        IUserRepository userRepository;
        public SetUserProfilePictureDomainEventHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task Handle(SetUserProfilePictureDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await this.userRepository.GetAsync(notification.UserID);
            this.userRepository.Update(user);
            await this.userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
