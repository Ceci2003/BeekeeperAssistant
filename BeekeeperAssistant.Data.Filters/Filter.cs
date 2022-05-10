using BeekeeperAssistant.Data.Filters.Contracts;
using BeekeeperAssistant.Data.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeekeeperAssistant.Services.Mapping;

namespace BeekeeperAssistant.Data.Filters
{
    public class Filter<T> : IFilter<T> where T: IDefaultOrder<T>, new()
    {
        public IOrderedQueryable<T> FilterCollection(IQueryable<T> query, FilterModel filterModel)
        {
            if (filterModel != null)
            {
                switch (filterModel.Order)
                {
                    case Order.Ascending:
                        return query.OrderByProeprty(filterModel.PropertyName);
                    case Order.Descending:
                        return query.OrderByProeprtyDescending(filterModel.PropertyName);
                }
            }

            var tType = new T();

            return tType.DefaultOrder(query);
        }
    }
}
