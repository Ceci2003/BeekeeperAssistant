﻿namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum FeedbackType
    {
        [Display(Name = "Помощ")]
        Help = 1,
        [Display(Name = "Отзив")]
        Feedback = 2,
        [Display(Name = "Сигнал за грешка")]
        Report = 3,
    }
}
