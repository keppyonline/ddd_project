using MediatR;
using Simple.Application.IntegrationEvent;
using Simple.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Application.Users.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {

        private readonly IUserRepository _userRepository;
        private readonly IMediator mediator;
        public CreateUserCommandHandler(IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            this.mediator = mediator;
        }
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // adding domain event inside.
            var user = User.Create(request.FirstName, request.LastName, request.Email);


            var userResult = this._userRepository.Add(user);

            // invoke domain event handler(s)
            var result = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            if(result)
            {
                //sending mail notification using integration event handler.
                var notificationEvent = new NotifyUserIntegrationEvent(request.Email, "Test_Email_Template");
                await mediator.Publish(notificationEvent);
            }
            return userResult.Id.Value;

        }
    }
}
