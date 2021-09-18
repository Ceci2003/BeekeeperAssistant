namespace BeekeeperAssistant.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IQuickChartService
    {
        string ImageUrl(string chartType, List<int> data, int height, int width);
    }
}
