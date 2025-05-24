using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.Request;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AprobacionProyectos.Application.Validators
{
    public class ProjectCreateValidator : IProjectValidator
    {
        private readonly IAreaService _areaService;
        private readonly IUserService _userService;
        private readonly IProjectTypeService _typeService;

        public ProjectCreateValidator(
            IAreaService areaService,
            IUserService userService,
            IProjectTypeService typeService)
        {
            _areaService = areaService;
            _userService = userService;
            _typeService = typeService;
        }
         
        public async Task<Dictionary<string, List<string>>> ValidateAsync(CreateProjectProposalRequestDto dto)
        {
            var errors = new Dictionary<string, List<string>>();

            AddErrorIf(dto.Title, string.IsNullOrWhiteSpace(dto.Title), "Title", "El Título es obligatorio.", errors);
            AddErrorIf(dto.Amount, dto.Amount <= 0, "Amount", "La cantidad debe ser mayor a cero.", errors);
            AddErrorIf(dto.Duration, dto.Duration <= 0, "Duration", "La duración debe ser mayor a cero.", errors);

            await ValidateExistence(dto.Area, _areaService.ExistsAsync, "Area", "área", errors);
            await ValidateExistence(dto.User, _userService.ExistsAsync, "User", "usuario", errors);
            await ValidateExistence(dto.Type, _typeService.ExistsAsync, "Type", "tipo de proyecto", errors);

            return errors;
        }

        private void AddErrorIf<T>(T value, bool condition, string propertyName, string errorMessage, Dictionary<string, List<string>> errors)
        {
            if (condition)
            { 
                if (!errors.ContainsKey(propertyName))
                    errors[propertyName] = new List<string>();

                errors[propertyName].Add(errorMessage);
            }
        }

        private async Task ValidateExistence(
            int id,
            Func<int, Task<bool>> existsFunc,
            string propertyName,
            string entityName,
            Dictionary<string, List<string>> errors)
        {
            if (id <= 0)
            {
                AddErrorIf(id, true, propertyName, $"Ingrese un {entityName} válido.", errors);
                return;
            }

            var exists = await existsFunc(id);
            if (!exists)
            {
                AddErrorIf(id, true, propertyName, $"El {entityName} con ID {id} no existe.", errors);
            }
        }
    }
}


