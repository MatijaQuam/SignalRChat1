using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

       public Task SendMessageToCaller(string user, string message)
        {
           return Clients.Caller.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToGroup(string user, string message, string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message+" ->"+groupName);
        }

        
       /* public async Task JoinGroup(string user,string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessageJoin",user +" "+ Context.ConnectionId, groupName);
        }*/
      
    }
}