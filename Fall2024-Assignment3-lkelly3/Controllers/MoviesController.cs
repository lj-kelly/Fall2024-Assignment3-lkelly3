using Fall2024_Assignment3_lkelly3.Data;
using Fall2024_Assignment3_lkelly3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_lkelly3.Services;
using VaderSharp2;
using Fall2024_Assignment3_lkelly3.Data.Migrations;
using static System.Net.Mime.MediaTypeNames;

namespace Fall2024_Assignment3_lkelly3.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OpenAIhit _openAIhit;
        public MoviesController(ApplicationDbContext context, OpenAIhit openAIHit)
        {
            _openAIhit = openAIHit;
            _context = context;
        }

        // GET: HomeController1
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: HomeController1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            var actors = await _context.MovieActor
                .Include(cs => cs.Actors)
                .Where(cs => cs.MovieId == movie.Id)
                .Select(cs => cs.Actors)
                .ToListAsync();

            List<string> reviews = await _openAIhit.WriteReviews(movie.Title);
            SentimentIntensityAnalyzer analysis = new SentimentIntensityAnalyzer();
            List<Review> reviewList = new List<Review>();

            int goodCount = 0;
            int badCount = 0;

            foreach (var review in reviews)
            {
                var senti = analysis.PolarityScores(review);
                string sentiment;

                if (senti.Compound <= -0.05)
                {
                    sentiment = "Bad";
                    badCount += 1;
                }
                else if (senti.Compound >= 0.05)
                {
                    sentiment = "Good";
                    goodCount += 1;
                }
                else
                {
                    sentiment = "Meh";
                }
                reviewList.Add(new Review { ReviewText = review, SentimentAnalysis = sentiment });
            }
            string overall;
            if (goodCount > badCount)
            {
                overall = "Good";
            }
            else
            {
                overall = "Bad";
            }
            MoviesDetailsViewModel mdvm = new MoviesDetailsViewModel(movie, actors,  reviewList, overall);
            return View(mdvm);
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Link,Genre,Year,Poster")] Movies movie)
        {
            var poster = Request.Form.Files["Poster"];

            if (ModelState.IsValid)
            {
                if (poster != null && poster.Length > 0)
                {
                    using var memoryStream = new MemoryStream(); // Dispose() for garbage collection 
                    await poster.CopyToAsync(memoryStream);
                    movie.Poster = memoryStream.ToArray();
                }
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        public async Task<IActionResult> GetImage(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null || movie.Poster == null)
            {
                return NotFound(); // Or return a default image
            }
            return File(movie.Poster, "image/jpeg"); // Assuming JPEG, adjust MIME type if necessary
        }

        // GET: HomeController1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Link,Genre,Year,Poster")] Movies movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }
            var poster = Request.Form.Files["Poster"];




            if (ModelState.IsValid)
            {
                try
                {
                    if(poster != null && poster.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await poster.CopyToAsync(memoryStream);
                        movie.Poster = memoryStream.ToArray();
                    }


                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: HomeController1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: HomeController1/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
