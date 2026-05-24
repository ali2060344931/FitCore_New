using FitCore.Common.Dto;

using System.Collections.Generic;

namespace FitCore.Application.Interfaces.IGym
{
    public interface IGetProvincesService
    {
        List<SelectListDto> Execute();
    }
}
