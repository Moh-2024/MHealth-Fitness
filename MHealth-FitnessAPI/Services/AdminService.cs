using MHealth_Fitness.Shared.DTO;
using MHealth_FitnessAPI.Data;
using MHealth_FitnessAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MHealth_FitnessAPI.Services
{
    public class AdminService
    {
        // storing instance of fitness context
        private readonly FitnessContext _fitnessContext;
        
        //constructor
        public AdminService(FitnessContext fitnessContext)
        {
            _fitnessContext = fitnessContext;
        }

        //checking if user is admin
        private async Task<User> ValidateAdmin(string userName)
        {
            
            var user = await _fitnessContext.user
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            //checking if user is coming as null 
            if (user == null)
            {
               //showing error
                throw new UnauthorizedAccessException("User does not exist.");
            }

            // checking if role name is admin
            if (user.Roles.RoleName != "Admin")
            {
                //if its not throw an error
                throw new UnauthorizedAccessException("Only Admin can access");
            }

            //returning user
            return user;
        }

        //getting all roles
        public async Task<IEnumerable<RoleDto>> GetAllRoles(string userName)
        {
            //calling validate admin method
            var AuthorizedUser = await ValidateAdmin(userName);

            //getting all the roles from the database
            var roles = await _fitnessContext.role.Select(u => new RoleDto
            {
                RoleID = u.RoleID,
                RoleName = u.RoleName
            }).ToListAsync();
            //returning roles
            return roles;
        }

        // Update Roles
        public async Task UpdateRole(string userName, int userID, int RoleID)
        {
            // calling validate admin  method
            var AuthorizedUser = await ValidateAdmin(userName);
            // finding the user in database using user id
            var users = await _fitnessContext.user.FindAsync(userID);
            // finding the role in database with the role id
            var role = await _fitnessContext.role.FindAsync(RoleID);

            // checking if the user is null
            if (users == null)
            {
                //showing error if it is
                throw new Exception($"Can find user ID with {userID}");
            }

            //updating the user role id with the new role id
            users.RoleID = role.RoleID;

            //saving changes in the database
            await _fitnessContext.SaveChangesAsync();
        }

        //deleting the user
        public async Task DeleteUser(string userName, int userID)
        {
            //calling the validate admin method with username passing
            var AuthorizedUser = await ValidateAdmin(userName);
            // finding the user in db with the user id
            var users = await _fitnessContext.user.FindAsync(userID);

            //checking if the user is null
            if (users == null)
            {
                //showing error if the user is null
                throw new Exception($"Cant find with user id {userID}");
            }

            //removing/deleting user from the db
            _fitnessContext.Remove(users);
            //saving changes 
            await _fitnessContext.SaveChangesAsync();
        }
    }
}