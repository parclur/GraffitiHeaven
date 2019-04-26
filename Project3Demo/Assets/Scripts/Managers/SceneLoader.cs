using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour { 

    [HideInInspector] public static SceneLoader instance;

    [SerializeField] private string levelName;

    [SerializeField] private string winName;

    [SerializeField] private string loseName;

    [SerializeField] private string nextScene;

    [SerializeField] private bool additive;

    [SerializeField] private string[] scenes;

    public PlayerDiverMovement diver;

    [SerializeField] GameObject parentMenu;

    bool inMenu;

    void Awake()
    {
        instance = this;
        if(additive){
            for(int i = 0; i < scenes.Length; i++){
                SceneManager.LoadScene(scenes[i], LoadSceneMode.Additive);
            }
        }
    }

    public void LoseGame()
    {
        SceneManager.LoadScene(loseName);
    }

    public void WinGame()
    {
        SceneManager.LoadScene(winName);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(levelName);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    void Update(){
        if(diver){
            if(diver.paused && !inMenu){
                inMenu = true;
                SceneManager.LoadScene("AdditiveTitle", LoadSceneMode.Additive);
            }
            if(diver.paused){
                parentMenu = GameObject.FindGameObjectWithTag("ParentMenu");
            }
            if(!diver.paused && parentMenu){
                inMenu = false;
                Destroy(parentMenu);
            }
        }
    }
}
