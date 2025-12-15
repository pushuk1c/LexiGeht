
using LexiGeht.Models;

namespace LexiGeht.Services.Interfaces
{
    public interface IQuizService
    {
        Task<int> GetQuizzesAsync();
        Task<List<Quiz>> GetAllQuizzesAsync();
        Task<Quiz?> GetQuizByIdAsync(int id);
        Task<int> CreateQuizAsync(Quiz quiz);
        Task UpdateQuizAsync(Quiz quiz);
        Task DeleteQuizAsync(int id);
    }
}
