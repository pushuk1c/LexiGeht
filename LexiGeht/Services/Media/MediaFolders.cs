

namespace LexiGeht.Services.Media
{
    public enum MediaFolder
    {
        QuizzesImages,
        QuestionsImages,
        QuestionsAudios,
        AnswersAudios
    }

    public static class MediaFolders
    {
        public const string QuizzesImages = "quizzes_images";
        public const string QuestionsImages = "questions_images";
        public const string QuestionsAudios = "questions_audios";
        public const string AnswersAudios = "answers_audios";

        public static string ToKey(this MediaFolder folder)
        {
            return folder switch
            {
                MediaFolder.QuizzesImages => QuizzesImages,
                MediaFolder.QuestionsImages => QuestionsImages,
                MediaFolder.QuestionsAudios => QuestionsAudios,
                MediaFolder.AnswersAudios => AnswersAudios,
                _ => throw new ArgumentOutOfRangeException(nameof(folder), folder, null)
            };
        }

    }
}
