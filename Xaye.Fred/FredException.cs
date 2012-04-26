using System;

namespace Xaye.Fred
{
    /// <summary>
    /// Exception class thrown by <see cref="Fred"/> when
    /// a given parameter is incorrect.
    /// </summary>
    public class FredExecption : Exception
    {
        /// <summary>
        /// Creates FredException with an error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        public FredExecption(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates FredException with an error message and the original exception.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="inner">The original exception that was thrown.</param>
        public FredExecption(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}