using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xaye.Fred
{
    /// <summary>
    /// Represents a category of data.
    /// </summary>
    public class Category : Item
    {
        private readonly Lazy<Task<IEnumerable<Category>>> _childern;
        private readonly Lazy<Task<Category>> _parent;
        private readonly Lazy<Task<IEnumerable<Category>>> _related;
        private readonly Lazy<Task<List<Series>>> _series;

        internal Category(Fred fred) : base(fred)
        {
            _childern = new Lazy<Task<IEnumerable<Category>>>(async () => await Fred.GetCategoryChildernAsync(Id));
            _parent = new Lazy<Task<Category>>(async () => Id == 0 ? this : await Fred.GetCategoryAsync(ParentId));
            _related = new Lazy<Task<IEnumerable<Category>>>(async () => await Fred.GetCategoryRelatedAsync(Id));
            _series = new Lazy<Task<List<Series>>>(
                async () =>
                {
                    var series = (List<Series>) await Fred.GetCategorySeriesAsync(Id, DateTime.Today, DateTime.Today);
                    var count = series?.Count;
                    var call = 1;
                    while (count == CallLimit)
                    {
                        var more = (List<Series>) await Fred.GetCategorySeriesAsync(Id, DateTime.Today, DateTime.Today, CallLimit, call * CallLimit);
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
        public Category GetParent() => _parent.Value.Result;

        /// <summary>
        /// This category's parent category. If this
        /// category is the root category, then the Parent == this.
        /// </summary>
        public async Task<Category> GetParentAsync() => await _parent.Value;

        /// <summary>
        /// Enumeration of the category's children categories.
        /// </summary>
        public IEnumerable<Category> GetChildern() => _childern.Value.Result;

        /// <summary>
        /// Enumeration of the category's children categories.
        /// </summary>
        public async Task<IEnumerable<Category>> GetChildernAsync() => await _childern.Value;

        /// <summary>
        /// Enumeration of all related categories.
        /// </summary>
        public IEnumerable<Category> GetRelated() => _related.Value.Result;

        /// <summary>
        /// Enumeration of all related categories.
        /// </summary>
        public async Task<IEnumerable<Category>> GetRelatedAsync() => await _related.Value;

        /// <summary>
        /// Enumeration of all series in the category.
        /// </summary>
        public IEnumerable<Series> GetSeries() => _series.Value.Result;

        /// <summary>
        /// Enumeration of all series in the category.
        /// </summary>
        public async Task<IEnumerable<Series>> GetSeriesAsync() => await _series.Value;
    }
}