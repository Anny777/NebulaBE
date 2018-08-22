using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProjectOrderFood.Models;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ProjectOrderFood.Services;

namespace ProjectOrderFood.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult OpenOrder()
        {
            return View();
        }

        public string Sync()
        {
            try
            {
                var fs = new Formula360Context();
                fs.SynchronizeDishes();
                return true.ToString();
            }
            catch (Exception e)
            {
               return e.Message;
            }
        }

        public ActionResult Angular()
        {
            var category = new JavaScriptSerializer().Serialize(new ApplicationDbContext().Categories.ToArray());
            return PartialView("PageOrderPartial", new[] { SerializeDishes(new ApplicationDbContext()), category });
        }

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

        private string replace(string str)
        {
            return str.Replace(" ", string.Empty).ToLowerInvariant().Replace("\"", "").Replace("«", "").Replace("»", "").Replace(".", "").Replace(",", "").Replace("-", "")
                .Replace("ё", "е");
        }

        public ActionResult BigHall()
        {
            return View();
        }
    }

    public static class DishHelper
    {
        public static Dish MapTo(this DishBaseObjects dishBase, Dish dish)
        {
            dish._id = dishBase._id;
            dish.extId = dishBase.extId;
            dish.shopID = dishBase.shopID;
            dish.barcode = dishBase.barcode;
            dish.name = dishBase.name;
            dish.nameFull = dishBase.nameFull;
            dish.VAT = dishBase.VAT;
            dish.sellingPrice = dishBase.sellingPrice;
            dish.useGroupMarginRule = dishBase.useGroupMarginRule;
            dish.isAlcohol = dishBase.isAlcohol;
            dish.alcoholCode = dishBase.alcoholCode;
            dish.ownMarginRule = dishBase.ownMarginRule;
            dish.modified = dishBase.modified;
            dish.__v = dishBase.__v;
            dish.isService = dishBase.isService;
            dish.uuid = dishBase.uuid;
            dish.isDelete = dishBase.isDelete;
            dish.code = dishBase.code;
            dish.isBeer = dishBase.isBeer;
            //dish.Unit = (dishBase.unit?.capacity + " " + dishBase.unit?.name).Trim();
            return dish;
        }
    }

    public class DishBase
    {
        public string _id { get; set; }
        public string extId { get; set; }
        public string shopID { get; set; }
        public string barcode { get; set; }
        [Display(Name = "Название")]
        public string name { get; set; }
        public string nameFull { get; set; }
        public string VAT { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Цена")]
        public decimal? sellingPrice { get; set; }
        public bool useGroupMarginRule { get; set; }
        public bool isAlcohol { get; set; }
        public string alcoholCode { get; set; }
        public string ownMarginRule { get; set; }
        public DateTime modified { get; set; } //01.01.0001
        public int __v { get; set; }
        public bool isService { get; set; }
        public string uuid { get; set; }
        public bool isDelete { get; set; }
        public string code { get; set; }
        public bool isBeer { get; set; }
    }


    public class DishBaseObjects : DishBase
    {
        public Unit unit { get; set; }
        public Quantity quantity { get; set; }
        public object group { get; set; }
        public Egaisinfo egaisInfo { get; set; }

    }

    public class Unit
    {
        public string name { get; set; }
        public float capacity { get; set; }
    }

    public class Quantity
    {
        public string numberDecimal { get; set; }
    }

    public class Egaisinfo
    {
        public string barcode { get; set; }
        public bool isBeer { get; set; }
        public bool isAlcohol { get; set; }
        public bool isNew { get; set; }
        public string ProductVCode { get; set; }
        public Producer Producer { get; set; }
        public bool AlcVolumeSpecified { get; set; }
        public float AlcVolume { get; set; }
        public bool CapacitySpecified { get; set; }
        public float Capacity { get; set; }
        public string AlcCode { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public bool TypeSpecified { get; set; }
        public string UnitType { get; set; }
        public string kegCapacity { get; set; }
        public int Type { get; set; }
    }

    public class Producer
    {
        public Address address { get; set; }
        public string KPP { get; set; }
        public string INN { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string ClientRegId { get; set; }
    }

    public class Address
    {
        public string description { get; set; }
        public string RegionCode { get; set; }
        public string Country { get; set; }
    }



}