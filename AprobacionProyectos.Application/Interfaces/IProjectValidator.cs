using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.DTOs.Request;
using static System.Web.Razor.Parser.SyntaxConstants;

namespace AprobacionProyectos.Application.Interfaces
{
    public interface IProjectValidator
    {
        Task<Dictionary<string, List<string>>> ValidateAsync(CreateProjectProposalRequestDto dto);
    }
}
