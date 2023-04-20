namespace ComputerShop.Models.ViewModels
{
    public class HomeViewModel
    {
        public CategoryGroup CategoryGroup { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
