using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    [SerializeField] GameObject panelMenu;

    [SerializeField] Slider volumeSlider;
    // Start is called before the first frame update

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();

        }
        else{
            Load();
        }
    }
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

    public void OpenMenu()
    {

        panelMenu.SetActive(!panelMenu.activeSelf);
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
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
