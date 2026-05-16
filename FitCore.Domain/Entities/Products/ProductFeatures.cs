using FitCore.Domain.Entities.Commons;

namespace FitCore.Domain.Entities.Products
{
    /// <summary>
    /// ویژه گی های محصول
    /// </summary>
    public class ProductFeatures : BaseEntity
    {
        public virtual Product Product { get; set; }
        public long ProductId { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }
}
