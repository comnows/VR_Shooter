using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;
using System.Collections;
using Normal.Realtime;

public class PlayerConnectionManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _prefabVRPlayer;
    [SerializeField] private GameObject _prefabVRGun;
    [SerializeField] private GameObject pcButton;
    [SerializeField] private GameObject vrButton;
    public GameObject spawnPoint;
    private Realtime _realtime;
    GameObject playerGameObject;
    GameObject vrGunGameObject;
    private UIAmmo uIAmmo;
    private UIScore uIScore;
    private RealtimeTransform _realtimeTransform;

    private enum Platform { VR, PC };
    private Platform currentPlatform;

    private void Awake()
    {
        _realtime = GetComponent<Realtime>();
        _realtime.didConnectToRoom += DidConnectToRoom;

        //uIAmmo = GameObject.FindObjectOfType<UIAmmo>();
        //uIScore = GameObject.FindObjectOfType<UIScore>();
    }

    private void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }

    public void SetPCPlatform()
    {
        currentPlatform = Platform.PC;
        _realtime.Connect("Map1");
        CloseButton();
    }

    public void SetVRPlatform()
    {
        currentPlatform = Platform.VR;
        _realtime.Connect("Map1");
        CloseButton();
    }

    private void DidConnectToRoom(Realtime realtime)
    {
        if (realtime.room.name == "Map1")
        {
            var options = new Realtime.InstantiateOptions
            {
                ownedByClient = true,
                preventOwnershipTakeover = false,
                useInstance = realtime
            };

            switch (currentPlatform)
            {
                case Platform.PC:
                    playerGameObject = Realtime.Instantiate(_prefab.name, options);
                    CreatePCPlayer();
                    break;
                case Platform.VR:
                    //GameObject.Find("HUD Canvas").SetActive(false);
                    playerGameObject = Realtime.Instantiate(_prefabVRPlayer.name, options);
                    vrGunGameObject = Realtime.Instantiate(_prefabVRGun.name, options);
                    vrGunGameObject.GetComponent<RealtimeTransform>().RequestOwnership();
                    Invoke(nameof(AssignVRGunVariable), 1);
                    //AssignVRGunVariable();
                    break;
                default:
                    playerGameObject = Realtime.Instantiate(_prefabVRPlayer.name, options);
                    vrGunGameObject = Realtime.Instantiate(_prefabVRGun.name, options);
                    vrGunGameObject.GetComponent<RealtimeTransform>().RequestOwnership();
                    Invoke(nameof(AssignVRGunVariable), 1);
                    break;
            }

            playerGameObject.transform.position = spawnPoint.transform.position;

            ChangePlayerName(playerGameObject);
            CloseButton();
        }

    }

    private void AssignVRGunVariable()
    {
        GameObject inventorySockets = playerGameObject.transform.GetChild(2).gameObject;
        GameObject arInventory = inventorySockets.transform.GetChild(0).gameObject;
        GameObject arInventoryAttach = arInventory.transform.GetChild(0).gameObject;
        GameObject arMagazineInventory = inventorySockets.transform.GetChild(1).gameObject;

        vrGunGameObject.transform.position = arInventoryAttach.transform.position;
        arMagazineInventory.GetComponent<VRMagazineGenerator>().AssignVRGun(vrGunGameObject);

        Invoke(nameof(AssignVRGunInARMagazine), 1);
    }

    private void AssignVRGunInARMagazine()
    {
        GameObject arMagazine = GameObject.FindGameObjectWithTag("ARMagazine");
        arMagazine.GetComponent<VRGunMagazine>().AssignVRGun(vrGunGameObject);
    }

    private void CreatePCPlayer()
    {
        GameObject cameraHolder = playerGameObject.transform.GetChild(0).gameObject;

        GameObject cameraRecoil = cameraHolder.transform.GetChild(0).gameObject;

        GameObject weaponCamera = cameraRecoil.transform.GetChild(0).gameObject;

        GameObject normalCamera = cameraRecoil.transform.GetChild(1).gameObject;

        GameObject rigLayers = weaponCamera.transform.GetChild(0).gameObject;

        rigLayers.SetActive(true);
        weaponCamera.GetComponent<Camera>().enabled = true;

        GameObject soldier = playerGameObject.transform.Find("Soldier").gameObject;
        soldier.SetActive(false);
        normalCamera.SetActive(true);

        RealtimeView _realtimeView = playerGameObject.GetComponent<RealtimeView>();
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            //uIAmmo.InitScript(playerGameObject);

            Gun gun = playerGameObject.GetComponent<Gun>();
            GameObject.Find("HUD Canvas").GetComponent<UIPlayerBullet>().RefreshPlayerAmmoText(gun.gunData.currentMagazineAmmo, gun.gunData.currentStashAmmo);
            //uIScore.InitScript(playerGameObject);
        }
    }

    private void CloseButton()
    {
        pcButton.SetActive(false);
        vrButton.SetActive(false);
    }

    private void ChangePlayerName(GameObject connectedPlayer)
    {
        GameObject[] allPlayerInGame = GameObject.FindGameObjectsWithTag("Player");
        string connectedPlayerName = "Player " + allPlayerInGame.Length;
        connectedPlayer.GetComponent<PlayerSyncData>().ChangedPlayerName(connectedPlayerName);
    }
}