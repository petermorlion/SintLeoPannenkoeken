namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class GebruikerCreatedDto
    {
        public GebruikerCreatedDto(GebruikerDto gebruiker, string resetPasswordCode)
        {
            Gebruiker = gebruiker;
            ResetPasswordCode = resetPasswordCode;
        }

        public GebruikerDto Gebruiker { get; set; }
        public string ResetPasswordCode { get; set; }
    }
}
