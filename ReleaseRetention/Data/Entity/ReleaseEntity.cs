using System;

namespace ReleaseRetention.Data.Entity
{
    /// <summary>
    /// Release data entity
    /// </summary>
    public class ReleaseEntity
    {
        public string Id { get; set; }

        public string ProjectId { get; set; }

        public string Version { get; set; }

        public DateTime Created { get; set; }
    }
}
