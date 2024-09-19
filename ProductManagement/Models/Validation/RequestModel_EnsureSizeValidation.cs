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
            string[] sizeListArray = Constants.sizeAvailable.Split(",");
            if (requestModel != null && requestModel.size is not null) {
                if (! sizeListArray.Contains(requestModel.size))
                {
                    return new ValidationResult("size should be either of small , medium or large");
                }
            }
            return ValidationResult.Success;
        }
    }
}
