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
    public class Filter<T> : IFilter<T> where T : IDefaultOrder<T>, new()
    {
        public IOrderedQueryable<T> FilterCollection(IQueryable<T> query, FilterModel filterModel)
        {
            var tType = new T();

            if (filterModel != null)
            {
                if (this.HasProperty(typeof(T), filterModel.PropertyName))
                {
                    switch (filterModel.Order)
                    {
                        case Order.Ascending:
                            return query.OrderByProeprty(filterModel.PropertyName);
                        case Order.Descending:
                            return query.OrderByProeprtyDescending(filterModel.PropertyName);
                    }
                }

            }


            return tType.DefaultOrder(query);
        }

        private bool HasProperty(Type type, string propertyName)
        {
            if (propertyName == null)
            {
                return false;
            }

            if (type.GetProperty(propertyName) == null)
            {
                return false;
            }

            return true;
        }
    }
}
