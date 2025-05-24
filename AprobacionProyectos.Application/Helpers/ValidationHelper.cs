using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AprobacionProyectos.Application.Helpers
{
    public static class ValidationHelper
    {
        public static IActionResult FromFluentValidationResult(FluentValidation.Results.ValidationResult result)
        {
            return new BadRequestObjectResult(new
            {
                errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray())
            });
        }
    }
}
