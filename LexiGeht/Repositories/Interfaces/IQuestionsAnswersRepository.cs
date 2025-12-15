

namespace LexiGeht.Repositories.Interfaces
{
    public interface IQuestionsAnswersRepository
    {
        Task SetFlagsBulkAsync(int questionId, IDictionary<int,bool> flagByAnswerId);
    }
}
