using UnityEngine;

public class music_menu : MonoBehaviour
{

    private static music_menu instance;

    private void Awake()
    {   
        
        if (instance==null){

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
        

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
