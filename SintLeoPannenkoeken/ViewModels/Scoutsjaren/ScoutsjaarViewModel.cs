﻿using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Scoutsjaren
{
    public class ScoutsjaarViewModel
    {
        private readonly Scoutsjaar _scoutsjaar;

        public ScoutsjaarViewModel(Scoutsjaar scoutsjaar)
        {
            _scoutsjaar = scoutsjaar;
        }

        public int Begin => _scoutsjaar.Begin;
        public int PannenkoekenPerPak => _scoutsjaar.PannenkoekenPerPak;
    }
}
