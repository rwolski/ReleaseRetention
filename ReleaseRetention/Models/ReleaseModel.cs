using System;

namespace ReleaseRetention.Models
{
    /// <summary>
    /// Release domain model
    /// </summary>
    public class ReleaseModel
    {
        public string Id { get; set; }

        public string ProjectId { get; set; }

        public string Version { get; set; }

        public DateTime Created { get; set; }
    }
}
