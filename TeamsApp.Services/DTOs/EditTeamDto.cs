using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsApp.Services.DTOs
{
    public class EditTeamDto
    {
        [Required]
        public int Points { get; set; }

        [Required]
        public int Goals { get; set; }
    }
}
