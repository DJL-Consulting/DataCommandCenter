using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class ObjectSearch
    {
        public string? ObjectType { get; set; }
        public int Id { get; set; }
        public string? SearchText { get; set; }
        public string? DisplayText { get; set; }
    }
}
