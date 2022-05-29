using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BTIT.EPM.MultiTenancy.HostDashboard.Dto;

namespace BTIT.EPM.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}