using UnityEngine;
using TMPro;
using System.Threading.Tasks;
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
    [SerializeField] TMP_Text messageTitle;
    [SerializeField] GameObject messageImage;
    [SerializeField] UnityEngine.UI.Button messageBackButton;

    //Audio

    [SerializeField] AudioSource buttonSource;
    [SerializeField] AudioSource errorSound;

    
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


            // try{
                string message = "Connecting to Room..." + roomURL+":"+roomPort;
                OpenMessagePanel(message,false, "Connecting...");

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
                 

            // }catch (System.Exception e)
            // {
            //     errorSound.Play(0);
            //     Debug.Log("[Start Game Handler] Error! "+e.Message);
            //     SendErrorMessage(e.Message, "Problem!", true);
            // }
    }


    public void SendErrorMessage(string message, string title, bool show_error_logo)
    {
        OpenMessagePanel(message,show_error_logo,title);
        messageBackButton.gameObject.SetActive(true);
    }

    public void OpenMessagePanel(string message, bool show_error_logo, string title)
    {
                
        messagePanel.SetActive(true);
        messageImage.SetActive(show_error_logo);
        messageTitle.SetText(title);
        messageText.SetText(message);

    }

    void Start()
    {
        messagePanel.SetActive(false);
        messageBackButton.gameObject.SetActive(false);
    }


}
