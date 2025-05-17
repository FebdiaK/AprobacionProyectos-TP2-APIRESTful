﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [ForeignKey(nameof(ApproverRole))]
        [Column("Role")] 
        public int RoleId { get; set; } 
        public ApproverRole ApproverRole { get; set; } = null!; 

        public ICollection<ProjectProposal> CreatedProposals { get; set; } = new List<ProjectProposal>();
        public ICollection<ProjectApprovalStep> ApprovalSteps { get; set; } = new List<ProjectApprovalStep>();

    }
}
