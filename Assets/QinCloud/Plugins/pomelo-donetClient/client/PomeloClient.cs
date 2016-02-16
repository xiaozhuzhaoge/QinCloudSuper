using System.Collections;
using SimpleJson;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace Pomelo.DotNetClient
{
    public class PomeloClient : IDisposable
    {
        public const string EVENT_DISCONNECT = "disconnect";
        private EventManager eventManager;
        private Socket socket;
        private Protocol protocol;
        private bool disposed = false;
        private uint reqId = 1;
        public bool eventListening = false;
		const int HEART_BEAT_INTERVAL = 7;
		public HeartBeat heartBeat = null;

        public PomeloClient(string host, int port)
        {
            this.eventManager = new EventManager();
            initClient(host, port);
            this.protocol = new Protocol(this, socket);

			if( heartBeat != null ) {
				heartBeat.Stop();
				heartBeat = null;
			}
			heartBeat = new HeartBeat(HEART_BEAT_INTERVAL, this);
        }

        private void initClient(string host, int port)
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(host), port);
            try
            {
                this.socket.Connect(ie);
            } catch (SocketException e)
            {
                Console.WriteLine(String.Format("unable to connect to server: {0}", e.ToString()));
                return;
            }

        }

        public bool Connected
        {
            get
            {
//                if (disposed)
//                {
//                    return false;
//                }
//                bool part1 = this.socket.Poll(1000, SelectMode.SelectRead);
//                bool part2 = (this.socket.Available == 0);
//                if (part1 && part2)
//                    return false;
//                else
//                    return true;

				if( heartBeat != null )
					return heartBeat.IsConnected;
				else
					return false;
            }
        }
        
        public void connect()
        {
            protocol.start(null, null);
        }
        
        public void connect(JsonObject user)
        {
            protocol.start(user, null);
        }
        
        public void connect(Action<JsonObject> handshakeCallback)
        {
            protocol.start(null, handshakeCallback);
        }
        
        public bool connect(JsonObject user, Action<JsonObject> handshakeCallback)
        {
            try
            {
                protocol.start(user, handshakeCallback);
                return true;
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public void request(string route, Action<JsonObject> action)
        {
            this.request(route, new JsonObject(), action);
        }
        
        public void request(string route, JsonObject msg, Action<JsonObject> action)
        {
            this.eventManager.AddCallBack(reqId, action);
            protocol.send(route, reqId, msg);

            reqId++;
        }
        
        public void notify(string route, JsonObject msg)
        {
            protocol.send(route, msg);
        }
        
        public void on(string eventName, Action<JsonObject> action)
        {
            eventManager.AddOnEvent(eventName, action);
        }

        internal void processMessage(Message msg)
        {
            if (msg.type == MessageType.MSG_RESPONSE)
            {
                eventManager.InvokeCallBack(msg.id, msg.data);
            } else if (msg.type == MessageType.MSG_PUSH)
            {
                eventManager.InvokeOnEvent(msg.route, msg.data);
            }
        }

        public void disconnect()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code 
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                // free managed resources
                this.protocol.close();
                try
                {
                    this.socket.Shutdown(SocketShutdown.Both);
                } catch (SocketException e)
                {
                    UnityEngine.Debug.Log(";;;;");
                    UnityEngine.Debug.LogWarning(e);
                }
                this.socket.Close();
                this.disposed = true;

				if( heartBeat != null ) {
					heartBeat.Stop();
					heartBeat = null;
				}

                //Call disconnect callback
                eventManager.InvokeOnEvent(EVENT_DISCONNECT, null);
            }
        }
    }
}

