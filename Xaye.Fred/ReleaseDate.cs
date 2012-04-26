using System;

namespace Xaye.Fred
{
    /// <summary>
    /// The date of release
    /// </summary>
    public class ReleaseDate
    {
        /// <summary>
        /// The ID of the release.
        /// </summary>
        public int ReleaseId { get; set; }

        /// <summary>
        /// The name of the release.
        /// </summary>
        public string ReleaseName { get; set; }

        /// <summary>
        /// The date of the release.
        /// </summary>
        public DateTime Date { get; set; }
    }
}