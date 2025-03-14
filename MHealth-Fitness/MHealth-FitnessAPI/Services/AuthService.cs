using MHealth_Fitness.Shared.DTO;
using MHealth_FitnessAPI.Data;
using MHealth_FitnessAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MHealth_FitnessAPI.Services
{
    public class AuthService
    {
        // instance of fitness contwxt
        private readonly FitnessContext _fitnessContext;

        //constructor
        public AuthService(FitnessContext fitnessContext)
        {
            _fitnessContext = fitnessContext;
        }

        //user login 
        public async Task<UserDto> Login(LoginDto login)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(login.UserName) || string.IsNullOrWhiteSpace(login.Password))
            {
                throw new ArgumentException("Username and password cannot be empty.");
            }

            // Fetch the user from the database
            var user = await _fitnessContext.user
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserName == login.UserName);

            // Check if user is null
            if (user == null)
            {
                // Show error message if user is null
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            // Hash the input password
            string hashedPassword = HashPass.HashPasswordWithSalt(login.Password, user.PasswordSalt);

            // Compare the hashed password with the stored password hash
            if (hashedPassword != user.PasswordHash)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            // Return user details
            return new UserDto
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                RoleID = user.RoleID,
                RoleName = user.Roles.RoleName,
            };
        }


        //user login but checking if its admin or mod
        public async Task<UserDto> LoginAuth(LoginDto login)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(login.UserName) || string.IsNullOrWhiteSpace(login.Password))
            {
                throw new ArgumentException("Username and password cannot be empty.");
            }

            // Fetch the user from the database
            var user = await _fitnessContext.user
                .Include(u => u.Roles) // Ensure Roles are loaded
                .FirstOrDefaultAsync(u => u.UserName == login.UserName);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            // hashing the password
            string hashedPassword = HashPass.HashPasswordWithSalt(login.Password, user.PasswordSalt);

            // Check if the user has the required role
            if (user.Roles.RoleName != "Admin" && user.Roles.RoleName != "Moderator")
            {
                throw new UnauthorizedAccessException("Access denied. You do not have the required role.");
            }
 
            //Return user details
            return new UserDto
            {
                UserName = user.UserName,
                RoleID = user.Roles.RoleID,
                RoleName = user.Roles.RoleName
            }; 
        }

        // verify user for forgot password
        public async Task<UserDto> VerifyUser(string userName)
        {
            //checking if the username containes null or empty
            if (string.IsNullOrWhiteSpace(userName))
            {
                //showing error
                throw new ArgumentException("Username cannot be empty.");
            }

            // finding username in database
            var user = await _fitnessContext.user.FirstOrDefaultAsync(u => u.UserName == userName);
            //checking if user is null
            if (user == null)
            {
                //user not found error
                throw new Exception("User not found.");
            }

            //returning username 
            return new UserDto
            {
                UserName = user.UserName,
            };
        }

        // forgot password
        public async Task<UserDto> ForgotPass(string userName, ForgotPassDto forgotPass)
        {
            //checking if username or password contains empty string or null
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(forgotPass.Password))
            {
                //showing error
                throw new ArgumentException("Username and password cannot be empty.");
            }

            // finding user in db
            var user = await _fitnessContext.user.FirstOrDefaultAsync(u => u.UserName == userName);

            //if user is null
            if (user == null)
            {
                // showing error
                throw new UnauthorizedAccessException("User not found.");
            }

            //updating password and hashing it
            user.PasswordHash = HashPass.HashPasswordWithSalt(forgotPass.Password, user.PasswordSalt);


            //updating user's password in db
            _fitnessContext.user.Update(user);
            //saving changes in db
            await _fitnessContext.SaveChangesAsync();

            //returning username
            return new UserDto
            {
                UserName = user.UserName,
            };
        }

        

    }
}
