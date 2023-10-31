using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class RespawnManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject player;
    moveTank tankControls;
    BulletHandler bulletControls;

    public float respawnTime = 3.0f;
    public Transform[] spawnPoints;
    public CameraManager cameraManager;
    public Healthbar oldHealthBar;



    public void Start(){
        tankControls = player.GetComponent<moveTank>();
        bulletControls = player.GetComponentInChildren<BulletHandler>();
        oldHealthBar = player.GetComponent<PlayerController>().healthBar;

    }
    public void RespawnPlayer()
    {
        Debug.Log("Respawning Player..");
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        cameraManager.thirdPerson();
        tankControls.LockControls();
        bulletControls.LockControls();
        yield return new WaitForSeconds(respawnTime);
        Destroy(player);
        Debug.Log("Starting RespawnCoroutine..");
        Transform selectedSpawnPoint = GetRandomSpawnPoint();
        GameObject newPlayer = Instantiate(playerPrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
        newPlayer.GetComponent<PlayerController>().healthBar = oldHealthBar;
        newPlayer.GetComponent<PlayerController>().respawnManager = this;
        player = newPlayer;
        newPlayer.SetActive(true);
        cameraManager.AssignTarget(newPlayer);
        tankControls = newPlayer.GetComponent<moveTank>();
        bulletControls = newPlayer.GetComponentInChildren<BulletHandler>();
        cameraManager.AssignPlayerCamera(player.GetComponentInChildren<Camera>());
    }

    private Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

}
