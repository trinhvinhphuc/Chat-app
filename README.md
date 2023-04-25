# Chat-App using Socket in C#
This is a simple chat application built using socket programming in C#. It allows users to connect to a server and communicate with each other via text messages.

## Requirements
To run this application, you will need:

- Visual Studio (2019 or later)
- .NET Framework 6

## Usage
To use the chat application, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution file (Chat-app.sln) in Visual Studio.
3. Build the solution.
4. Start the server by running the Server project.
5. Start multiple instances of the client by running the Client project.
6. Connect to the server by entering the IP address and port number of the server in the client UI.
7. Once connected, you can send messages to other connected clients.

## Features
The chat application has the following features:

- Multiple clients can connect to the server and communicate with each other.
- Each client can send and receive messages in real-time.

## Architecture
The chat application uses a simple client-server architecture. The server listens for incoming connections on a specified port number. When a client connects, the server creates a new thread to handle the client's requests. Each client is identified by a unique ID.

The communication between the client and server is done using a TCP socket. Messages are sent as byte arrays and are encoded and decoded using the UTF-8 encoding.
