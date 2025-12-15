using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using LexiGeht.Services.Interfaces;

namespace LexiGeht.Services.Implementations
{
    public class GoogleSheetService : IGoogleSheetService
    {
        private readonly SheetsService _sheetsService;

        private readonly string _spreadsheetId;

        public GoogleSheetService(Stream credentialStream, string spreadsheetId)
        {
            var credential = GoogleCredential.FromStream(credentialStream)
                              .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            _sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "LexiGeht Sheets Service",
            });

            _spreadsheetId = spreadsheetId;
        }

        public GoogleSheetService(string credentialPath, string spreadsheetId)
        {
            // credentialPath = шлях, який ти передаєш у GoogleSheetService
            if (!File.Exists(credentialPath))
                throw new FileNotFoundException("Credential file not found", credentialPath);

            var json = File.ReadAllText(credentialPath);
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("Credential file is empty.");

            using var doc = System.Text.Json.JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("type", out var typeProp) ||
                typeProp.GetString() != "service_account")
                throw new InvalidOperationException("Credential file is not a service account key (type must be 'service_account').");


            GoogleCredential credential;
            using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);
            }

            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "LexiGeht Sheets Service",
            });

            _spreadsheetId = spreadsheetId;
        }

        public async Task<IList<IList<object>>> GetSheetDataAsync(string range)
        {
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);
            var response = await request.ExecuteAsync();
            return response.Values;
        }

    }
}
