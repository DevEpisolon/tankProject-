using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab; 
using PlayFab.ClientModels; 
using Newtonsoft.Json; 
using UnityEngine.UI; 
using UnityEngine; 
using UnityEngine.SceneManagement; 

public class PlayFabManager : MonoBehaviour
{
    [Header("UI")]
    public Text messageText;
    public InputField emailInput; 
    public InputField passwordInput; 
    public void RegisterButton(){
        var request = new RegisterPlayFabUserRequest{
             Email = emailInput.text, 
             Password = passwordInput.text, 
             RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }
    
    void OnRegisterSuccess(RegisterPlayFabUserResult result){
        SceneManager.LoadSceneAsync(1);
    }
    public void LoginButton(){

    }

    public void ResetPasswordButton(){

    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result){

    }
  
}
