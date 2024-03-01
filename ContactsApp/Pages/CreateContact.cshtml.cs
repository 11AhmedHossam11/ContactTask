using ContactsApp.Models;
using ContactsApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Pages
{
    public class CreateContactModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ContactHub> _hubContext;

        public CreateContactModel(AppDbContext context, IHubContext<ContactHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public ContactDTO Contact { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

         var contact= new Models.Contact()
            {
                Name = Contact.Name,
                Address = Contact.Address,
                Notes = Contact.Notes,
                Phone = Contact.Phone,
                IsEditing = false,
                
            };

            _context.Contacts.Add(contact);

            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveContacts", contact);

            return RedirectToPage("./Index");
        }
    }
}
