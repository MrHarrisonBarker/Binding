using System;

namespace Binding.Models
{
    public class Block
    {
        public Guid Id { get; set; }
        public BlockType Type { get; set; }
        public string Content { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        public int Order { get; set; }
        
        // one to many pages
        public Page Page { get; set; }
    }

    public class BlockViewModel
    {
        
    }
}