"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
document.getElementById("joinButton").disabled = true;
document.getElementById("leaveButton").disabled = true;




connection.on("ReceiveMessage", function (user, message, group) {

    var div = document.createElement("div");
  //  var p = document.createElement("p");

    div.style.borderRadius = '5px'
    div.style.backgroundColor = '#66B2FF'
    div.style.marginTop = '5px'

    document.getElementById("messagesList").appendChild(div);
   // document.getElementById("groupList").appendChild(p)
    // We can assign user-supplied strings to an element's textContent because it is not interpreted as markup.
    // If you're assigning in any other way, you should be aware of possible script injection concerns.
    div.textContent = `${user}: ${message}`;
 //   p.textContent = `${group}`;
});

connection.on("NewGroup", function (group) {

    var p = document.createElement("p");


    document.getElementById("groupList").appendChild(p)
    // We can assign user-supplied strings to an element's textContent because it is not interpreted as markup.
    // If you're assigning in any other way, you should be aware of possible script injection concerns.
   
    p.textContent = `${group}`;
});



connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    document.getElementById("joinButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});


document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var groupName = document.getElementById("groupNameInput").value;
    document.getElementById("messageInput").value="";
    connection.invoke("SendMessageToGroup", user, message, groupName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("joinButton").addEventListener("click", function (event) {

    document.getElementById("joinButton").disabled = true;
    document.getElementById("joinButton").hidden = true;
    document.getElementById("leaveButton").disabled = false;
    document.getElementById("leaveButton").hidden = false;
    document.getElementById("sendButton").hidden = false;
    document.getElementById("userInput").disabled = true;
    document.getElementById("groupNameInput").disabled = true;
    document.getElementById("messageInput").disabled = false;

    var user = document.getElementById("userInput").value;
    var groupName = document.getElementById("groupNameInput").value;

    connection.invoke("JoinGroup", user, groupName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("leaveButton").addEventListener("click", function (event) {

    document.getElementById("leaveButton").disabled = true;
    document.getElementById("leaveButton").hidden = true;
    document.getElementById("joinButton").disabled = false;
    document.getElementById("joinButton").hidden = false;
    document.getElementById("sendButton").hidden = true;
    document.getElementById("userInput").disabled = false
    document.getElementById("groupNameInput").disabled = false;
    document.getElementById("messageInput").disabled = true;
    document.getElementById("messagesList").replaceChildren();

    var user = document.getElementById("userInput").value;
    var groupName = document.getElementById("groupNameInput").value;
    connection.invoke("LeaveGroup", user, groupName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

