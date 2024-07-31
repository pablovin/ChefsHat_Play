using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunicationProtocol;
using TMPro;
using UnityEngine;
using System.Threading;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine.UI;
using LoadLocalResources;
using UnityEngine.UIElements;
using UnityEngine.U2D;
using static Unity.Burst.Intrinsics.X86.Avx;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;
using System;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerGameState
{
    public string name;

    public bool passed;

    public int cardsAtHand;
    public List<int> cards;

    public int score; 
    public int position;
    public string role;
    
    public List<string> possibleActions;
    public List<int> possibleActionIndices;

}

public class Game_handler : MonoBehaviour
{
    
    PlayerObject player;
    List<string> originalPlayersOrders;

    List<PlayerGameState> playersGameState;

    // Dictionary<string,int> score;
    // Start is called before the first frame update

    //Game State
    int playedMatches;
    int donePizzas;
    int currentMatch;

    //Message Panel
    [SerializeField] GameObject messagePanel;
    [SerializeField] TMP_Text messageText;
    [SerializeField] GameObject btn_back;


    //Score Panel 
    [SerializeField] GameObject logsPanel;
    [SerializeField] TMP_Text logsTxt;


    //Score Panel 
    [SerializeField] GameObject scorePanel;

    [SerializeField] TMP_Text scoreTxtPosition1Score;
    [SerializeField] TMP_Text scoreTxtPosition1Name;
    [SerializeField] TMP_Text scoreTxtPosition2Score;
    [SerializeField] TMP_Text scoreTxtPosition2Name;
    [SerializeField] TMP_Text scoreTxtPosition3Score;
    [SerializeField] TMP_Text scoreTxtPosition3Name;
    [SerializeField] TMP_Text scoreTxtPosition4Score;
    [SerializeField] TMP_Text scoreTxtPosition4Name;

    [SerializeField] TMP_Text scoreTxtNumbermatches;
    [SerializeField] TMP_Text scoreTxtCurrentMatch;
    [SerializeField] TMP_Text scoreTxtCurrentPizzas;


    //Players Panels
    [SerializeField] TMP_Text playerTxtPlayer1Name;
    [SerializeField] TMP_Text playerTxtPlayer1Cards;

    [SerializeField] GameObject btn_skip;
    [SerializeField] GameObject playerimgPlayer1Card1;
    [SerializeField] GameObject playerimgPlayer1Card2;
    [SerializeField] GameObject playerimgPlayer1Card3;
    [SerializeField] GameObject playerimgPlayer1Card4;
    [SerializeField] GameObject playerimgPlayer1Card5;
    [SerializeField] GameObject playerimgPlayer1Card6;
    List<GameObject> player1CardsBoard;

    [SerializeField] TMP_Text playerTxtPlayer2Name;
    [SerializeField] TMP_Text playerTxtPlayer2Cards;
    [SerializeField] GameObject playerimgPlayer2Card1;
    [SerializeField] GameObject playerimgPlayer2Card2;
    [SerializeField] GameObject playerimgPlayer2Card3;
    [SerializeField] GameObject playerimgPlayer2Card4;
    [SerializeField] GameObject playerimgPlayer2Card5;
    [SerializeField] GameObject playerimgPlayer2Card6;
    List<GameObject> player2CardsBoard;

    [SerializeField] TMP_Text playerTxtPlayer3Name;
    [SerializeField] TMP_Text playerTxtPlayer3Cards;
    [SerializeField] GameObject playerimgPlayer3Card1;
    [SerializeField] GameObject playerimgPlayer3Card2;
    [SerializeField] GameObject playerimgPlayer3Card3;
    [SerializeField] GameObject playerimgPlayer3Card4;
    [SerializeField] GameObject playerimgPlayer3Card5;
    [SerializeField] GameObject playerimgPlayer3Card6;
    List<GameObject> player3CardsBoard;

    [SerializeField] TMP_Text playerTxtPlayer4Name;
    [SerializeField] TMP_Text playerTxtPlayer4Cards;
    [SerializeField] GameObject playerimgPlayer4Card1;
    [SerializeField] GameObject playerimgPlayer4Card2;
    [SerializeField] GameObject playerimgPlayer4Card3;
    [SerializeField] GameObject playerimgPlayer4Card4;
    [SerializeField] GameObject playerimgPlayer4Card5;
    [SerializeField] GameObject playerimgPlayer4Card6;
    List<GameObject> player4CardsBoard;

    //Upper Panel 
    [SerializeField] TMP_Text upperTxtInformation;
    [SerializeField] GameObject upperPanel;


    //Board Panel    
    [SerializeField] GameObject cardBoardPosition1;
    [SerializeField] GameObject cardBoardPosition2;
    [SerializeField] GameObject cardBoardPosition3;
    [SerializeField] GameObject cardBoardPosition4;
    [SerializeField] GameObject cardBoardPosition5;
    [SerializeField] GameObject cardBoardPosition6;
    [SerializeField] GameObject cardBoardPosition7;
    [SerializeField] GameObject cardBoardPosition8;
    [SerializeField] GameObject cardBoardPosition9;
    [SerializeField] GameObject cardBoardPosition10;
    [SerializeField] GameObject cardBoardPosition11;

    [SerializeField] GameObject cardBoardPassPosition1;
    [SerializeField] GameObject cardBoardPassPosition2;
    [SerializeField] GameObject cardBoardPassPosition3;
    [SerializeField] GameObject cardBoardPassPosition4;

    [SerializeField] GameObject cardBoardRolePosition1;
    [SerializeField] GameObject cardBoardRolePosition2;
    [SerializeField] GameObject cardBoardRolePosition3;
    [SerializeField] GameObject cardBoardRolePosition4;

    List<GameObject> spritesCardsInBoard;


    // Cards Panel

    [SerializeField] UnityEngine.UI.Button cardsDiscardButton;
    [SerializeField] GameObject cardsPanel;

    [SerializeField] GameObject cardPass;
    [SerializeField] GameObject cardPosition1;
    [SerializeField] GameObject cardPosition2;
    [SerializeField] GameObject cardPosition3;
    [SerializeField] GameObject cardPosition4;
    [SerializeField] GameObject cardPosition5;
    [SerializeField] GameObject cardPosition6;
    [SerializeField] GameObject cardPosition7;
    [SerializeField] GameObject cardPosition8;
    [SerializeField] GameObject cardPosition9;
    [SerializeField] GameObject cardPosition10;
    [SerializeField] GameObject cardPosition11;
    [SerializeField] GameObject cardPosition12;
    [SerializeField] GameObject cardPosition13;
    [SerializeField] GameObject cardPosition14;
    [SerializeField] GameObject cardPosition15;
    [SerializeField] GameObject cardPosition16;
    [SerializeField] GameObject cardPosition17;

    List<GameObject> spritesCardsInHand;
    


    //Player Interaction Functions

    public void openCloseScore(bool open = false)
    {
        if (!open) { 
            scorePanel.SetActive(!scorePanel.activeSelf);
        }
        else
        {
            scorePanel.SetActive(open);
        }
    }

    public void openCloseLogs()
    {
        logsPanel.SetActive(!logsPanel.activeSelf);
    }

    public void DiscardCards()
    {
        Debug.Log("[Game Handler] Discard button pressed!");

        Dictionary<string, int> selectedCardsDic = new Dictionary<string,int>();


        foreach (GameObject card in spritesCardsInHand)
        {
            if (card.GetComponent<element_selected>().CardSelected)
            {
                string name = card.GetComponent<UnityEngine.UI.Image>().sprite.name.Split("@")[0].Split("_")[1];
                
                Debug.Log("[Game Handler] Selected Card" + name);

                if (selectedCardsDic.ContainsKey(name))
                {
                    selectedCardsDic[name] += 1;

                    Debug.Log("[Game Handler] Selected cards exists in the dic. Updated value: " + selectedCardsDic[name]);
                }
                else
                {
                    selectedCardsDic.Add(name, 1);
                    Debug.Log("[Game Handler] Creating selected card in the dic! ");
                }

                Debug.Log("[Game Handler] Updating Dic Selected cards: " + name);
            }

            Debug.Log("[Game Handler] Dictionary: " + String.Join(", ", selectedCardsDic.Keys));
        }

        if (cardPass.GetComponent<element_selected>().CardSelected)
        {
            selectedCardsDic.Add("Pass", 1);
        }

        Debug.Log("[Game Handler] Selected cards calculated! Total Types of selected cards: " + selectedCardsDic.Keys.Count);
        

        if (selectedCardsDic.Keys.Count > 0) {

            List<string> selectedCards = new List<string>();
            int numberJokers = 0;

            if (selectedCardsDic.ContainsKey("Joker"))
            {
                numberJokers = selectedCardsDic["Joker"];
            }

            Debug.Log("[Game Handler] Number of Jokers selected: " + numberJokers);

            foreach (string key in selectedCardsDic.Keys)
            {
                if (key == "Pass")
                {
                    selectedCards.Add("pass");
                }
                else if (key != "Joker")
                {
                    selectedCards.Add("C" + key + ";Q" + selectedCardsDic[key] + ";J" + numberJokers);
                }
            }

            Debug.Log("[Game Handler] Selected actions: " + selectedCards.ToString());

            bool validAction = ValidateDiscard(selectedCards);

            Debug.Log("[Game Handler] Is the Selected action valid: " + validAction);


            if (validAction)
            {

                string actionTaken = selectedCards[0];
                int indiceActionTaken = playersGameState[0].possibleActions.IndexOf(actionTaken);
                int indicePossibleAction = playersGameState[0].possibleActionIndices[indiceActionTaken];

                int[] action = Enumerable.Repeat(0, 200).ToArray();
                action[indicePossibleAction] = 1;
                player.SendDiscardAction(action);

                cardsPanel.SetActive(false);

                //Send response to the server
            }
            else
            {
                StartCoroutine(SendMessageUpperPanel("Invalid Action!"));
                
                //send error message
            }

        }
        else
        {
            StartCoroutine(SendMessageUpperPanel("Select cards!"));
        }
                    
       

    }

    public bool ValidateDiscard(List<string> selectedCards)
    {
        bool validAction = true;

        Debug.Log("Amount of possible actions selected: " + selectedCards.Count);
        Debug.Log("Selected actions: "+String.Join(", ", selectedCards));

        //Check if there are more than one type of cards selected
        if (selectedCards.Count > 1)
        {
            validAction= false;
        }
        else {

            

            //Check if the selected discard action is allowed
            List<string> possibleActions = playersGameState[0].possibleActions;

            Debug.Log("Pssible actions: " + String.Join(", ", possibleActions));

            bool actionPresent = false;
            foreach (string action in selectedCards) {
        
                if (possibleActions.Contains(action))
                {
                    actionPresent = true;
                    break;
                }
            
            }

            validAction = actionPresent;
        }


        return validAction;

    }

    /// Screen Update Functions


    private void updateLog(string author, string message)
    {
        if (logsTxt.text.Count() > 500)
        {
            logsTxt.text = logsTxt.text[message.Count()..logsTxt.text.Count()];
        }
        logsTxt.text = logsTxt.text+"\n"+"["+author+"]"+" "+message;
    }
    IEnumerator SendMessageUpperPanel(string message)
    {

        Debug.Log("[Game Handler] Sending a message!");

        upperPanel.SetActive(true);
        upperTxtInformation.text = message;
        yield return new WaitForSeconds(3);
        upperPanel.SetActive(false);

    }

    private void UpdateScoreBoard()
    {   
        
        //Update the score board values based on the current game state
        scoreTxtNumbermatches.text = playedMatches + " Matches";
        scoreTxtCurrentMatch.text = "Match: " + currentMatch.ToString();
        scoreTxtCurrentPizzas.text = "Pizzas: "+donePizzas.ToString();

        //Updating the score board
        List<PlayerGameState> sortedPlayers = playersGameState.OrderBy(o=>o.score).ToList();

        scoreTxtPosition1Score.text=sortedPlayers.ElementAt(0).score.ToString();
        scoreTxtPosition1Name.text=sortedPlayers.ElementAt(0).name;

        scoreTxtPosition2Score.text=sortedPlayers.ElementAt(1).score.ToString();
        scoreTxtPosition2Name.text=sortedPlayers.ElementAt(1).name;

        scoreTxtPosition3Score.text=sortedPlayers.ElementAt(2).score.ToString();
        scoreTxtPosition3Name.text=sortedPlayers.ElementAt(2).name;

        scoreTxtPosition4Score.text=sortedPlayers.ElementAt(3).score.ToString();
        scoreTxtPosition4Name.text=sortedPlayers.ElementAt(3).name;

        print ("[Game Handler] Score board updated!");        
    }


    private void updateBoard(int[] cardsInBoard)
    {

        var positiveCardsInBoard = cardsInBoard.Where(n => n > 0);

        foreach (GameObject cardPosition in spritesCardsInBoard)
        {
            cardPosition.SetActive(false);
        }

        if (positiveCardsInBoard.Count()>0 && positiveCardsInBoard.ElementAt(0)!=13)
        { 
            for (int i = 0; i < positiveCardsInBoard.Count(); i++)
            {
                GameObject thisCardPosition = spritesCardsInBoard[i];
                int thisCardValue = cardsInBoard[i];

                thisCardPosition.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetSprite(thisCardValue);

                thisCardPosition.SetActive(true);

            }
        }

        print("[Game Handler] Game Board updated!");

    }
    
    private void setActivePlayer(int activePlayer)
    {

        //Iterate over all positions, making them transparent. If the active player is that position, does not make it transparent.

        //Position 1
        if (activePlayer == playersGameState[0].position)
        {
            playerTxtPlayer1Name.color = new Color(255, 218, 0, 255);            
        }
        else
        {
            playerTxtPlayer1Name.color = new Color(255, 255, 255, 255);
        }
        
        
        
        //foreach (GameObject cardPosition in player1CardsBoard)
        //{
        //    Color tmp = cardPosition.GetComponent<UnityEngine.UI.Image>().color;
        //    if (activePlayer == playersGameState[0].position)
        //    {
        //        tmp.a = 1;
        //    }
        //    else { tmp.a = 0.2f; }

        //    cardPosition.GetComponent<UnityEngine.UI.Image>().color = tmp;

        //}

        ////Position 2
        if (activePlayer == playersGameState[1].position)
        {
            playerTxtPlayer2Name.color = new Color(255, 218, 0, 255);
        }
        else
        {
            playerTxtPlayer2Name.color = new Color(255, 255, 255, 255);
        }

        //foreach (GameObject cardPosition in player2CardsBoard)
        //{
        //    Color tmp = cardPosition.GetComponent<UnityEngine.UI.Image>().color;
        //    if (activePlayer == playersGameState[0].position)
        //    {
        //        tmp.a = 1;
        //    }
        //    else { tmp.a = 0.2f; }

        //    cardPosition.GetComponent<UnityEngine.UI.Image>().color = tmp;
        //}


        //Position 3
        if (activePlayer == playersGameState[2].position)
        {
            playerTxtPlayer3Name.color = new Color(255, 218, 0, 255);
        }
        else
        {
            playerTxtPlayer3Name.color = new Color(255, 255, 255, 255);
        }

        //foreach (GameObject cardPosition in player3CardsBoard)
        //{
        //    Color tmp = cardPosition.GetComponent<UnityEngine.UI.Image>().color;
        //    if (activePlayer == playersGameState[0].position)
        //    {
        //        tmp.a = 1;
        //    }
        //    else { tmp.a = 0.2f; }

        //    cardPosition.GetComponent<UnityEngine.UI.Image>().color = tmp;
        //}


        //Position 4
        if (activePlayer == playersGameState[3].position)
        {
            playerTxtPlayer4Name.color = new Color(255, 218, 0, 255);
        }
        else
        {
            playerTxtPlayer4Name.color = new Color(255, 255, 255, 255);
        }

        //foreach (GameObject cardPosition in player4CardsBoard)
        //{
        //    Color tmp = cardPosition.GetComponent<UnityEngine.UI.Image>().color;
        //    if (activePlayer == playersGameState[0].position)
        //    {
        //        tmp.a = 1;
        //    }
        //    else { tmp.a = 0.2f; }

        //    cardPosition.GetComponent<UnityEngine.UI.Image>().color = tmp;
        //}
        print("[Game Handler] Active Player set to:" + activePlayer);
    }

    private void updatePlayersInformation()
    {
        //Update the information on the players fields

        playerTxtPlayer1Name.text = playersGameState[0].name;
        playerTxtPlayer1Cards.text = "Ingredients: " + playersGameState[0].cardsAtHand.ToString();

        bool activePassPlayer1 = false;
        if (playersGameState[0].passed) activePassPlayer1 = true;        
        cardBoardPassPosition1.SetActive(activePassPlayer1);


        playerTxtPlayer2Name.text = playersGameState[1].name;
        playerTxtPlayer2Cards.text = "Ingredients: " + playersGameState[1].cardsAtHand.ToString();
        bool activePassPlayer2 = false;
        if (playersGameState[1].passed) activePassPlayer2 = true;
        cardBoardPassPosition2.SetActive(activePassPlayer2);


        playerTxtPlayer3Name.text = playersGameState[2].name;
         playerTxtPlayer3Cards.text = "Ingredients: " + playersGameState[2].cardsAtHand.ToString();
        bool activePassPlayer3 = false;
        if (playersGameState[2].passed) activePassPlayer3 = true;
        cardBoardPassPosition3.SetActive(activePassPlayer3);

        playerTxtPlayer4Name.text = playersGameState[3].name;
        playerTxtPlayer4Cards.text = "Ingredients: " + playersGameState[3].cardsAtHand.ToString();

        bool activePassPlayer4 = false;
        if (playersGameState[3].passed) activePassPlayer4 = true;
        cardBoardPassPosition4.SetActive(activePassPlayer4);

        print ("[Game Handler] Player info updated!");        

    }
    

    private void updateCardsInHand()
    {
       
        foreach (GameObject cardPosition in spritesCardsInHand)
        {            
            Color tmp = cardPosition.GetComponent<UnityEngine.UI.Image>().color;
            tmp.a = 0.5f;
            cardPosition.GetComponent<UnityEngine.UI.Image>().color = tmp;

            cardPosition.SetActive(false);
            cardPosition.GetComponent<element_selected>().CardSelected = false;
        }


        Color tmpPass = cardPass.GetComponent<UnityEngine.UI.Image>().color;
        tmpPass.a = 0.5f;
        cardPass.GetComponent<UnityEngine.UI.Image>().color = tmpPass;
        cardPass.GetComponent<element_selected>().CardSelected = false;

        List<int> cardsInHand = playersGameState[0].cards;

        var positiveCardsInBoard = cardsInHand.Where(n => n > 0);
        

        for (int i = 0; i < positiveCardsInBoard.Count(); i++)
        {
            GameObject thisCardPosition = spritesCardsInHand[i];
            int thisCardValue = positiveCardsInBoard.ElementAt(i);

            thisCardPosition.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetSprite(thisCardValue);

            thisCardPosition.SetActive(true);
            

        }

        print ("[Game Handler] Cards at hand updated...");      
         
    }


    IEnumerator OpenCardsPanel(bool discardButton=false)
    {

        updateCardsInHand();
        cardsPanel.SetActive(true);

        if (discardButton)
        {
            cardsDiscardButton.gameObject.SetActive(true);
        }
        else{
            cardsDiscardButton.gameObject.SetActive(false);
        }
        
        print ("[Game Handler] Cards panel opened...");              

        yield return StartCoroutine(SendMessageUpperPanel("Your turn!"));  
        
    }

    /// Player Object Handling
    IEnumerator ProcessMessage(RoomMessage message) 
        {   
            
            if (message is MatchStartedMessage)
            {
                MatchStartedMessage messageStartMessage = (MatchStartedMessage)message;

                print ("[Game Handler] Processing the match started message...");
                
                messageText.SetText("Everyone connected! Starting the Game...");            

                yield return new WaitForSeconds(2);   

                messagePanel.SetActive(false);

                print("[Game Handler] Starting the game with: "+ messageStartMessage.starting_player);

                yield return StartCoroutine(StartGame(messageStartMessage.players, messageStartMessage.cards, messageStartMessage.starting_player));

                int activePlayer = 0;
                foreach(PlayerGameState player in playersGameState)
                {
                    if (player.name == messageStartMessage.starting_player)
                        {
                    activePlayer = player.position;
                    break;
                        }
                }
                setActivePlayer(activePlayer);
                yield return new WaitForSeconds(2);

        }
            else if (message is UpdateOthersMessage)
            {
                UpdateOthersMessage messageUpdateOthers = (UpdateOthersMessage)message;

                print("[Game Handler] Processing the update others message...");

                string playerName = originalPlayersOrders[messageUpdateOthers.thisPlayer];
                string nextPlayer = originalPlayersOrders[messageUpdateOthers.currentPlayer];

                int playerIndex = playersGameState.FindIndex(p => p.name == playerName);
                int nextPlayerIndex = playersGameState.FindIndex(p => p.name == nextPlayer);

                int[] scores = messageUpdateOthers.score;
                                
            //print("[Game Handler] Action size: "+ messageUpdateOthers.action.Count());

                bool isPass = messageUpdateOthers.action[199] == 1;

                playersGameState[playerIndex].passed = isPass;

                playersGameState[playerIndex].cardsAtHand = messageUpdateOthers.RemainingCardsPerPlayer[messageUpdateOthers.thisPlayer];

                //Update user action!
                string userAction = messageUpdateOthers.lastActionTypes[messageUpdateOthers.thisPlayer];

                
                
                updateBoard(messageUpdateOthers.board);
                yield return StartCoroutine(SendMessageUpperPanel(playerName + " did "+ userAction));
                updateLog(playerName, "Did "+ userAction);

                yield return new WaitForSeconds(1);

                if (playerName == player.Name)
                {
                    btn_skip.SetActive(true);                    
                }
                
                if (messageUpdateOthers.isPizzaReady)
                {
                    donePizzas = donePizzas+1;
                    foreach (PlayerGameState playerGameState in playersGameState) {
                        playerGameState.passed = false;                        
                    }

                    yield return StartCoroutine(SendMessageUpperPanel(nextPlayer + " Declared Pizza!"));
                    updateLog(nextPlayer, "I Made a Pizza!");               
                                
                }

                UpdateScoreBoard();
                updatePlayersInformation();           
                setActivePlayer(nextPlayerIndex);
                yield return StartCoroutine(SendMessageUpperPanel("Next Player: " + nextPlayer));
                updateLog("System", "Next player: " + nextPlayer);
                yield return new WaitForSeconds(2);

        }
        else if (message is RequestActionMessage)
            {
                 RequestActionMessage messageRequestAction = (RequestActionMessage)message;
                 print ("[Game Handler] Processing the request action message...");
                                
                 int[] cardsInHand = messageRequestAction.observations[11..28].Select(x => (int)(x * 13)).ToArray();
                 int[] possibleActionsVector = messageRequestAction.observations[28..228].Select(f => (int)f).ToArray(); ;
                 string[] possibleActions = messageRequestAction.possibleActionsDecoded;

                 var possibleActionIndices = Enumerable.Range(0, possibleActionsVector.Length)
                                .Where(i => possibleActionsVector[i] == 1)
                                .ToList();

                playersGameState[0].cards = cardsInHand.ToList<int>();
                playersGameState[0].cardsAtHand = cardsInHand.Count();
                playersGameState[0].possibleActions = possibleActions.ToList<string>();
            playersGameState[0].possibleActionIndices = possibleActionIndices;

            yield return StartCoroutine(OpenCardsPanel(true));                

        }        
        else if (message is MatchOver)
        {
            MatchOver messageMatchOver = (MatchOver)message;
            print("[Game Handler] Processing the request action message...");

            updateLog("[System]", "Match over!");
            yield return StartCoroutine(SendMessageUpperPanel("Match Over!"));
            yield return new WaitForSeconds(1);
            openCloseScore(true);
            yield return new WaitForSeconds(2);

            int[] scores = messageMatchOver.score;
            string[] roles = messageMatchOver.currentRoles;
            playedMatches = messageMatchOver.matches;
            currentMatch = playedMatches+1;

            // foreach (int score in scores) 
            // {
            //     print("Score: " + score);
            // }

            // foreach (string role in roles)
            // {
            //     print("Role: " + role);
            // }
            

            string readingMessage = player.PlayerStatus;
            UpdateScore(scores);
            UpdateScoreBoard();

            if(readingMessage != PlayerObject.PLAYER_STATUS["gameOver"])
            {
                StartMatch(scores, roles);
                yield return new WaitForSeconds(2);

                openCloseScore(false);
                yield return StartCoroutine(SendMessageUpperPanel("Starting Match Number "+ currentMatch));
                
                updateLog("[System]", "New match!");          
            }

        }
        else if (message is GameOver)
        {

            Debug.Log("[Game Handler] Game Over...");            
            updateLog("[System]", "Game over!");

            messageText.SetText("End of the Game!");
            messagePanel.SetActive(true);        

            yield return new WaitForSeconds(2);            

            btn_back.SetActive(true);

            yield return new WaitForSeconds(2);            
            player.PlayerStatus = PlayerObject.PLAYER_STATUS["gameFinished"];



            // MatchOver messageMatchOver = (MatchOver)message;
            // print("[Game Handler] Processing the request action message...");

            // updateLog("[System]", "Game over!");

            // messageText.SetText("End of the Game!");
            // messagePanel.SetActive(true);
            // openCloseScore(true);

            // yield return new WaitForSeconds(2);            

            // btn_back.SetActive(true);

        }

    }
        
        IEnumerator ReadPlayerQ()
    {        
        string readingMessage = player.PlayerStatus;
        while(readingMessage != PlayerObject.PLAYER_STATUS["gameFinished"])
        {
            //   print ("[Game Handler] Current Player`s Message Q: "+player.MessagesQ.Count);
              if (player.MessagesQ.Count > 0)
                {
                    var currentMessage = player.MessagesQ.Dequeue();

                    print ("[Game Handler] Message found: "+currentMessage);
                                        
                    yield return StartCoroutine(ProcessMessage(currentMessage));                
                }        

                yield return new WaitForSeconds(1);    
        }
        
               
    }


    //Game State Functions

    public static List<T> ReorderList<T>(List<T> originalList, T startingItem)
    {
       
        int startIndex = originalList.IndexOf(startingItem);
        List<T> orderedList = new List<T>();

        // Add from the starting item to the end of the list
        for (int i = startIndex; i < originalList.Count; i++)
        {
            orderedList.Add(originalList[i]);
        }

        // Add from the beginning of the list to the starting item
        for (int i = 0; i < startIndex; i++)
        {
            orderedList.Add(originalList[i]);
        }

        return orderedList;
    }

    public void UpdateScore(int[] scores)
    {
        //Update the Gamestates
        for (int indexServer = 0; indexServer < scores.Count(); indexServer++)
        {
            string thisPlayerName = originalPlayersOrders[indexServer];

            int playerIndex = playersGameState.FindIndex(p => p.name == thisPlayerName);            
            playersGameState[playerIndex].score = scores[indexServer];                        

        }

    }

    public void UpdateRoles(string[] roles)
    {
          //Update the Gamestates
        for (int indexServer = 0; indexServer < roles.Count(); indexServer++)
        {
            string thisPlayerName = originalPlayersOrders[indexServer];

            int playerIndex = playersGameState.FindIndex(p => p.name == thisPlayerName);
            if (roles.Count()>0) { playersGameState[playerIndex].role = roles[indexServer]; }
                                    

        }
    }
    
    public void StartMatch(int[] scores, string[] roles)
    {

        print("[Game Handler] Starting the match! Updating all the gamestates...");

        donePizzas = 0;
        currentMatch = 1;
        
        UpdateScore(scores);
        UpdateRoles(roles);
        //Update the Gamestates
        for (int indexServer = 0; indexServer < scores.Count(); indexServer++)
        {
            string thisPlayerName = originalPlayersOrders[indexServer];

            int playerIndex = playersGameState.FindIndex(p => p.name == thisPlayerName);          
            playersGameState[playerIndex].cardsAtHand = 17;

        }

        //Update the Scores
        UpdateScoreBoard();



    }


    IEnumerator StartGame(string[] playerNames, int[] cardsInHand, string startingPlayer)
    {

         print ("[Game Handler] Starting the game! Reseting all the internal states...");


        //Reset all internal states

        playedMatches = 0;

        originalPlayersOrders = playerNames.ToList<string>();

        playersGameState = new List<PlayerGameState>();

        List<string> playerNamesOrdered =  ReorderList(playerNames.ToList<string>(), player.Name);
        

        for (int i=0; i<playerNamesOrdered.Count();i++ )
        {
            print("Player position+ " + i +" - Name: " + playerNamesOrdered.ElementAt(i));
            PlayerGameState thisPlayer = new PlayerGameState(){

            name = playerNamesOrdered.ElementAt(i),
            cardsAtHand = 17,
            score = 0, 
            position = i,
            role=""                
            };

            if (i==0)
            {
                thisPlayer.cards = cardsInHand.ToList();
            }
            playersGameState.Add(thisPlayer);            
        }

        StartMatch(new int[] { 0, 0, 0, 0 }, new string[] { "", "", "", "" });

        UpdateScoreBoard();
        updatePlayersInformation();

        updateLog("System", "Game Started!");
        yield return StartCoroutine(SendMessageUpperPanel("Game Started! "+ startingPlayer+" Starts playing!"));         

    }


    // Scene Management functions

    IEnumerator Start()
    {

        upperPanel.SetActive(false);
        cardsPanel.SetActive(false);
        scorePanel.SetActive(false);
        logsPanel.SetActive(false);
        btn_back.SetActive(false);
        btn_skip.SetActive(false);



        //Add all the positions of the cards at hand to the list
        spritesCardsInHand = new List<GameObject>
        {
            cardPosition1,
            cardPosition2,
            cardPosition3,
            cardPosition4,
            cardPosition5,
            cardPosition6,
            cardPosition7,
            cardPosition8,
            cardPosition9,
            cardPosition10,
            cardPosition11,
            cardPosition12,
            cardPosition13,
            cardPosition14,
            cardPosition15,
            cardPosition16,
            cardPosition17
        };        

        //Add all the cards to the board and then hide them
        spritesCardsInBoard = new List<GameObject>
        {
            cardBoardPosition1,
            cardBoardPosition2,
            cardBoardPosition3,
            cardBoardPosition4,
            cardBoardPosition5,
            cardBoardPosition6,
            cardBoardPosition7,
            cardBoardPosition8,
            cardBoardPosition9,
            cardBoardPosition10,
            cardBoardPosition11,
        };

        foreach (GameObject cardBoard in spritesCardsInBoard)
        {
            cardBoard.SetActive(false);
        }


        //Initialize elements of the player1 card board
        player1CardsBoard = new List<GameObject>
        {
        playerimgPlayer1Card1, playerimgPlayer1Card2, playerimgPlayer1Card3, playerimgPlayer1Card4, playerimgPlayer1Card5, playerimgPlayer1Card6,
        };

         player2CardsBoard = new List<GameObject>
        {
        playerimgPlayer2Card1, playerimgPlayer2Card2, playerimgPlayer2Card3, playerimgPlayer2Card4, playerimgPlayer2Card5, playerimgPlayer2Card6,
        };

        player3CardsBoard = new List<GameObject>
        {
        playerimgPlayer3Card1, playerimgPlayer3Card2, playerimgPlayer3Card3, playerimgPlayer3Card4, playerimgPlayer3Card5, playerimgPlayer3Card6,
        };

        player4CardsBoard = new List<GameObject>
        {
        playerimgPlayer4Card1, playerimgPlayer4Card2, playerimgPlayer4Card3, playerimgPlayer4Card4, playerimgPlayer4Card5, playerimgPlayer4Card6,
        };

        cardBoardPassPosition1.SetActive(false);
        cardBoardPassPosition2.SetActive(false);
        cardBoardPassPosition3.SetActive(false);
        cardBoardPassPosition4.SetActive(false);

        cardBoardRolePosition1.SetActive(false);
        cardBoardRolePosition2.SetActive(false);
        cardBoardRolePosition3.SetActive(false);
        cardBoardRolePosition4.SetActive(false);

        GameObject music_menu = GameObject.Find("music_menu");
        Destroy(music_menu);

        player = PlayerObject.player;

        messageText.SetText("Waiting for other players to connect...");
        updateLog("System", "Waiting for players...");

        yield return new WaitForSeconds(3f);

        StartCoroutine(ReadPlayerQ());

    }





    // Update is called once per frame
    void Update()
    {
                             
        
    }
}
