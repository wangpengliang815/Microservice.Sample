﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Api.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        [Column(TypeName = "VARCHAR(16)")]
        public string Name { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [Required]
        public int Stock { get; set; }
    }
}
