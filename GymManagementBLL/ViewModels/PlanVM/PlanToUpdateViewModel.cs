using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanVM
{
    public class PlanToUpdateViewModel
    {
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is Required")]
        [StringLength(70, MinimumLength =5 , ErrorMessage = "Description must be between 5 and 70")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Price is Required")]
        [Range(250, 10000, ErrorMessage = "Price must be between 250 and 10000 EGP")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "DurationDays is Required")]
        [Range(1,365,ErrorMessage = "DurationDays must be between 1 and 365")]
        public int DurationDays { get; set; }
    }
}
