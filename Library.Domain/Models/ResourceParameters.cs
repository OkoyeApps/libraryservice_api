using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class ResourceParameters
    {
        const int maxPageSize = 20;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;

            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }

    public enum ResourceUriType
    {
        PreviousPage, NextPage, Current
    }

    public class ControllerLink
    {
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string ActionMethod { get; set; }
        public object ActionParams { get; set; }
    }
}
