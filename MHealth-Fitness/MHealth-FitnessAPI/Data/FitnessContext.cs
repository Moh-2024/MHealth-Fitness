using MHealth_Fitness.Shared.DTO;
using MHealth_FitnessAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MHealth_FitnessAPI.Data
{
    //inheriting dbcontext
    public class FitnessContext  : DbContext
    {       
        //constructor
        public FitnessContext(DbContextOptions<FitnessContext> options) : base(options) { }
        // each dbset represents the tabale in SQL
        public DbSet<BodyPart> bodyPart { get; set; }
        public DbSet<Exercise> exercise { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<Routine> routine { get; set; }
        public DbSet<RoutineExercise> routineExercise { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<Favorite> favorite { get; set; }
        public DbSet<SetsPerExercise> setsPerExercise { get; set; }
    }
}
