using ContactsApp.Models;
using Microsoft.AspNetCore.SignalR;

public class ContactHub : Hub
{
    public async Task SendContacts(Contact contact)
    {
        await Clients.All.SendAsync("ReceiveContacts", contact);
    }

    public async Task LockContact(Contact contact)
    {
        await Clients.All.SendAsync("ReceiveLockedContact", contact);
    }

    public async Task UnLockContact(Contact contact)
    {
        await Clients.All.SendAsync("ReceiveUnLockedContact", contact);
    }
}
