

namespace LexiGeht.Data.Entities
{
    public sealed class QuizAgregateRowEntity
    {
        // Quiz properties
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public string QuizDescription { get; set; }
        public string QuizImagePath { get; set; }

        // Question properties
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionDescription { get; set; }
        public string QuestionImagePath { get; set; }
        public string QuestionAudioPath { get; set; }

        // Answer properties
        public int AnswerId { get; set; }
        public string AnswerTitle { get; set; }
        public string AnswerAudioPath { get; set; }
        public bool IsCorrect { get; set; }

    }
}
