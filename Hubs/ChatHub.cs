using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {

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

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("ReceiveMessage", user + " |" + time + "|", message);
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveMessage", user + " |"+time+"| ", message);

            string name = user + Context.ConnectionId;
            string strValue = "User name: " +user + ", Group name: " + groupName + ", Date and Time: " + logDateTime + " Message: " + message + ", Action: " + "message send.";
            writeLog(strValue, name);
        }
        public async Task JoinGroup(string user, string groupName)
        {
            string logDateTime = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
            string time = DateTime.UtcNow.ToString("HH:mm");

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("ReceiveMessage", user, " you have joined the chat at " + time + ".");
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveMessage", user, " has join your chat at" + time + ".");

            string name = user + Context.ConnectionId;
            string strValue = "User name: " + user + ", Group name: " + groupName + ", Date and Time: " + logDateTime + ", Action: " + "Join group.";
            writeLog(strValue, name);
        }
        public async Task LeaveGroup(string user, string groupName)
        {
            string logDateTime = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
            string time = DateTime.UtcNow.ToString("HH:mm");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("ReceiveMessage", user, " you have left the chat " + time + ".");
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveMessage", user, " has left your chat at "+ time +".");

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