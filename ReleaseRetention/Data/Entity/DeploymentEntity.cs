using System;

namespace ReleaseRetention.Data.Entity
{
    /// <summary>
    /// Deployment data entity
    /// </summary>
    public class DeploymentEntity
    {
        public string Id { get; set; }

        public string ReleaseId { get; set; }

        public string EnvironmentId { get; set; }

        public DateTime DeployedAt { get; set; }
    }
}
