using ShephardTech.Financials.Application.Authentication;

namespace ShephardTech.Financials.Domain.Interfaces
{
    public interface IAuthenticationRepository
    {
        IEnumerable<UserModel> FindAll();
        
        Task<UserModel> FindUserById(int id);

        
        Task<string> FindUserPasswordSalt(int userId);

        Task<UserModel> FindUserByEmail(string email);

        Task<UserModel> FindUserByName(string username);

        Task<bool> AddUser(UserAccountViewModel model,string passwordSalt);

        Task<bool> AddUserToRole(int userId, List<RoleViewModel> roles);

        void LogToken(string token, DateTime expiryDate, string username);

        Task<LoginResult> LoginWithPassword(string username, string password);        

        Task<IEnumerable<RoleViewModel>> FetchRoles();

        Task<RoleViewModel> FindRoleById(int roleId);

        Task<RoleViewModel> FindRoleByRoleName(string roleName);

        Task<IEnumerable<UserRoleViewModel>> FindUserRole(int userId);

        Task<UserModel> FindUserByPhone(string phone);


    }
}
