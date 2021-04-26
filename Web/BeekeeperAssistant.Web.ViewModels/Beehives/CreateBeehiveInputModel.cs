﻿namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using Microsoft.Extensions.DependencyInjection;

    public class CreateBeehiveInputModel : IValidatableObject
    {
        [Required]
        public int Number { get; set; }

        [Required]
        public BeehiveSystem BeehiveSystem { get; set; }

        [Required]
        public BeehiveType BeehiveType { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ApiaryId { get; set; }

        [Required]
        public BeehivePower BeehivePower { get; set; }

        public IEnumerable<KeyValuePair<int, string>> AllApiaries { get; set; }

        public bool HasDevice { get; set; }

        public bool HasPolenCatcher { get; set; }

        public bool HasPropolisCatcher { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();
            var beehiveRepository = validationContext.GetService<IDeletableEntityRepository<Beehive>>();
            var beehive = beehiveRepository.All().FirstOrDefault(b => b.Number == this.Number && b.ApiaryId == this.ApiaryId);

            if (beehive != null)
            {
                errorList.Add(new ValidationResult("Beehive already exists!"));
            }

            return errorList;
        }
    }
}
