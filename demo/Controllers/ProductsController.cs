// Comments added for clarity throughout the controller

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demo.Models;
using demo.Data;

namespace demo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductDemoContext _context;

        public ProductsController(ProductDemoContext context)
        {
            _context = context;
        }

        // GET: Products
        /*public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }*/

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Product.FindAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] Product product)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Products with filtering by name and price range
        public async Task<IActionResult> Index(string nameFilter, decimal? priceFilter_max, decimal? pricefilter_min)
        {
            var products = _context.Product.AsQueryable();

            // Filter by product name if specified
            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                products = products.Where(p => p.Name.Contains(nameFilter));
            }

            // Filter by maximum price if specified
            if (priceFilter_max.HasValue)
            {
                products = products.Where(p => p.Price <= priceFilter_max.Value);
            }

            // Filter by minimum price if specified
            if (pricefilter_min.HasValue)
            {
                products = products.Where(p => p.Price >= pricefilter_min.Value);
            }

            return View(await products.ToListAsync());
        }

        // Helper method to check if a product exists by ID
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
