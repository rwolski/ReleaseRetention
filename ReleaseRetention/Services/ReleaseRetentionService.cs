using Microsoft.Extensions.Logging;
using ReleaseRetention.Data.Services;
using ReleaseRetention.Models;
using ReleaseRetention.Services.Results;
using System.Collections.Generic;
using System.Linq;

namespace ReleaseRetention.Services
{
    /// <summary>
    /// Service for retrieving the most recently deployed releases for each project/environment combination
    /// </summary>
    public class ReleaseRetentionService
    {
        private readonly EnvironmentDataService _environmentDataService;
        private readonly ProjectDataService _projectDataService;
        private readonly ReleaseDataService _releaseDataService;
        private readonly DeploymentDataService _deploymentDataService;
        private readonly ILogger<ReleaseRetentionService> _logger;

        public ReleaseRetentionService(
            EnvironmentDataService environmentDataService,
            ProjectDataService projectDataService, 
            ReleaseDataService releaseDataService,
            DeploymentDataService deploymentDataService,
            ILogger<ReleaseRetentionService> logger)
        {
            _environmentDataService = environmentDataService;
            _projectDataService = projectDataService;
            _releaseDataService = releaseDataService;
            _deploymentDataService = deploymentDataService;
            _logger = logger;
        }

        public IEnumerable<ReleaseRetentionResult> GetRecentReleases(int? limit = null)
        {
            if (limit <= 0)
            {
                // Handle bad input
                return new ReleaseRetentionResult[0];
            }

            // Get all of the data required and convert environments, projects and releases to dictionaries for faster lookup given larger data sets
            Dictionary<string, EnvironmentModel> environments = _environmentDataService.Query().ToDictionary(e => e.Id, e => e);
            Dictionary<string, ProjectModel> projects = _projectDataService.Query().ToDictionary(p => p.Id, p => p);
            Dictionary<string, ReleaseModel> releases = _releaseDataService.Query().ToDictionary(r => r.Id, r => r);

            return GetRecentReleasesForDeployments(_deploymentDataService.Query(), projects, environments, releases, limit);
        }

        /// <summary>
        /// Scans the deployments and returns the last N valid releases for each project/environment set
        /// </summary>
        /// <param name="deployments">A list of all deployments</param>
        /// <param name="projects">A dictionary of all projects for referencing</param>
        /// <param name="environments">A dictionary of all environments for referencing</param>
        /// <param name="releases">A dictionary of all releases for referencing</param>
        /// <param name="limit">An optional number of recent releases to return per project/environment</param>
        protected IEnumerable<ReleaseRetentionResult> GetRecentReleasesForDeployments(IEnumerable<DeploymentModel> deployments, IReadOnlyDictionary<string, ProjectModel> projects, IReadOnlyDictionary<string, EnvironmentModel> environments,
            IReadOnlyDictionary<string, ReleaseModel> releases, int? limit = null)
        {
            // Order deployments in descending order such that recent deployments will be the first to be retained
            deployments = deployments.OrderByDescending(d => d.DeployedAt);

            Dictionary<string, ReleaseRetentionResult> results = new Dictionary<string, ReleaseRetentionResult>();

            foreach (var deployment in deployments)
            {
                // Lookup the project, environment and release for this deployment
                if (!releases.TryGetValue(deployment.ReleaseId, out var release))
                {
                    _logger.LogInformation($"Skipping release deployment {deployment.Id} as release {deployment.ReleaseId} was not found.");
                    continue;
                }
                if (!environments.TryGetValue(deployment.EnvironmentId, out var environment))
                {
                    _logger.LogInformation($"Skipping release deployment {deployment.Id} as deployment environment {deployment.EnvironmentId} was not found.");
                    continue;
                }
                if (!projects.TryGetValue(release.ProjectId, out var project))
                {
                    _logger.LogInformation($"Skipping release deployment {deployment.Id} as release project {release.ProjectId} was not found.");
                    continue;
                }

                // Save the release
                RetainRelease(results, project, environment, release, deployment, limit);
            }

            return results.Select(r => r.Value).ToList();
        }

        /// <summary>
        /// Adds a release/deployment to a project/environment specific ReleaseRetentionResult object
        /// </summary>
        /// <param name="results">The results to append to</param>
        /// <param name="project">The deployment project</param>
        /// <param name="environment">The deployment environment</param>
        /// <param name="release">The release being deployed</param>
        /// <param name="deployment">The deployment event</param>
        /// <param name="limit">An optional number of recent releases to return per project/environment</param>
        protected void RetainRelease(Dictionary<string, ReleaseRetentionResult> results, ProjectModel project, EnvironmentModel environment, ReleaseModel release, DeploymentModel deployment, int? limit = null)
        {
            string projectEnvironmentKey = release.ProjectId + deployment.EnvironmentId;

            if (results.TryGetValue(projectEnvironmentKey, out var resultSet))
            {
                if (limit != null && resultSet.Releases.Count == limit)
                {
                    _logger.LogInformation($"Discarding release {release.Id} as last release deployment {deployment.Id} occurs after the most recent {limit} deployed releases.");
                    return;
                }

                // Add this release deployment to the set for the project/environment
                resultSet.Releases.Add(new DeployedRelease(release, deployment));

                _logger.LogInformation($"Keeping release {release.Id} as deployment {deployment.Id} is a recent deployment for this project/environment configuration ({resultSet.Releases.Count}/{limit}).");
            }
            else
            {
                // Add a new project/env type
                resultSet = new ReleaseRetentionResult(project, environment);
                resultSet.Releases.Add(new DeployedRelease(release, deployment));
                results.Add(projectEnvironmentKey, resultSet);

                _logger.LogInformation($"Keeping release {release.Id} as deployment {deployment.Id} is the most recent deployment for this project/environment configuration  (1/{limit}).");
            }
        }
    }
}
