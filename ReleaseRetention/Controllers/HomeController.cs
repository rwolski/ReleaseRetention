using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReleaseRetention.Models;
using ReleaseRetention.Services;
using ReleaseRetention.Services.Results;

namespace ReleaseRetention.Controllers
{
    public class HomeController : Controller
    {
        private readonly ReleaseRetentionService _releaseRetentionService;

        public HomeController(ReleaseRetentionService releaseRetentionService)
        {
            _releaseRetentionService = releaseRetentionService;
        }

        public IActionResult Index(RetainedReleases model)
        {
            var releases = _releaseRetentionService.GetRecentReleases(model.Limit);
            model.ProjectReleases = releases;

            return View(model);
        }

        //[HttpGet("releases")]
        //public IActionResult GetReleases(int? limit)
        //{            
        //    return View("Index", );
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class RetainedReleases
    {
        public IEnumerable<ReleaseRetentionResult> ProjectReleases { get; set; }

        public int? Limit { get; set; }
    }
}
