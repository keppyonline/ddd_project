using Simple.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Domain.Users
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> GetAsync(Guid userID);

        User Add(User user);

        User Update(User user);

        User Delete(UserID id);
    }
}
