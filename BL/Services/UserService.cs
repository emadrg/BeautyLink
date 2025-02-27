using BL.UnitOfWork;
using Common.AppSettings;
using Common.Constants;
using Common.Enums;
using Common.Interfaces;
using DA.Entities;
using DTOs.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text;
using Utils;

namespace BL.Services
{
    public class UserService : BaseService
    {
     
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public UserService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<UserDetailsDTO?> GetUserDetailsById(Guid id)
        {
            var user = await UnitOfWork.Queryable<User>().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) 
            { 
                return null; 
            }
            return Mapper.Map<User, UserDetailsDTO>(user);
        }

        public async Task<UserDetailsDTO?> GetUserDetailsByCredentials(string email, string password)
        {
            var user = await UnitOfWork.Queryable<User>()
                .Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user.ProfilePicture == null)
            {
                user.ProfilePicture = UnitOfWork.Queryable<AppFile>().FirstOrDefault(af => af.Id == UserConstants.NoProfilePictureId);
                    
            }

            if (user == null || !(await SecurityExtensions.ComparePasswords(user.Password, email, password, user.Id.ToString())))
            {
                return null;
            }
            return Mapper.Map<User, UserDetailsDTO>(user);
        }

        public async Task<string> GetUserProfilePicture(Guid id)
        {
            var user = await UnitOfWork.Queryable<User>()
                .Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            if (user.ProfilePicture == null)
            {
                return "no_profile_picture.jpg";
            }



            return $"{user.ProfilePicture.Name}.{user.ProfilePicture.Extesion}";
        }

        public async Task<List<UserListItemDTO>> GetUserList(IQueryBuilder query)
        {
            return await Mapper.Map<User, UserListItemDTO>(UnitOfWork.Repository<User>().GetByQuery(query)).ToListAsync();
        }

        public async Task<int> ResetPassword(Guid userId)
        {
            var user = await UnitOfWork.Queryable<User>().FirstOrDefaultAsync(u => u.Id == userId);
            user.Password = await SecurityExtensions.GetPasswordHashString(user.Email, "Copernic@1234", user.Id.ToString());
            UnitOfWork.Repository<User>().Update(user);
            return await Save();
        }

        public async Task<int> RegisterUser(RegisterUserDTO newUser)
        {
            var newDbUser = Mapper.Map<RegisterUserDTO, User>(newUser);
            newDbUser.CreatedBy = newDbUser.Id;
            newDbUser.LastModifiedBy = newDbUser.Id;
            newDbUser.Password = await SecurityExtensions.GetPasswordHashString(newDbUser.Email, newUser.Password, newDbUser.Id.ToString());
            if(newUser.ProfilePicture != null)
            {
                var picture = newUser.ProfilePicture;
                var pictureObject = new AppFile
                {
                    Extesion = picture.FileName.Split(".")[1],
                    Path = $"{FileUtils.BasePath}{picture.FileName}", 
                    Name = picture.FileName.Split(".")[0]

                };
                newDbUser.ProfilePicture = pictureObject; 
                FileUtils.CreateFile(newUser.ProfilePicture);
            }
            UnitOfWork.Repository<User>().Add(newDbUser);
            return await Save();
        }

       
        public async Task<int?> DeactivateUser(Guid userId)
        {
            var dbUser = await UnitOfWork.Queryable<User>().FirstOrDefaultAsync(u => u.Id == userId);
            if (dbUser == null)
            {
                return null;
            }
            dbUser.StatusId = (int)UserStatusTypes.Inactive;
            dbUser.LastModifiedBy = CurrentUser.Id();
            dbUser.LastModifiedDate = DateTime.Now;
            UnitOfWork.Repository<User>().Update(dbUser);

            return await Save();
        }
        public async Task<int?> DeleteUser(Guid userId)
        {
            var dbUser = await UnitOfWork.Queryable<User>().FirstOrDefaultAsync(u => u.Id == userId);
            if (dbUser == null)
            {
                return null;
            }
            UnitOfWork.Repository<User>().Remove(dbUser);

            return await Save();
        }

        public async Task<bool> EmailIsUsed(string email)
        {
            return await UnitOfWork.Queryable<User>().AnyAsync(u => u.Email == email);
        }

        public async Task<List<DispalyUserDTO>> GetAllUsers(int? roleId, int skip, int take)
        {
            var dbUsers =  await UnitOfWork.Queryable<User>()
                .Where(u => roleId == null|| u.RoleId == roleId)
                  .Skip(skip)
                  .Take(take)
                .ToListAsync();

            return Mapper.Map<User, DispalyUserDTO>(dbUsers);

        }

        public async Task<int?> UpdateUserDetails (DispalyUserDTO userDTO)
        {
            var currentUserId = CurrentUser.Id();

            var dbUser = await UnitOfWork.Queryable<User>()
                .Where(u => u.Id == userDTO.GuidId)
                .FirstOrDefaultAsync();

            if (dbUser == null)
            {
                return null;
            }

            dbUser.Id = userDTO.GuidId;
            dbUser.FirstName = userDTO.FirstName;
            dbUser.LastName = userDTO.LastName;
            dbUser.Email = userDTO.Email;
            dbUser.RoleId = userDTO.RoleId;
            dbUser.StatusId = userDTO.StatusId;
            dbUser.LastModifiedBy = currentUserId;
            dbUser.LastModifiedDate = DateTime.Now;

            UnitOfWork.Repository<User>().Update(dbUser);
            return await Save();
        }


/*        public async Task<List<DispalyUserDTO>> GetLeastActiveClientsStylists()
        {
            var users = await UnitOfWork.Queryable<Stylist>()
                .Include(s => s.ServiceStylists)
                    .ThenInclude(ss => ss.Appointments)
                 .Where(s => s.ServiceStylists.Any(ss => ss.Appointments.Where(a => a.EndDate)))

        }
*/

    }
}
