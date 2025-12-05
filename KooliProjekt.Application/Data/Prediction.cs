using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Prediction : Entity
    {
        [Required]
        [Unicode]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int MatchId { get; set; }
        [Required]
        public int HomeGoals { get; set; }
        [Required]
        public int GuestGoals { get; set; }
        public int Points { get; set; }
        [MaxLength(20)]
        public string Status { get; set; }
    }
}
