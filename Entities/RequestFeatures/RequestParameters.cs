﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public abstract class RequestParameters
    {
        public const int maxPageSize = 50;

        // Auto-Implemented Property
        public int PageNumber { get; set; }

        // Full-Property
        private int _pageSize;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }

        public String? OrderBy { get; set; }
        public String? Fields { get; set; }
    }
}