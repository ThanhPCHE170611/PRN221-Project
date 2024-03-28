"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Disable the send button until the connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var messagesList = document.getElementById("messagesList");

    // Create a new list item element
    var li = document.createElement("li");

    // Assign a class based on the index of the list item
    var index = messagesList.children.length;
    if (index % 2 !== 0) {
        li.classList.add("text-success", "font-weight-bold");
    } else {
        li.classList.add("text-dark", "font-weight-bold");
    }

    // Create a timestamp
    var timestamp = new Date().toLocaleTimeString();

    // Set the content of the list item with the timestamp
    li.textContent = `[${timestamp}] ${user}: ${message}`;

    // Append the list item to the messagesList
    messagesList.appendChild(li);

    // Check if the messagesList is scrolled to the bottom
    var isScrolledToBottom = messagesList.scrollHeight - messagesList.clientHeight <= messagesList.scrollTop + 1;

    // If not scrolled to the bottom, auto-scroll to show the latest message
    if (!isScrolledToBottom) {
        messagesList.scrollTop = messagesList.scrollHeight;
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var messageInput = document.getElementById("messageInput");
    var message = messageInput.value.trim(); // Trim removes leading and trailing whitespaces

    // Check if the message is not empty before sending
    if (message !== "") {
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
    }

    // Clear the input field
    messageInput.value = "";

    event.preventDefault();
});
