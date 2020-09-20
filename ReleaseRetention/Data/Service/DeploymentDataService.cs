using ReleaseRetention.Data.Entity;
using ReleaseRetention.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ReleaseRetention.Data.Services
{
    /// <summary>
    /// Data service for deployments.
    /// Mocked query returning stubbed json data
    /// </summary>
    public class DeploymentDataService
    {
        public DeploymentDataService()
        {
        }

        public virtual IQueryable<DeploymentModel> Query()
        {
            string deploymentsText = File.ReadAllText("Files/Deployments.json");
            IEnumerable<DeploymentEntity> deployments = JsonSerializer.Deserialize<IEnumerable<DeploymentEntity>>(deploymentsText);

            // TODO: Move mapping logic into a mapper class
            return deployments.Select(d => new DeploymentModel
            {
                Id = d.Id,
                EnvironmentId = d.EnvironmentId,
                ReleaseId = d.ReleaseId,
                DeployedAt = d.DeployedAt
            }).AsQueryable();
        }
    }
}
