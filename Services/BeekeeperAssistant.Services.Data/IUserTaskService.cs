namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserTaskService
    {
        Task CreateAsync(string userId, string title, string content, string color, DateTime? startDate, DateTime? endDate, bool isCompleted = false);

        Task EditAsync(int taskId, string title, string content, string color, DateTime? startDate, DateTime? endDate, bool isCompleted = false);

        Task DeleteAsync(int taskId);

        int GetUserTasksCount(string userId);

        int GetUserTasksCountByColor(string userId, string color);

        T GetUserTaskById<T>(int taskId);

        IEnumerable<T> GetAllUserTasks<T>(string userId);

        IEnumerable<T> GetLastUserTasks<T>(string userId);

        IEnumerable<T> GetAllUserTasksByMonthAndYear<T>(string userId, int month, int year);
    }
}
