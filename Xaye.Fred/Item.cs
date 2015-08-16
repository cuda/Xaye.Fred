namespace Xaye.Fred
{
    /// <summary>
    /// Base class for items that internally keep 
    /// a reference to a <see cref="Fred"/> object.
    /// Used to lazily retrieve additional data.
    /// </summary>
    public abstract class Item
    {
        protected static int CallLimit = 1000;

        protected Item(Fred fred)
        {
            Fred = fred;
        }

        protected object Lock { get; } = new object();

        internal Fred Fred { get; set; }

        /// <summary>
        /// Notes about an Item.
        /// </summary>
        public string Notes { get; set; }
    }
}