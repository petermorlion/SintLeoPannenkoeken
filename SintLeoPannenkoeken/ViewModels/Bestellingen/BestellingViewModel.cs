﻿using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class BestellingViewModel
    {
        private readonly Bestelling _bestelling;

        public BestellingViewModel(Bestelling bestelling)
        {
            _bestelling = bestelling;
        }

        public int Id => _bestelling.Id;
        public int BestellingNummer => _bestelling.BestellingNummer;
        public string Naam => _bestelling.Naam;
        public string Telefoon => _bestelling.Telefoon;
        public int AantalPakken => _bestelling.AantalPakken;
        public string Opmerkingen => _bestelling.Opmerkingen;
        public bool Betaald => _bestelling.Betaald;
        public LidViewModel Lid => new LidViewModel(_bestelling.Lid);
        public TakViewModel Tak => new TakViewModel(_bestelling.Tak);
        public int StraatId => _bestelling.StraatId;
        public string Straat => _bestelling.Straat.Naam;
        public string Nummer => _bestelling.Nummer;
        public string Bus => _bestelling.Bus;
        public string Code => $"{_bestelling?.Straat?.Zone?.Naam} {_bestelling?.Straat?.Nummer}";
        public bool Geleverd => _bestelling.Geleverd;
    }
}
