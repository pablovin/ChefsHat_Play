using System.Collections.Generic;
using UnityEngine;

namespace CommunicationProtocol {


    public class RoomMessage{
        public string GetJSON()
        {
            return JsonUtility.ToJson(this);
        }
    }

    public class StandardInfo : RoomMessage
    {
        public string type;

        public int Matches;
        public int Rounds;
        public string[] Player_Names;
        public int Author_Index;
        public string[] Author_Possible_Actions;
        public float[] Observation_Before;
        public float[] Observation_After;
        public bool Action_Valid;
        public bool Action_Random;
        public int Action_Index;
        public string Action_Decoded;
        public bool Is_Pizza;
        public int Pizza_Author;
        public bool[] Finished_Players;
        public int[] Cards_Per_Player;
        public string[] Last_action_Per_Player;
        public int Next_Player;

        public int[] Board_Before;
        public int[] Board_After;
        public string[] Current_Roles;
        public int[] Match_Score;
        public int[] Game_Score;
        public float[] Game_Performance_Score;
       
        
        // public int[] score;
        // public float[] performanceScore;
        // public int thisPlayer;
        // public bool thisPlayerFinished;
        // public bool[] PlayersFinished;
        // public bool isPizzaReady;

        // public float[] boardBefore;
        // public float[] boardAfter;
        // public int[] board;        

        // public float[] action;
        // public int thisPlayerPosition;
        // public string[] lastActionPlayers;
        // public string[] lastActionTypes;

        // public int[] RemainingCardsPerPlayer;


        // public string[] currentRoles;
        // public int currentPlayer;

        // public string[] possibleActionsDecoded;
    }


    [System.Serializable]
    public class MatchStartedMessage : RoomMessage
    {
        public string type;
        public int[] cards;
        public string[] players;
        public string starting_player;
               
    }

    [System.Serializable]
    public class RequestActionMessage : RoomMessage
    {
        public string type;
        public float[] observations;
        public string[] possibleActionsDecoded;        
    }

    [System.Serializable]
    public class UpdateOthersMessage : StandardInfo
    {
        

    }

    [System.Serializable]
    public class MatchOver : StandardInfo
    {       

    }

    [System.Serializable]
    public class DoSpecialAction : RoomMessage
    {       
        public string special_action;
        public string type;
    }

    [System.Serializable]
    public class SpecialActionUpdate : RoomMessage
    {       
        public string special_action;
        public int player;
        public string type;
    }

    public class ExchangeCards : RoomMessage
    {       
        public int[] cards;
        public int amount;
        public string type;
    }

        public class UpdateExchangeCards : RoomMessage
    {       
        public int[] cards_sent;
        public int[] cards_received;        
        public string type;
    }

    [System.Serializable]
    public class GameOver : RoomMessage
    {
        public string type;
    }

    [System.Serializable]
    public class RoomResponse
    {
        public string type;
        public string message;

    }



    /// <summary>
    /// Messages to be sent to the room
    /// </summary>

    [System.Serializable]
    public class ConnectRoomMessage : RoomMessage
    {
        public string playerName;
        public string password;
        public string author;

    }

    [System.Serializable]
    public class DiscardAction : RoomMessage
    {
        public int[] agent_action;       

    }

    [System.Serializable]
    public class SpecialAction : RoomMessage
    {
        public bool agent_action;       

    }

    [System.Serializable]
    public class CardsExchanged : RoomMessage
    {
        public int[] agent_action;       

    }

}