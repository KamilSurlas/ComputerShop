namespace ComputerShop.Models.ViewModels
{
    public class NavigationBarViewModel
    {
        public CategoryGroup CategoryGroup { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
