namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllUserQueensViewModel
    {
        public IEnumerable<UserQueenViewModel> UserQueens { get; set; }
    }
}
