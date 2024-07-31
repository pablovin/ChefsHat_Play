using Unity.Burst;
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
        public bool actionIsRandom;
        public bool validAction;
        public int matches;
        public int rounds;
        public int[] score;
        public float[] performanceScore;
        public int thisPlayer;
        public bool thisPlayerFinished;
        public bool[] PlayersFinished;
        public bool isPizzaReady;

        public float[] boardBefore;
        public float[] boardAfter;
        public int[] board;
        public int[] possibleActions;

        public float[] action;
        public int thisPlayerPosition;
        public string[] lastActionPlayers;
        public string[] lastActionTypes;

        public int[] RemainingCardsPerPlayer;
        public string[] players;

        public string[] currentRoles;
        public int currentPlayer;

        public string[] possibleActionsDecoded;
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

    }

    [System.Serializable]
    public class DiscardAction : RoomMessage
    {
        public int[] agent_action;       

    }


}