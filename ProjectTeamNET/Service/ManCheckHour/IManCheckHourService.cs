using ProjectTeamNET.Service.ManCheckHour.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.ManCheckHour
{
    public interface IManCheckHourService
    {
        Task<List<ResultManCheckHourDto>> Init();
    }
}
