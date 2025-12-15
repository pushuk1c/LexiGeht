using LexiGeht.Data.Entities;
using LexiGeht.Models;

namespace LexiGeht.Repositories.Interfaces
{
    public interface IQuizAgregateRepository
    {
        Task<IReadOnlyList<QuizAgregateRowEntity>> GetAgregateAsync(int id);
    }
}
