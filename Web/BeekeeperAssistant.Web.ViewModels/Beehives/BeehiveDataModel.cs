namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using BeekeeperAssistant.Data.Filters.Contracts;
    using BeekeeperAssistant.Data.Filters.Models;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;

    public class BeehiveDataModel : IMapFrom<Beehive>, IMapFrom<BeehiveDataModel>, IDefaultOrder<BeehiveDataModel>
    {
        public int Id { get; set; }

        public Apiary Apiary { get; set; }

        public string ApiaryNumber { get; set; }

        public int Number { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public BeehiveSystem BeehiveSystem { get; set; }

        public BeehiveType BeehiveType { get; set; }

        public bool IsBookMarked { get; set; }

        public bool HasDevice { get; set; }

        public bool IsItMovable { get; set; }

        public Access BeehiveAccess { get; set; }

        public string CreatorId { get; set; }

        public MarkFlagType? MarkFlagType { get; set; }

        public DateTime CreatedOn { get; set; }

        public IOrderedQueryable<BeehiveDataModel> DefaultOrder(IQueryable<BeehiveDataModel> query)
        {
            return query.OrderByDescending(x => x.IsBookMarked).ThenByDescending(x => x.CreatedOn);
        }
    }
}
