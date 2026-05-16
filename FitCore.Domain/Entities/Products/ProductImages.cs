using FitCore.Domain.Entities.Commons;

namespace FitCore.Domain.Entities.Products
{
    /// <summary>
    /// تصویر محصولات
    /// </summary>
    public class ProductImages : BaseEntity
    {
        public virtual Product Product { get; set; }
        public long ProductId { get; set; }
        /// <summary>
        /// آدرس تصویر
        /// </summary>
        public string Src { get; set; }
    }
}
