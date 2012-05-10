using System;
using System.Collections.Generic;
using System.Linq;

namespace Xaye.Fred
{
    /// <summary>
    /// Represents a category of data.
    /// </summary>
    public class Category : Item
    {
        private volatile IEnumerable<Category> _childern;
        private volatile Category _parent;
        private volatile IEnumerable<Category> _related;
        private volatile List<Series> _series;

        internal Category(Fred fred) : base(fred)
        {
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
        public Category Parent
        {
            get
            {
                if (_parent == null)
                {
                    lock (Lock)
                    {
                        if (_parent == null)
                        {
                            _parent = Id == 0 ? this : Fred.GetCategory(ParentId);
                        }
                    }
                }
                return _parent;
            }
        }

        /// <summary>
        /// Enumeration of the category's children categories. Lazy loaded.
        /// </summary>
        public IEnumerable<Category> Childern
        {
            get
            {
                if (_childern == null)
                {
                    lock (Lock)
                    {
                        if (_childern == null)
                        {
                            _childern = Fred.GetCategoryChildern(Id);
                        }
                    }
                }

                return _childern;
            }
        }

        /// <summary>
        /// Enumeration of all related categories. Lazy loaded.
        /// </summary>
        public IEnumerable<Category> Related
        {
            get
            {
                if (_related == null)
                {
                    lock (Lock)
                    {
                        if (_related == null)
                        {
                            _related = Fred.GetCategoryRelated(Id);
                        }
                    }
                }

                return _related;
            }
        }

        /// <summary>
        /// Enumeration of all series in the category. Lazy loaded.
        /// </summary>
        public IEnumerable<Series> Series
        {
            get
            {
                if (_series == null)
                {
                    lock (Lock)
                    {
                        if (_series == null)
                        {
                            const int limit = 1000;
                            _series = (List<Series>) Fred.GetCategorySeries(Id, DateTime.Today, DateTime.Today, limit, 0);
                            var count = _series.Count;
                            var call = 1;
                            while (count == limit)
                            {
                                var more = (List<Series>) Fred.GetCategorySeries(Id, DateTime.Today, DateTime.Today, limit, call*limit);
                                _series.AddRange(more);
                                count = more.Count;
                                call++;
                            }
                        }
                    }
                }

                return _series;
            }
        }
    }
}