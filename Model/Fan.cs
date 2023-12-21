using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Football_App.Model
{
    public class Fan
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FanId { get; set; }
        [Required]
        public string FanName { get; set; }

        // Ссылка многие к многим (к клубам)
        public IList<FanClub> FanClubs { get; set; }
    }
}
