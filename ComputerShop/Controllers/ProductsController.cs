using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComputerShop.Data;
using ComputerShop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ComputerShop.Data.SD;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace ComputerShop.Controllers
{
    [Authorize(Roles = SD.Role_User_Admin)]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category).Include(p => p.Producer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Amount,ProducerId,CategoryId,CoverImage,Images")] Product product)
        {
            if (ModelState.IsValid)
            {
                
                if (product.CoverImage != null)
                {
                    product.CoverImageUrl = AddImage("products/cover/", product.CoverImage);
                }
                if (product.Images != null)
                {
                    product.productImages = new List<ProductImage>();
                    foreach (var item in product.Images)
                    {
                        product.productImages.Add(new ProductImage() { URL = AddImage("products/gallery/", item), ProductId = product.Id });
                    }
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name", product.ProducerId);
            return View(product);
        }

        private string AddImage(string folderPath, IFormFile image)
        {
            folderPath += Guid.NewGuid().ToString() + image.FileName;
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            using (FileStream imageToCopy = new(imagePath, FileMode.Create))
            {
                image.CopyTo(imageToCopy);
            }
            return "/" + folderPath;
        }
        private IFormFile? DowloadImage(string path) // Ta funkcja jest nie używana ale zostawiam ją jakby się kiedyś miała przydać 
        {
            Debug.WriteLine(path);
            path = _webHostEnvironment.WebRootPath + path;
            using (FileStream imageToDowload = new(path, FileMode.Open))
            {
                FormFile file = new FormFile(imageToDowload, 0, imageToDowload.Length,"", imageToDowload.Name);
                if (System.IO.File.Exists(file.FileName))
                {
                    return file;
                }
            }
            return null;
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound();

            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name", product.ProducerId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Amount,ProducerId,CategoryId,CoverImage,Images")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(product.CoverImage == null)
                    {
                        product.CoverImageUrl = _context.Products.AsNoTracking().Where(x=>x.Id == id).Select(x=>x.CoverImageUrl).FirstOrDefault();
                    }
                    else
                    {
                        var oldProductCover = _context.Products.AsNoTracking().Where(x => x.Id == id).Select(x => x.CoverImageUrl).First();
                        if (System.IO.File.Exists(_webHostEnvironment.WebRootPath + oldProductCover))
                        {
                            System.IO.File.Delete(_webHostEnvironment.WebRootPath + oldProductCover);
                        }
                        product.CoverImageUrl = AddImage("products/cover/", product.CoverImage);
                    }
                   if(product.Images != null)
                    {
                        var images = _context.ProductImages.Where(x => x.ProductId == id);
                        if (images.Any())
                        {
                            foreach (var item in images)
                            {
                                if (System.IO.File.Exists(_webHostEnvironment.WebRootPath + item.URL))
                                {
                                    System.IO.File.Delete(_webHostEnvironment.WebRootPath + item.URL);
                                }
                                _context.ProductImages.Remove(item);
                            }
                        }
                        product.productImages = new List<ProductImage>();
                        foreach (var item in product.Images)
                        {
                            product.productImages.Add(new ProductImage() { URL = AddImage("products/gallery/", item), ProductId = product.Id });
                        }
                    }
                        
                      
                 
                    _context.Update(product);
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name", product.ProducerId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                if (System.IO.File.Exists(_webHostEnvironment.WebRootPath + product.CoverImageUrl))
                {
          
                    System.IO.File.Delete(_webHostEnvironment.WebRootPath + product.CoverImageUrl);
                }                            
                _context.Products.Remove(product);
                var images = _context.ProductImages.Where(x => x.ProductId == id);
                if (images.Any())
                {
                    foreach (var item in images)
                    {
                        if (System.IO.File.Exists(_webHostEnvironment + item.URL))
                        {
                            System.IO.File.Delete(_webHostEnvironment.WebRootPath + item.URL);
                        }
                        _context.ProductImages.Remove(item);
                    }
                }
                
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
