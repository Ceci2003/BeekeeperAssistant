namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using BeekeeperAssistant.Data.Filters.Contracts;
    using BeekeeperAssistant.Data.Filters.Models;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryDataModel : IMapFrom<Apiary>, IMapFrom<ApiaryDataModel>, IDefaultOrder<ApiaryDataModel>
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public bool IsBookMarked { get; set; }

        public ApiaryType ApiaryType { get; set; }

        public string CreatorId { get; set; }

        public bool IsDeleted { get; set; }

        public string CreatorUserName { get; set; }

        public string Adress { get; set; }

        public int Id { get; set; }

        public int BeehivesCount { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsRegistered { get; set; }

        public bool IsClosed { get; set; }

        public IOrderedQueryable<ApiaryDataModel> DefaultOrder(IQueryable<ApiaryDataModel> query)
        {
            return query.OrderByDescending(x => x.IsBookMarked).ThenByDescending(x => x.CreatedOn);
        }
    }
}
