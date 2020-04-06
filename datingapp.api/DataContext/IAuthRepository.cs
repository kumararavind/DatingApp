using System.Threading.Tasks;
using datingapp.api.Models;

namespace datingapp.api.DataContext
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);

         Task<User> Login(string Username, string password);
    

         Task<bool> UserExists(string Username);

    }
}