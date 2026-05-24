using FitCore.Common.Dto;

using System.Collections.Generic;

namespace FitCore.Application.Interfaces.IGym
{
    public interface IGetCitiesService
    {
        List<SelectListDto> Execute(int provinceId);
    }
}
