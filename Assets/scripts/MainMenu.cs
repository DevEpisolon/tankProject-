using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    public void Login(){
        SceneManager.LoadSceneAsync(4);
    }

    public void SignUp(){
        SceneManager.LoadSceneAsync(3);
    }
}
