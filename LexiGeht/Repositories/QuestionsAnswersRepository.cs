using LexiGeht.Data;
using LexiGeht.Repositories.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Repositories
{
    internal class QuestionsAnswersRepository : IQuestionsAnswersRepository
    {
        private readonly IDatabase _database;
        private readonly SQLiteAsyncConnection _connection;        

        public QuestionsAnswersRepository(IDatabase database) 
        {            
            _database = database;
            _connection = _database.Connection;
        }

        public async Task SetFlagsBulkAsync(int questionId, IDictionary<int, bool> flagByAnswerId)
        {
             foreach (var kv in flagByAnswerId)
             {
                int answerId = kv.Key;
                bool IsCorrect = kv.Value;

                await _connection.ExecuteAsync(
                    "INSERT INTO QuestionsAnswers (QuestionId, AnswerId, IsCorrect)" +
                    "VALUES (?, ?, ?)" +
                    "ON CONFLICT(QuestionId, AnswerId) " +
                    "DO UPDATE SET IsCorrect = excluded.IsCorrect;",
                    questionId, answerId, IsCorrect);

             }            
        }
    }
}
