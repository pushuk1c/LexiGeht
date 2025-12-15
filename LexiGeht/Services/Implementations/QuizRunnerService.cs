using LexiGeht.Models;
using LexiGeht.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Services.Implementations
{
    public class QuizRunnerService : IQuizRunnerService
    {
        private IList<Question> _questions = new List<Question>();
        private int _currentIndex = -1;
        private bool _submittedThisQuestion = false;

        public Question? CurrentQuestion => (_currentIndex >= 0 && _currentIndex < _questions.Count) ? _questions[_currentIndex] : null;

        public int Index => _currentIndex + 1;

        public int Total => _questions.Count;

        public int Score { get; private set; }

        public bool IsFinished => _currentIndex >= _questions.Count;

        public void StartQuiz(IList<Question> questions)
        {

            _questions = questions ?? new List<Question>();
            _currentIndex = _questions.Count > 0 ? 0 : -1;
            _submittedThisQuestion = false;
            Score = 0;
        }

        public void SubmitAnswer(IEnumerable<int> selectedAnswersIds)
        {
            if (CurrentQuestion is null || _submittedThisQuestion)
                return;

            var selected = (selectedAnswersIds ?? Array.Empty<int>()).ToHashSet();

            var corectAnswers = CurrentQuestion.CorrectAnswers.Select(a => a.Id).ToHashSet();

            if (selected.SetEquals(corectAnswers))
                Score++;

            _submittedThisQuestion = true;
        }

        public void Next()
        {
            if (_questions.Count == 0 || _currentIndex < 0)
                return;

            _currentIndex++;
            _submittedThisQuestion = false;

            if (_currentIndex >= _questions.Count)
                _currentIndex = _questions.Count;

        }
    }
}
