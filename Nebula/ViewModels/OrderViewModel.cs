using ProjectOrderFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectOrderFood.ViewModels
{
    public class OrderViewModel
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public int Id;
        /// <summary>
        /// Коллекция блюд
        /// </summary>
        public IEnumerable<DishViewModel> Dishes;
        /// <summary>
        /// Номер стола
        /// </summary>
        public int Table;
    }

    public class Dish {
        /// <summary>
        /// Идентификатор заказанного блюда
        /// </summary>
        public CookingDish cookingDish;
        /// <summary>
        /// Комментарий к блюду
        /// </summary>
        public string Comment;
    }
}