using LexiGeht.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Services.Implementations
{
    public class DialogService : IDialogService
    {
        public Task AlertAsync(string title, string message, string ok = "OK")
        {
            return MainThread.InvokeOnMainThreadAsync(() => Application.Current.MainPage.DisplayAlert(title, message, ok));
        }
    }
}
