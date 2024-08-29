using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    public record CategoryNodeModel
    {
        public string Name { get; set; }
        public CategoryNodeModel Parent { get; set; }
        public List<CategoryNodeModel> Children { get; set; } = [];
    }
}
