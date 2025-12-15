using LexiGeht.Data.Entities;
using LexiGeht.Data.Mappers;
using LexiGeht.Models;
using LexiGeht.Repositories.Interfaces;
using LexiGeht.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly IEntityRepository<QuizEntity> _quizRepository;

        public QuizService(IEntityRepository<QuizEntity> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<List<Quiz>> GetAllQuizzesAsync()
        {            
            var quizzesEntities = await _quizRepository.GetAllAsync();

            return quizzesEntities.Select(q => QuizMapper.ToModel(q)).ToList();
        }

        public Task<Quiz?> GetQuizByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetQuizzesAsync()
        {
            throw new NotImplementedException();
        }
        public Task<int> CreateQuizAsync(Quiz quiz)
        {
            throw new NotImplementedException();
        }

        public Task DeleteQuizAsync(int id)
        {
            throw new NotImplementedException();
        }        

        public Task UpdateQuizAsync(Quiz quiz)
        {
            throw new NotImplementedException();
        }
    }
}
