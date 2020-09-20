using ReleaseRetention.Data.Entity;
using ReleaseRetention.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ReleaseRetention.Data.Services
{
    /// <summary>
    /// Data service for releases.
    /// Mocked query returning stubbed json data
    /// </summary>
    public class ReleaseDataService
    {
        public ReleaseDataService()
        {
        }

        public virtual IQueryable<ReleaseModel> Query()
        {
            string releasesText = File.ReadAllText("Files/Releases.json");
            IEnumerable<ReleaseEntity> releases = JsonSerializer.Deserialize<IEnumerable<ReleaseEntity>>(releasesText);

            // TODO: Move mapping logic into a mapper class
            return releases.Select(r => new ReleaseModel
            {
                Id = r.Id,
                ProjectId = r.ProjectId,
                Version = r.Version,
                Created = r.Created
            }).AsQueryable();
        }
    }
}
