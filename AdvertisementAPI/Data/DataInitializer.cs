using Microsoft.EntityFrameworkCore;

namespace AdvertisementAPI.Data
{
    public class DataInitializer
    {
        private readonly ApplicationDbContext _adContext;
        public DataInitializer(ApplicationDbContext adContext)
        {
            _adContext = adContext;
        }
        public void MigrateData()
        {
            _adContext.Database.Migrate();
            SeedData();
            _adContext.SaveChanges();
        }
        private void SeedData()
        {
            if(!_adContext.Advertisements.Any(a => a.CompanyName == "Nike"))
            {
                _adContext.Add(new Advertisement
                {
                    CompanyName = "Nike",
                    Slogan = "Just do it!"
                });
            }
            if (!_adContext.Advertisements.Any(a => a.CompanyName == "Volvo"))
            {
                _adContext.Add(new Advertisement
                {
                    CompanyName = "Volvo",
                    Slogan = "For life."
                });
            }
        }
    }
}
