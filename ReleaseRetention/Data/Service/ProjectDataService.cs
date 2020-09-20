using ReleaseRetention.Data.Entity;
using ReleaseRetention.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ReleaseRetention.Data.Services
{
    /// <summary>
    /// Data service for projects.
    /// Mocked query returning stubbed json data
    /// </summary>
    public class ProjectDataService
    {
        public ProjectDataService()
        {
        }

        public virtual IQueryable<ProjectModel> Query()
        {
            string projectsText = File.ReadAllText("Files/Projects.json");
            IEnumerable<ProjectEntity> projects = JsonSerializer.Deserialize<IEnumerable<ProjectEntity>>(projectsText);

            // TODO: Move mapping logic into a mapper class
            return projects.Select(p => new ProjectModel
            {
                Id = p.Id,
                Name = p.Name
            }).AsQueryable();
        }
    }
}
