using BeekeeperAssistant.Data.Filters.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Data.Filters.Models
{
    public class FilterModel
    {
        public Order Order { get; set; } = Order.Default;

        public string PropertyName { get; set; } = null;

        public string KeyWord { get; set; } = null;

        [BindNever]
        public FilterData Data { get; set; }
    }
}
