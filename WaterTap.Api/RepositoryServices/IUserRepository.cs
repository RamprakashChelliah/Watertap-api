using WaterTap.Api.Models;

namespace WaterTap.Api.RepositoryServices
{
    public interface IUserRepository
    {
        //Users details
        List<UserModel> GetAllUsers();
        UserModel GetUser(string emailId);
        string AddNewUser(UserModel user);
        Task<string> UpdateUser(string emailId, UserModel user);
    }
}