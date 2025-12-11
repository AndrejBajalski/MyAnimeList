using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DomainModels;
using Repository.Data;

namespace Repository.Implementation
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> entites;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            this.entites = _context.Set<User>();
        }

        public User GetUserById(string id)
        {
            return entites.First(ent => ent.Id == id);
        }
    }
}
