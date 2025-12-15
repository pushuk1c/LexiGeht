

using LexiGeht.Data.Entities;
using LexiGeht.Models;

namespace LexiGeht.Data.Mappers
{
    public static class QuizMapper
    {


        public static Quiz ToModel(QuizEntity entity)
        {
            if (entity == null) return null;
            return new Quiz
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                ImagePath = entity.ImagePath,
                Questions = new List<Question>()
            };
        }
        public static QuizEntity ToEntity(Quiz model)
        {
            if (model == null) return null;
            return new QuizEntity
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                ImagePath = model.ImagePath,
                                
            };
        }
    }
}
