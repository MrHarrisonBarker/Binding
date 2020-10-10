using System;
using System.Collections.Generic;

namespace Binding.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        // many to one pages
        public IList<Page> Pages { get; set; }
    }

    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public IList<PageWithNoBlocksViewModel> Pages { get; set; }
    }
}