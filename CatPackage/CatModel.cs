using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InvntoryManagementSoftware.CatPackage
{
    class CatModel
    {//CatName,CatBarCode,MainCatName,SubCatName,SalePrice,BuyPrice,Quantity,UnitName,Description
        [Required]
        public int Id { get; set; }
        [Required]
        public string CatName { get; set; }
        [Required]
        public string character { get; set; }
        [Required]
        public string CatBarCode { get; set; }
        [Required]
        public string MainCatName { get; set; }
        [Required]
        public string SubCatName { get; set; }
        [Required]
        public string SalePrice { get; set; }
        [Required]
        public string BuyPrice { get; set; }
        [Required]
        public string Quantity { get; set; }
        [Required]
        public string UnitName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Brush BgColor { get; set; }
    }
}
