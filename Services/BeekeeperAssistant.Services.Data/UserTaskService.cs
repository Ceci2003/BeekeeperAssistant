namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class UserTaskService : IUserTaskService
    {
        private readonly IDeletableEntityRepository<UserTask> userTaskRepository;

        public UserTaskService(
            IDeletableEntityRepository<UserTask> userTaskRepository)
        {
            this.userTaskRepository = userTaskRepository;
        }

        public async Task CreateAsync(string userId, string title, string content, string color, DateTime? startDate, DateTime? endDate, bool isCompleted = false)
        {
            var task = new UserTask()
            {
                UserId = userId,
                Title = title,
                Content = content,
                Color = color,
                StartDate = startDate,
                EndDate = endDate,
                IsCompleted = isCompleted,
            };

            await this.userTaskRepository.AddAsync(task);
            await this.userTaskRepository.SaveChangesAsync();
        }

        public async Task EditAsync(int taskId, string title, string content, string color, DateTime? startDate, DateTime? endDate, bool isCompleted = false)
        {
            var task = this.userTaskRepository
                .All()
                .FirstOrDefault(t => t.Id == taskId);

            task.Title = title;
            task.Content = content;
            task.Color = color;
            task.StartDate = startDate;
            task.EndDate = endDate;
            task.IsCompleted = isCompleted;

            await this.userTaskRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int taskId)
        {
            var task = this.userTaskRepository
                .All()
                .FirstOrDefault(t => t.Id == taskId);

            this.userTaskRepository.Delete(task);
            await this.userTaskRepository.SaveChangesAsync();
        }

        public int GetUserTasksCount(string userId) =>
            this.userTaskRepository
                .All()
                .Where(t => t.UserId == userId)
                .Count();

        public int GetUserTasksCountByColor(string userId, string color) =>
            this.userTaskRepository
                .All()
                .Where(t => t.UserId == userId && t.Color == color)
                .Count();

        public T GetUserTaskById<T>(int taskId) =>
            this.userTaskRepository
                .All()
                .Where(t => t.Id == taskId)
                .To<T>()
                .FirstOrDefault();

        public IEnumerable<T> GetAllUserTasks<T>(string userId) =>
            this.userTaskRepository
                .All()
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.StartDate)
                .ThenByDescending(t => t.EndDate)
                .ThenByDescending(t => t.CreatedOn)
                .To<T>();

        public IEnumerable<T> GetAllUserTasksByMonthAndYear<T>(string userId, int month, int year) =>
            this.userTaskRepository
                .All()
                .Where(t => t.UserId == userId && t.StartDate.Value.Month == month && t.StartDate.Value.Year == year)
                .To<T>();

        public IEnumerable<T> GetLastUserTasks<T>(string userId) =>
            this.userTaskRepository
                .All()
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedOn)
                .Take(5)
                .To<T>();
    }
}
