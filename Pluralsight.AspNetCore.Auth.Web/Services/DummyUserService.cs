using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pluralsight.AspNetCore.Auth.Web.Services
{
    public class DummyUserService : IUserService
    {
        private IDictionary<string, User> _users = new Dictionary<string, User>();


        public Task<User> GetById(string id)
        {
            if (!_users.ContainsKey(id))
                return Task.FromResult<User>(null);

            return Task.FromResult(_users[id]);
        }

        public Task<User> Create(string id, string displayName, string email)
        {
            var user = User.Create(id, displayName, email);
            _users.Add(id, user);
            return Task.FromResult(user);
        }
    }
}
