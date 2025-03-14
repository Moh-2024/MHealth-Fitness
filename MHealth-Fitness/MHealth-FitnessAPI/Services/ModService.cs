using MHealth_FitnessAPI.Data;
using MHealth_Fitness.Shared.DTO;
using MHealth_FitnessAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MHealth_FitnessAPI.Services
{
    public class ModService
    {
        //instance of fitness context
        private readonly FitnessContext _fitnessContext;

        //constructor
        public ModService(FitnessContext fitnessContext)
        {
            _fitnessContext = fitnessContext;
        }

        // validate if the user is admin or mod
        private async Task<User> ValidateModeratorOrAdmin(string userName)
        {
            //finding the user in db
            var user = await _fitnessContext.user
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            //if user is null
            if (user == null)
            {
                //show error
                throw new UnauthorizedAccessException("User does not exist.");
            }

            //checking if users role name does not equal to mod or admin
            if (user.Roles.RoleName != "Moderator" && user.Roles.RoleName != "Admin")
            {
                //show error if user is not mod or admin
                throw new UnauthorizedAccessException("Only Mod or Admin");
            }

            return user; //returning the user
        }



        // Get all users
        public async Task<IEnumerable<UserDto>> GetAllUsers(string userName)
        {
            // Validate if the user is a moderator or admin
            var AuthorizedUser = await ValidateModeratorOrAdmin(userName);

            // Query the database to get all users and their roles
            var users = await _fitnessContext.user.Select(u => new  UserDto
            {
                UserID = u.UserID,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                RoleName = u.Roles.RoleName,
                RoleID = u.RoleID
            }).ToListAsync();
            // Return the list of users
            return users;
        }




        // Get all exercises
        public async Task<IEnumerable<ExerciseDto>> GetAllExercises()
        {
            //var AuthorizedUser = await ValidateModeratorOrAdmin(userName);
            var exercises = await _fitnessContext.exercise.Select(e => new ExerciseDto
            {
                ExerciseID = e.ExerciseID,
                ExerciseName = e.ExerciseName,
                ExerciseDesc = e.ExerciseDesc,
                BodyPartID = e.BodyPartID,
                ImageUrl1 = e.ImageUrl1,
                ImageUrl2 = e.ImageUrl2,
                ImageUrl3 = e.ImageUrl3

            }).ToListAsync();

            // Return the list of exercises
            return exercises;
        }

        // getting exercise by ID
        public async Task<ExerciseDto?> GetExerciseByID(int exerciseID)
        {
            // Validate if the exercise exists
            var currentExercise = await _fitnessContext.exercise.FindAsync(exerciseID);
            // If the exercise is not found, throw an exception
            if (currentExercise == null)
            {
                throw new Exception($"Exercise with ID {exerciseID} was not found");
            }

            // Convert the Exercise model to ExerciseDto
            var exerciseDto = new ExerciseDto
            {
                ExerciseID = currentExercise.ExerciseID,
                ExerciseName = currentExercise.ExerciseName,
                ExerciseDesc = currentExercise.ExerciseDesc,
                BodyPartID = currentExercise.BodyPartID,
                ImageUrl1 = currentExercise.ImageUrl1,
                ImageUrl2 = currentExercise.ImageUrl2,
                ImageUrl3 = currentExercise.ImageUrl3
            };

            // Return the ExerciseDto
            return exerciseDto;
        }
        // Add exercise
        public async Task AddExercise(string userName, ExerciseDto exerciseDto)
        {
            // Validate if the user is a moderator or admin
            var AuthorizedUser = await ValidateModeratorOrAdmin(userName);

            // Check if the body part exists
            var bodyPartExists = await _fitnessContext.bodyPart.AnyAsync(bp => bp.BodyPartID == exerciseDto.BodyPartID);
            // If the body part does not exist, throw an exception
            if (!bodyPartExists)
            {
                throw new Exception("Invalid BodyPartId. The associated body part does not exist.");
            }

            // Convert ExerciseDto to Exercise model
            var exercise = new Exercise
            {
                ExerciseName = exerciseDto.ExerciseName,
                ExerciseDesc = exerciseDto.ExerciseDesc,
                BodyPartID = exerciseDto.BodyPartID,
                ImageUrl1 = exerciseDto.ImageUrl1,
                ImageUrl2 = exerciseDto.ImageUrl2,
                ImageUrl3 = exerciseDto.ImageUrl3
            };

            // Add the new exercise to the database
            await _fitnessContext.exercise.AddAsync(exercise);
            // Save the changes to the database
            await _fitnessContext.SaveChangesAsync();
        }


        // Update exercise
        public async Task UpdateExercise(string userName, int exerciseID, ExerciseDto exercise)
        {
            // Validate if the user is a moderator or admin
            var AuthorizedUser = await ValidateModeratorOrAdmin(userName);
            // Check if the exercise exists
            var existingExercise = await _fitnessContext.exercise.FindAsync(exerciseID);

            // If the exercise does not exist, throw an exception
            if (existingExercise == null)
            {
                throw new Exception($"Exercise with ID {exerciseID} not found.");
            }
            // updating the exercise details with new values
            existingExercise.ExerciseName = exercise.ExerciseName;
            existingExercise.ExerciseDesc = exercise.ExerciseDesc;
            existingExercise.BodyPartID = exercise.BodyPartID;

            // Only update the image URLs if the new values are not null or empty
            if (!string.IsNullOrEmpty(exercise.ImageUrl1))
            {
                existingExercise.ImageUrl1 = exercise.ImageUrl1;
            }
            if (!string.IsNullOrEmpty(exercise.ImageUrl2))
            {
                existingExercise.ImageUrl2 = exercise.ImageUrl2;
            }
            if (!string.IsNullOrEmpty(exercise.ImageUrl3))
            {
                existingExercise.ImageUrl3 = exercise.ImageUrl3;
            }
            else
            {
                // If the new image URLs are null or empty, keep the existing values
                existingExercise.ImageUrl1 = existingExercise.ImageUrl1;
                existingExercise.ImageUrl2 = existingExercise.ImageUrl2;
                existingExercise.ImageUrl3 = existingExercise.ImageUrl3;
            }

            // Save the changes to the database
            await _fitnessContext.SaveChangesAsync();
        }


        // Delete exercise
        public async Task DeleteExercise(string userName, int exerciseID)
        {
            // Validate if the user is a moderator or admin
            var AuthorizedUser = await ValidateModeratorOrAdmin(userName);

            // Check if the exercise exists
            var existingExercise = await _fitnessContext.exercise.FindAsync(exerciseID);

            // If the exercise does not exist, throw an exception
            if (existingExercise == null)
            {
                throw new Exception($"Exercise with ID {exerciseID} not found.");
            }

            // Remove the exercise from the database
            _fitnessContext.exercise.Remove(existingExercise);
            // Save the changes to the database
            await _fitnessContext.SaveChangesAsync();
        }




        // Get all body parts
        public async Task<IEnumerable<BodyPartDto>> GetAllBodyParts()
        {
            // getting the list of body parts from the database
            var bodyParts = await _fitnessContext.bodyPart.Select(e => new BodyPartDto
            {
                BodyPartID = e.BodyPartID,
                BodyPartName = e.BodyPartName
            }).ToListAsync();

            // Return the list of body parts
            return bodyParts;

        }

        // Add body part
        public async Task AddBodyPart(string userName, BodyPart bodyPart)
        {
            // Validate if the user is a moderator or admin
            var AuthorizedUser = await ValidateModeratorOrAdmin(userName);

            // adding the bodypart to the database
            await _fitnessContext.bodyPart.AddAsync(bodyPart);
            // saving the changes to the database
            await _fitnessContext.SaveChangesAsync();
        }

        // Update body part
        public async Task UpdateBodyPart(string userName, int bodyPartID, BodyPart bodyPart)
        {
            // Validate if the user is a moderator or admin
            var AuthorizedUser = await ValidateModeratorOrAdmin(userName);

            // Check if the body part exists
            var existingBodyPart = await _fitnessContext.bodyPart.FindAsync(bodyPartID);

            // If the body part does not exist, throw an exception
            if (existingBodyPart == null)
            {
                throw new Exception($"Body part with ID {bodyPartID} not found.");
            }

            // updating the body part details with new values
            existingBodyPart.BodyPartName = bodyPart.BodyPartName;
            // Save the changes to the database
            await _fitnessContext.SaveChangesAsync();
        }

        // Delete body part
        public async Task DeleteBodyPart(string userName, int bodyPartID)
        {
            // Validate if the user is a moderator or admin
            var AuthorizedUser = await ValidateModeratorOrAdmin(userName);

            // Check if the body part exists
            var existingBodyPart = await _fitnessContext.bodyPart.FindAsync(bodyPartID);

            // If the body part does not exist, throw an exception
            if (existingBodyPart == null)
            {
                throw new Exception($"Body part with ID {bodyPartID} not found.");
            }
            // Remove the body part from the database
            _fitnessContext.bodyPart.Remove(existingBodyPart);
            // Save the changes to the database
            await _fitnessContext.SaveChangesAsync();
        }
    }
}
