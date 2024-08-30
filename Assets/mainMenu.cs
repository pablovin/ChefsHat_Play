using System.Collections;
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


    [SerializeField] AudioSource buttonSource;
    // Start is called before the first frame update
    public void StartGame()
    {
        Debug.Log("into the start game menu!");
        buttonSource.Play(0);
        Debug.Log("Sound played, now loading the scene");        
        StartCoroutine(DelaySceneLoad("start_game", 1));
    
    }

    IEnumerator DelaySceneLoad(string scene, int time)
    {   
        Debug.Log("Waiting to load the scene!");

        yield return new WaitForSeconds(time);

        Debug.Log("Loading scene!");
        
        SceneManager.LoadScene(scene);

        
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
