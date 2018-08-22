using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ProjectOrderFood.Models;

namespace ProjectOrderFood.Controllers
{
    public class WorkersController: Controller
    {
        private string SerializeDishes(ApplicationDbContext db)
        {
            var collection = db.Dishes.Select(Dish => new
            {
                Dish.Id,
                Dish.name,
                Dish.sellingPrice,
                Dish.Unit,
                Dish.Category,
                SubCategoryName = Dish.SubCategory.Name,// конкретное свойство нужно у объекта получать иначе он будет весь десериализоваться, а этот объект ссылается обратно
                Dish.Consist,
                Dish.IsAvailable,
                Dish.WorkshopType
            }).ToList();
            var serialize = new JavaScriptSerializer().Serialize(collection.ToArray());
            return serialize;
        }

        [Authorize]
        public ActionResult Kitchen()
        {
            var category = new JavaScriptSerializer().Serialize(new ApplicationDbContext().Categories.ToArray());
            return View(new[] { SerializeDishes(new ApplicationDbContext()), category });
        }

        [Authorize]
        public ActionResult Bar()
        {
            return View();
        }
    }
}