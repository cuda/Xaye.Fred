namespace Xaye.Fred
{
    /// <summary>
    /// How to transform the downloaded data.
    /// </summary>
    /// <remarks>See http://alfred.stlouisfed.org/help#growth_formulas for more information.</remarks>
    public enum Transformation
    {
        /// <summary>
        /// Data is not transformed.
        /// </summary>
        None = 0,

        /// <summary>
        /// Change from one period to the next: x(t) - x(t-1)
        /// </summary>
        Change,

        /// <summary>
        /// Change from a year ago: x(t) - x(t-n_obs_per_yr)
        /// </summary>
        ChangeYear,

        /// <summary>
        /// Percentage change from one period to the next:
        /// ((x(t)/x(t-1)) - 1) * 100
        /// </summary>
        PercentChange,

        /// <summary>
        /// Percentage change from a year ago:
        /// ((x(t)/x(t-n_obs_per_yr)) - 1) * 10
        /// </summary>
        PercentChangeYear,

        /// <summary>
        /// Compounded annual rate of change:
        /// (((x(t)/x(t-1)) ** (n_obs_per_yr)) - 1) * 100
        /// </summary>
        CompoundedAnnualRateChange,

        /// <summary>
        /// Continuously compounded rate of change:
        /// (ln(x(t)) - ln(x(t-1))) * 100
        /// </summary>
        ContinuouslyCompoundedRateChange,

        /// <summary>
        /// Continuously compounded annual rate of change:
        /// ((ln(x(t)) - ln(x(t-1))) * 100) * n_obs_per_yr
        /// </summary>
        ContinuouslyCompoundedAnnualRateChange,

        /// <summary>
        /// Natural Log: ln(x(t))
        /// </summary>
        Log
    }
}