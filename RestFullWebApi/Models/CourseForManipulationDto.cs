using RestFullWebApi.Validation_Attribute;
using System.ComponentModel.DataAnnotations;

namespace RestFullWebApi.Models
{
    [TitleAttributeMustBeDifferentFromDescriptioAttribute
    (ErrorMessage = "Tittle must be different from description")]
    public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a tittle")]
        [MaxLength(100, ErrorMessage = "The tittle shouldn't have more than 100 characters.")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The descriptions shouldn't have more than 1500 characters.")]
        public virtual string Description { get; set; }
    }
}
