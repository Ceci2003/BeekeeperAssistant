namespace BeekeeperAssistant.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IQuickChartService
    {
        string ImageUrl(
            string chartType,
            List<int> data,
            string[] colors,
            string borderColor = "'#110400'",
            int height = 300,
            int width = 500);
    }
}
