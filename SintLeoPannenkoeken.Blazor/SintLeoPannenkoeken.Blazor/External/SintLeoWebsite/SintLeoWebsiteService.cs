using ClosedXML.Excel;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.External.SintLeoWebsite
{
    public class SintLeoWebsiteService
    {
        private readonly HttpClient _httpClient;
        private readonly string _bestellingenExportEndpoint;

        public SintLeoWebsiteService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _bestellingenExportEndpoint = Environment.GetEnvironmentVariable("BestellingenExportEndpoint") ?? "";
        }

        public async Task<IList<PendingOnlineBestellingDto>> GetOnlineBestelingen()
        {
            var requestUri = $"/{_bestellingenExportEndpoint}";
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            using (var memoryStream = new MemoryStream())
            {
                await response.Content.CopyToAsync(memoryStream);
                var excelWorkbook = new XLWorkbook(memoryStream);
                var worksheet = excelWorkbook.Worksheet(1);

                var bestellingen = new List<PendingOnlineBestellingDto>();

                var idColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString() == "ID inzending");
                var firstNameColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString() == "Voornaam");
                var lastNameColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString() == "Familienaam");
                var zelfAfhalenColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString() == "Keuze leveren of zelf afhalen");
                var straatColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString() == "Straat");
                var nummerColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString() == "Huisnummer");
                var busColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString() == "Bus (optioneel)");
                var telefoonColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString() == "Telefoon");
                var emailColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString() == "E-mail");
                var aantalColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString() == "Aantal");
                var lidColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString().Contains("het lid"));
                var takColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString().Contains("Tak van"));
                var opmerkingColumn = worksheet.Row(1).Cells().FirstOrDefault(c => c.Value.ToString().Contains("Opmerking"));

                if (idColumn == null || firstNameColumn == null || lastNameColumn == null || zelfAfhalenColumn == null ||
                    straatColumn == null || nummerColumn == null || busColumn == null || telefoonColumn == null ||
                    emailColumn == null || aantalColumn == null || lidColumn == null || takColumn == null ||
                    opmerkingColumn == null)
                {
                    throw new Exception("Een of meer vereiste kolommen ontbreken in het Excel-bestand.");
                }

                foreach (var row in worksheet.RowsUsed().Skip(1))
                {
                    var bestelling = new PendingOnlineBestellingDto
                    {
                        Id = (int)row.Cell(idColumn.Address.ColumnNumber).GetDouble(),
                        Naam = $"{row.Cell(firstNameColumn.Address.ColumnNumber).GetString()} {row.Cell(lastNameColumn.Address.ColumnNumber).GetString()}",
                        Telefoon = row.Cell(telefoonColumn.Address.ColumnNumber).GetString(),
                        AantalPakken = (int)row.Cell(aantalColumn.Address.ColumnNumber).GetDouble(),
                        Opmerkingen = row.Cell(opmerkingColumn.Address.ColumnNumber).GetString(),
                        Betaald = false,
                        Geleverd = row.Cell(zelfAfhalenColumn.Address.ColumnNumber).GetString().ToLower().Contains("geleverd"),
                        Straat = row.Cell(straatColumn.Address.ColumnNumber).GetString(),
                        Nummer = row.Cell(nummerColumn.Address.ColumnNumber).GetString(),
                        Bus = row.Cell(busColumn.Address.ColumnNumber).GetString(),
                        Lid = row.Cell(lidColumn.Address.ColumnNumber).GetString(),
                    };

                    bestellingen.Add(bestelling);
                }

                return bestellingen;
            }
        }
    }
}
