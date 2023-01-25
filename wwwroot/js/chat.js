"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
document.getElementById("sendPrivateButton").disabled = true;
document.getElementById("sendGroupMessageButton").disabled = true

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user}: ${message}`;
});

connection.on("ReceiveMessageJoin", function (user,groupName) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} joins ${groupName}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    document.getElementById("sendPrivateButton").disabled = false;
    document.getElementById("sendGroupMessageButton").disabled = false;
    // document.getElementById("joinButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});


document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendPrivateButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessageToCaller", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendGroupMessageButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var groupName = document.getElementById("groupNameInput").value;
    connection.invoke("SendMessageToGroup", user, message, groupName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


/*document.getElementById("joinButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var groupName = document.getElementById("groupNameInput").value;
    connection.invoke("JoinGroup",user, groupName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});*/