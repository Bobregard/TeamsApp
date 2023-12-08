using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsApp.Services.DTOs
{
    public class AddMatchDto
    {
        [Required]
        public string HomeTeamName { get; set; }

        [Required]
        public string AwayTeamName { get; set; }

        [Required]
        public int HomeTeamScore { get; set; }

        [Required]
        public int AwayTeamScore { get; set; }
    }
}
