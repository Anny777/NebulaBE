using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ProjectOrderFood.Models;

namespace ProjectOrderFood.Controllers
{
    public class OrderDishController : Controller
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
                SubCategoryName = Dish.SubCategory.Name,
                Dish.Consist,
                Dish.IsAvailable,
                Dish.WorkshopType
            }).ToList();
            var serialize = new JavaScriptSerializer().Serialize(collection.ToArray());
            return serialize;
        }
        [Authorize]
        public ActionResult Index(string tableNumber)
        {
            var category = new JavaScriptSerializer().Serialize(new ApplicationDbContext().Categories.ToArray());
            return View(new[] { SerializeDishes(new ApplicationDbContext()), category, tableNumber.ToString() });
        }


        public class ObjectDish
        {
            public int Id { get; set; }
            public string Comment { get; set; }
        }

        //метод сборки заказа для отправки  на кухню
        public void CustomForCooking(string data, int tableNumber)
        {
            var db = new ApplicationDbContext();
            var eCustom = db.Customs.FirstOrDefault(c => c.TableNumber == tableNumber && c.IsOpened);
            var isNew = eCustom == null;
            var orderDish = eCustom ?? new Custom();
            ObjectDish[] deserialize = new JavaScriptSerializer().Deserialize<ObjectDish[]>(data);

            foreach (var dishId in deserialize)
            {
                var cook = new CookingDish();
                var item = db.Dishes.Find(dishId.Id);
                cook.Dish = item;
                cook.DishState = DishState.InWork;
                cook.Comment = dishId.Comment;
                orderDish.CookingDishes.Add(cook);
                if (isNew)
                {
                    orderDish.IsOpened = true;
                    orderDish.TableNumber = tableNumber;
                }
            }

            if (isNew)
            {
                db.Customs.Add(orderDish);
            }

            db.SaveChanges();
        }

       

        

        public string GetReadyCustom(DishState state)
        {
            var db = new ApplicationDbContext();

            var dish = db.CookingDishes.Where(c => c.DishState == state).Select(c => new { c.Dish.name, c.Dish.Unit, c.Dish.WorkshopType, c.Id, c.DishState, c.Custom.TableNumber });
            var serialize = new JavaScriptSerializer().Serialize(dish);
            return serialize;
        }

        //метод меняющий состояние блюда на новое (для отображения в чеке якобы удалённого блюда)
        public void ChangeStateInCheck(int idDish, int customId)
        {
            var db = new ApplicationDbContext();
            db.CookingDishes.First(c => c.Dish.Id == idDish && c.Custom.Id == customId && c.DishState != DishState.Deleted).DishState = DishState.Deleted;
            db.SaveChanges();
        }

        public string OpenOrders()
        {
            var db = new ApplicationDbContext();
            var dishes = db.CookingDishes.Where(c => c.Custom.IsOpened && c.DishState != DishState.Deleted).Select(c => new {dishId = c.Dish.Id, c.Dish.name, c.Dish.Unit, c.Custom.Id, c.DishState, c.Dish.sellingPrice, c.Custom.TableNumber });
            var serialize = new JavaScriptSerializer().Serialize(dishes);
            return serialize;
        }

        
    }
}

