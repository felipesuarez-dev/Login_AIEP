using API.Models;

namespace API.Services.Interfaces
{
    public interface ILogin
    {
        public string CreateUser(Login login);

        public string GetUser(Login login);
    }
}
