
using LexiGeht.Common;
using LexiGeht.Models;

namespace LexiGeht.Services.Interfaces
{
    public interface IQuizAgregateService
    {
        Task<OperationResult<Quiz>> GetAsync(int quizId);
    }
}
