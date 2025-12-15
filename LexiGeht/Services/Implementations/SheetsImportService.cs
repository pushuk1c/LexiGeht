

using LexiGeht.Common;
using LexiGeht.Data;
using LexiGeht.Data.Entities;
using LexiGeht.Repositories;
using LexiGeht.Repositories.Interfaces;
using LexiGeht.Services.Interfaces;
using SQLite;
using System.Runtime.CompilerServices;

namespace LexiGeht.Services.Implementations
{
    public class SheetsImportService : ISheetsImportService
    {

        private readonly IGoogleSheetService _sheets;
        private readonly IDatabase _db;
      
        private readonly IRelationSyncService _relSync;

        private readonly IEntityRepository<QuizEntity> _quizzesRepo;
        private readonly IEntityRepository<QuestionEntity> _questionsRepo;
        private readonly IEntityRepository<AnswerEntity> _answersRepo;
        private readonly IQuestionsAnswersRepository _questionAnswersRepository;

        public SheetsImportService(
            IGoogleSheetService sheets, 
            IDatabase db, 
            IRelationSyncService relSync,
            IEntityRepository<QuizEntity> quizzesRepo,
            IEntityRepository<QuestionEntity> questionsRepo,
            IEntityRepository<AnswerEntity> answersRepo,
            IQuestionsAnswersRepository questionAnswersRepository
            )
        {
            _sheets = sheets;
            _db = db;
            _relSync = relSync;
            _quizzesRepo = quizzesRepo;
            _questionsRepo = questionsRepo;
            _answersRepo = answersRepo;
            _questionAnswersRepository = questionAnswersRepository;
        }
                        
        public async Task<OperationResult<bool>> ImportAsync()
        {
            try
            {
                // Read data from Google Sheets
                var qzRows = await _sheets.GetSheetDataAsync("'Quizzes'");
                var quRows = await _sheets.GetSheetDataAsync("'Questions'");
                var anRows = await _sheets.GetSheetDataAsync("'Answers'");
                var qqRows = await _sheets.GetSheetDataAsync("'Quizzes_Questions'");
                var qaRows = await _sheets.GetSheetDataAsync("'Questions_Answers'");

                // Parse data into entities
                var quizzes = qzRows?
                    .Skip(1)
                    .Select(r => new QuizEntity { Id = ToInt(r, 0), Title = ToStr(r, 1), Description = ToStr(r, 2), ImagePath = $"quiz_{ToInt(r, 0)}.png" })
                    .Where(q => q.Id > 0)            
                    .ToList() ?? new List<QuizEntity>();

                var questions = quRows?
                    .Skip(1).Select(r => new QuestionEntity { Id = ToInt(r, 0), Title = ToStr(r, 1), Description = ToStr(r, 2), ImagePath = $"ques_{ToInt(r, 0)}.png", AudioPath = $"questions/ques_{ToInt(r, 0)}" })
                    .Where(q => q.Id > 0)
                    .ToList() ?? new List<QuestionEntity>();

                var answers = anRows?
                    .Skip(1)
                    .Select(r => new AnswerEntity { Id = ToInt(r, 0), Title = ToStr(r, 1), AudioPath = $"answ_{ToInt(r, 0)}" })
                    .Where(a => a.Id > 0)
                    .ToList() ?? new List<AnswerEntity>();

                // Upsert data into the database within a transaction
                await _db.RunInTrx(async () =>
                {
                    await _quizzesRepo.UpsertManyAsync(quizzes);
                    await _questionsRepo.UpsertManyAsync(questions);
                    await _answersRepo.UpsertManyAsync(answers);

                    var ggValid = (qqRows ?? new List<IList<object>>())
                        .Skip(1)
                        .Where(r => ToInt(r, 1) > 0 && ToInt(r, 2) > 0);                  

                    var ggByQuiz = ggValid
                        .Skip(1)
                        .GroupBy(r => ToInt(r, 1))
                        .ToDictionary
                        (
                            g => g.Key,
                            g => g.Select(r => ToInt(r, 2)).ToHashSet()
                        );

                    foreach (var kvp in ggByQuiz)
                        await _relSync.SyncRelationsAsync("QuizzesQuestions", "QuizId", "QuestionId", ownerId: kvp.Key, newItemsIds: kvp.Value);

                    var qaValid = (qaRows ?? new List<IList<object>>())
                      .Skip(1)
                      .Where(r => ToInt(r, 1) > 0 && ToInt(r, 2) > 0);

                    var gaByQuestion = qaValid
                        .Skip(1)
                        .GroupBy(r => ToInt(r, 1));

                    foreach (var g in gaByQuestion)
                    {
                        int questionId = g.Key;
                        var answerIds = g.Select(r => ToInt(r, 2)).ToHashSet();

                        await _relSync.SyncRelationsAsync("QuestionsAnswers", "QuestionId", "AnswerId", ownerId: questionId, newItemsIds: answerIds);

                        var flags = g.ToDictionary(
                            r => ToInt(r, 2), 
                            r => ToBool(r, 3));

                        await _questionAnswersRepository.SetFlagsBulkAsync(questionId, flags);                                                
                    }

                });

                return OperationResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Import failed: {ex.Message}");
            }
        }

        private static int ToInt(IList<object> r, int i) =>
            i < r.Count && int.TryParse(r[i]?.ToString(), out var v) ? v : 0;

        private static string ToStr(IList<object> r, int i) => 
            i < r.Count ? r[i]?.ToString() ?? string.Empty : string.Empty;

        private static bool ToBool(IList<object> r, int i)
        {
            if (i >= r.Count)
                return false;
           
            var s = (r[i]?.ToString() ?? string.Empty).Trim();
           
            return s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase) || s.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }
         

    }   
}
