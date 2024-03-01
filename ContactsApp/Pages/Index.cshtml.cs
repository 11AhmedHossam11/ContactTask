using ContactsApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Authorize] 
public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Contact> PagedContacts { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public int PreviousPageNumber => PageNumber - 1;
    public int NextPageNumber => PageNumber + 1;

    [BindProperty]
    public Contact contact { get; set; }
    public async Task OnGetAsync([FromQuery] int? page, string nameFilter, string phoneFilter, string addressFilter, string notesFilter)
    {

        const int pageSize = 5;
        PageNumber = page ?? 1;

        var contacts = _context.Contacts.AsQueryable();
       
        if (!string.IsNullOrEmpty(nameFilter))
        {
            contacts = contacts.Where(c => c.Name.Contains(nameFilter));
        }
        if (!string.IsNullOrEmpty(phoneFilter))
        {
            contacts = contacts.Where(c => c.Phone.Contains(phoneFilter));
        }
        if (!string.IsNullOrEmpty(addressFilter))
        {
            contacts = contacts.Where(c => c.Address.Contains(addressFilter));
        }
        if (!string.IsNullOrEmpty(notesFilter))
        {
            contacts = contacts.Where(c => c.Notes.Contains(notesFilter));
        }

        TotalPages = (int)Math.Ceiling(await contacts.CountAsync() / (double)pageSize);

        PagedContacts = await contacts.Skip((PageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

        foreach (var contact in PagedContacts)
        {
            if (contact.IsEditing && contact.LockedBy != null && contact.LockedBy == User.Identity?.Name)
            {
                contact.IsEditing = false;
                contact.LockedBy = null;
                _context.Contacts.Update(contact);
                _context.SaveChanges();
            }
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingContact = await _context.Contacts.FindAsync(contact.Id);

        if (existingContact == null)
        {
            return NotFound();
        }

        existingContact.Name = contact.Name;
        existingContact.Phone = contact.Phone;
        existingContact.Address = contact.Address;
        existingContact.Notes = contact.Notes;
        existingContact.IsEditing = false;
        existingContact.LockedBy = null;
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

   
}
