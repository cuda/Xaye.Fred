namespace Xaye.Fred
{
    /// <summary>
    /// How to aggregate data when requesting a frequency that is
    /// less frequent than the data is provided (i.e. converting
    /// quarterly data to annual).
    /// </summary>
    /// <remarks>See http://api.stlouisfed.org/docs/fred/series_observations.html#aggregation_method for more details.</remarks>
    public enum AggregationMethod
    {
        /// <summary>
        /// Average the data.
        /// </summary>
        Average = 0,

        /// <summary>
        /// Sum the data.
        /// </summary>
        Sum,

        /// <summary>
        /// Retrieve the last value of the period.
        /// </summary>
        EndOfPeriod
    }
}