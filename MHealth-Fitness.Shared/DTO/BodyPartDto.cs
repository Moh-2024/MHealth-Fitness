using System.ComponentModel.DataAnnotations;

namespace MHealth_Fitness.Shared.DTO
{
    // BodyPartDto class
    public class BodyPartDto
    {
        // BodyPartID property
        public int BodyPartID { get; set; }
        [Required]// Required attribute
        public string BodyPartName { get; set; }
    }
}
