using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class Integration
    {
        public Integration()
        {
            IntegrationFlows = new HashSet<IntegrationFlow>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? IntegrationType { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }

        public virtual ICollection<IntegrationFlow> IntegrationFlows { get; set; }
    }
}
