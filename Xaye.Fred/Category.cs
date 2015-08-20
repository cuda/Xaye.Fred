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
        private readonly Lazy<List<Series>> _series;

        internal Category(Fred fred) : base(fred)
        {
            _childern = new Lazy<IEnumerable<Category>>(() => Fred.GetCategoryChildern(Id));
            _parent = new Lazy<Category>(() => Id == 0 ? this : Fred.GetCategory(ParentId));
            _related = new Lazy<IEnumerable<Category>>(() => Fred.GetCategoryRelated(Id));
            _series = new Lazy<List<Series>>(
                () =>
                {
                    var series = (List<Series>) Fred.GetCategorySeries(Id, DateTime.Today, DateTime.Today);
                    var count = series?.Count;
                    var call = 1;
                    while (count == CallLimit)
                    {
                        var more = (List<Series>) Fred.GetCategorySeries(Id, DateTime.Today, DateTime.Today, CallLimit, call* CallLimit);
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
        public Category GetParent() => _parent.Value;

        /// <summary>
        /// Enumeration of the category's children categories.
        /// </summary>
        public IEnumerable<Category> GetChildern() => _childern.Value;

        /// <summary>
        /// Enumeration of all related categories.
        /// </summary>
        public IEnumerable<Category> GetRelated() => _related.Value;

        /// <summary>
        /// Enumeration of all series in the category.
        /// </summary>
        public IEnumerable<Series> GetSeries() => _series.Value;
    }
}