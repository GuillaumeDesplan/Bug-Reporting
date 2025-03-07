using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tickets_bug_BSD2023_2025.Models;

namespace tickets_bug_BSD2023_2025.Data
{
    public class tickets_bug_BSD2023_2025Context : DbContext
    {
        public tickets_bug_BSD2023_2025Context (DbContextOptions<tickets_bug_BSD2023_2025Context> options)
            : base(options)
        {
        }

        public DbSet<tickets_bug_BSD2023_2025.Models.Ticket> Ticket { get; set; } = default!;
        public DbSet<tickets_bug_BSD2023_2025.Models.Home> Home { get; set; } = default!;
    }
}
