using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommunicationProtocol;
using System.Threading;

public class PlayerObject
{
    // Position Translation
    public static Dictionary<int,string> POSITIONS = new Dictionary<int, string>()
    {
        {0,"Dishwasher"},
        {1,"Waiter"},
        {2,"Souschef"},
        {3,"Chef"},
        
    };

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
        {"updateExchangedCards", "updateExchangedCards"},
        {"updateMatchStart", "updateMatchStart"},                
    };

    public static Dictionary<string,string> PLAYER_STATUS = new Dictionary<string, string>(){
        {"waitingStart","waitingStart"},      
        {"playing","playing"},    
        {"gameOver","gameOver"},      
        {"gameFinished","gameFinished"},      
        {"skiping","skiping"},    
        {"no_connection","no_connection"},    
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
        
    public static List<string> GetHighLevelActions()
    {
        int maxCardNumber = 11;
        
        List<string> highLevelActions = new List<string>();

        for (int cardNumber = 0; cardNumber < maxCardNumber; cardNumber++)
        {
            for (int cardQuantity = 0; cardQuantity <= cardNumber; cardQuantity++)
            {
                highLevelActions.Add("C" + (cardNumber + 1) + ";Q" + (cardQuantity + 1) + ";J0");
                highLevelActions.Add("C" + (cardNumber + 1) + ";Q" + (cardQuantity + 1) + ";J1");
                highLevelActions.Add("C" + (cardNumber + 1) + ";Q" + (cardQuantity + 1) + ";J2");
            }
        }

        highLevelActions.Add("C0;Q0;J1");
        highLevelActions.Add("pass");

        return highLevelActions;
    }
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
            if (!SocketConnected(Socket))
            {
                PlayerStatus = PLAYER_STATUS["no_connection"];                
                break;
            }
            var received = await Socket.ReceiveAsync(Bytes, SocketFlags.None); 
            await Task.Run(() =>  ProcessMessage(received));    

            Thread.Sleep(1); ///Wait for 0.001 seconds                        
        }
    }

    private bool SocketConnected(Socket s)
    {
        return true;
        // // Exit if socket is null
        // if (s == null)
        //     return false;
            
        // bool part1 = s.Poll(1000, SelectMode.SelectRead);
        // bool part2 = (s.Available == 0);
        // if (part1 && part2)
        //     return false;
        // else
        // {
        //     try
        //     {
        //         int sentBytesCount = s.Send(new byte[1], 1, 0);
        //         return sentBytesCount == 1;
        //     }
        //     catch
        //     {
        //     return false;
        //     }
        // }
    }

    public async void SendDiscardAction(int[] action)
    {
        
        if (!SocketConnected(Socket))
            {
                PlayerStatus = PLAYER_STATUS["no_connection"];
            }
                
        else
            {
                var discardMessage = new DiscardAction()
                {
                    agent_action = action
                    
                };


                Debug.Log("[Player Object] Sending Discard message to the server:" + discardMessage.GetJSON());

                byte[] messageInBytes = Encoding.ASCII.GetBytes(discardMessage.GetJSON()); // System.Text.Json

                int bytesSent = Socket.Send(messageInBytes);
            }

        
    }

    public async void SendSpecialAction(bool doSpecialAction)
    {
        
        if (!SocketConnected(Socket))
            {
                PlayerStatus = PLAYER_STATUS["no_connection"];
            }
                
        else
            {
                var discardMessage = new SpecialAction()
                {
                    agent_action = doSpecialAction
                    
                };


                Debug.Log("[Player Object] Sending Do Special Action message to the server:" + discardMessage.GetJSON());

                byte[] messageInBytes = Encoding.ASCII.GetBytes(discardMessage.GetJSON()); // System.Text.Json

                int bytesSent = Socket.Send(messageInBytes);
            }

        
    }


    public async void SendExchangedCards(int[] cards)
    {
        
        if (!SocketConnected(Socket))
            {
                PlayerStatus = PLAYER_STATUS["no_connection"];
            }
                
        else
            {
                var discardMessage = new CardsExchanged()
                {
                    agent_action = cards
                    
                };


                Debug.Log("[Player Object] Sending Do Special Action message to the server:" + discardMessage.GetJSON());

                byte[] messageInBytes = Encoding.ASCII.GetBytes(discardMessage.GetJSON()); // System.Text.Json

                int bytesSent = Socket.Send(messageInBytes);
            }

        
    }
    private async void ProcessMessage(int receivedMessage)
    {
        string response = Encoding.ASCII.GetString(Bytes, 0, receivedMessage);            

        if (response.Contains(REQUEST_TYPE["updateMatchStart"]))
        {
            PlayerStatus = PLAYER_STATUS["playing"];

            MatchStartedMessage roomMessage = JsonUtility.FromJson<MatchStartedMessage>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Message Received! Adding message to the Q: " + roomMessage.type);

        }
        else if (response.Contains(REQUEST_TYPE["requestAction"]))
        {

            RequestActionMessage roomMessage = JsonUtility.FromJson<RequestActionMessage>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Message Received! Adding message to the Q: " + roomMessage.type);

        }
        else if (response.Contains(REQUEST_TYPE["actionUpdate"]))
        {
            UpdateOthersMessage roomMessage = JsonUtility.FromJson<UpdateOthersMessage>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Message Received! Adding message to the Q: " + roomMessage.type+ " Related to my own action.");

        }
        else if (response.Contains(REQUEST_TYPE["updateOthers"]))
        {
            UpdateOthersMessage roomMessage = JsonUtility.FromJson<UpdateOthersMessage>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Message Received! Adding message to the Q: " + roomMessage.type+ " Related to the action of player: "+roomMessage.Player_Names[roomMessage.Author_Index] + ".");

        }
        else if (response.Contains(REQUEST_TYPE["gameOver"]))
        {

            PlayerStatus = PLAYER_STATUS["gameOver"];
            GameOver roomMessage = JsonUtility.FromJson<GameOver>(response);
            
            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Message Received! Adding message to the Q: " + roomMessage.type);

        }
        else if (response.Contains(REQUEST_TYPE["matchOver"]))
        {
            MatchOver roomMessage = JsonUtility.FromJson<MatchOver>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Message Received! Adding message to the Q: " + roomMessage.type);

        }
        else if (response.Contains(REQUEST_TYPE["doSpecialAction"]))
        {
            DoSpecialAction roomMessage = JsonUtility.FromJson<DoSpecialAction>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Message Received! Adding message to the Q: " + roomMessage.type);

        }
        else if (response.Contains(REQUEST_TYPE["specialActionUpdate"]))
        {
            SpecialActionUpdate roomMessage = JsonUtility.FromJson<SpecialActionUpdate>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Message Received! Adding message to the Q: " + roomMessage.type);

        }
        else if (response.Contains(REQUEST_TYPE["exchangeCards"]))
        {
            ExchangeCards roomMessage = JsonUtility.FromJson<ExchangeCards>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Message Received! Adding message to the Q: " + roomMessage.type);

        }        
        else if (response.Contains(REQUEST_TYPE["updateExchangedCards"]))
        {
            UpdateExchangeCards roomMessage = JsonUtility.FromJson<UpdateExchangeCards>(response);

            MessagesQ.Enqueue(roomMessage);
            Debug.Log("[Player Object] Message Received! Adding message to the Q: " + roomMessage.type);

        }    

        Debug.Log("[Player Object] Current messages on the Q: " + MessagesQ.Count);


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
                password = password,
                author="Player"
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
