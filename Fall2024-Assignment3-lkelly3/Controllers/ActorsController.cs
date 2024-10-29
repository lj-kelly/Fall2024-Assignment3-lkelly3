using Fall2024_Assignment3_lkelly3.Data;
using Fall2024_Assignment3_lkelly3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fall2024_Assignment3_lkelly3.Controllers
{
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ActorsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: ActorsController
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actors.ToListAsync());
        }


        // GET: ActorsController/Details/5
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.Id == Id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // GET: ActorsController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActorsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Actors actor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: ActorsController/Edit/5
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(Id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: ActorsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, Actors actor)
        {
            if (Id != actor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id))
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
            return View(actor);
        }

        // GET: ActorsController/Delete/5
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.Id == Id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: ActorsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? Id)
        {
            var actor = await _context.Actors.FindAsync(Id);
            if (actor != null)
            {
                _context.Actors.Remove(actor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ActorExists(int Id)
        {
            return _context.Actors.Any(e => e.Id == Id);
        }
    }
}
