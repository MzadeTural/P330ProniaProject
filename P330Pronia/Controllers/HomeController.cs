using Microsoft.EntityFrameworkCore;
using P330Pronia.ViewModels;

namespace P330Pronia.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var sliders = _context.Sliders;
        var features = _context.Features;
        var categories= _context.Categories;
        var product = _context.Products;

        HomeViewModel homeViewModel = new HomeViewModel
        {
            Sliders = sliders,
            Features = features,
            Categories = categories,
            Products = product.Include(c => c.Category),

        };

        return View(homeViewModel);
    }
}