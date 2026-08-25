using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}
