using System.Threading.Tasks;

namespace Pluralsight.AspNetCore.Auth.Web.Services
{
    public interface IUserService
    {
        Task<User> GetById(string id);
        Task<User> Create(string id, string displayName, string email);

    }
}
