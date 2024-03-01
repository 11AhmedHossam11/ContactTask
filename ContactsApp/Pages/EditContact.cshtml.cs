using ContactsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Pages
{
    [Authorize]
    public class EditContactModel : PageModel
    {
        private readonly AppDbContext _context; 
        private readonly IHubContext<ContactHub> _hubContext;

        public EditContactModel(AppDbContext context, IHubContext<ContactHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Contact Contact { get; set; } 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contact = await _context.Contacts.FindAsync(id);

            if (Contact == null)
            {
                return NotFound();
            }

            Contact.IsEditing = true;
            Contact.LockedBy = User.Identity?.Name;
            _context.Contacts.Update(Contact);
            _context.SaveChanges();
            await _hubContext.Clients.All.SendAsync("ReceiveLockedContact", Contact);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Contact.IsEditing = false;
            Contact.LockedBy = null;
            _context.Attach(Contact).State = EntityState.Modified;

            try
            {
                
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveUnLockedContact", Contact);

                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(Contact.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index"); 
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }

   
    }
}
