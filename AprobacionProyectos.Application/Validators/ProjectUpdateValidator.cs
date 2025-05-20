using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.Validators
{
    public class ProjectUpdateValidator
    {
        public static List<string> Validate(string title, string description, int duration)
        {
            var errors = new List<string>();

            if (duration <= 0)
            {
                errors.Add("La duración debe ser un número mayor de cero.");
            }

            if (!string.IsNullOrWhiteSpace(title) && ((title.Length < 5) || (title.Length >= 200)))
            {
                errors.Add("El título debe tener entre 5 y 200 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(description) && (description.Length < 10 || description.Length >= 1000))
            {
                errors.Add("La descripción debe tener entre 10 y 1000 caracteres");
            }
            return errors;
        }
    }
}
