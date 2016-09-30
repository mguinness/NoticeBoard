using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoticeBoard.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace NoticeBoard.Controllers
{
    [Authorize]
    public class PagesController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ContentContext _context;
        private const string PagesKey = "PageList";

        public PagesController(IMemoryCache memoryCache, ContentContext context)
        {
            _memoryCache = memoryCache;
            _context = context;    
        }

        // GET: Pages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pages.ToListAsync());
        }

        // GET: Pages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {
                page.CreatedBy = User.Identity.Name;
                page.CreatedDate = DateTime.Now;

                _context.Add(page);
                if (await _context.SaveChangesAsync() > 0)
                    _memoryCache.Remove(PagesKey);
                return RedirectToAction("Index");
            }
            return View(page);
        }

        // GET: Pages/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages.SingleOrDefaultAsync(m => m.Pagename == id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newPage = await _context.Pages.SingleOrDefaultAsync(m => m.Pagename == page.Pagename);
                    newPage.Title = page.Title;
                    newPage.ModifiedBy = User.Identity.Name;
                    newPage.ModifiedDate = DateTime.Now;

                    _context.Update(newPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Pages.Any(e => e.Pagename == page.Pagename))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(page);
        }

        // GET: Pages/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Pages.SingleOrDefaultAsync(m => m.Pagename == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var page = await _context.Pages.SingleOrDefaultAsync(m => m.Pagename == id);
            _context.Pages.Remove(page);
            if (await _context.SaveChangesAsync() > 0)
                _memoryCache.Remove(PagesKey);
            return RedirectToAction("Index");
        }
    }
}
