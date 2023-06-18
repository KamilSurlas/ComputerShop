namespace ComputerShop.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> CartList { get; set; }
        public Order Order { get; set; }
    }
}
