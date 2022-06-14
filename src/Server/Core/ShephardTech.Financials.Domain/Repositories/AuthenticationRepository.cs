using Microsoft.EntityFrameworkCore;
using ShephardTech.Financials.Application.Authentication;
using ShephardTech.Financials.Domain.Interfaces;
using ShephardTech.Financials.Entities;
using ShephardTech.Financials.Persistence.StorageContexts.Financials;

namespace ShephardTech.Financials.Domain.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ShepardFinContext _context;
        public AuthenticationRepository(ShepardFinContext context)
        {
            _context = context;
        }

        public async  Task<bool> AddUser(UserAccountViewModel model, string passwordSalt)
        {
            var user = new ShepardUser()
            {
                Username = model.username,
                Email = model.email,
                MobilePhone = model.mobilePhone,
                Password = model.password,
                PasswordSalt = passwordSalt,
                DateCreated =DateTime.Now,
                FailPasswordCount = 0,
                Deleted =false,
                IsActive = true,
                IsLockedOut = false,
                CreatedBy = "System"
            };

            await _context.Users.AddAsync(user);

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> AddUserToRole(int userId, List<RoleViewModel> roles)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RoleViewModel>> FetchRoles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> FindAll()
        {
            throw new NotImplementedException();
        }

        public async Task<RoleViewModel> FindRoleById(int roleId)
        {
            throw new NotImplementedException();
        }

        public async Task<RoleViewModel> FindRoleByRoleName(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> FindUserByEmail(string email)
        {
            return await _context.Users.AsNoTracking()
                .Where(u => u.Email == email).Select(x => new UserModel()
                {
                    userId = x.UserId,
                    email = x.Email,
                    username = x.Username,
                    mobilePhone = x.MobilePhone

                }).FirstOrDefaultAsync();
        }

        public async Task<UserModel> FindUserById(int id)
        {
            return await _context.Users.AsNoTracking()
                .Where(u => u.UserId == id).Select(x => new UserModel()
                {
                    userId = x.UserId,
                    email = x.Email,
                    username = x.Username,
                    mobilePhone = x.MobilePhone

                }).FirstOrDefaultAsync();
        }

        public async Task<UserModel> FindUserByName(string username)
        {
            return await _context.Users.AsNoTracking()
                 .Where(u => u.Username ==username).Select(x => new UserModel()
                 {
                     userId = x.UserId,
                     email = x.Email,
                     username = x.Username,
                     mobilePhone = x.MobilePhone

                 }).FirstOrDefaultAsync();
        }

        public async Task<UserModel> FindUserByPhone(string phone)
        {
            return await _context.Users.AsNoTracking()
                .Where(u => u.MobilePhone == phone).Select(x => new UserModel()
                {
                    userId = x.UserId,
                    email = x.Email,
                    username = x.Username,
                    mobilePhone = x.MobilePhone

                }).FirstOrDefaultAsync();
        }

        public async Task<string> FindUserPasswordSalt(int userId)
        {
            return  await _context.Users.AsNoTracking().Where(x=>x.UserId==userId)
                .Select(x => x.PasswordSalt).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserRoleViewModel>> FindUserRole(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResult> LoginWithPassword(string username, string password)
        {
            var potentialUser = await _context.Users.AsNoTracking()
                .Where(u => u.Username == username).FirstOrDefaultAsync();

            if(potentialUser==null)
                potentialUser = await _context.Users.AsNoTracking()
                    .Where(u => u.Email == username).FirstOrDefaultAsync();


            if (potentialUser == null) return new LoginResult
            {
                status = LoginStatus.Failed,
                message = "Wrong username or password"
            };


            if (!potentialUser.IsActive)
                return new LoginResult
                {
                    status = LoginStatus.Deactivated,
                    message = "Your account has been deactivated, please contact your administrator"
                };

            if (potentialUser.IsLockedOut)
                return new LoginResult
                {
                    status = LoginStatus.AccountLocked,
                    message = "Your account has been locked, please contact your administrator"
                };

            if (!potentialUser.Password.Equals(password))
            {
                return new LoginResult
                {
                    status = LoginStatus.Failed,
                    message = "Wrong username or password"
                };
            }

            var userRoles = (from r in _context.Roles
                             join ur in _context.UserRoles on r.RoleId equals ur.RoleId
                             where ur.UserId == potentialUser.UserId
                             select new UserRoleViewModel
                             {
                                 roleId = r.RoleId,
                                 roleName = r.RoleName
                             }).ToList();


            return new LoginResult
            {
                status = LoginStatus.Success,
                message = "Login success",
                user = new UserModel
                {
                    userId = potentialUser.UserId,
                    email = potentialUser.Email,
                    mobilePhone = potentialUser.MobilePhone,
                    username = potentialUser.Username,
                    roles = userRoles.Select(x => x.roleName).ToList()
                }
            };


        }

        public void LogToken(string token, DateTime expiryDate, string username)
        {
            var logToken = new LoginToken()
            {
                AuthToken = token,
                ExpiryDate = expiryDate,
                Username = username
            };

            _context.LoginTokens.Add(logToken);

            _context.SaveChanges();
        }
    }
}
