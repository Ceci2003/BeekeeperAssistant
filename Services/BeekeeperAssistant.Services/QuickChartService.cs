namespace BeekeeperAssistant.Services
{
    using System.Collections.Generic;

    using QuickChart;

    public class QuickChartService : IQuickChartService
    {
        public string ImageUrl(string chartType, List<int> data, int height = 300, int width = 500)
        {
            Chart chart = new Chart();

            chart.Width = 500;
            chart.Height = 300;
            chart.Config = @"{
						  type: 'pie',
						  data: {
						    datasets: [{ 
						      data: [10, 20, 30]
						    }]
						  }
						}";

            chart.Config = "{ " +
                                "type: 'pie', " +
                                "data: { " +
                                    "datasets: [{ " +
                                        $"data: [{string.Join(", ", data)}] " +
                                    "}] " +
                                "} " +
                            "}";

            // Get the URL
            var url = chart.GetUrl();
            return url;
        }
    }
}
