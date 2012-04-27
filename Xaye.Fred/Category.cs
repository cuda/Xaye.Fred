﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Xaye.Fred
{
    /// <summary>
    /// Represents a category of data.
    /// </summary>
    public class Category : Item
    {
        private readonly object _lok = new object();
        private IEnumerable<Category> _childern;
        private Category _parent;
        private IEnumerable<Category> _related;
        private List<Series> _series;

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
                lock (_lok)
                {
                    if (_parent == null)
                    {
                        _parent = Id == 0 ? this : Fred.GetCategory(ParentId);
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
                lock (_lok)
                {
                    if (_childern == null)
                    {
                        _childern = Fred.GetCategoryChildern(Id);
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
                lock (_lok)
                {
                    if (_related == null)
                    {
                        _related = Fred.GetCategoryRelated(Id);
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
                lock (_lok)
                {
                    if (_series == null)
                    {
                        const int limit = 1000;
                        _series = (List<Series>)Fred.GetCategorySeries(Id, DateTime.Now, DateTime.Now, limit, 0);
                        var count = _series.Count();
                        var call = 1;
                        while (count == limit)
                        {
                            var more = (List<Series>)Fred.GetCategorySeries(Id, DateTime.Now, DateTime.Now, limit, call * limit);
                            _series.AddRange(more);
                            count = more.Count();
                            call++;
                        }
                    }
                }

                return _series;
            }
        }
    }
}