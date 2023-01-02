using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class ServiceListCountSpecification : BaseSpecifcation<Service>
    {
        public ServiceListCountSpecification(InventorySpecParams inventorySpecParams) : base(x =>
        !x.Deleted &&
           (string.IsNullOrEmpty(inventorySpecParams.Search) ||
        x.Name.ToLower().Contains(inventorySpecParams.Search) ||
        x.Id.ToString().ToLower().Contains(inventorySpecParams.Search) ||
        x.Category.Name.ToLower().Contains(inventorySpecParams.Search) ||
        x.CreateUser.FirstName.ToLower().Contains(inventorySpecParams.Search) ||
        x.CreateUser.LastName.ToLower().Contains(inventorySpecParams.Search)


        ) &&
        (!inventorySpecParams.CreateFromDate.HasValue || x.CreateDate >= inventorySpecParams.CreateFromDate) &&
        (!inventorySpecParams.CreateToDate.HasValue || x.CreateDate <= inventorySpecParams.CreateToDate) &&
        (!inventorySpecParams.CategoryId.HasValue || x.CategoryId == inventorySpecParams.CategoryId)

      )
        {
            

        }

  
    }
}
