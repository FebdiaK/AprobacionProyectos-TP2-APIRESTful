using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.EntitiesDTOs;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Mappers
{
    public static class DtoMapper
    {
        public static AreaDto ToDto(this Area area) => new()
        {
            Id = area.Id,
            Name = area.Name
        };

        public static TypeDto ToDto(this ProjectType type) => new()
        {
            Id = type.Id,
            Name = type.Name
        };

        public static StatusDto ToDto(this ApprovalStatus status) => new()
        {
            Id = status.Id,
            Name = status.Name
        };

        public static ApproverRoleDto ToDto(this ApproverRole role) => new()
        {
            Id = role.Id,
            Name = role.Name
        };

        public static UserDto ToDto(this User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.ApproverRole.ToDto()
        };
    }

}
