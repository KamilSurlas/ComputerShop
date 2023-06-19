using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerShop.Models.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<SelectListItem>? StatusesList { get; set; }
    }
}
