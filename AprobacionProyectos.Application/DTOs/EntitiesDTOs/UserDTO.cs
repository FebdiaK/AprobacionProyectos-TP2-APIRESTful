﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.DTOs.EntitiesDTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name  { get; set; }
        public string Email { get; set; }
        public ApproverRoleDto Role { get; set; }
     }
}
