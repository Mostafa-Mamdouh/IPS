using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class ServiceListSpecification : BaseSpecifcation<Service>
    {
        public ServiceListSpecification(InventorySpecParams inventorySpecParams) : base(x =>
        !x.Deleted &&
           (string.IsNullOrEmpty(inventorySpecParams.Search) ||
        x.Name.ToLower().Contains(inventorySpecParams.Search) ||
        x.Id.ToString().ToLower().Contains(inventorySpecParams.Search) ||
        x.Category.Name.ToLower().Contains(inventorySpecParams.Search) ||
        x.CreateUser.FirstName.ToLower().Contains(inventorySpecParams.Search) ||
        x.CreateUser.LastName.ToLower().Contains(inventorySpecParams.Search) 


        )&&
        (!inventorySpecParams.CreateFromDate.HasValue || x.CreateDate >= inventorySpecParams.CreateFromDate) &&
        (!inventorySpecParams.CreateToDate.HasValue || x.CreateDate <= inventorySpecParams.CreateToDate) &&
        (!inventorySpecParams.CategoryId.HasValue || x.CategoryId==inventorySpecParams.CategoryId)

      )
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.CreateUser);
            AddOrderByDescending(x => x.CreateDate);
            ApplyPaging(inventorySpecParams.PageSize * (inventorySpecParams.PageIndex - 1), inventorySpecParams.PageSize);

            if (!string.IsNullOrEmpty(inventorySpecParams.Sort))
            {
                switch (inventorySpecParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(p => p.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(p => p.Name);
                        break;
                    case "idAsc":
                        AddOrderBy(p => p.Id);
                        break;
                    case "idDesc":
                        AddOrderByDescending(p => p.Id);
                        break;
                    default:
                        AddOrderByDescending(n => n.CreateDate);
                        break;
                }
            }

        }

        public ServiceListSpecification(int serviceId) : base(x => x.Id == serviceId)
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.CreateUser);

        }

        public ServiceListSpecification(int categoryId,int id,string name) : base(x => (x.CategoryId == categoryId && x.Name==name) && x.Id!=id)
        {
        }
    }
}
