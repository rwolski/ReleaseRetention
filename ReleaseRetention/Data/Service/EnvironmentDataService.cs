using ReleaseRetention.Data.Entity;
using ReleaseRetention.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ReleaseRetention.Data.Services
{
    /// <summary>
    /// Data service for environments.
    /// Mocked query returning stubbed json data
    /// </summary>
    public class EnvironmentDataService
    {
        public EnvironmentDataService()
        {
        }

        public virtual IQueryable<EnvironmentModel> Query()
        {
            string environmentsText = File.ReadAllText("Files/Environments.json");
            IEnumerable<EnvironmentEntity> environments = JsonSerializer.Deserialize<IEnumerable<EnvironmentEntity>>(environmentsText);

            // TODO: Move mapping logic into a mapper class
            return environments.Select(e => new EnvironmentModel
            {
                Id = e.Id,
                Name = e.Name
            }).AsQueryable();
        }
    }
}
