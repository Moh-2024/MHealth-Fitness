using MHealth_Fitness.Shared.DTO;
using MHealth_FitnessAPI.Data;
using MHealth_FitnessAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MHealth_FitnessAPI.Services
{
    public class UserService
    {
        // instance of fitness context
        private readonly FitnessContext _fitnessContext;

        // constructor
        public UserService(FitnessContext fitnessContext)
        {
            _fitnessContext = fitnessContext;
        }

//User

        //add a user
        public async Task<User> Register(RegisterDto userDto)
        {
            // checking if username already exists in db
            var existingUser = await _fitnessContext.user
             .FirstOrDefaultAsync(u => u.UserName == userDto.UserName);

            // If username already exists throw an error
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }

            // hashing the password
            var (hashedPassword, salt) = HashPass.HashPassword(userDto.Password);

            // new user
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                RoleID = userDto.RoleID // Default to 3
            };

            // adding new user to db
            await _fitnessContext.user.AddAsync(user);
            // saving new user
            await _fitnessContext.SaveChangesAsync();

            return user;
        }

        // getting user by ID
        public async Task<User?> GetByID(int userID)
        {
            // finding user in db with user id
            var currentUser = await _fitnessContext.user.FindAsync(userID);
            // checking if user is null
            if (currentUser == null)
            {
                // showing error
                throw new Exception($"User Id with {userID} was not found");
            }

            //returning specefic user
            return currentUser;
        }

        // updating user by ID
        public async Task UpdatUser(int userID, UserDto user)
        {
            // find the user in db
            var currentUser = await _fitnessContext.user.FindAsync(userID);
            // if user is null
            if (currentUser == null)
            {
                throw new Exception($"User Id with {userID} was not found");
            }

            //hashing the password
            var (hashedPassword, salt) = HashPass.HashPassword(user.Password);

            // updating the existing values with new values
            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.UserName = user.UserName;
            currentUser.PasswordHash = hashedPassword;
            currentUser.PasswordSalt = salt;

            //saving changes to the db
            await _fitnessContext.SaveChangesAsync();
        }


// Routine


        // Adding a Routine
        public async Task<Routine> AddRoutine(RoutineDto routineDto)
        {
            // finding the user in db
            var existingUser = await _fitnessContext.user
           .FirstOrDefaultAsync(u => u.UserID == routineDto.UserID);

            // new routine with values
            var routine = new Routine
            {
                RoutineName = routineDto.RoutineName,
                UserID = routineDto.UserID
            };
            //adding the routine to db
            await _fitnessContext.routine.AddAsync(routine);
            //saving the changes
            await _fitnessContext.SaveChangesAsync();  
            
            return routine;
        }

        // Get All routines
        public async Task<IEnumerable<Routine>> GetAllRoutine(int userID)
        {
            var currentUser = await _fitnessContext.user.FindAsync(userID);

            // If user does not exist, throw a specific exception
            if (currentUser == null)
            {
                throw new KeyNotFoundException($"User with ID {userID} was not found.");
            }

            // Return list of all routines for the user
            return await _fitnessContext.routine
            .Where(r => r.UserID == userID) // Filter by UserId
            .ToListAsync();
        }

        // get routine by ID
        public async Task<Routine?> GetRoutineByID(int routineID)
        {
            // find the routine in db
            var currentRoutine = await _fitnessContext.routine.FindAsync(routineID);

            // if routine is null
            if (currentRoutine == null)
            {
                throw new Exception($"Routine with {routineID} was not found.");
            }
            // return the routine
            return currentRoutine;
        }

        // update routine by ID
        public async Task UpdateRoutine(int routineID, RoutineDto routine)
        {
            // find the routine in db
            var currentRoutine = await _fitnessContext.routine.FindAsync(routineID);
            // if routine is null
            if (currentRoutine == null)
            {
                throw new Exception($"Routine with the {routineID} was not found");
            }

            // updating the existing values with new values
            currentRoutine.RoutineName = routine.RoutineName;

            // saving changes to the db
            await _fitnessContext.SaveChangesAsync();
        }

        // delete routine by ID
        public async Task DeleteRoutine(int routineID)
        {
            // find the routine in db
            var currentRoutine = await _fitnessContext.routine.FindAsync(routineID);
            // if routine is null
            if (currentRoutine == null)
            {
                throw new Exception($"Routine with the {routineID} was not found");
            }
            // remove the routine
            _fitnessContext.Remove(currentRoutine);
            // save the changes
            await _fitnessContext.SaveChangesAsync();
        }


//Routine Exercise


        // Get all routine exercises
        public async Task<IEnumerable<RoutineExercise>> GetAllRoutineExercise(int routineID)
        {
            // Find the routine in the database
            var currentRoutine = await _fitnessContext.routine.FindAsync(routineID);
            // If routine does not exist, throw a specific exception
            if (currentRoutine == null)
            {
                throw new KeyNotFoundException($"Routine with ID {routineID} was not found.");
            }

            // Return list of all routine exercises for the routine
            return await _fitnessContext.routineExercise
            .Where(r => r.RoutineID == routineID) // Filter by rouitineID
            .ToListAsync();
        }

        // Add a routine exercise
        public async Task AddRoutineExercise(RoutineExercise routineExercise)
        {
            // Find the routine in the database
            await _fitnessContext.routineExercise.AddAsync(routineExercise);

            // Save the changes
            await _fitnessContext.SaveChangesAsync();
        }

        // Get routine exercise by ID
        public async Task<RoutineExercise?> GetRoutineexerciseByID(int routineExerciseID)
        {
            // Find the routine exercise in the database
            var currentRoutineExercise = await _fitnessContext.routineExercise.FindAsync(routineExerciseID);
            // If routine exercise does not exist, throw a specific exception
            if (currentRoutineExercise == null)
            {
                throw new Exception($"Routine exercise with {routineExerciseID} was not found.");
            }
            // Return the routine exercise
            return currentRoutineExercise;
        }

        // Update routine exercise by ID
        public async Task UpdateRoutineExercise(int routineExerciseID, RoutineExercise routineExercise)
        {
            // Find the routine exercise in the database
            var currentRoutineExercise = await _fitnessContext.routineExercise.FindAsync(routineExerciseID);
            // If routine exercise does not exist, throw a specific exception
            if (currentRoutineExercise == null)
            {
                throw new Exception($"Routine with the {routineExerciseID} was not found");
            }
            // Update the existing values with new values
            currentRoutineExercise.Date = routineExercise.Date;

            // Save the changes
            await _fitnessContext.SaveChangesAsync();
        }

        // Delete routine exercise by ID
        public async Task DeleteRoutineExercise(int routineExerciseID)
        {
            // Find the routine exercise in the database
            var currentRoutineExercise = await _fitnessContext.routineExercise.FindAsync(routineExerciseID);
            // If routine exercise does not exist, throw a specific exception
            if (currentRoutineExercise == null)
            {
                throw new Exception($"Routine exercise with the {routineExerciseID} was not found");
            }
            // Remove the routine exercise
            _fitnessContext.Remove(currentRoutineExercise);
            // Save the changes
            await _fitnessContext.SaveChangesAsync();
        }

        //SetsPerExercise

        // Get all sets for a given RoutineExercise ID
        public async Task<IEnumerable<SetsPerExerciseDto>> GetAllSetsPerExercise(int routineExerciseID)
        {
            var routineExercise = await _fitnessContext.routineExercise.FindAsync(routineExerciseID);
            if (routineExercise == null)
            {
                throw new KeyNotFoundException($"Routine exercise with ID {routineExerciseID} was not found.");
            }

            return await _fitnessContext.setsPerExercise
                .Where(s => s.RoutineExerciseId == routineExerciseID)
                .Select(s => new SetsPerExerciseDto
                {
                    SetId = s.SetId,
                    RoutineExerciseId = s.RoutineExerciseId,
                    SetNumber = s.SetNumber,
                    Reps = s.Reps,
                    Weight = (double)s.Weight
                })
                .ToListAsync();
        }

        // Add a new set to a routine exercise
        public async Task AddSetPerExercise(SetsPerExerciseDto setDto)
        {
            // Get the maximum set number for the given RoutineExerciseId
            int maxSetNumber = await _fitnessContext.setsPerExercise
                .Where(s => s.RoutineExerciseId == setDto.RoutineExerciseId)
                .Select(s => (int?)s.SetNumber)
                .MaxAsync() ?? 0;

            var set = new SetsPerExercise
            {
                RoutineExerciseId = setDto.RoutineExerciseId,
                SetNumber = maxSetNumber + 1,
                Reps = setDto.Reps,
                Weight = (decimal)setDto.Weight
            };

            await _fitnessContext.setsPerExercise.AddAsync(set);
            await _fitnessContext.SaveChangesAsync();
        }

        // Get a specific set by ID
        public async Task<SetsPerExerciseDto?> GetSetPerExerciseByID(int setID)
        {
            var set = await _fitnessContext.setsPerExercise.FindAsync(setID);
            if (set == null)
            {
                throw new Exception($"Set with ID {setID} was not found.");
            }

            return new SetsPerExerciseDto
            {
                SetId = set.SetId,
                RoutineExerciseId = set.RoutineExerciseId,
                SetNumber = set.SetNumber,
                Reps = set.Reps,
                Weight = (double)set.Weight
            };
        }

        // Update a set by ID
        public async Task UpdateSetPerExercise(int setID, SetsPerExerciseDto setDto)
        {
            var existingSet = await _fitnessContext.setsPerExercise.FindAsync(setID);
            if (existingSet == null)
            {
                throw new Exception($"Set with ID {setID} was not found.");
            }

            // Update fields
            existingSet.Reps = setDto.Reps;
            existingSet.Weight = (decimal)setDto.Weight;

            await _fitnessContext.SaveChangesAsync();
        }

        // Delete a set by ID
        public async Task DeleteSetPerExercise(int setID)
        {
            var existingSet = await _fitnessContext.setsPerExercise.FindAsync(setID);
            if (existingSet == null)
            {
                throw new Exception($"Set with ID {setID} was not found.");
            }

            _fitnessContext.setsPerExercise.Remove(existingSet);
            await _fitnessContext.SaveChangesAsync();
        }

        //Favorite


        // Get all favorites
        public async Task<IEnumerable<Favorite>> GetByFavoriteID(int userID)
		{
            // Find the user in the database
            var currentUser = await _fitnessContext.user.FindAsync(userID);

            // If user does not exist, throw a specific exception
            if (currentUser == null)
            {
                throw new KeyNotFoundException($"User with ID {userID} was not found.");
            }

            // Return list of all favorites for the user
            return await _fitnessContext.favorite
            .Where(f => f.UserID == userID)
		    .ToListAsync();
			
		}

        // Get favorite by ID
        public async Task<Favorite?> GetFavoriteByUserAndExercise(int userID, int exerciseID)
        {
            // Find the favorite in the database
            return await _fitnessContext.favorite
                .FirstOrDefaultAsync(f => f.UserID == userID && f.ExerciseID == exerciseID);
        }

        // Add a favorite
        public async Task AddFavorite(Favorite favorite)
		{
            // Add Favorite to the db
			await _fitnessContext.favorite.AddAsync(favorite);
            // Save the changes
            await _fitnessContext.SaveChangesAsync();
		}

        // Delete favorite by ID
        public async Task DeleteFavorite(int userID, int favoriteID)
		{
            // Find the favorite in the database
            var favorite = await _fitnessContext.favorite.FirstOrDefaultAsync(f => f.UserID == userID && f.FavoriteID == favoriteID);

            // If favorite does not exist, throw a specific exception
            if (favorite == null)
			{
				throw new Exception($"Favorite with the {favoriteID} was not found");
			}

            // Remove the favorite
            _fitnessContext.Remove(favorite);
            // Save the changes
            await _fitnessContext.SaveChangesAsync();
		}
	}
}
