using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProjectOrderFood.Controllers;

namespace ProjectOrderFood.Models
{
    public class Dish : DishBase
    {
        public int Id { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        [Display (Name = "Состав")]
        public virtual string Consist { get; set; }
        [Display(Name = "Единица измерения")]
        public virtual string Unit { get; set; }
        [Display(Name = "Наличие ингредиентов")]
        public virtual bool IsAvailable { get; set; }
        [Display(Name = "Цех")]
        public virtual WorkshopType WorkshopType {  get; set; }

    }
}