using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Team
    {
        [Required]
        [Unicode]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }

        public string Title { get; set; }
    }
}
