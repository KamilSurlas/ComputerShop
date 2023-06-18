namespace ComputerShop.Models.ViewModels
{
    public class OverviewPageViewModel
    {    
        public List<Product> PromotedProducts { set; get; }
        public List<Product> Products { get; set; }
        public List<string> Producents { get; set; }
        public OverviewPageViewModel()
        {
            Products = new List<Product>();
            PromotedProducts = new List<Product>();
            Producents = new List<string>();
        }
    }
}
