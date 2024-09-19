using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models.Validation
{
    /// <summary>
    /// Used for input validation
    /// </summary>
    public class RequestModel_EnsureSizeValidation: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var requestModel = validationContext.ObjectInstance as RequestModel;
            if (requestModel != null && requestModel.size is not null) {
                if (!requestModel.size.Contains("small") || !requestModel.size.Contains("medium")|| requestModel.size.Contains("large"))
                {
                    return new ValidationResult("size should be either of small , medium or large");
                }
            }
            return ValidationResult.Success;
        }
    }
}
