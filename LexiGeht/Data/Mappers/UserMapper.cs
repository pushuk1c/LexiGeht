

using LexiGeht.Data.Entities;
using LexiGeht.Models;

namespace LexiGeht.Data.Mappers
{
    public static class UserMapper
    {
        public static User ToModel(UserEntity entity)
        {
            if (entity == null) return null;
            return new User
            {
                Id = entity.Id,
                Username = entity.Username,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash
            };
        }
        public static UserEntity ToEntity( User model)
        {
            if (model == null) return null;
            return new Entities.UserEntity
            {
                Id = model.Id,
                Username = model.Username,
                Email = model.Email,
                PasswordHash = model.PasswordHash
            };
        }

    }
}
