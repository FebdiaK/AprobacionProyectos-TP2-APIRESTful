using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.Validators
{
    public static class ProjectQueryValidator
    {
        public static List<string> Validate(string? title, int? status, int? applicant, int? approvalUser)
        {
            var errors = new List<string>();

            if (status.HasValue && status <= 0)
                errors.Add("El parámetro 'status' debe ser mayor de cero o null.");

            if (applicant.HasValue && applicant <= 0)
                errors.Add("El parámetro 'applicant' debe ser mayor de cero o null.");

            if (approvalUser.HasValue && approvalUser <= 0)
                errors.Add("El parámetro 'approvalUser' debe ser mayor de cero o null.");

            if (!string.IsNullOrWhiteSpace(title) && title.Length > 100)
                errors.Add("El parámetro 'title' no puede exceder los 100 caracteres o null.");

            return errors;
        }
    }

}
