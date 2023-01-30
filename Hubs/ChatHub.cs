using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        List<string> GroupList = new List<string>();

        public void AddGroupToList(string groupName)
        {
            bool containsItem = GroupList.Any(item => item.Contains(groupName));
            if (!containsItem)
            {
                GroupList.Add(groupName);
            }

        }
        private static void LogWrite(string logMessage, StreamWriter w)
        {
            w.WriteLine("{0}", logMessage);
            w.WriteLine("----------------------------------------");
        }
        public void writeLog(string strValue,string name)
        {
            try
            {
                //Logfile
                string path = @"C:\Users\lukaq\source\repos\logdata\";
                path = path + name + ".log";
                StreamWriter sw;
                if (!File.Exists(path))
                { sw = File.CreateText(path); }
                else
                { sw = File.AppendText(path); }

                LogWrite(strValue, sw);

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SendMessageToGroup(string user, string message, string groupName)
        {
            string logDateTime = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
            string time = DateTime.UtcNow.ToString("HH:mm");

            await Clients.Caller.SendAsync("ReceiveMessage", user + " [" + time + "] ", message, groupName);
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveMessage", user + " ["+time+"] ", message, groupName);

            string name = user + Context.ConnectionId;
            string strValue = "User name: " +user + ", Group name: " + groupName + ", Date and Time: " + logDateTime + " Message: " + message + ", Action: " + "message send.";
            writeLog(strValue, name);
        }
        public async Task JoinGroup(string user, string groupName)
        {
            string logDateTime = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
            string time = DateTime.UtcNow.ToString("HH:mm");

            AddGroupToList(groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.All.SendAsync("NewGroup", GroupList);
            await Clients.Caller.SendAsync("ReceiveMessage", user+" [" + time + "] ", " Has joined the chat. ", GroupList );
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveMessage", user + " [" + time + "] ", " Has joined the chat. ", GroupList);

            
            string name = user + Context.ConnectionId;
            string strValue = "User name: " + user + ", Group name: " + groupName + ", Date and Time: " + logDateTime + ", Action: " + "Join group.";
            writeLog(strValue, name);
        }
        public async Task LeaveGroup(string user, string groupName)
        {
            string logDateTime = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
            string time = DateTime.UtcNow.ToString("HH:mm");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("ReceiveMessage", user + " [" + time + "] ", " Has left the chat. ",groupName);
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveMessage", user+" [" + time + "] ", " Has left the chat. ",groupName);

            string name = user + Context.ConnectionId;
            string strValue = "User name: " + user + ", Group name: " + groupName + ", Date and Time: " + logDateTime + ", Action: " + "Leave group.";
            writeLog(strValue, name);
        }



        //OLD CODE

        /*public async Task SendMessage(string user, string message)
          {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
          } */

        /*public Task SendMessageToCaller(string user, string message)
         {
           return Clients.Caller.SendAsync("ReceiveMessage", user, message);
          }*/

        /*public async Task JoinGroup(string user,string groupName)
         {  
             await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
             await Clients.Group(groupName).SendAsync("ReceiveMessageJoin",user +" "+ Context.ConnectionId, groupName);
         }*/

    }
}