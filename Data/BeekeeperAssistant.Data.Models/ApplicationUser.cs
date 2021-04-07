// ReSharper disable VirtualMemberCallInConstructor
namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;

    using BeekeeperAssistant.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Apiaries = new HashSet<Apiary>();
            this.Beehives = new HashSet<Beehive>();
            this.Tasks = new HashSet<Duty>();
            this.Notes = new HashSet<Note>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Apiary> Apiaries { get; set; }

        public virtual ICollection<Beehive> Beehives { get; set; }

        public virtual ICollection<Queen> Queens { get; set; }

        public virtual ICollection<Duty> Tasks { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}
