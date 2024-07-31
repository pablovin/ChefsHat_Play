using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor.UI;
using UnityEditor.PackageManager;
using System.Threading.Tasks;
using System.Linq.Expressions;
using UnityEngine.SceneManagement;

public class start_game_handler : MonoBehaviour
{

    //PlayerObject

    PlayerObject player;
   
     //Input Forms Create Game

    [SerializeField] TMP_InputField playerNameField;
    [SerializeField] TMP_InputField roomURLField;
    [SerializeField] TMP_InputField roomPortField;
    [SerializeField] TMP_InputField roomPassField;

    //Message Panel
    [SerializeField] GameObject messagePanel;
    [SerializeField] TMP_Text messageText;
    [SerializeField] UnityEngine.UI.Button messageBackButton;

    
    private void validateForm(string name, string roomURL, string roomPort)
    {        
        //Validate the user name
        if (string.IsNullOrEmpty(name))
        {   
            throw new System.Exception("You have to choose a nickname!");            

        }        

        //Validate the connection information
        int portNumber;

        if (!int.TryParse(roomPort, out portNumber))
            {
             throw new System.Exception("The port information has to be a number!");  
            }

    }

    public void CloseMessagePanel()
    {
        messagePanel.SetActive(false);
        messageBackButton.gameObject.SetActive(false);
    }

    async public void ConnectToGame()
    {

        string playerName = playerNameField.text;
        string roomURL = roomURLField.text;
        string roomPort = roomPortField.text;
        string roomPass = roomPassField.text;


            try{
                string message = "Connecting to Room..." + roomURL+":"+roomPort;
                OpenMessagePanel(message);

                //Validating the form and converting data
                validateForm(playerName, roomURL, roomPort);
                int portNumber;
                int.TryParse(roomPort, out portNumber);

                //Creating a new player
                player = PlayerObject.CreatePlayer(playerName);    

                //Connecting to the room                                
                await Task.Run(() => player.ConnectToRoom(roomURL, portNumber, roomPass));                


                // player.ConnectToRoom(roomURL, portNumber, roomPass);                
                Debug.Log("[Start Game Handler] Continue with the room connection!");                                    
                SceneManager.LoadScene("Game");
                 



            }catch (System.Exception e)
            {
                Debug.Log("[Start Game Handler] Error! "+e.Message);
                SendErrorMessage(e.Message);
            }
    }


    public void SendErrorMessage(string message)
    {
        OpenMessagePanel(message);
        messageBackButton.gameObject.SetActive(true);
    }

    public void OpenMessagePanel(string message)
    {
                
        messagePanel.SetActive(true);
        messageText.SetText(message);

    }

    void Start()
    {
        messagePanel.SetActive(false);
        messageBackButton.gameObject.SetActive(false);
    }


}
