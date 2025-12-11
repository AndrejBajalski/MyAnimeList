using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Models.DomainModels
{
    public class User : IdentityUser
    {
        public string? Name { get; set;  }
        public string? Surname { get; set; }
        public List<UnwatchedAnime>? UnwatchedAnimes { get; set; }
    }
}
