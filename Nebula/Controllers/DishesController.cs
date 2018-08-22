//using System.Linq;
//using System.Net;
using System.Web.Mvc;
using ProjectOrderFood.Models;

namespace ProjectOrderFood.Views.Dishes
{
    public class DishesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        // GET: Dishes
        public ActionResult Index()
        {
            return View(db.Dishes.OrderBy(c => c.name).ToList());
        }

        // GET: Dishes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dishes.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // GET: Dishes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Consist,Unit,IsAvailable,WorkshopType,_id,extId,shopID,barcode,name,nameFull,VAT,sellingPrice,useGroupMarginRule,isAlcohol,alcoholCode,ownMarginRule,modified,__v,isService,uuid,isDelete,code,isBeer")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Dishes.Add(dish);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dish);
        }

        // GET: Dishes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dishes.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "Consist,Unit,IsAvailable,WorkshopType")]
        //Dish dish
        public ActionResult Edit(Dish dish)
        {
            if (ModelState.IsValid)
            {
                var localDish = db.Dishes.Find(dish.Id);
                localDish.name = dish.name;
                localDish.Consist = dish.Consist;
                localDish.Unit = dish.Unit;
                localDish.IsAvailable = dish.IsAvailable;
                localDish.WorkshopType = dish.WorkshopType;
                localDish.Category = db.Categories.Find(dish.Category.Id);
                localDish.SubCategory = db.SubCategories.Find(dish.SubCategory.Id);
                localDish.sellingPrice = dish.sellingPrice;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dishes.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dish dish = db.Dishes.Find(id);
            db.Dishes.Remove(dish);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
