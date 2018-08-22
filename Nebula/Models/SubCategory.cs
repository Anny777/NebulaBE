using System.ComponentModel.DataAnnotations;

namespace ProjectOrderFood.Models
{
    public class SubCategory
    {
        public virtual int Id { get; set; }

        [Display(Name = "Подкатегория")]
        public virtual string Name { get; set; }
    }
}