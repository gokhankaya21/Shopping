using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{
    public class ProductImage
    {
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int64 ImageId { get; set; }
        public Int64 ProductId { get; set; }
        public Int64 VariantId { get; set; }
        public string FileName { get; set; }
    }
}