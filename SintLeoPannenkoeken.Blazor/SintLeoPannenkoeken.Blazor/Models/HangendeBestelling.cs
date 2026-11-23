namespace SintLeoPannenkoeken.Blazor.Models
{
    public class HangendeBestelling
    {
        public HangendeBestelling()
        {
        }

        public int Id { get; set; }
        
        /// <summary>
        /// The ID from the external system (Gravity Forms)
        /// </summary>
        public int OnlineBestellingId { get; set; }
        
        public string Naam { get; set; }
        public string Telefoon { get; set; }
        public string Email { get; set; }
        public int AantalPakken { get; set; }
        public string Opmerkingen { get; set; }
        public bool Betaald { get; set; }
        public bool Geleverd { get; set; }
        
        /// <summary>
        /// Street name from the external system
        /// </summary>
        public string Straat { get; set; }
        
        /// <summary>
        /// House number from the external system
        /// </summary>
        public string Nummer { get; set; }
        
        /// <summary>
        /// Bus number from the external system
        /// </summary>
        public string Bus { get; set; }
        
        /// <summary>
        /// Postal code from the external system
        /// </summary>
        public string Postcode { get; set; }
        
        /// <summary>
        /// City from the external system
        /// </summary>
        public string Gemeente { get; set; }
        
        /// <summary>
        /// Member name from the external system
        /// </summary>
        public string LidNaam { get; set; }
        
        /// <summary>
        /// Tak (age group) from the external system
        /// </summary>
        public string Tak { get; set; }
        
        /// <summary>
        /// Delivery method: "Leveren" or "Afhalen"
        /// </summary>
        public string Levering { get; set; }
        
        /// <summary>
        /// Date when the order was created in the external system
        /// </summary>
        public DateTime? DatumAangemaakt { get; set; }
        
        /// <summary>
        /// The scoutsjaar this order belongs to
        /// </summary>
        public int ScoutsjaarId { get; set; }
        
        /// <summary>
        /// Proposed match for StraatId (nullable, to be filled manually)
        /// </summary>
        public int? ProposedStraatId { get; set; }
        
        /// <summary>
        /// Proposed match for LidId (nullable, to be filled manually)
        /// </summary>
        public int? ProposedLidId { get; set; }
        
        /// <summary>
        /// Indicates if this pending order has been processed (imported or rejected)
        /// </summary>
        public bool Verwerkt { get; set; }
        
        /// <summary>
        /// JSON representation of the raw entry data from the external system
        /// </summary>
        public string? RawData { get; set; }
    }
}
