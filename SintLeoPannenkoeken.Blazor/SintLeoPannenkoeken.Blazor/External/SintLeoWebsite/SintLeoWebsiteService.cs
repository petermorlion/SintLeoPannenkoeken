using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.External.SintLeoWebsite
{
    public class SintLeoWebsiteService
    {
        private readonly HttpClient _httpClient;
        private readonly SintLeoWebsiteSettings _settings;
        private readonly ILogger<SintLeoWebsiteService> _logger;

        public SintLeoWebsiteService(HttpClient httpClient, IConfiguration configuration, ILogger<SintLeoWebsiteService> logger = null)
        {
            _httpClient = httpClient;
            _settings = configuration.GetSection("SintLeoWebsite").Get<SintLeoWebsiteSettings>() ?? new SintLeoWebsiteSettings();
            _logger = logger;
        }

        public async Task<IList<PendingOnlineBestellingDto>> GetOnlineBestelingen(int scoutsjaarBegin, DateTime? startDate = null)
        {
            var allEntries = new List<PendingOnlineBestellingDto>();
            int page = 1;
            bool hasMore = true;

            if (startDate == null)
            {
                startDate = new DateTime(scoutsjaarBegin, 1, 1).AddMonths(-_settings.StartDateMonthsBeforeJanuary);
            }

            while (hasMore)
            {
                var entriesResponse = await GetEntriesPage(page, _settings.BatchSize, startDate.Value);

                if (entriesResponse == null || entriesResponse.Entries == null || entriesResponse.Entries.Count == 0)
                {
                    hasMore = false;
                    break;
                }

                var filteredEntries = entriesResponse.Entries.Where(e => 
                    DateTime.TryParseExact(e.DateCreated, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var entryDate) &&
                    entryDate >= startDate.Value).ToList();
                
                allEntries.AddRange(ConvertToPendingOnlineBestellingDto(filteredEntries));
                hasMore = allEntries.Count < entriesResponse.TotalCount;
                page++;
            }

            return allEntries;
        }

        private async Task<GravityFormsResponse?> GetEntriesPage(int page, int perPage, DateTime startDate)
        {
            // Build base query parameters
            var queryParams = new SortedDictionary<string, string>
            {
                { "form_ids", _settings.FormId },
                { "per_page", perPage.ToString() },
                { "page", page.ToString() }
            };

            // Generate OAuth parameters (including signature) - this will include the base query params
            var allParams = GenerateOAuthQueryParams("GET", $"{_settings.BaseUrl}/entries", "", queryParams);
            
            // allParams already contains the base query params plus OAuth params, so use it directly
            queryParams = allParams;

            // Build URL with all parameters
            var queryString = string.Join("&", queryParams.Select(kvp => $"{UrlEncode(kvp.Key)}={UrlEncode(kvp.Value)}"));
            var urlBuilder = new UriBuilder($"{_settings.BaseUrl}/entries")
            {
                Query = queryString
            };

            _logger?.LogDebug("Request URL: {Url}", MaskSensitiveData(urlBuilder.Uri.ToString()));

            var request = new HttpRequestMessage(HttpMethod.Get, urlBuilder.Uri);

            var response = await _httpClient.SendAsync(request);

            _logger?.LogDebug("Response status code: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger?.LogError("API call failed with status {StatusCode}. Response: {ErrorContent}", response.StatusCode, errorContent);
                throw new HttpRequestException($"API call failed with status {response.StatusCode}: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            _logger?.LogDebug("Response content length: {ContentLength} bytes", content.Length);
            return ParseGravityFormsResponse(content);
        }

        private GravityFormsResponse ParseGravityFormsResponse(string json)
        {
            var response = new GravityFormsResponse();
            var jsonDoc = System.Text.Json.JsonDocument.Parse(json);

            if (jsonDoc.RootElement.TryGetProperty("total_count", out var totalCount))
            {
                response.TotalCount = totalCount.GetInt32();
            }

            if (jsonDoc.RootElement.TryGetProperty("entries", out var entries) && entries.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var entryElement in entries.EnumerateArray())
                {
                    var entry = new GravityFormsEntry();

                    if (entryElement.TryGetProperty("id", out var id)) entry.Id = id.GetString() ?? "";
                    if (entryElement.TryGetProperty("form_id", out var formId)) entry.FormId = formId.GetString() ?? "";
                    if (entryElement.TryGetProperty("date_created", out var dateCreated)) entry.DateCreated = dateCreated.GetString() ?? "";
                    if (entryElement.TryGetProperty("date_updated", out var dateUpdated)) entry.DateUpdated = dateUpdated.GetString() ?? "";
                    if (entryElement.TryGetProperty("status", out var status)) entry.Status = status.GetString() ?? "";
                    if (entryElement.TryGetProperty("payment_status", out var paymentStatus)) entry.PaymentStatus = paymentStatus.GetString() ?? "";
                    if (entryElement.TryGetProperty("is_fulfilled", out var isFulfilled)) entry.IsFulfilled = isFulfilled.GetString() ?? "";

                    foreach (var prop in entryElement.EnumerateObject())
                    {
                        string stringValue = prop.Value.ValueKind switch
                        {
                            System.Text.Json.JsonValueKind.String => prop.Value.GetString() ?? "",
                            System.Text.Json.JsonValueKind.Number => prop.Value.ToString(),
                            System.Text.Json.JsonValueKind.True => "1",
                            System.Text.Json.JsonValueKind.False => "0",
                            System.Text.Json.JsonValueKind.Null => "",
                            _ => prop.Value.ToString()
                        };

                        if (!new[] { "id", "form_id", "date_created", "date_updated", "status", "payment_status", "is_fulfilled" }.Contains(prop.Name))
                        {
                            entry.Fields[prop.Name] = stringValue;
                        }
                    }

                    response.Entries.Add(entry);
                }
            }

            return response;
        }

        private SortedDictionary<string, string> GenerateOAuthQueryParams(string httpMethod, string baseUrl, string queryString, SortedDictionary<string, string> existingParams = null)
        {
            var oauthConsumerKey = _settings.OAuthConsumerKey;
            var oauthConsumerSecret = _settings.OAuthConsumerSecret;
            var oauthToken = _settings.OAuthToken;
            var oauthTokenSecret = _settings.OAuthTokenSecret;

            _logger?.LogDebug("Generating OAuth query params. ConsumerKey: {ConsumerKey}, Token: {Token}, TokenSecret: [REDACTED]", 
                oauthConsumerKey, oauthToken);

            if (string.IsNullOrEmpty(oauthConsumerKey) || string.IsNullOrEmpty(oauthConsumerSecret))
            {
                throw new InvalidOperationException("OAuth Consumer Key and Consumer Secret must be configured.");
            }

            var timestamp = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            var nonce = GenerateNonce();

            _logger?.LogDebug("OAuth nonce: {Nonce}, timestamp: {Timestamp}", nonce, timestamp);

            var oauthParams = new SortedDictionary<string, string>
            {
                { "oauth_consumer_key", oauthConsumerKey },
                { "oauth_nonce", nonce },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", timestamp },
                { "oauth_version", "1.0" }
            };

            if (!string.IsNullOrEmpty(oauthToken))
            {
                oauthParams.Add("oauth_token", oauthToken);
            }

            // Add existing query parameters to oauthParams for signature calculation
            if (existingParams != null)
            {
                foreach (var kvp in existingParams)
                {
                    if (!oauthParams.ContainsKey(kvp.Key))
                    {
                        oauthParams.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(queryString))
            {
                var pairs = queryString.Split('&');
                foreach (var pair in pairs)
                {
                    var parts = pair.Split(new[] { '=' }, 2);
                    if (parts.Length == 2)
                    {
                        var key = HttpUtility.UrlDecode(parts[0]);
                        var value = HttpUtility.UrlDecode(parts[1]);
                        if (!string.IsNullOrEmpty(key) && !oauthParams.ContainsKey(key))
                        {
                            oauthParams.Add(key, value);
                        }
                    }
                    else if (parts.Length == 1 && !string.IsNullOrEmpty(parts[0]))
                    {
                        var key = HttpUtility.UrlDecode(parts[0]);
                        if (!oauthParams.ContainsKey(key))
                        {
                            oauthParams.Add(key, "");
                        }
                    }
                }
            }

            // Build URL without query string for signature base
            var uri = new Uri(baseUrl);
            var signatureBase = BuildSignatureBase(httpMethod, uri, oauthParams, null);
            var signingKey = UrlEncode(oauthConsumerSecret) + "&" + UrlEncode(oauthTokenSecret ?? "");

            _logger?.LogDebug("Signature base: {SignatureBase}", signatureBase);
            _logger?.LogDebug("Signing key: {SigningKeyPrefix}[REDACTED]", 
                signingKey.Substring(0, signingKey.IndexOf('&') > 0 ? signingKey.IndexOf('&') : signingKey.Length) + "[REDACTED]");

            var signature = GenerateSignature(signingKey, signatureBase);
            oauthParams.Add("oauth_signature", signature);

            _logger?.LogDebug("Generated signature: {Signature}", signature);

            return oauthParams;
        }

        private string BuildSignatureBase(string httpMethod, Uri uri, SortedDictionary<string, string> oauthParams, string requestBody)
        {
            // Sort by encoded key, then encoded value (RFC 5849 §3.4.1.3.2)
            var paramString = string.Join("&",
                oauthParams
                    .Select(kvp => new { Key = UrlEncode(kvp.Key), Value = UrlEncode(kvp.Value) })
                    .OrderBy(p => p.Key, StringComparer.Ordinal)
                    .ThenBy(p => p.Value, StringComparer.Ordinal)
                    .Select(p => p.Key + "=" + p.Value));

            var host = uri.Host.ToLowerInvariant();
            if (!uri.IsDefaultPort) host += ":" + uri.Port;
            var baseUrl = uri.Scheme + "://" + host + uri.AbsolutePath;

            // Encode the WHOLE parameter string a second time
            return httpMethod.ToUpperInvariant() + "&" + UrlEncode(baseUrl) + "&" + UrlEncode(paramString);
        }

        private string GenerateSignature(string key, string baseString)
        {
            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(baseString));
                return Convert.ToBase64String(hash);
            }
        }

        private string GenerateNonce()
        {
            var nonceBytes = new byte[24];
            RandomNumberGenerator.Fill(nonceBytes);

            // Base64 of 24 bytes = 32 chars; swap URL-unsafe chars instead of removing them,
            // so the length stays a constant 32 characters
            return Convert.ToBase64String(nonceBytes)
                .Replace('+', 'a')
                .Replace('/', 'b');
        }

        private string MaskSensitiveData(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            try
            {
                // Simple masking: replace values after oauth_consumer_secret= and oauth_token_secret=
                var result = input;
                
                // Find and replace oauth_consumer_secret value in Authorization header format
                int secretStart = result.IndexOf("oauth_consumer_secret=\"");
                if (secretStart >= 0)
                {
                    int quoteStart = secretStart + "oauth_consumer_secret=\"".Length;
                    int quoteEnd = result.IndexOf("\"", quoteStart);
                    if (quoteEnd >= 0)
                    {
                        result = result.Substring(0, quoteStart) + "[REDACTED]" + result.Substring(quoteEnd);
                    }
                }
                
                // Find and replace oauth_consumer_secret value in query string format
                secretStart = result.IndexOf("oauth_consumer_secret=");
                if (secretStart >= 0)
                {
                    int valueStart = secretStart + "oauth_consumer_secret=".Length;
                    int valueEnd = result.IndexOf('&', valueStart);
                    if (valueEnd < 0) valueEnd = result.Length;
                    result = result.Substring(0, valueStart) + "[REDACTED]" + result.Substring(valueEnd);
                }
                
                // Find and replace oauth_token_secret value in Authorization header format
                secretStart = result.IndexOf("oauth_token_secret=\"");
                if (secretStart >= 0)
                {
                    int quoteStart = secretStart + "oauth_token_secret=\"".Length;
                    int quoteEnd = result.IndexOf("\"", quoteStart);
                    if (quoteEnd >= 0)
                    {
                        result = result.Substring(0, quoteStart) + "[REDACTED]" + result.Substring(quoteEnd);
                    }
                }
                
                // Find and replace oauth_token_secret value in query string format
                secretStart = result.IndexOf("oauth_token_secret=");
                if (secretStart >= 0)
                {
                    int valueStart = secretStart + "oauth_token_secret=".Length;
                    int valueEnd = result.IndexOf('&', valueStart);
                    if (valueEnd < 0) valueEnd = result.Length;
                    result = result.Substring(0, valueStart) + "[REDACTED]" + result.Substring(valueEnd);
                }
                
                return result;
            }
            catch
            {
                return input;
            }
        }

        private string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            var encoded = Uri.EscapeDataString(value);
            encoded = encoded.Replace("+", "%20");
            encoded = encoded.Replace("*", "%2A");
            encoded = encoded.Replace("'", "%27");
            encoded = encoded.Replace("(", "%28");
            encoded = encoded.Replace(")", "%29");
            return encoded;
        }

        private List<PendingOnlineBestellingDto> ConvertToPendingOnlineBestellingDto(List<GravityFormsEntry> entries)
        {
            var result = new List<PendingOnlineBestellingDto>();

            foreach (var entry in entries)
            {
                var firstName = GetFieldValue(entry, "7.6");
                var lastName = GetFieldValue(entry, "7.3");
                var naam = (firstName + " " + lastName).Trim();

                var dto = new PendingOnlineBestellingDto
                {
                    Id = int.TryParse(entry.Id, out var id) ? id : 0,
                    DatumAangemaakt = DateTime.TryParseExact(entry.DateCreated, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateCreated)
                        ? dateCreated
                        : (DateTime.TryParse(entry.DateCreated, out dateCreated) ? dateCreated : (DateTime?)null),
                    Naam = naam,
                    Straat = GetFieldValue(entry, "44"),
                    Nummer = GetFieldValue(entry, "18"),
                    Bus = GetFieldValue(entry, "46", "20"),
                    Postcode = GetFieldValue(entry, "49"),
                    Gemeente = GetFieldValue(entry, "26"),
                    Telefoon = GetFieldValue(entry, "28"),
                    Email = GetFieldValue(entry, "9"),
                    AantalPakken = int.TryParse(GetFieldValue(entry, "30"), out var aantal) ? aantal : 0,
                    Lid = GetFieldValue(entry, "32.3", "32.1", "32"),
                    Tak = GetFieldValue(entry, "33"),
                    Opmerkingen = GetFieldValue(entry, "35"),
                    Levering = GetFieldValue(entry, "17"),
                    Betaald = entry.PaymentStatus == "Paid" || entry.PaymentStatus == "Processing",
                    Geleverd = entry.IsFulfilled == "1",
                    RawData = System.Text.Json.JsonSerializer.Serialize(entry)
                };

                result.Add(dto);
            }

            return result;
        }

        private string GetFieldValue(GravityFormsEntry entry, params string[] fieldIds)
        {
            foreach (var fieldId in fieldIds)
            {
                if (entry.Fields != null && entry.Fields.TryGetValue(fieldId, out var value) && !string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }
            return "";
        }

        public class GravityFormsResponse
        {
            public int TotalCount { get; set; }
            public List<GravityFormsEntry> Entries { get; set; } = new List<GravityFormsEntry>();
        }

        public class GravityFormsEntry
        {
            public string Id { get; set; } = "";
            public string FormId { get; set; } = "";
            public string DateCreated { get; set; } = "";
            public string DateUpdated { get; set; } = "";
            public string Status { get; set; } = "";
            public string PaymentStatus { get; set; } = "";
            public string PaymentAmount { get; set; } = "";
            public string PaymentMethod { get; set; } = "";
            public string IsFulfilled { get; set; } = "";
            public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
        }
    }
}
