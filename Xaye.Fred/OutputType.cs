namespace Xaye.Fred
{
    /// <summary>
    /// The type of data to retrieve.
    /// </summary>
    /// <remarks>See http://alfred.stlouisfed.org/help/downloaddata#outputformats for 
    /// more details.</remarks>
    public enum OutputType
    {
        /// <summary>
        /// All data as it is available today (may have been revised since its initial release).
        /// </summary>
        RealTime,

        /// <summary>
        /// All data that was available on a specific date in history.
        /// </summary>
        VintageAll,

        /// <summary>
        /// New or revised data that was made available on a specific date in history.
        /// </summary>
        VintageNewRevised,

        /// <summary>
        /// All data as it was initially released.
        /// </summary>
        IntialReleaseOnly
    }
}