using LexiGeht.Data.Entities;
using SQLite;

namespace LexiGeht.Data
{
    public class Database : IDatabase
    {
        public SQLiteAsyncConnection Connection { get; }

        public Database(string dbPath)
        {
            Connection = new SQLiteAsyncConnection(dbPath);
        }
           

        public async Task InitializeAsync()
        {
           
            await Connection.CreateTableAsync<UserEntity>();

            await Connection.CreateTableAsync<QuizEntity>();
            await Connection.CreateTableAsync<QuestionEntity>();
            await Connection.CreateTableAsync<AnswerEntity>();
            await Connection.CreateTableAsync<CoursEntity>();
            await Connection.CreateTableAsync<CategoryEntity>();

            await Connection.CreateTableAsync<QuizQuestionEntity>();
            await Connection.CreateTableAsync<QuestionAnswerEntity>();
            await Connection.CreateTableAsync<CoursQuizEntity>();
            await Connection.CreateTableAsync<CategoryQuizEntity>();

            await Connection.ExecuteAsync("CREATE UNIQUE INDEX IF EXISTS ux_ qq ON QuizzesQuestions(QuizId, QuestionId)");
            await Connection.ExecuteAsync("CREATE UNIQUE INDEX IF EXISTS ux_ qq ON QuestionsAnswers(QuestionId, AnswerId)");
            await Connection.ExecuteAsync("CREATE UNIQUE INDEX IF EXISTS ux_ qq ON CoursesQuizzes(CoursId, QuizId)");
            await Connection.ExecuteAsync("CREATE UNIQUE INDEX IF EXISTS ux_ qq ON CategoriesQuizzes(CategoryId, QuizId)");

            await Connection.ExecuteAsync("PRAGMA foreign_keys = ON;");

        }

        public async Task RunInTrx(Func<Task> action)
        {
            await Connection.ExecuteAsync("BEGIN");
            
            try 
            {   await action();
                await Connection.ExecuteAsync("COMMIT");
            }
            catch
            {
                await Connection.ExecuteAsync("ROLLBACK");
                throw;
            }
        }

        public async Task<T> RunInTrx<T>(Func<Task<T>> action)
        {
            await Connection.ExecuteAsync("BEGIN");

            try
            {
                var result = await action();
                await Connection.ExecuteAsync("COMMIT");
                return result;
            }
            catch
            {
                await Connection.ExecuteAsync("ROLLBACK");
                throw;
            }
        }
    }
}
