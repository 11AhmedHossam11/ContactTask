using ContactsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContactsApp.Pages
{
    [Authorize]

    public class DeleteContactModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ContactHub> _hubContext;

        public DeleteContactModel(AppDbContext context, IHubContext<ContactHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync([FromQuery]int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);

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

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contact = await _context.Contacts.FindAsync(id);

            if (Contact != null)
            {
                _context.Contacts.Remove(Contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
