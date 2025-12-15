

using LexiGeht.Models;

namespace LexiGeht.Services.Interfaces
{
    public interface IQuizRunnerService
    {
        Question? CurrentQuestion { get; }
        int Index { get; }
        int Total { get; }
        int Score { get; }
        bool IsFinished { get; }
        void StartQuiz(IList<Question> questions);
        void SubmitAnswer(IEnumerable<int> selectedIds);
        void Next();   
    }
    
}
