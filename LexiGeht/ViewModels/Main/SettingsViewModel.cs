using LexiGeht.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LexiGeht.ViewModels.Main
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly ISheetsImportService _sheetsImportService;
        private readonly IDialogService _dialogService;

        public ICommand UpdateQuizzesCommand { get; }

        public SettingsViewModel(ISheetsImportService sheetsImportService, IDialogService dialogService)
        {
            _sheetsImportService = sheetsImportService;
            _dialogService = dialogService;

            UpdateQuizzesCommand = new Command(async () => await LoadDataFromGoogleSheetAsync());
        }   

        public async Task LoadDataFromGoogleSheetAsync()
        {
            var resultat = await _sheetsImportService.ImportAsync();
            if (resultat.IsSuccess)
            {
                await _dialogService.AlertAsync("Import Successful", "The quizzes have been successfully updated from the Google Sheet.", "OK");
            }
            else
            {
                await _dialogService.AlertAsync("Import Error", resultat.ErrorMessage, "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
