using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class ProductListSpecification : BaseSpecifcation<Product>
    {
        public ProductListSpecification(int productId) : base(x => x.Id == productId)
        {
        }
        public ProductListSpecification(int categoryId ,string code,int id) : base(x =>( x.CategoryId == categoryId && x.Code==code) && x.Id != id)
        {
        }
    }
}
