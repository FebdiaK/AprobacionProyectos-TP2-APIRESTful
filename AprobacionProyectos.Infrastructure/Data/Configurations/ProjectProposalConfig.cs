﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Infrastructure.Data.Configurations
{
    public class ProjectProposalConfig : IEntityTypeConfiguration<ProjectProposal>
    {
        public void Configure(EntityTypeBuilder<ProjectProposal> builder)
        {
            builder.HasOne(p => p.CreatedBy) // relacion con el creador del proyecto (un user)
                   .WithMany(u => u.CreatedProposals) // relacion inversa con el usuario
                   .HasForeignKey(p => p.CreatedById) // foreign key del User
                   .OnDelete(DeleteBehavior.Restrict); // cambiado a Restrict para evitar la eliminacion en cascada

            builder.HasOne(p => p.Area)
                   .WithMany(a => a.ProjectProposals)
                   .HasForeignKey(p => p.AreaId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(p => p.Type)
                   .WithMany(t => t.ProjectProposals)
                   .HasForeignKey(p => p.TypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Status)
                   .WithMany(s => s.ProjectProposals)
                   .HasForeignKey(p => p.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.EstimatedAmount)
                     .HasColumnType("decimal(18,2)"); // definido como decimal con 18 digitos en total y 2 decimales

        }
    }
}
