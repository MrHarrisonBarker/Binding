using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Binding.Models
{
    public class Page
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        public int Order { get; set; }
        
        public User Owner { get; set; }
        
        // many blocks to one page
        public IList<Block> Blocks { get; set; }
        
        // many children to one parent page
        [AllowNull]
        public IList<Page> Childern { get; set; }
        
        // one parent to many page
        [AllowNull]
        public Page Parent { get; set; }
    }
}