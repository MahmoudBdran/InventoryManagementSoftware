using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InvntoryManagementSoftware.MowaredeenPackage
{
    internal class Mowaredeen
    {

        [Required]
        public string MName { get; set; }
        public int Id { get; set; }
        public string character { get; set; }
        [Required]
        public string MPhone { get; set; }
        [Required]
        public string MCompanyName { get; set; }
        public string MGov { get; set; }
        public string MArea { get; set; }
        public string MEmail { get; set; }
        public string MNotes { get; set; }
        public string MState { get; set; }
        public string MMoney { get; set; }
        public DateTime CreatedFullDate { get; set; }
        public DateTime CreatedTime { get; set; }
        [Required]
        public Brush BgColor { get; set; }
    }
}
