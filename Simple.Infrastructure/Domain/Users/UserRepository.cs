using MediatR;
using Npgsql;
using NpgsqlTypes;
using Simple.Application.Configuration.Data;
using Simple.Domain.SeedWork;
using Simple.Domain.Users;
using Simple.Infrastructure.DomainContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Infrastructure.Domain.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext unitofWork;
        private readonly IMediator mediator;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return unitofWork;
            }
        }

        public UserRepository (UserContext context, IMediator mediator)
        {
            this.unitofWork = context ?? throw new ArgumentNullException(nameof(context));
            this.mediator = mediator;
        }

        public async Task<User> GetAsync(Guid userID)
        {
            return await Task.FromResult(this.unitofWork.user).ConfigureAwait(false);
        }
        public User Add(User user)
        {
            this.unitofWork.user = user;
            return this.unitofWork.user;
        }

        public User Update(User user)
        {
            this.unitofWork.user = user;
            return this.unitofWork.user;
        }

        public User Delete(UserID id)
        {
            var result = (this.unitofWork.user.Id == id);
            return result ? this.unitofWork.user : null;
        }
    }
}
