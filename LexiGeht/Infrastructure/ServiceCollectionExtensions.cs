

using LexiGeht.Data;
using LexiGeht.Data.Entities;
using LexiGeht.Repositories;
using LexiGeht.Repositories.Interfaces;
using LexiGeht.Services.Implementations;
using LexiGeht.Services.Interfaces;
using LexiGeht.ViewModels.Auth;
using LexiGeht.ViewModels.Main;
using LexiGeht.Views.Auth;
using LexiGeht.Views.Main;

namespace LexiGeht.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string dbPath)
        {
            // HttpClient
            services.AddSingleton(new HttpClient());

            // Repositories
            services.AddSingleton<IDatabase>(s => new Database(dbPath));
            
            services.AddSingleton<IUserRepository, UserRepository>();

            // Repositories unitaires
            services.AddSingleton<IEntityRepository<CoursEntity>,       SQLiteRepository<CoursEntity>>();
            services.AddSingleton<IEntityRepository<CategoryEntity>,    SQLiteRepository<CategoryEntity>>();
            services.AddSingleton<IEntityRepository<QuizEntity>,        SQLiteRepository<QuizEntity>>();
            services.AddSingleton<IEntityRepository<QuestionEntity>,    SQLiteRepository<QuestionEntity>>();
            services.AddSingleton<IEntityRepository<AnswerEntity>,      SQLiteRepository<AnswerEntity>>();

            // Repositories agregates
            services.AddSingleton<IQuizAgregateRepository, QuizAgregateRepository>();

            // Repositories helpers
            services.AddSingleton<IRelationSyncService, RelationSyncService>();
            services.AddSingleton<IQuestionsAnswersRepository, QuestionsAnswersRepository>();

            // Services
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ISecurityService, SecurityService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IAppNavigator, AppNavigator>();
            services.AddSingleton<IAppAssetsService, AppAssetsService>();
            services.AddSingleton<ISheetsImportService, SheetsImportService>();
            services.AddSingleton<IQuizService, QuizService>();
            services.AddSingleton<IQuizAgregateService, QuizAgregateService>();
            services.AddSingleton<IQuizRunnerService, QuizRunnerService>();
            services.AddSingleton<IMediaStoreService, MediaStoreService>();

            // Shell
            services.AddTransient<AppShell>();

            // ViewModels and Views
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginPage>();
            services.AddTransient<SignupViewModel>();
            services.AddTransient<SignupPage>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<ProfilePage>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<AchievementsPage>();
            services.AddTransient<QuizPage>();
            services.AddTransient<QuizViewModel>();
            services.AddTransient<QuestionPage>();
            services.AddTransient<QuestionViewModel>();
            services.AddTransient<QuizResultPage>();
            services.AddTransient<QuizResultViewModel>();


            services.AddSingleton<IGoogleSheetService>(sp =>
            {
                const string spreadsheetId = "1UGvLUKXlRiJQOjxe3Ye2deFiThhU1QSjfwQ0je23LkE";

                var stream = FileSystem.OpenAppPackageFileAsync("lexigeht8bcbe84390ad.json")
                                       .GetAwaiter().GetResult();
                using (stream)
                    return new GoogleSheetService(stream, spreadsheetId);
            });

            return services;
        }

    }
}
