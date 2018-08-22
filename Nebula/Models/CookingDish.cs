namespace ProjectOrderFood.Models
{
    public class CookingDish 
    {
        public int Id { get; set; }
        public virtual Dish Dish { get; set; }
        public virtual DishState DishState { get; set; }
        public virtual Custom Custom { get; set; }
        public virtual string Comment { get; set; }
    }
}