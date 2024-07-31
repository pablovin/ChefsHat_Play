using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviour
{

    // private static mainMenu instance;

    // private void Awake()
    // {   
    //     if (instance==null){

    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else{
    //         Destroy(gameObject);
    //     }

        
    // }

    // Start is called before the first frame update
    public void StartGame()
    {
        Debug.Log("into the start game menu!");
        SceneManager.LoadScene("start_game");
    
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void back_MainMenu(){
        Debug.Log("back to main menu!");
        SceneManager.LoadScene("main_menu");
    }
}
