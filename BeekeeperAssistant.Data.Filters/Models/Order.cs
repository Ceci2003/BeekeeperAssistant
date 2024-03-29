﻿using System.ComponentModel.DataAnnotations;

namespace BeekeeperAssistant.Data.Filters.Models
{
    public enum Order
    {
        [Display(Name = "Филтър по подразбиране")]
        Default = 0,
        [Display(Name = "Възходящ")]
        Ascending = 1,
        [Display(Name = "Низходящ")]
        Descending = 2,
    }
}
