using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using System.Threading.Tasks;
using CommunicationProtocol;
using System.Threading;

public class PlayerObject
{

    //Message Protocol
    static Dictionary<string,string> MESSAGE_TYPE = new Dictionary<string, string>()
    {
        {"OK","OK"},
        {"ERROR","ERROR"}
    };

    static Dictionary<string,string> REQUEST_TYPE = new Dictionary<string, string>(){
        {"requestAction","requestAction"},
        {"sendAction","sendAction"},
        {"actionUpdate","actionUpdate"},
        {"updateOthers", "updateOthers"},
        {"matchOver", "matchOver"},
        {"gameOver", "gameOver"},
        {"doSpecialAction", "doSpecialAction"},
        {"specialActionUpdate", "specialActionUpdate"},
        {"exchangeCards", "exchangeCards"},
        {"updateMatchStart", "updateMatchStart"},
    };

    public static Dictionary<string,string> PLAYER_STATUS = new Dictionary<string, string>(){
        {"waitingStart","waitingStart"},      
        {"playing","playing"},    
        {"gameOver","gameOver"},      
        {"gameFinished","gameFinished"},      
        {"skiping","skiping"},    
    };


    //Player Information
    public string Name {get; set;}
    public string Avatar {get; set;}

    //Room Information
    private int RoomURL {get; set;}
    private int RoomPort {get; set;}
    private int RoomPass {get; set;}

    private Socket Socket {get;set;}
     byte[] Bytes = new byte[6144];

    //Player Behavior
    private int ConnectionTimeout {get; set;}

    public string PlayerStatus {get;set;}

    public Queue<RoomMessage> MessagesQ;

    //Input Forms Create Game
    public static PlayerObject player;
    
    public static PlayerObject CreatePlayer(string name)
    {
        player = new PlayerObject();
        player.Name = name;       
        Debug.Log("[Player Object] Player created!:"+player.Name );
        return player;
    }

    public async void ListenMessages()
    {
        Debug.Log("[Player Object] Listening to game room messages...");              

        while (PlayerStatus != PLAYER_STATUS["gameOver"])
        {
            var received = await Socket.ReceiveAsync(Bytes, SocketFlags.None); 
            await Task.Run(() =>  ProcessMessage(received));    

            Thread.Sleep(10); ///Wait for 0.01 seconds                        
        }
    }

    public async void SendDiscardAction(int[] action)
    {

        var discardMessage = new DiscardAction()
        {
            agent_action = action
            
        };


        Debug.Log("[Player Object] Sending Discard message to the server:" + discardMessage.GetJSON());

        byte[] messageInBytes = Encoding.ASCII.GetBytes(discardMessage.GetJSON()); // System.Text.Json

        int bytesSent = Socket.Send(messageInBytes);
    }

    private async void ProcessMessage(int receivedMessage)
    {
            string response = Encoding.ASCII.GetString(Bytes, 0, receivedMessage);            
            // Debug.Log("[Player Object] Message received: "+response);

        if (response.Contains(REQUEST_TYPE["updateMatchStart"]))
        {

            PlayerStatus = PLAYER_STATUS["playing"];

            MatchStartedMessage roomMessage = JsonUtility.FromJson<MatchStartedMessage>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Adding message to the Q: " + roomMessage);

        }
        else if (response.Contains(REQUEST_TYPE["requestAction"]))
        {

            RequestActionMessage roomMessage = JsonUtility.FromJson<RequestActionMessage>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Adding message to the Q: " + roomMessage);

        }
        else if (response.Contains(REQUEST_TYPE["actionUpdate"]))
        {
            UpdateOthersMessage roomMessage = JsonUtility.FromJson<UpdateOthersMessage>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Adding message to the Q: " + roomMessage);

        }
        else if (response.Contains(REQUEST_TYPE["updateOthers"]))
        {
            UpdateOthersMessage roomMessage = JsonUtility.FromJson<UpdateOthersMessage>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Adding message to the Q: " + roomMessage);

        }
        else if (response.Contains(REQUEST_TYPE["gameOver"]))
        {

            PlayerStatus = PLAYER_STATUS["gameOver"];
            GameOver roomMessage = JsonUtility.FromJson<GameOver>(response);
            
            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Adding message to the Q: " + roomMessage);

        }
        else if (response.Contains(REQUEST_TYPE["matchOver"]))
        {
            MatchOver roomMessage = JsonUtility.FromJson<MatchOver>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Adding message to the Q: " + roomMessage);

        }

        }

    public void ConnectToRoom(string url, int port, string password)
    {   

        // IPHostEntry host = Dns.GetHostEntry("localhost");
        // IPAddress ipAddress = host.AddressList[0];
        // IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, port);
        

        var ipAddress = Dns.GetHostEntry("localhost").AddressList;

        IPEndPoint serverEndPoint = new(ipAddress[1], port);


        Socket = new Socket(serverEndPoint.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
        
        try{

            Debug.Log("[Player Object] Connecting to:"+serverEndPoint.ToString());

            Socket.Connect(serverEndPoint);

            Debug.Log("[Player Object] Player connected to:"+Socket.RemoteEndPoint.ToString());

            var connectionMessage = new ConnectRoomMessage()
            {
                playerName = Name,
                password = password
            };

        
            Debug.Log("[Player Object] Sending message:"+connectionMessage.GetJSON());                      

            byte[] messageInBytes = Encoding.ASCII.GetBytes(connectionMessage.GetJSON()); // System.Text.Json

            int bytesSent = Socket.Send(messageInBytes);

            Debug.Log("[Player Object] Waiting for a confirmation from the server...");                      

            int bytesRec = Socket.Receive(Bytes);

            string response = Encoding.ASCII.GetString(Bytes, 0, bytesRec);
            Debug.Log("[Player Object] Message received: "+response);                      

            var roomResponse = JsonUtility.FromJson<RoomResponse>(response);

            Debug.Log("[Player Object] Message de-serialized: "+roomResponse.type);                      

            if (roomResponse.type==MESSAGE_TYPE["ERROR"])
            {
                Debug.Log("[Player Object] Connection error: "+roomResponse.message);  
                throw new System.Exception("Connection error: "+roomResponse.message);
            }


            PlayerStatus = PLAYER_STATUS["waitingStart"];

            MessagesQ = new Queue<RoomMessage>();    

            Debug.Log("[Player Object] All good! Waiting for players to join the game...");
            
            Task.Run(() =>  ListenMessages());
                       

        }catch (SocketException se)
            {
                throw new System.Exception("Connection error: "+se.Message);
            }
    }


    
}
