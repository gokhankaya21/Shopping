﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DotNetShopping.Models
{
    public class Cart
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public Int64 VariantId { get; set; }
        public int Quantity { get; set; }
    }
    public class CartListModel
    {
        public string UserId { get; set; }
        public Int64 VariantId { get; set; }
        public Int64 ProductId { get; set; }
        public int Quantity { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal TotalPrice { get; set; }
        public string VariantName { get; set; }
        public string ProductName { get; set; }
        public string PhotoName { get; set; }
        public int Stock { get; set; }

        public IEnumerable<CartListModel> GetCart(string UserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var model = db.Carts
                .Join(db.Variants, c => c.VariantId, v => v.VariantId, (c, v) => new { Cart = c, Variant = v })
                .Join(db.Products, cv => cv.Variant.ProductId, p => p.ProductId, (cv, p) => new { cv, Product = p })
                .Where(x => x.cv.Cart.UserId == UserId)
                .Select(x => new CartListModel
                {
                    UserId = x.cv.Cart.UserId,
                    VariantId = x.cv.Cart.VariantId,
                    ProductId = x.cv.Variant.ProductId,
                    Quantity = x.cv.Cart.Quantity,
                    VariantName = x.cv.Variant.Name,
                    ProductName = x.cv.Variant.Product.Name,
                    UnitPrice = x.cv.Variant.UnitPrice,
                    TotalPrice = x.cv.Variant.UnitPrice * x.cv.Cart.Quantity,
                    Stock=x.cv.Variant.Stock,
                    PhotoName = db.ProductImages.Where(pi => pi.VariantId == x.cv.Variant.VariantId)
                    .OrderBy(pi => pi.Sequence).FirstOrDefault().FileName
                }).ToList();
            return model;
        }
    }
}