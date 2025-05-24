using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AprobacionProyectos.Application.Validators
{
    public class ValidateModelAttribute : ActionFilterAttribute 
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Title = "Error de validación de modelo",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Uno o más datos enviados son inválidos.",
                    Instance = context.HttpContext.Request.Path
                };

                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
