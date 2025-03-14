using System.ComponentModel.DataAnnotations;

namespace MHealth_FitnessAPI.Models
{
    // BodyPart class
    public class BodyPart
	{
		[Key]// Primary key
        public int BodyPartID {  get; set; } // BodyPartID property
        public string BodyPartName { get; set; } // BodyPartName property
    }
}
