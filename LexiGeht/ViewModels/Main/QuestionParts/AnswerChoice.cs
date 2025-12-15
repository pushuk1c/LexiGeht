using System.ComponentModel;

namespace LexiGeht.ViewModels.Main.QuestionParts
{
    public enum AnswerState { None, Selected, Correct, InCorrect }
    public class AnswerChoice : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        private AnswerState _state = AnswerState.None;
        public AnswerState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                PropertyChanged?.Invoke(this,new(nameof(State)));
            }
        }
           
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
