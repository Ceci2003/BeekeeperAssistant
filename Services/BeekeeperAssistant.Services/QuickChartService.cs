namespace BeekeeperAssistant.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using QuickChart;

    public class QuickChartService : IQuickChartService
    {
        public string ImageUrl(
            string chartType,
            List<int> data,
            string[] colors,
            string borderColor = "'#110400'",
            int height = 300,
            int width = 500)
        {
            Chart chart = new Chart();

            chart.Width = 500;
            chart.Height = 300;

            chart.Config = "{ " +
                                "type: 'pie', " +
                                "data: { " +
                                    "datasets: [{ " +
                                        $"data: [{string.Join(", ", data)}], " +

                                        // "backgroundColor:['#4e73df', '#0f3dc4']" +
                                        // $"backgroundColor:['#008995', '#009386', '#139a67', '#579e3e', '#8d9b00', '#c69000', '#ff7800']" +
                                        $"backgroundColor:[{string.Join(", ", colors)}]," +
                                        $"borderColor: '#7B7B7B', " +
                                    "}] " +
                                "}, " +
                                "options: { " +
                                    "legend: { " +
                                        "display: false " +
                                    "}, " +
                                    "plugins: { " +
                                        "datalabels: { " +
                                            "display: true, " +
                                            "formatter: (val, ctx) => { " +
                                                "return ctx.chart.data.labels[ctx.dataIndex]; " +
                                            "}, " +
                                            "color: '#fff', " +
                                            "backgroundColor: '#404040',  " +
                                            "font: { " +
                                                "size: 30, " +
                                                "weight: 'bold' " +
                                            "}," +
                                        "}," +
                                    "} " +
                                "}" +
                            "}";

            // Get the URL
            var url = chart.GetUrl();
            return url;
        }
    }
}
