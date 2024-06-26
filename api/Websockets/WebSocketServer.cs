﻿using Fleck;
using LogLevel = Fleck.LogLevel;
using IWebSocketServer = api.Interfaces.IWebSocketServer;

namespace api.Websockets
{
    public class WebSocketServer : IWebSocketServer
    {
        private readonly Fleck.WebSocketServer _server;
        private readonly Dictionary<string, List<IWebSocketConnection>> _roomConnections;

        public WebSocketServer(string url)
        {
            _server = new Fleck.WebSocketServer(url);
            _roomConnections = new Dictionary<string, List<IWebSocketConnection>>();

            FleckLog.Level = LogLevel.Info;
        }

        public void Start(Action<IWebSocketConnection> configure)
        {
            _server.Start(socket =>
            {
                configure(socket);
                socket.OnOpen = () =>
                {
                    Console.WriteLine("WebSocket connection opened.");
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("WebSocket connection closed.");
                    RemoveConnection(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine($"Received message: {message}");
                    HandleMessage(socket, message);
                };
                socket.OnError = (error) =>
                {
                    Console.WriteLine($"WebSocket error: {error.Message}");
                };
            });

            Console.WriteLine("WebSocket server started and listening on " + _server.Location);
        }

        public void Dispose()
        {
            foreach (var room in _roomConnections.Keys.ToList())
            {
                foreach (var socket in _roomConnections[room])
                {
                    socket.Close();
                }
            }
            _server.Dispose();
        }

        private void HandleMessage(IWebSocketConnection socket, string message)
        {
            var parts = message.Split(':');
            if (parts.Length < 2)
            {
                socket.Send("Invalid message format. Use: join:<officeName/roomName> or message:<officeName/roomName>:<yourMessage> or leave:<officeName/roomName>");
                return;
            }

            var command = parts[0];
            var roomName = parts[1];

            if (command == "join")
            {
                JoinRoom(socket, roomName);
            }
            else if (command == "message" && parts.Length >= 3)
            {
                var userMessage = string.Join(':', parts.Skip(2));
                SendMessageToRoom(roomName, userMessage, socket);
            }
            else if (command == "leave")
            {
                LeaveRoom(socket, roomName);
            }
            else
            {
                socket.Send("Invalid command. Use: join:<officeName/roomName> or message:<officeName/roomName>:<yourMessage> or leave:<officeName/roomName>");
            }
        }

        private void JoinRoom(IWebSocketConnection socket, string roomName)
        {
            if (!_roomConnections.ContainsKey(roomName))
            {
                _roomConnections[roomName] = new List<IWebSocketConnection>();
            }

            if (!_roomConnections[roomName].Contains(socket))
            {
                _roomConnections[roomName].Add(socket);
                socket.Send($"Joined room: {roomName}");
            }
        }

        private void LeaveRoom(IWebSocketConnection socket, string roomName)
        {
            if (_roomConnections.ContainsKey(roomName) && _roomConnections[roomName].Contains(socket))
            {
                _roomConnections[roomName].Remove(socket);
                socket.Send($"Left room: {roomName}");
            }
            else
            {
                socket.Send($"You are not in the room: {roomName}");
            }
        }

        private void SendMessageToRoom(string roomName, string message, IWebSocketConnection sender)
        {
            if (_roomConnections.ContainsKey(roomName) && _roomConnections[roomName].Contains(sender))
            {
                foreach (var socket in _roomConnections[roomName])
                {
                    if (socket != sender)
                    {
                        socket.Send(message);
                    }
                }
            }
            else
            {
                sender.Send($"You are not in the room: {roomName}");
            }
        }

        private void RemoveConnection(IWebSocketConnection socket)
        {
            foreach (var room in _roomConnections.Keys.ToList())
            {
                if (_roomConnections[room].Contains(socket))
                {
                    _roomConnections[room].Remove(socket);
                    if (!_roomConnections[room].Any())
                    {
                        _roomConnections.Remove(room);
                    }
                }
            }
        }

        public void Broadcast(string roomName, string message)
        {
            if (_roomConnections.ContainsKey(roomName))
            {
                foreach (var socket in _roomConnections[roomName])
                {
                    socket.Send(message);
                }
            }
        }
    }
}
