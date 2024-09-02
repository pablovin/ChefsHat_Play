using System.Collections;
using System.Collections.Generic;
using CommunicationProtocol;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEditor;
using LoadLocalResources;
using System;
using System.Text.RegularExpressions;

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
    // public List<int> possibleActionIndices;

}

public class Game_handler : MonoBehaviour
{
    
    PlayerObject player;
    List<string> originalPlayersOrders;

    Dictionary<string, PlayerGameState> playersGameState;

    // Dictionary<string,int> score;
    // Start is called before the first frame update

    //Game State
    int playedMatches;
    int donePizzas;
    int currentMatch;


    //Audio
    [SerializeField] AudioSource audioStart;
    [SerializeField] AudioSource audioDealCard;

    [SerializeField] AudioSource audioPass;
    [SerializeField] AudioSource audioDiscard;
    [SerializeField] AudioSource audioMatchOver;
    [SerializeField] AudioSource audioPizzaReady;

    //Message Panel
    [SerializeField] GameObject messagePanel;
    [SerializeField] TMP_Text messageText;
    [SerializeField] TMP_Text messageTitle;
    [SerializeField] GameObject errorImage;
    [SerializeField] GameObject chefImage;
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

    //Lower Panel 
    [SerializeField] TMP_Text upperTxtInformation;
    [SerializeField] GameObject upperPanel;

    [SerializeField] TMP_Text nextPlayerText;
    [SerializeField] GameObject nextPlayerPanel;



    //Pizza Panel 
    [SerializeField] TMP_Text textPizzaPanel;
    [SerializeField] GameObject pizzaPanel;    


    //Handling Cards Panel    
    [SerializeField] TMP_Text handlingCardsText;   
    [SerializeField] GameObject handlingCardsPanel;   

    //Handling Cards Panel    
    [SerializeField] TMP_Text exchangeCardsTxt;   
    [SerializeField] GameObject exchangeCardsSent1;  
    [SerializeField] GameObject exchangeCardsSent2;  

    [SerializeField] GameObject exchangeCardsReceived1;  
    [SerializeField] GameObject exchangeCardsReceived2;      
    [SerializeField] GameObject exchangeCardsPanel;  

    //EndMatch Panel 
    [SerializeField] TMP_Text txtChef;
    [SerializeField] TMP_Text txtSouschef;
    [SerializeField] TMP_Text txtWaiter;
    [SerializeField] TMP_Text txtDishwasher;
    [SerializeField] GameObject endMatchPanel;    


    //SpecialAction Panel 
    [SerializeField] TMP_Text txtTitleSpecialAction;
    [SerializeField] TMP_Text txtDescriptionSpecialAction;
    [SerializeField] GameObject specialActionCard;
    [SerializeField] GameObject specialActionButtonYes;
    [SerializeField] GameObject specialActionButtonNo;
    [SerializeField] GameObject specialActionPanel;    

    //Board Panel    

    [SerializeField] GameObject imageActivePlayer1;

    [SerializeField] GameObject imageActivePlayer2;
    [SerializeField] GameObject imageActivePlayer3;
    [SerializeField] GameObject imageActivePlayer4;

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

    [SerializeField] GameObject cardsDiscardButton;
    [SerializeField] GameObject cardsSendCardsButton;
    [SerializeField] TMP_Text txtCardsPanel;
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

    public void DoSpecialActionButton()
    {
        player.SendSpecialAction(true);
    }

    public void DoNotSpecialActionButton()
    {
        player.SendSpecialAction(false);
    }


    public void SendExchangedCards()
    {
        Debug.Log("[Game Handler] Send Exchanged Cards button pressed!");       

        List<int> selectedCards = new List<int>();

        foreach (GameObject card in spritesCardsInHand)
        {
            
            if (card.GetComponent<element_selected>().CardSelected)
            {
                string name = card.GetComponent<UnityEngine.UI.Image>().sprite.name.Split("@")[0].Split("_")[1];

                if (name == "Joker")
                {
                    selectedCards.Add(12);
                }
                else{
                    selectedCards.Add(int.Parse(name));
                }
                
                Debug.Log("[Game Handler] Selected Card" + name);                
            }
        }        
        
        Regex regex = new Regex(@"\d+");
        Match match = regex.Match(txtCardsPanel.text);

        int amount = 0;
        if (match.Success)
        {
            // Convert the matched value to an integer
            amount = int.Parse(match.Value);
        }        

        Debug.Log("[Game Handler] Allowed cards: " + amount + ". Selected cards: "+selectedCards.Count);             

        if (selectedCards.Count == amount) {
                       
                player.SendExchangedCards(selectedCards.ToArray());
                cardsPanel.SetActive(false);                

        }
        else
        {
            StartCoroutine(SendMessageUpperPanel("Select "+amount+" card(s)!"));
        }
                    
       

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
                int indiceActionTaken = PlayerObject.GetHighLevelActions().IndexOf(actionTaken);

                // int indicePossibleAction = playersGameState[0].possibleActionIndices[indiceActionTaken];

                int[] action = Enumerable.Repeat(0, 200).ToArray();
                action[indiceActionTaken] = 1;
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
            List<string> possibleActions = playersGameState[player.Name].possibleActions;

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



    IEnumerator displaySpecialActionPanel(string title, string description, string specialAction, bool showButton)
    {
        specialActionPanel.SetActive(true);
        txtTitleSpecialAction.SetText(title);
        txtDescriptionSpecialAction.SetText(description);

        specialActionButtonYes.SetActive(false);
        specialActionButtonNo.SetActive(false);


        specialActionCard.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetSpecialActionCard(specialAction);

        if (showButton)
        {
            specialActionButtonYes.SetActive(true);
            specialActionButtonNo.SetActive(true);
        }
        else{
            yield return new WaitForSeconds(5);
            specialActionPanel.SetActive(false);
        }

        
        // audioPizzaReady.Play(0);
        // pizzaPanel.SetActive(true);        
        // textPizzaPanel.SetText(player+" Made a Pizza!");
        
        
        
    }


    /// Screen Update Functions
    IEnumerator displayPizzaPanel(string name)
    {
        audioPizzaReady.Play(0);
        pizzaPanel.SetActive(true);        
        textPizzaPanel.SetText(name+" Made a Pizza!");
        yield return new WaitForSeconds(3);
        pizzaPanel.SetActive(false);
        
    }

    IEnumerator displayHandlingCards(string text)
    {
        audioDealCard.Play(0);
        handlingCardsPanel.SetActive(true);  
        handlingCardsText.SetText(text);              
        yield return new WaitForSeconds(5);
        handlingCardsPanel.SetActive(false);
        
    }
    IEnumerator displayExchangeCard(string text, int[] cardsReceived, int[] cardsSent)
    {
        // audioDealCard.Play(0);
        exchangeCardsPanel.SetActive(true);  
        exchangeCardsTxt.SetText(text);   
        
        
        // [SerializeField] GameObject exchangeCardsSent1;  
        // [SerializeField] GameObject exchangeCardsSent2;  

        // [SerializeField] GameObject exchangeCardsReceived1;  
        // [SerializeField] GameObject exchangeCardsReceived2;   
    
        exchangeCardsSent1.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetSprite(cardsSent[0]);
        if (cardsReceived.Count()>1) exchangeCardsSent2.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetSprite(cardsSent[1]);

        exchangeCardsReceived1.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetSprite(cardsReceived[0]);
        if (cardsReceived.Count()>1) exchangeCardsReceived2.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetSprite(cardsReceived[1]);

        yield return new WaitForSeconds(5);
        exchangeCardsPanel.SetActive(false);
        
    }    
    IEnumerator displayEndMatchPanel(Dictionary<string,string> current_roles)
    {
        StartCoroutine(SendMessageUpperPanel("Match Over! Updating Score Board!"));
        updateLog("[System]", "Match over!");

        
        endMatchPanel.SetActive(true);        
        audioMatchOver.Play(0);
        txtChef.SetText(current_roles.FirstOrDefault(x => x.Value == "Chef").Key+" (+3 points)");
        txtSouschef.SetText(current_roles.FirstOrDefault(x => x.Value == "Souschef").Key+"(+2 points)");
        txtWaiter.SetText(current_roles.FirstOrDefault(x => x.Value == "Waiter").Key+"(+1 points)");
        txtDishwasher.SetText(current_roles.FirstOrDefault(x => x.Value == "Dishwasher").Key+"(+0 points)");
        yield return new WaitForSeconds(10);
        endMatchPanel.SetActive(false);
        
    }

    private void updateMessagePanel(string title, string text, bool show_error_image, bool show_button)
    {
        messagePanel.SetActive(true);
        messageTitle.SetText(title);
        messageText.SetText(text);
        chefImage.SetActive(!show_error_image);
        errorImage.SetActive(show_error_image);
        btn_back.SetActive(show_button);

    }


    private void updateLog(string author, string message)
    {
        if (logsTxt.text.Count() > 500)
        {
            logsTxt.text = logsTxt.text[message.Count()..logsTxt.text.Count()];
        }
        logsTxt.text = logsTxt.text+"\n"+"["+author+"]"+" "+message;
    }

     IEnumerator SendMessageNextPlayer(string playerName)
    {        

        nextPlayerPanel.SetActive(true);
        nextPlayerText.text = "Waiting for: "+playerName;
        yield return new WaitForSeconds(3);
        nextPlayerPanel.SetActive(false);

    }


    IEnumerator SendMessageUpperPanel(string message)
    {

        // Debug.Log("[Game Handler] Sending a message!");

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
        // List<PlayerGameState> sortedPlayers = playersGameState.OrderBy(o=>o.score).ToList();

        List<PlayerGameState> sortedPlayers = playersGameState
            .Values
            .OrderBy(player => player.score) // Order by score in ascending order
            .ToList();

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

    private void clearBoard()
    {
        foreach (GameObject cardPosition in spritesCardsInBoard)
        {
            cardPosition.SetActive(false);
        }
    }
    IEnumerator updateBoard(int[] cardsInBoard)
    {

        var positiveCardsInBoard = cardsInBoard.Where(n => n > 0);        
        
        string message = "";
        foreach(int card in positiveCardsInBoard)
        {
            message+=", "+card;            
        }

        Debug.Log("Card in board: " +message);
        
        clearBoard();

        if (positiveCardsInBoard.Count()>0 && positiveCardsInBoard.ElementAt(0)!=13)
        { 
            for (int i = 0; i < positiveCardsInBoard.Count(); i++)
            {
                GameObject thisCardPosition = spritesCardsInBoard[i];
                int thisCardValue = cardsInBoard[i];

                thisCardPosition.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetSprite(thisCardValue);

                thisCardPosition.SetActive(true);

                audioDiscard.Play(0);
                yield return new WaitForSeconds(0.5f);


            }        

        }

    }
    
    private void setActivePlayer(string activePlayer)
    {   

        StartCoroutine(SendMessageNextPlayer(playersGameState[activePlayer].name));   
        imageActivePlayer1.SetActive(false);
        imageActivePlayer2.SetActive(false);
        imageActivePlayer3.SetActive(false);
        imageActivePlayer4.SetActive(false);


        if (playersGameState[activePlayer].position==0) imageActivePlayer1.SetActive(true);
        if (playersGameState[activePlayer].position==1) imageActivePlayer2.SetActive(true);
        if (playersGameState[activePlayer].position==2) imageActivePlayer3.SetActive(true);
        if (playersGameState[activePlayer].position==3) imageActivePlayer4.SetActive(true);

        //Iterate over all positions, making them transparent. If the active player is that position, does not make it transparent.

        List<TMP_Text>  playerTxtNames = new List<TMP_Text>(){
            playerTxtPlayer1Name,
            playerTxtPlayer2Name,
            playerTxtPlayer3Name,
            playerTxtPlayer4Name
        };

        int count=0;
        foreach (string name in playersGameState.Keys)
        {
            if (activePlayer == name)
            {
                playerTxtNames[count].color = new Color(255, 218, 0, 255);
            }
            else{
                playerTxtNames[count].color = new Color(255, 255, 255, 255);
            }
            count+=1;
            
        }
        // //Position 1
        // if (activePlayer ==player.Name)
        // {
        //     playerTxtPlayer1Name.color = new Color(255, 218, 0, 255);            
        // }
        // else
        // {
        //     playerTxtPlayer1Name.color = new Color(255, 255, 255, 255);
        // }
        
        
                

        // ////Position 2
        // if (activePlayer == playersGameState[1].position)
        // {
        //     playerTxtPlayer2Name.color = new Color(255, 218, 0, 255);
        // }
        // else
        // {
        //     playerTxtPlayer2Name.color = new Color(255, 255, 255, 255);
        // }

        // //foreach (GameObject cardPosition in player2CardsBoard)
        // //{
        // //    Color tmp = cardPosition.GetComponent<UnityEngine.UI.Image>().color;
        // //    if (activePlayer == playersGameState[0].position)
        // //    {
        // //        tmp.a = 1;
        // //    }
        // //    else { tmp.a = 0.2f; }

        // //    cardPosition.GetComponent<UnityEngine.UI.Image>().color = tmp;
        // //}


        // //Position 3
        // if (activePlayer == playersGameState[2].position)
        // {
        //     playerTxtPlayer3Name.color = new Color(255, 218, 0, 255);
        // }
        // else
        // {
        //     playerTxtPlayer3Name.color = new Color(255, 255, 255, 255);
        // }

        // //foreach (GameObject cardPosition in player3CardsBoard)
        // //{
        // //    Color tmp = cardPosition.GetComponent<UnityEngine.UI.Image>().color;
        // //    if (activePlayer == playersGameState[0].position)
        // //    {
        // //        tmp.a = 1;
        // //    }
        // //    else { tmp.a = 0.2f; }

        // //    cardPosition.GetComponent<UnityEngine.UI.Image>().color = tmp;
        // //}


        // //Position 4
        // if (activePlayer == playersGameState[3].position)
        // {
        //     playerTxtPlayer4Name.color = new Color(255, 218, 0, 255);
        // }
        // else
        // {
        //     playerTxtPlayer4Name.color = new Color(255, 255, 255, 255);
        // }

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
        Debug.Log("[Game Handler] Active Player set to:" + activePlayer);

        
        
    }

    private void updatePlayersInformation()
    {
         
        //Update the information on the players fields
        List<TMP_Text>  playerTxtNames = new List<TMP_Text>(){
            playerTxtPlayer1Name,
            playerTxtPlayer2Name,
            playerTxtPlayer3Name,
            playerTxtPlayer4Name
        };

        List<TMP_Text>  playerTxtCards = new List<TMP_Text>(){
            playerTxtPlayer1Cards,
            playerTxtPlayer2Cards,
            playerTxtPlayer3Cards,
            playerTxtPlayer4Cards
        };

        List<GameObject>  playerCardBoardPass = new List<GameObject>(){
            cardBoardPassPosition1,
            cardBoardPassPosition2,
            cardBoardPassPosition3,
            cardBoardPassPosition4
        };        

        int count = 0;
        foreach(string name in playersGameState.Keys)
        {
            playerTxtNames[count].text = name;
            playerTxtCards[count].text = "Ingredients: " + playersGameState[name].cardsAtHand.ToString();
            
            playerCardBoardPass[count].SetActive( playersGameState[name].passed);
            Debug.Log("Card Pass "+name + ": "+playersGameState[name].passed);
            
            count+=1;             

        }

        // playerTxtPlayer1Name.text = playersGameState[0].name;
        // playerTxtPlayer1Cards.text = "Ingredients: " + playersGameState[0].cardsAtHand.ToString();
        // bool activePassPlayer1 = false;
        // if (playersGameState[0].passed) activePassPlayer1 = true;        
        // cardBoardPassPosition1.SetActive(activePassPlayer1);
        // Debug.Log("Card Pass player1: "+activePassPlayer1);

        // playerTxtPlayer2Name.text = playersGameState[1].name;
        // playerTxtPlayer2Cards.text = "Ingredients: " + playersGameState[1].cardsAtHand.ToString();
        // bool activePassPlayer2 = false;
        // if (playersGameState[1].passed) activePassPlayer2 = true;
        // cardBoardPassPosition2.SetActive(activePassPlayer2);
        // Debug.Log("Card Pass player2: "+activePassPlayer2);


        // playerTxtPlayer3Name.text = playersGameState[2].name;
        //  playerTxtPlayer3Cards.text = "Ingredients: " + playersGameState[2].cardsAtHand.ToString();
        // bool activePassPlayer3 = false;
        // if (playersGameState[2].passed) activePassPlayer3 = true;
        // cardBoardPassPosition3.SetActive(activePassPlayer3);
        // Debug.Log("Card Pass player3: "+activePassPlayer3);


        // playerTxtPlayer4Name.text = playersGameState[3].name;
        // playerTxtPlayer4Cards.text = "Ingredients: " + playersGameState[3].cardsAtHand.ToString();
        // bool activePassPlayer4 = false;
        // if (playersGameState[3].passed) activePassPlayer4 = true;
        // cardBoardPassPosition4.SetActive(activePassPlayer4);
        // Debug.Log("Card Pass player4: "+activePassPlayer4);

        Debug.Log("[Game Handler] Player info updated!");        

    }
    

    private void updateCardsInHand(bool show_pass)
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

        if (!show_pass)
        {
            cardPass.SetActive(false);
        }
        else{
            cardPass.SetActive(true);
        }

        List<int> cardsInHand = playersGameState[player.Name].cards;

        var positiveCardsInBoard = cardsInHand.Where(n => n > 0);
        

        for (int i = 0; i < positiveCardsInBoard.Count(); i++)
        {
            GameObject thisCardPosition = spritesCardsInHand[i];
            int thisCardValue = positiveCardsInBoard.ElementAt(i);

            thisCardPosition.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetSprite(thisCardValue);

            thisCardPosition.SetActive(true);            
        }

        Debug.Log("[Game Handler] Cards at hand updated...");      
         
    }


    IEnumerator OpenCardsPanel(bool show_pass, bool discardButton=false, bool sendCardsButton = false, string text="")
    {
        txtCardsPanel.SetText(text);
        cardsSendCardsButton.SetActive(false);
        cardsDiscardButton.SetActive(false);

        if (sendCardsButton)
        {
            cardsSendCardsButton.SetActive(true);            
        }
        
        if (discardButton)
        {
            cardsDiscardButton.SetActive(true);
        }

        updateCardsInHand(show_pass);

        cardsPanel.SetActive(true);

        

        
        Debug.Log("[Game Handler] Cards panel opened...");              

        yield return StartCoroutine(SendMessageUpperPanel("Your turn!"));  
        
    }

    /// Player Object Handling
    IEnumerator ProcessMessage(RoomMessage message) 
        {   
            
            if (message is MatchStartedMessage)
            {
                MatchStartedMessage messageStartMessage = (MatchStartedMessage)message;              
                Debug.Log("[Game Handler] Started Game Message Received! Players: "+ messageStartMessage.starting_player);

                updateMessagePanel("All set!", "All players connected, let`s start the game!", false, false);

                audioStart.Play(0);

                yield return new WaitForSeconds(2);
                // messageText.SetText("Everyone connected! Starting the Game...");                            
                messagePanel.SetActive(false);
                
                yield return StartCoroutine(StartGame(messageStartMessage.players, messageStartMessage.cards, messageStartMessage.starting_player));

                string activePlayer = "";
                foreach(string name in playersGameState.Keys)
                {
                    
                    if (playersGameState[name].name == messageStartMessage.starting_player)
                        {
                            activePlayer = playersGameState[name].name;
                    break;
                        }
                }
                setActivePlayer(activePlayer);
                yield return new WaitForSeconds(1);

        }
            else if (message is UpdateOthersMessage)
            {
                UpdateOthersMessage messageUpdateOthers = (UpdateOthersMessage)message;
                                
                string playerName = originalPlayersOrders[messageUpdateOthers.Author_Index];
                
                Debug.Log("[Game Handler] Processing Update Others message based on the action of: "+ playerName);

                string nextPlayerName = originalPlayersOrders[messageUpdateOthers.Next_Player];

                // int playerIndex = playersGameState.FindIndex(p => p.name == playerName);
                // int nextPlayerIndex = playersGameState.FindIndex(p => p.name == nextPlayer);

                // Dictionary<string,int> scores = messageUpdateOthers.Game_Score;
                                
                // print("[Game Handler] Action size: "+ messageUpdateOthers.action.Count());                
                //Update user action!
                
                string userAction = messageUpdateOthers.Action_Decoded;  
                bool isPass = userAction == "pass";
                // bool isPass = userAction == "PASS";                                

                // print("[Game Handler] Is Pass: "+ isPass);

                // foreach (var kvp in messageUpdateOthers.Cards_Per_Player)
                // {
                //     Debug.Log($"Player: {kvp.Key}, Cards Value: {kvp.Value}");
                // }
                playersGameState[playerName].passed = isPass;
                playersGameState[playerName].cardsAtHand = messageUpdateOthers.Cards_Per_Player[messageUpdateOthers.Author_Index];

                // foreach (int boardBefore in messageUpdateOthers.Board_After)
                // {
                //     Debug.Log("Board Before: " + boardBefore);
                // }
                // Debug.Log("---------------------");

                // foreach (int boardBefore in messageUpdateOthers.Board_Before)
                // {
                //     Debug.Log("Board After: " +boardBefore);
                // }
                
                Debug.Log("Action from: "+playerName+" - Is PAss: " + isPass);

                updatePlayersInformation();    
                
                if (isPass)
                {                 
                    audioPass.Play(0);                    
                    yield return new WaitForSeconds(0.5f);
                }                
                else
                {                                           
                    yield return updateBoard(messageUpdateOthers.Board_After);  
                }                
                
                yield return StartCoroutine(SendMessageUpperPanel(playerName + " did "+ userAction));
                updateLog(playerName, "Did "+ userAction);

                // yield return new WaitForSeconds(1);

                if (playersGameState[player.Name].cardsAtHand == 0)                
                {
                    btn_skip.SetActive(true);                    
                }
                
                UpdateScoreBoard();
                 

                if (messageUpdateOthers.Is_Pizza)
                {
                    donePizzas+=1;
                    string pizzaAuthor = originalPlayersOrders[messageUpdateOthers.Pizza_Author];
                    
                    foreach (string name in playersGameState.Keys) {
                        playersGameState[name].passed = false;                        
                    }
                    Debug.Log("[Game Handler] Player "+ pizzaAuthor + " Declared Pizza!");                    
                    updateLog(pizzaAuthor, "I Made a Pizza!");
                    yield return StartCoroutine(SendMessageUpperPanel(pizzaAuthor + " Declared Pizza!"));
                    yield return StartCoroutine(displayPizzaPanel(pizzaAuthor));

                    clearBoard();               
                                
                }

                updatePlayersInformation();     

                setActivePlayer(nextPlayerName);                       
                updateLog("System", "Next player: " + nextPlayerName);                
                yield return new WaitForSeconds(1);

        }
        else if (message is RequestActionMessage)
            {
                 RequestActionMessage messageRequestAction = (RequestActionMessage)message;
                 print ("[Game Handler] Processing message requesting a discard action!");
                                
                 int[] cardsInHand = messageRequestAction.observations[11..28].Select(x => (int)(x * 13)).ToArray();
                 int[] possibleActionsVector = messageRequestAction.observations[28..228].Select(f => (int)f).ToArray();

                 string[] possibleActions = messageRequestAction.possibleActionsDecoded;

                //  var possibleActionIndices = Enumerable.Range(0, possibleActionsVector.Length)
                //                 .Where(i => possibleActionsVector[i] == 1)
                //                 .ToList();

                playersGameState[player.Name].cards = cardsInHand.ToList<int>();
                playersGameState[player.Name].cardsAtHand = cardsInHand.Count();
                playersGameState[player.Name].possibleActions = possibleActions.ToList<string>();
                // playersGameState[0].possibleActionIndices = possibleActionIndices;

                

                yield return StartCoroutine(OpenCardsPanel(true, true, false, "Cards In My Hand"));                

        }        
        else if (message is MatchOver)
        {
                MatchOver messageMatchOver = (MatchOver)message;            

                
                // Dictionary<string,int> match_score = messageMatchOver.Match_Score;
                // Dictionary<string,int> acumulate_score = messageMatchOver.Game_Score;
                // Dictionary<string,string> current_roles = messageMatchOver.Current_Roles;

                string[] players_names = messageMatchOver.Player_Names;

                int[] match_score_message = messageMatchOver.Match_Score;
                int[] acumulate_score_message = messageMatchOver.Game_Score;
                string[] current_roles_message = messageMatchOver.Current_Roles;
                
                Dictionary<string,int> match_score  = new Dictionary<string,int>();
                for (int i=0; i<4;i++){ match_score.Add(players_names[i], match_score_message[i]);}

                Dictionary<string,int> acumulate_score  = new Dictionary<string,int>();
                for (int i=0; i<4;i++){ acumulate_score.Add(players_names[i], acumulate_score_message[i]);}
                
                Dictionary<string,string> current_roles  = new Dictionary<string,string>();
                for (int i=0; i<4;i++){ current_roles.Add(players_names[i], current_roles_message[i]);}

                              
                playedMatches = messageMatchOver.Matches;                                
                
                StartCoroutine(EndMatch(acumulate_score, current_roles));                        

                // updateMessagePanel("Match Over!", scoreMessage, false, false);
                // yield return new WaitForSeconds(5);

                // yield return StartCoroutine(SendMessageUpperPanel("Match Over! Updating Score Board!"));
                // yield return new WaitForSeconds(1);
                // openCloseScore(true);                
                yield return new WaitForSeconds(5);
                
                string readingMessage = player.PlayerStatus;
                // UpdateScore(scores);
                // UpdateScoreBoard();

                if(readingMessage != PlayerObject.PLAYER_STATUS["gameOver"])
                {   

                    // string chef = originalPlayersOrders[Array.IndexOf(match_score, 3)];
                    // string sousChef = originalPlayersOrders[Array.IndexOf(match_score, 2)];
                    // string waiter = originalPlayersOrders[Array.IndexOf(match_score, 1)];
                    // string dishwasher = originalPlayersOrders[Array.IndexOf(match_score, 0)];
                    
                    // Debug.Log("Dishwasher: " + dishwasher);
                    // Debug.Log("Waiter: " + waiter);
                    // Debug.Log("Souschef: " + sousChef);
                    // Debug.Log("Chef: " + chef);

                    string messageText = "";

                    string thisPlayerRole = current_roles[playersGameState[player.Name].name];

                    if (thisPlayerRole == PlayerObject.POSITIONS[0] )
                    {
                        messageText ="Cards Shuffled! As you are the Dishwasher, you had to give two of your lowest card to the Chef!";
                    }
                    else if (thisPlayerRole == PlayerObject.POSITIONS[1])
                    {
                        messageText ="Cards Shuffled! As you are the Waiter, you had to give your lowest card to the Souschef!";
                    }
                    else if (thisPlayerRole == PlayerObject.POSITIONS[2] ){
                        messageText ="Cards Shuffled! As you are the SousChef, you can choose which cards to give to the Waiter!";
                    }else
                    {
                        messageText ="Cards Shuffled! As you are the Chef, you can choose which cards to give to the Dishwasher!";
                    }

                    yield return StartCoroutine(displayHandlingCards(messageText));    
                    
                    yield return StartMatch(acumulate_score, current_roles);                
                    yield return new WaitForSeconds(1);

                    // openCloseScore(false);                
                    
                    updateLog("[System]", "New match!");          
                }

        }
        
        else if (message is DoSpecialAction)
        {
            DoSpecialAction messageMatchOver = (DoSpecialAction)message;                            
            
            string special_action = messageMatchOver.special_action;    

             yield return StartCoroutine( displaySpecialActionPanel("Special Action", "After receiving your cards, you have two jokers in your hand! Do You want to do the following special action:", special_action,true)   );                                    
        }
        else if (message is SpecialActionUpdate)
        {
            SpecialActionUpdate messageMatchOver = (SpecialActionUpdate)message;                            
            
            string special_action = messageMatchOver.special_action;    
            int special_action_player = messageMatchOver.player;    

            yield return StartCoroutine( displaySpecialActionPanel("Special Action", originalPlayersOrders[special_action_player] + " declared this specil action: ", special_action,false));                                    
        }
        else if (message is ExchangeCards)
        {
            ExchangeCards messageMatchOver = (ExchangeCards)message;                            
            
            int[] cards = messageMatchOver.cards;    
            int amount = messageMatchOver.amount;    

            string sendTo = "";

            if (amount==1) sendTo ="Waiter";
            else sendTo="Dishwasher";
            playersGameState[player.Name].cards = cards.ToList<int>();            

            yield return StartCoroutine(OpenCardsPanel(false, false, true, "Select "+amount+" card(s) to send to the "+sendTo));         

            // yield return StartCoroutine( displaySpecialActionPanel("Special Action", originalPlayersOrders[special_action_player] + " declared this specil action: ", special_action,false));                                    
        }
        else if (message is UpdateExchangeCards)
        {
            UpdateExchangeCards messageMatchOver = (UpdateExchangeCards)message;                            
            
            int[] cards_received = messageMatchOver.cards_received;    
            int[] cards_sent = messageMatchOver.cards_sent;     

            string playerRole = playersGameState[player.Name].role;

            string message_exchangePanel = "Card Exchange! \n";
            if (playerRole == PlayerObject.POSITIONS[0])
            {
                message_exchangePanel = "As you are the Dishwasher, you have to give two of your most precious ingredients to the Chef! And you received two cards from the Chef!";
            }else if (playerRole == PlayerObject.POSITIONS[1])
            {
                message_exchangePanel = "As you are the Waiter, you have to give one of your most precious ingredients to the Souschef! And you received one card from the Souschef!";
            }else if (playerRole == PlayerObject.POSITIONS[2])
            {
                message_exchangePanel = "As you are the Souschef, you chose one card to give to the Waiter! And you received the most precious ingridient from the Waiter!";
            }else if (playerRole == PlayerObject.POSITIONS[3])
            {
                message_exchangePanel = "As you are the Chef, you chose two cards to give to the Dishwasher! And you received two of the most precious ingridient from the Dishwasher!";
            }
            
            yield return StartCoroutine(displayExchangeCard(message_exchangePanel, cards_received, cards_sent) );         

                     

            // string sendTo = "";

            // if (amount==1) sendTo ="Waiter";
            // else sendTo="Dishwasher";
            // playersGameState[0].cards = cards.ToList<int>();            

            // yield return StartCoroutine(OpenCardsPanel(false, true, "Select "+amount+" card(s) to send to the "+sendTo));         

            // yield return StartCoroutine( displaySpecialActionPanel("Special Action", originalPlayersOrders[special_action_player] + " declared this specil action: ", special_action,false));                                    
        }
        else if (message is GameOver)
        {

            Debug.Log("[Game Handler] Processing Game Over message!");            
            updateLog("[System]", "Game over!");

            // messageText.SetText("End of the Game!");


            messagePanel.SetActive(true);        
            updateMessagePanel("Congratulations!","Your game is over, but this is not the end! You can always play again and try to gain the Chef`s Hat!", false, true);

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
        
        string currentStatus = PlayerObject.PLAYER_STATUS["gameFinished"];

         try{
                currentStatus = player.PlayerStatus;
            }catch (Exception e)
            {
                updateMessagePanel("Error!!",e.Message, true, true);
                
            }
        
            while(currentStatus != PlayerObject.PLAYER_STATUS["gameFinished"])
            {

                if (player.PlayerStatus == PlayerObject.PLAYER_STATUS["no_connection"])
                {
                    //TODO ADD CONNECTION ERROR MESSAGE!
                    updateMessagePanel("Attention!!","The server is not responding! Please start the game again and contact the game host!", true, true);
                    break;
                }
                //   print ("[Game Handler] Current Player`s Message Q: "+player.MessagesQ.Count);
                if (player.MessagesQ.Count > 0)
                    {
                        var currentMessage = player.MessagesQ.Dequeue();

                        print ("[Game Handler] Message found: "+currentMessage);
                                            
                        yield return StartCoroutine(ProcessMessage(currentMessage));                
                    }        

                    yield return new WaitForSeconds(0.01F);    
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

    public void UpdateScore(Dictionary<string,int> scores)
    {   

        foreach (string name in scores.Keys)
        {
            playersGameState[name].score = scores[name];       
        }

        // //Update the Gamestates
        // for (int indexServer = 0; indexServer < scores.Count(); indexServer++)
        // {
        //     string thisPlayerName = originalPlayersOrders[indexServer];

        //     int playerIndex = playersGameState.FindIndex(p => p.name == thisPlayerName);            
        //     playersGameState[playerIndex].score = scores[indexServer];                        

        // }

    }

    public void UpdateRoles(Dictionary<string,string> roles)
    {        
        string message = "";
        foreach (string key in roles.Keys){
            message+= key+" - "+roles[key]+", ";
        }
        Debug.Log(message);

        List<GameObject> spritesPosition = new List<GameObject>
        {
            cardBoardRolePosition1,
            cardBoardRolePosition2,
            cardBoardRolePosition3,
            cardBoardRolePosition4
        };

        int count=0;
        foreach (string name in roles.Keys)
        {
            playersGameState[name].role = roles[name];

            spritesPosition[count].GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetRoleCard(roles[name]);
            spritesPosition[count].SetActive(true);

            count+=1;
        }

        //   //Update the Gamestates
        // for (int indexServer = 0; indexServer < scores.Count(); indexServer++)
        // {
        //     string thisPlayerName = originalPlayersOrders[indexServer];

        //     int playerIndex = playersGameState.FindIndex(p => p.name == thisPlayerName);
        //     if (scores.Count()>0) { playersGameState[playerIndex].role = PlayerObject.POSITIONS[scores[indexServer]]; }


        //     if (indexServer == 0)cardBoardRolePosition1.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetRoleCard(scores[indexServer]);
        //     if (indexServer == 1)cardBoardRolePosition2.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetRoleCard(scores[indexServer]);
        //     if (indexServer == 2)cardBoardRolePosition3.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetRoleCard(scores[indexServer]);
        //     if (indexServer == 3)cardBoardRolePosition4.GetComponent<UnityEngine.UI.Image>().sprite = ResourceCards.GetRoleCard(scores[indexServer]);                                                                            

        // }
    }
     IEnumerator EndMatch(Dictionary<string,int> acumulated_score,Dictionary<string,string> current_roles)
        {
        
            // openCloseScore(true);
            UpdateScore(acumulated_score);
            UpdateRoles(current_roles);
            UpdateScoreBoard(); 

            yield return StartCoroutine(displayEndMatchPanel(current_roles));                        

            // updateMessagePanel("Match Over!", scoreMessage, false, false);
            // yield return new WaitForSeconds(5);

            // yield return new WaitForSeconds(1);
            
            // yield return new WaitForSeconds(3);

            
                       
            
        }

    IEnumerator StartMatch(Dictionary<string,int> acumulated_score, Dictionary<string,string> current_roles)
    {
        
        yield return StartCoroutine(SendMessageUpperPanel("Dealing all the cards!"));                     
        // yield return new WaitForSeconds(2);


        Debug.Log("[Game Handler] Starting the match! Updating all the gamestates...");

        donePizzas = 0;
        currentMatch = playedMatches+1;
        
        UpdateScore(acumulated_score);
        UpdateRoles(current_roles);
        //Update the Gamestates

        foreach( string name in playersGameState.Keys)
        {
            playersGameState[name].cardsAtHand = 17;
        }

        // for (int indexServer = 0; indexServer < scores.Count(); indexServer++)
        // {
        //     string thisPlayerName = originalPlayersOrders[indexServer];

        //     int playerIndex = playersGameState.FindIndex(p => p.name == thisPlayerName);          
        //     playersGameState[playerIndex].cardsAtHand = 17;

        // }

        //Update the Scores        
        UpdateScoreBoard();

        yield return StartCoroutine(SendMessageUpperPanel("Starting Match Number "+ currentMatch));        

    }


    IEnumerator StartGame(string[] playerNames, int[] cardsInHand, string startingPlayer)
    {

         Debug.Log("[Game Handler] Starting the game! Reseting all the internal states...");

        //  yield return StartCoroutine(displayHandlingCards());
        
        //Reset all internal states

        playedMatches = 0;

        originalPlayersOrders = playerNames.ToList<string>();

        playersGameState = new Dictionary<string, PlayerGameState>();

        List<string> playerNamesOrdered =  ReorderList(playerNames.ToList<string>(), player.Name);
        

        for (int i=0; i<playerNamesOrdered.Count();i++ )
        {
            Debug.Log("Player position+ " + i +" - Name: " + playerNamesOrdered.ElementAt(i));
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
            playersGameState.Add(thisPlayer.name, thisPlayer);            
        }

        foreach(string key in playersGameState.Keys) Debug.Log("Key: "+key+" - Player: "+playersGameState[key].name);


        StartMatch(new Dictionary<string, int>(), new Dictionary<string, string>());

        UpdateScoreBoard();
        updatePlayersInformation();

        updateLog("System", "Game Started!");
                
        yield return StartCoroutine(SendMessageUpperPanel("Game Started!"));         

    }


    // Scene Management functions

    IEnumerator Start()
    {

        upperPanel.SetActive(false);
        cardsPanel.SetActive(false);
        // scorePanel.SetActive(false);
        logsPanel.SetActive(false);
        btn_back.SetActive(false);
        btn_skip.SetActive(false);
        nextPlayerPanel.SetActive(false);
        messagePanel.SetActive(false);
        pizzaPanel.SetActive(false);
        specialActionPanel.SetActive(false);
        handlingCardsPanel.SetActive(false);
        exchangeCardsPanel.SetActive(false);

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

        updateMessagePanel("Waiting....", "Waiting for the other players, hang in there!", false, false);

        // messageText.SetText("Waiting for other players to connect...");
        updateLog("System", "Waiting for players...");

        yield return new WaitForSeconds(1);

        StartCoroutine(ReadPlayerQ());

    }


    // Update is called once per frame
    void Update()
    {
                             
        
    }
}
