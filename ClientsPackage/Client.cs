using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InvntoryManagementSoftware.ClientsPackage
{
    internal class Client
    {

        [Required]
        public string CName { get; set; }
        public int Id { get; set; }
        public string character { get; set; }
        [Required]
        public string CPhone { get; set; }
        [Required]
        public string CGender { get; set; }
        public string CGov { get; set; }
        public string CArea { get; set; }
        public string CEmail { get; set; }
        public string CNotes { get; set; }
        public string CBareed { get; set; }
        public string CState { get; set; }
        public string CMoney { get; set; }
        public DateTime CreatedFullDate { get; set; }
        public DateTime CreatedTime { get; set; }
        [Required]
        public Brush BgColor { get; set; }
    }
}
