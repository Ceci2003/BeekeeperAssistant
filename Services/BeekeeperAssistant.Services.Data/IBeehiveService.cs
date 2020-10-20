namespace BeekeeperAssistant.Services.Data
{
    using BeekeeperAssistant.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IBeehiveService
    {
        IEnumerable<T> GetAllUserBeehivesByApiaryId<T>(int apiaryId);

        bool NumberExists(int number, ApplicationUser user);
    }
}
