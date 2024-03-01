
namespace ContactsApp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public string? LockedBy { get; set; }
        public bool IsEditing { get;  set; } = false;
    }
}
