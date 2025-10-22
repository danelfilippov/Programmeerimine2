using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Prediction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public int HomeGoals { get; set; }
        public int GuestGoals { get; set; }
        public int Points { get; set; }
        public string Status { get; set; }
    }
}
