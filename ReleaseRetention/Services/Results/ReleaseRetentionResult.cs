using ReleaseRetention.Models;
using System;
using System.Collections.Generic;

namespace ReleaseRetention.Services.Results
{
    public class ReleaseRetentionResult
    {
        public string ProjectName { get; }

        public string EnvironmentName { get; }

        public HashSet<DeployedRelease> Releases { get; }

        public ReleaseRetentionResult(ProjectModel project, EnvironmentModel environment)
        {
            ProjectName = project.Name;
            EnvironmentName = environment.Name;
            Releases = new HashSet<DeployedRelease>();
        }
    }

    public class DeployedRelease
    {
        public string ReleaseId { get; set; }

        public string DeploymentId { get; set; }

        public string Version { get; set; }

        public DateTime Created { get; set; }

        public DateTime DeployedAt { get; set; }

        public DeployedRelease(ReleaseModel release, DeploymentModel deployment)
        {
            ReleaseId = release.Id;
            DeploymentId = deployment.Id;
            Version = release.Version;
            Created = release.Created;
            DeployedAt = deployment.DeployedAt;
        }
    }
}
