using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class testLobby : MonoBehaviour
{

    private Lobby joinedLobby;
    private Lobby hostLobby; 
    private float heartbeatTimer;
    private string playerName;
    private Lobby joinLobby; 
    private float lobbyUpdateTimer; 


    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "stephen" + UnityEngine.Random.Range(10, 99);
        Debug.Log(playerName);
        CreateRelay();
    }

    private async void CreateRelay()
    {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
        }
        catch (RelayServiceException error) {
            Debug.LogError(error.Message);
        }
        
    }

    private void Update(){ 
        HandleLobbyHeartBeat();
        HandleLobbyPollForUpdates();
    }

    private async void HandleLobbyHeartBeat(){
        if(hostLobby != null){
            heartbeatTimer -= Time.deltaTime; 
            if(heartbeatTimer < 0f){
                float heartbeatTimerMax = 15; 
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void HandleLobbyPollForUpdates(){
        if(joinedLobby != null){
            lobbyUpdateTimer -= Time.deltaTime; 
            if(lobbyUpdateTimer < 0f){
                float lobbyUpdateTimerMax = 1.1f; 
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby; 
            }
        }
    }
    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "My Lobby";
            int maxPlayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag")}
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            hostLobby = lobby;
            joinedLobby = hostLobby; 

            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void ListLobbies(){
        try {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions {
                Count = 25, 
                Filters = new List<QueryFilter>{
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT), 
                   
                }, 
                Order = new List<QueryOrder>{
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found" + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results){
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
            }
        } catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }
	
    private async void JoinLobbyByCode(string lobbyCode){
        try {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions{
                Player = GetPlayer()
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinedLobby = lobby;
            Debug.Log("Joined Lobby by code " + lobbyCode);
            PrintPlayers(lobby);

        }catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void QuickJoinLobby(){
        try { 
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }catch(LobbyServiceException e){
            Debug.Log(e);
        }
        
    }

    private Player GetPlayer(){
        return new Player {
            Data = new Dictionary<string, PlayerDataObject> {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
            }
        };
    }

    private void PrintPlayers(){
        PrintPlayers(joinLobby);
    }
    
    private void PrintPlayers(Lobby lobby){
        Debug.Log("Players in Lobby: " + lobby.Name + " " + lobby.Data["GameMode"].Value);
        foreach(Player player in lobby.Players){
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }
    private async void UpdateLobbyGameMode(string gameMode){
        try {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions{
            Data = new Dictionary<string, DataObject>{
                {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode)}
            }
            
            });
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);
        } catch(LobbyServiceException e){
            Debug.Log(e);
        }
        

    }

    private async void UpdatePlayerName(string newPlayerName){
        try { 
                playerName = newPlayerName; 
                await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions{
                Data = new Dictionary<string, PlayerDataObject> { 
                        {"Playername", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                    }
                });

        } catch(LobbyServiceException e){
            Debug.Log(e);
        }

    }

    private void LeaveLobby(){
        try {
            LobbyService.Instance.RemovePlayerAsync(joinLobby.Id, AuthenticationService.Instance.PlayerId);
        } catch(LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    //kick a player from the lobby
    private async void KickPlayer(){
        try {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);

        } catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void MigrateLobbyHost(){
        try {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions{
                HostId = joinedLobby.Players[1].Id
            });
            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);
        }catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void startLobby() { 
        
    }

    
}