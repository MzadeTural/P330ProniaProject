using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P330Pronia.Areas.Admin.ViewModels.FeatureViewModels;
using P330Pronia.Areas.Admin.ViewModels.ProductViewModels;
using P330Pronia.Exceptions;
using P330Pronia.Models;
using P330Pronia.Services.Interfaces;

namespace P330Pronia.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{

    private string _errorMessage;
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileService _fileService;

    public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment, IFileService fileService)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.AsNoTracking().ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        await getCategoriesAsync();


        return View();
    }
    public async Task getCategoriesAsync()
    {
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel productCreateVM)
    {
        await getCategoriesAsync();
        if (!ModelState.IsValid)
            return View();
        bool isEXist = _context.Products.Any(p => p.Name.ToLower().Trim() == productCreateVM.Name.ToLower().Trim());
        if (isEXist)
        {
            ModelState.AddModelError("Name", "Product is already exist");
            return View();

        }
        string fileName = string.Empty;
        try
        {
            fileName = await _fileService.CreateFileAsync(file: productCreateVM.Image, path: Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images"), maxFileSize: 100, fileType: "image/");
        }
        catch (FileSizeException ex)
        {
            ModelState.AddModelError("Image", ex.Message);
            return View();
        }
        catch (FileTypeException ex)
        {
            ModelState.AddModelError("Image", ex.Message);
            return View();
        }

        Product product = new Product()
        {
            Name = productCreateVM.Name,
            Price = productCreateVM.Price,
            Description = productCreateVM.Description,
            Rating = productCreateVM.Rating,
            CategoryId = productCreateVM.CategoryId,
            Image = fileName,
            CreatedBy = "Admin",
            CreatedDate = DateTime.UtcNow,
            UpdatedBy = "Admin",
            UpdatedDate = DateTime.UtcNow,
        };
        await _context.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));



    }
}