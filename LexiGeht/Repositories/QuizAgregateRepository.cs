using LexiGeht.Data;
using LexiGeht.Data.Entities;
using LexiGeht.Models;
using SQLite;
using System.Runtime.CompilerServices;

namespace LexiGeht.Repositories.Interfaces
{
    public class QuizAgregateRepository : IQuizAgregateRepository
    {
        private readonly SQLiteAsyncConnection _connection;

        public QuizAgregateRepository(IDatabase database) => _connection = database.Connection;
        
        public async Task<IReadOnlyList<QuizAgregateRowEntity>> GetAgregateAsync(int id)
        {
            const string query = @"
                SELECT qz.id AS QuizId,
                          qz.title AS QuizTitle,
                          qz.description AS QuizDescription,
                          qz.imagePath AS QuizImagePath,
                          qu.id AS QuestionId,
                          qu.title AS QuestionTitle,
                          qu.description AS QuestionDescription,
                          qu.imagePath AS QuestionImagePath,
                          qu.audioPath AS QuestionAudioPath,
                          an.id AS AnswerId,
                          an.title AS AnswerTitle,
                          an.audioPath AS AnswerAudioPath,
                          qa.isCorrect AS IsCorrect 
                FROM Quizzes qz
                JOIN QuizzesQuestions qq ON qz.Id = qq.QuizId
                JOIN Questions qu ON qu.Id = qq.QuestionId
                LEFT JOIN QuestionsAnswers qa ON qu.Id = qa.QuestionId
                LEFT JOIN Answers an ON an.Id = qa.AnswerID
                WHERE qz.Id = ?
                ORDER BY qq.Id, qa.Id";

            return await _connection.QueryAsync<QuizAgregateRowEntity>(query, id)
                            .ContinueWith(t => (IReadOnlyList<QuizAgregateRowEntity>)t.Result);
            
        }
    }
}
