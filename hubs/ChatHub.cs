using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ASP_Lab_clans.Models;
namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public Task sendmessagetogroup(string groupname, string sender, string message)
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var newmessage = new Message();
                var PlayerData = dc.Users.FirstOrDefault(a => a.Username == sender);
                var CurrrentClanID = dc.Clan_member.FirstOrDefault(a => a.User_ID == PlayerData.UserID);
                newmessage.Clan_ID = CurrrentClanID.Clan_ID;
                newmessage.sender = PlayerData.UserID;
                newmessage.message1 = message;
                dc.Messages.Add(newmessage);
                dc.SaveChanges();
                return Clients.Group(groupname).sendmessagetopage(sender, message);
            }
        }

        public async Task joingroup(string groupName, string user)
        {
            await Groups.Add(Context.ConnectionId, groupName);
            await Clients.Group(groupName).sendmessagetopage(user, " has joined the chat.");
        }

        public void loadmessages(string user)
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var PlayerData = dc.Users.FirstOrDefault(a => a.Username == user);
                var CurrrentClanID = dc.Clan_member.FirstOrDefault(a => a.User_ID == PlayerData.UserID);
                List<Message> Messages = dc.Messages.Include("User").Where(a => a.Clan_ID == CurrrentClanID.Clan_ID).ToList();
                foreach (var Message in Messages)
                {
                    Clients.Caller.sendmessagetopage(Message.User.Username, Message.message1);
                }
            }
        }
        public async Task removegroup(string groupName, string user)
        {
            await Groups.Remove(Context.ConnectionId, groupName);

            await Clients.Group(groupName).sendmessagetopage(user, " has joined the chat.");
        }
    }
}