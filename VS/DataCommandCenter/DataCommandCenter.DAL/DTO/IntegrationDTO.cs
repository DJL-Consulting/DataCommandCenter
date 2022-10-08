using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommandCenter.DAL.DTO
{
    public class IntegrationDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? IntegrationType { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }
        public string? Created { get; set; }
        public string? LastModified { get; set; }
    }
}
