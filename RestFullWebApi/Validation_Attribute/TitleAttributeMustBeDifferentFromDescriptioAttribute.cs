using RestFullWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace RestFullWebApi.Validation_Attribute
{
    public class TitleAttributeMustBeDifferentFromDescriptioAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseForManipulationDto)validationContext.ObjectInstance;
            if (course.Title == course.Description)
            {
                return new ValidationResult(ErrorMessage,
                     new[] { "CourseForManipulationDto" });
            }

            return ValidationResult.Success;
        }
    }
}
