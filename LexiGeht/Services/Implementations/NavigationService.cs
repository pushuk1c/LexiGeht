using LexiGeht.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiGeht.Services.Implementations
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private bool IsShell => Application.Current.MainPage is AppShell;

        public NavigationService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public Task PushAsync<TPage>(object? parameter = null) where TPage : Page
        {
            if(IsShell)
            {
                var route = typeof(TPage).Name;

                if (parameter != null)
                    return MainThread.InvokeOnMainThreadAsync(() => 
                            Shell.Current.GoToAsync($"{route}", true, new Dictionary<string, object>
                            {
                                { "Data", parameter }
                            }));

                return MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync(route));
            }
            else
            {
                var page = _serviceProvider.GetRequiredService<TPage>();

                if (page.BindingContext is IAcceptParameter vm)
                    vm.Apply(parameter);

                return MainThread.InvokeOnMainThreadAsync(async () =>
                        await Application.Current.MainPage!.Navigation.PushAsync(page));
            }           

        }

        public Task GoToRootAsync()
        {
            if (IsShell)
                return MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync("//MainPage"));

            return MainThread.InvokeOnMainThreadAsync(async () => 
                    await Application.Current.MainPage!.Navigation.PopToRootAsync());
        }

        public Task PopAsync()
        {
            if (IsShell)
                return MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync(".."));

            return MainThread.InvokeOnMainThreadAsync(async () => 
                    await Application.Current.MainPage!.Navigation.PopAsync());
        }
        
    }
}
