using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tickets_bug_BSD2023_2025.Data;
using tickets_bug_BSD2023_2025.Models;

namespace tickets_bug_BSD2023_2025.Controllers
{
    public class TicketsController : Controller
    {
        private readonly tickets_bug_BSD2023_2025Context _context;

        public TicketsController(tickets_bug_BSD2023_2025Context context)
        {
            _context = context;
        }

        // GET: Tickets
        public ActionResult Index()
        {
            string userEmail = HttpContext.Session.GetString("SessionUserName");
            string userRole = HttpContext.Session.GetString("SessionRole");

            IQueryable<Ticket> ticketsQuery;

            if (!string.IsNullOrEmpty(userRole) && userRole == "Admin")
            {
                // Pour l'admin, on affiche tous les tickets
                ticketsQuery = _context.Ticket;
            }
            else
            {
                // Pour un utilisateur non-admin, on affiche uniquement les tickets qui lui sont assignés
                ticketsQuery = _context.Ticket.Where(t => t.AssignedTo == userEmail);
            }

            var tickets = ticketsQuery.ToList();
            return View(tickets);
        }



        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("SessionRole")== null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Statuses = Enum.GetValues(typeof(statusTicket)).Cast<statusTicket>().ToList();
            var userEmails = _context.Home.Select(u => u.Email).ToList();
            ViewBag.UserEmails = new SelectList(userEmails);
            return View();
        }
        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Status,IsResolved,AssignedTo,CreatedAt")] Ticket ticket)
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewBag.Statuses = Enum.GetValues(typeof(statusTicket)).Cast<statusTicket>().ToList();
            var userEmails = _context.Home.Select(u => u.Email).ToList();
            ViewBag.UserEmails = new SelectList(userEmails);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Status,IsResolved,AssignedTo,CreatedAt")] Ticket ticket)
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
