using BeekeeperAssistant.Data.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Data.Filters.Contracts
{
    public interface IFilter<T>
    {
        public IOrderedQueryable<T> FilterCollection(IQueryable<T> elements, FilterModel filterModel);
    }
}
