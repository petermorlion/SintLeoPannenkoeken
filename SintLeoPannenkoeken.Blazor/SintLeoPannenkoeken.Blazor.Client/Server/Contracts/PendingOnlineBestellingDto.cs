using System.Text.Json.Serialization;

namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class PendingOnlineBestellingDto
    {
        public int Id { get; set; }
        public string Naam { get; set; } = "";
        public string Telefoon { get; set; } = "";
        public string Email { get; set; } = "";
        public int AantalPakken { get; set; }
        public string Opmerkingen { get; set; } = "";
        public bool Betaald { get; set; }
        public bool Geleverd { get; set; }
        
        /// <summary>
        /// Street name from the external system
        /// </summary>
        public string Straat { get; set; } = "";
        public int? ProposedStraatId { get; set; }
        
        /// <summary>
        /// Transient property for UI binding - the selected street DTO
        /// </summary>
        [JsonIgnore]
        public StraatDto? ProposedStraat { get; set; }
        
        /// <summary>
        /// House number from the external system
        /// </summary>
        public string Nummer { get; set; } = "";
        
        /// <summary>
        /// Bus number from the external system
        /// </summary>
        public string Bus { get; set; } = "";
        
        /// <summary>
        /// Postal code from the external system
        /// </summary>
        public string Postcode { get; set; } = "";
        
        /// <summary>
        /// City from the external system
        /// </summary>
        public string Gemeente { get; set; } = "";
        
        /// <summary>
        /// Member name from the external system
        /// </summary>
        public string Lid { get; set; } = "";
        public int? ProposedLidId { get; set; }
        
        /// <summary>
        /// Transient property for UI binding - the selected member DTO
        /// </summary>
        [JsonIgnore]
        public LidDto? ProposedLid { get; set; }
        
        /// <summary>
        /// Tak (age group) from the external system
        /// </summary>
        public string Tak { get; set; } = "";
        
        /// <summary>
        /// Delivery method: "Leveren" or "Afhalen"
        /// </summary>
        public string Levering { get; set; } = "";
        
        /// <summary>
        /// Date when the order was created in the external system
        /// </summary>
        public DateTime? DatumAangemaakt { get; set; }
        
        /// <summary>
        /// Indicates if this pending order has been processed
        /// </summary>
        public bool Verwerkt { get; set; }
        
        /// <summary>
        /// JSON representation of the raw entry data
        /// </summary>
        public string? RawData { get; set; }
    }
}
