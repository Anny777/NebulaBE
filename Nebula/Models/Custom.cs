using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectOrderFood.Models
{
    public class Custom
    {
        public Custom()
        {
            CookingDishes = new Collection<CookingDish>();
        }

        public int Id { get; set; }
        public virtual ICollection<CookingDish> CookingDishes { get; set; }

        [Display(Name = "Состояние заказа")] public virtual bool IsOpened { get; set; }
        [Display(Name = "Номер стола")] public virtual int TableNumber { get; set; }

    }
}