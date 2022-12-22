﻿using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class TakViewModel
    {
        private readonly Tak _tak;

        public TakViewModel(Tak lid)
        {
            _tak = lid;
            if (_tak == null)
            {
                _tak = new Tak("");
            }
        }

        public int Id => _tak.Id;
        public string Afkorting => _tak.Afkorting;
    }
}
