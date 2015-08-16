using System;
using System.Collections.Generic;

namespace Xaye.Fred
{
    /// <summary>
    /// Represents a category of data.
    /// </summary>
    public class Category : Item
    {
        private readonly Lazy<IEnumerable<Category>> _childern;
        private readonly Lazy<Category> _parent;
        private readonly Lazy<IEnumerable<Category>> _related;
        private readonly Lazy<IList<Series>> _series;

        internal Category(Fred fred) : base(fred)
        {
            _childern = new Lazy<IEnumerable<Category>>(() => Fred.GetCategoryChildern(Id).Result);
            _parent = new Lazy<Category>(() => Id == 0 ? this : Fred.GetCategory(ParentId).Result);
            _related = new Lazy<IEnumerable<Category>>(() => Fred.GetCategoryRelated(Id).Result);
            _series = new Lazy<IList<Series>>(
                () =>
                {
                    var series = (List<Series>) Fred.GetCategorySeries(Id, DateTime.Today, DateTime.Today).Result;
                    var count = series?.Count;
                    var call = 1;
                    while (count == CallLimit)
                    {
                        var more = (List<Series>) Fred.GetCategorySeries(Id, DateTime.Today, DateTime.Today, CallLimit, call*CallLimit).Result;
                        series.AddRange(more);
                        count = more.Count;
                        call++;
                    }

                    return series;
                }
                );
        }

        /// <summary>
        /// Category ID. Top category ID of the category tree is 0.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The category id of this category's parent. If this
        /// category is the root category, then the ParentId == Id == 0.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// This category's parent category. If this
        /// category is the root category, then the Parent == this.
        /// </summary>
        public Category Parent => _parent.Value;

        /// <summary>
        /// Enumeration of the category's children categories. Lazy loaded.
        /// </summary>
        public IEnumerable<Category> Childern => _childern.Value;

        /// <summary>
        /// Enumeration of all related categories. Lazy loaded.
        /// </summary>
        public IEnumerable<Category> Related => _related.Value;

        /// <summary>
        /// Enumeration of all series in the category. Lazy loaded.
        /// </summary>
        public IList<Series> Series => _series.Value;
    }
}