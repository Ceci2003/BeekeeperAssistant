using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Data.Filters.Contracts
{
    public interface IDefaultOrder<T>
    {
        IOrderedQueryable<T> DefaultOrder(IQueryable<T> query);

    }
}
