using System;

namespace ReleaseRetention.Models
{
    /// <summary>
    /// Deployment domain model
    /// </summary>
    public class DeploymentModel
    {
        public string Id { get; set; }

        public string ReleaseId { get; set; }

        public string EnvironmentId { get; set; }

        public DateTime DeployedAt { get; set; }
    }
}
