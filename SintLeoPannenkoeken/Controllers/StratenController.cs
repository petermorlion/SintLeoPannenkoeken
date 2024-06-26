﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.Data;
using SintLeoPannenkoeken.ViewModels.Straten;

namespace SintLeoPannenkoeken.Controllers
{
    [Authorize(Roles = "Admin,FinanciePloeg")]
    public class StratenController : Controller
    {
        private ILogger<StratenController> _logger;
        private ApplicationDbContext _dbContext;

        public StratenController(ILogger<StratenController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var straten = _dbContext.Straten.ToList();
            var zones = _dbContext.Zones.ToList();
            return View(new IndexViewModel(straten, zones));
        }
    }
}
