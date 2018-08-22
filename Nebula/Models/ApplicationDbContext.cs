using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProjectOrderFood.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext()
            : base("ProjectOrderFood", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Category> Categories  { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<CookingDish> CookingDishes { get; set; }
        public virtual DbSet<Custom> Customs { get; set; }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}