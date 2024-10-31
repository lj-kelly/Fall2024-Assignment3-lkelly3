using Fall2024_Assignment3_lkelly3.Data;
using Fall2024_Assignment3_lkelly3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_lkelly3.Services;
using VaderSharp2;
using Fall2024_Assignment3_lkelly3.Data.Migrations;
namespace Fall2024_Assignment3_lkelly3.Controllers
{
    public class ActorsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly OpenAIhit _openAIhit;
        public ActorsController(ApplicationDbContext context, OpenAIhit openAIhit)
        {
            _openAIhit = openAIhit;
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
            var movies = await _context.MovieActor
                .Include(cs => cs.Movies)
                .Where(cs => cs.ActorId == actor.Id)
                .Select(cs => cs.Movies)
            .ToListAsync();

            List<string> tweets = new List<string>();
            tweets = await _openAIhit.WriteTweets(actor.Name);

            SentimentIntensityAnalyzer analysis = new SentimentIntensityAnalyzer();
            List<Review> feed = new List<Review>();

            int goodCount = 0;
            int badCount = 0;

            foreach (var tweet in tweets)
            {
                var senti = analysis.PolarityScores(tweet);
                string sentiment;
                
                if(senti.Compound <= -0.05)
                {
                    sentiment = "Bad";
                    badCount += 1;
                }
                else if (senti.Compound >= 0.05){
                    sentiment = "Good";
                    goodCount += 1;
                }
                else
                {
                    sentiment = "Meh";
                }

                feed.Add(new Review { ReviewText = tweet, SentimentAnalysis = sentiment });
                
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
            ActorsDetailsViewModel advm = new ActorsDetailsViewModel(actor, movies, feed, overall);
            return View(advm);
        }

        // GET: ActorsController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActorsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Link,Gender,Age,Photo")] Actors actor)
        {
            var photo = Request.Form.Files["Photo"];
            if (ModelState.IsValid)
            {
                if (photo != null && photo.Length > 0)
                {
                    using var memoryStream = new MemoryStream(); // Dispose() for garbage collection 
                    await photo.CopyToAsync(memoryStream);
                    actor.Photo = memoryStream.ToArray();
                }
                
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }
        public IActionResult GetImage(int id)
        {
            var actor = _context.Actors.Find(id);
            if (actor == null || actor.Photo == null)
            {
                return NotFound(); // Or return a default image
            }
            return File(actor.Photo, "image/jpeg"); // Assuming JPEG, adjust MIME type if necessary
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
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,Link,Gender,Age,Photo")] Actors actor)
        {
            if (Id != actor.Id)
            {
                return NotFound();
            }

            var photo = Request.Form.Files["Photo"];

            if (ModelState.IsValid)
            {
                try
                {
                    if (photo != null && photo.Length > 0) // Check if a new photo was uploaded
                    {
                        using var memoryStream = new MemoryStream();
                        await photo.CopyToAsync(memoryStream);
                        actor.Photo = memoryStream.ToArray(); // Update the photo
                    }
                    else
                    {
                        // If no new photo, fetch the existing actor to retain the current photo
                        var existingActor = await _context.Actors.AsNoTracking().FirstOrDefaultAsync(m => m.Id == Id);
                        actor.Photo = existingActor.Photo; // Retain the existing photo
                    }
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
