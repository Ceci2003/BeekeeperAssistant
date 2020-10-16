namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IBeehiveService
    {
        IEnumerable<T> GetAllUserBeehivesByApiaryId<T>(int apiaryId);
    }
}
