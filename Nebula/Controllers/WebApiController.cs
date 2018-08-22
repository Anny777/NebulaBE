using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ProjectOrderFood.Models;
using ProjectOrderFood.Services;
using ProjectOrderFood.ViewModels;

namespace ProjectOrderFood.Controllers
{
    public class WebApiController : ApiController
    {
        public void SetToken(string token)
        {
            Formula360Connection.SetToken(token);
        }

        /// <summary>
        /// Получение списка блюд
        /// </summary>
        /// <returns></returns>
        
        public IHttpActionResult GetListDishes()
        {

            var db = new ApplicationDbContext();
            var response = db.Dishes.Select(c => new DishViewModel()
            {
                Id = c.Id,
                Name = c.name,
                Consist = c.Consist,
                Price = c.sellingPrice,
                Unit = c.Unit
            }).OrderBy(b => b.Name).ToList();
            return Json(response);
        }

        /// <summary>
        /// Создание нового заказа; Дополнение существующего заказа; Запрос на удаление блюда;
        /// </summary>
        /// <param name="order">заказ</param>
        /// <returns></returns>
        public IHttpActionResult Order(OrderViewModel order)
        {
            if (order == null || order.Dishes == null)
            {
                return BadRequest("Не получены необходимые данные"); 
            }
            var db = new ApplicationDbContext();

            // Новый заказ
            if (order.Id <= 0)
            {
                var od = order.Dishes.Select(d => d.Id).Distinct().ToArray();
                var dishes = db.Dishes.Where(c => od.Contains(c.Id)).ToList();
                var o = new Custom()
                {
                    IsOpened = true,
                    TableNumber = order.Table,
                    CookingDishes = order.Dishes.Select(a => new CookingDish()
                    {
                        Comment = a.Comment,
                        Dish = dishes.Single(c => c.Id == a.Id),
                        DishState = DishState.InWork
                    }).ToArray()
                };
                db.Customs.Add(o);
            }
            else
            {
                var od = order.Dishes.Where(d => d.CookingDishId <= 0).Select(d => d.Id).Distinct().ToArray();
                // Дополнение существующего заказа
                var dishes = db.Dishes.Where(c => od.Contains(c.Id)).ToList();

                var custom = db.Customs.Find(order.Id);

                if (custom == null)
                {
                    return BadRequest("Не найден заказ");
                }
                // Запрос на удаление блюда из заказа (с помощью Except возвращает блюда, которым нужно сменить статус)
                //var crd = custom.CookingDishes.Select(c => c.Id)
                //    .Except(order.Dishes.Where(c => c.CookingDishId > 0).Select(c => c.CookingDishId));

                //db.CookingDishes.Where(c => crd.Contains(c.Id)).ToList()
                //    .ForEach(c => { c.DishState = DishState.CancellationRequested; });

                order.Dishes.Select(a => new CookingDish()
                {
                    Comment = a.Comment,
                    Dish = dishes.Single(c => c.Id == a.Id),
                    DishState = DishState.InWork
                }).ToList().ForEach(o => custom.CookingDishes.Add(o));
            }

            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Получение открытых заказов (официант, кухня и бар будут брать блюда отсюда)
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetOrders()
        {
            var db = new ApplicationDbContext();
            var orders = db.Customs.Where(c => c.IsOpened).Select(c => new OrderViewModel()
            {
                Id = c.Id,
                Table = c.TableNumber,
                Dishes = c.CookingDishes.Where(a => a.DishState != DishState.Deleted).Select(b => new DishViewModel()
                {
                    Id = b.Dish.Id,
                    CookingDishId = b.Id,
                    Name = b.Dish.name,
                    Unit = b.Dish.Unit,
                    Consist = b.Dish.Consist,
                    Comment = b.Comment,
                    State = b.DishState,
                    WorkshopType = b.Dish.WorkshopType
                })
            });

            return Json(orders.ToArray());
        }

        /// <summary>
        /// Смена состояния блюда на готовое
        /// </summary>
        /// <param name="id">идентификатор блюда</param>
        /// <param name="dishState">статус блюда</param>
        /// <returns></returns>
        public IHttpActionResult ChangeState(int id, DishState dishState)
        {
            var db = new ApplicationDbContext();
            var dish = db.CookingDishes.Find(id);
            if (dish == null)
            {
                return BadRequest("Блюдо не найдено!");
            }
            dish.DishState = dishState;
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Закрытие заказа 
        /// </summary>
        /// <param name="tableNumber">номер стола</param>
        /// <returns></returns>
        public IHttpActionResult CloseOrder(int tableNumber)
        {
            var db = new ApplicationDbContext();
            var customs = db.Customs.FirstOrDefault(c => c.TableNumber == tableNumber && c.IsOpened);
            if (customs != null)
            {
                customs.IsOpened = false;
                db.SaveChanges();
                return Ok();
            }
            return BadRequest("Заказ не найден!");
        }
    }
}