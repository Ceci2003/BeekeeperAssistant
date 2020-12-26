using BeekeeperAssistant.Data.Models;
using BeekeeperAssistant.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    public class AllUserQueensViewModel
    {
        public IEnumerable<UserQueenViewModel> UserQueens { get; set; }
    }
}
