using LexiGeht.Common;
using LexiGeht.Models;
using LexiGeht.Repositories.Interfaces;
using LexiGeht.Services.Interfaces;

namespace LexiGeht.Services.Implementations
{
    public class QuizAgregateService : IQuizAgregateService
    {
        private readonly IQuizAgregateRepository _quizAgregateRepository;
        public QuizAgregateService(IQuizAgregateRepository quizAgregateRepository)
        {
            _quizAgregateRepository = quizAgregateRepository;
        }

        public async Task<OperationResult<Quiz>> GetAsync(int quizId)
        {

            var quizRows = await _quizAgregateRepository.GetAgregateAsync(quizId);
            if (quizRows == null || quizRows.Count == 0)
                return OperationResult<Quiz>.Fail($"Quiz with id {quizId} not found.");

            var firstRow = quizRows[0];
            var quiz = new Quiz
            {
                Id = firstRow.QuizId,
                Title = firstRow.QuizTitle,
                Description = firstRow.QuizDescription,
                ImagePath = firstRow.QuizImagePath,
                Questions = new List<Question>()
            };

            foreach (var g in quizRows.GroupBy(r => r.QuestionId))
            {
                var firstQuestionRow = g.First();
                var question = new Question
                {
                    Id = firstQuestionRow.QuestionId,
                    Title = firstQuestionRow.QuestionTitle,
                    Description = firstQuestionRow.QuestionDescription,
                    ImagePath = firstQuestionRow.QuestionImagePath,
                    AudioPath = firstQuestionRow.QuestionAudioPath,
                    Answers = new List<Answer>(),
                    CorrectAnswers = new List<Answer>()
                };

                foreach (var r in g)
                {
                    if (r.AnswerId == 0) 
                        continue;

                    var answer = new Answer
                    {
                        Id = r.AnswerId,
                        Title = r.AnswerTitle,
                        AudioPath = r.AnswerAudioPath
                    };

                    question.Answers.Add(answer);

                    if (r.IsCorrect)
                        question.CorrectAnswers.Add(answer);
                }

                quiz.Questions.Add(question);
            }

            return OperationResult<Quiz>.Ok(quiz);
        }
    }
}
