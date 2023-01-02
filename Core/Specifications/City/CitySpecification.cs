using Core.Entities;

namespace Core.Specifications
{
    public class CitySpecification : BaseSpecifcation<City>
    {
        public CitySpecification(int countryId) : base(x => x.CountryId == countryId)
        {
          
        }
    }
}
