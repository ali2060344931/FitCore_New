using Microsoft.AspNetCore.Http;

using System.Collections.Generic;

namespace FitCore.Application.Services.Products.Commands.AddNewProduct
{
    /// <summary>
    /// درخواست ویژه گی های محصول از طرف کاربر
    /// </summary>
    public class RequestAddNewProductDto
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public long CategoryId { get; set; }
        public bool Displayed { get; set; }
        /// <summary>
        /// لیستی ار عکس های محصول
        /// </summary>
        public List<IFormFile> Images { get; set; }
         /// <summary>
         /// لیستی از ویژه گی های محصول
         /// </summary>
        public List<AddNewProduct_Features> Features { get; set; }
    }
}
