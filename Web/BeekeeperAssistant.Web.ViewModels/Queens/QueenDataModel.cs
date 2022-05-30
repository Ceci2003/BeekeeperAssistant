namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    using System;
    using System.Linq;
    using BeekeeperAssistant.Data.Filters.Contracts;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class QueenDataModel : IMapFrom<Queen>, IMapFrom<QueenDataModel>, IDefaultOrder<QueenDataModel>
    {
        public int Id { get; set; }

        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public int BeehiveApiaryId { get; set; }

        public string BeehiveApiaryNumber { get; set; }

        public string BeehiveApiaryName { get; set; }

        public QueenColor Color { get; set; }

        public DateTime GivingDate { get; set; }

        public string Origin { get; set; }

        public QueenBreed Breed { get; set; }

        public bool IsBookMarked { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime FertilizationDate { get; set; }

        public IOrderedQueryable<QueenDataModel> DefaultOrder(IQueryable<QueenDataModel> query)
        {
            return query.OrderByDescending(x => x.IsBookMarked).ThenByDescending(x => x.CreatedOn);
        }
    }
}
