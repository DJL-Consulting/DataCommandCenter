using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class SsisPackageHeader
    {
        public int Id { get; set; }
        public string? PackagePath { get; set; }
        public string? PackageName { get; set; }
        public string? Dtsid { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
