using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.SceneManagement;
public class PlayerSyncData : RealtimeComponent<PlayerSyncDataModel>
{
    public int _playerHP;
    public int _playerScore;
    public Vector2 _playerMoveInput;
    public float _playerMoveSpeedMultiplier;
    public string _playerName;
    public bool _playerIsCanEnterNextLV;
    private GameObject weaponModel;
    private RealtimeView _realtimeView;

    private void Awake() 
    {
        _playerHP = 100;
        _playerScore = 0;
        _playerMoveInput = new Vector2(0.0f,0.0f);
        _playerMoveSpeedMultiplier = 0;
        _playerName = "Player";
        _playerIsCanEnterNextLV = false;
    }

    private void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        _realtimeView = gameObject.GetComponent<RealtimeView>();
        Debug.Log("this player = " + gameObject.name + " ownerIDInHierarchy " + _realtimeView.ownerIDInHierarchy);
        Debug.Log("this player = " + gameObject.name + " ownerIDSelf " + _realtimeView.ownerIDSelf);
    }
    

    protected override void OnRealtimeModelReplaced(PlayerSyncDataModel previousModel, PlayerSyncDataModel currentModel) 
    {
        if (previousModel != null) {
            // Unregister from events
            previousModel.playerHPDidChange -= PlayerHPDidChange;
            previousModel.playerScoreDidChange -= PlayerScoreDidChange;
            previousModel.playerMoveInputDidChange -= PlayerMoveInputDidChange;
            previousModel.playerMoveSpeedMultiplierDidChange -= PlayerMoveSpeedMultiplierDidChange;
            previousModel.playerNameDidChange -= PlayerNameDidChange;
            previousModel.playerIsCanEnterNextLVDidChange -= PlayerIsCanEnterNextLVDidChange;
        }
        
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.playerHP = _playerHP;
                currentModel.playerScore = _playerScore;
                currentModel.playerMoveInput = _playerMoveInput;
                currentModel.playerMoveSpeedMultiplier = _playerMoveSpeedMultiplier;
                currentModel.playerName = _playerName;
                currentModel.playerIsCanEnterNextLV = _playerIsCanEnterNextLV;
            }
            // Update the mesh render to match the new model
            UpdatePlayerHP();
            UpdatePlayerScore();
            UpdatePlayerMoveInput();
            UpdatePlayerMoveSpeedMultiplier();
            UpdatePlayerName();
            UpdateIsCanEnterNextLV();
            // Register for events so we'll know if the color changes later
            currentModel.playerHPDidChange += PlayerHPDidChange;
            currentModel.playerScoreDidChange += PlayerScoreDidChange;
            currentModel.playerMoveInputDidChange += PlayerMoveInputDidChange;
            currentModel.playerMoveSpeedMultiplierDidChange += PlayerMoveSpeedMultiplierDidChange;
            currentModel.playerNameDidChange += PlayerNameDidChange;
            currentModel.playerIsCanEnterNextLVDidChange += PlayerIsCanEnterNextLVDidChange;
        }
    }

    private void PlayerHPDidChange(PlayerSyncDataModel model, int value) 
    {
        UpdatePlayerHP();
    }
    
    private void PlayerScoreDidChange(PlayerSyncDataModel model, int value) 
    {
        UpdatePlayerScore();
    }

    private void PlayerMoveInputDidChange(PlayerSyncDataModel model, Vector2 value)
    {
        UpdatePlayerMoveInput();
    }

    private void PlayerMoveSpeedMultiplierDidChange(PlayerSyncDataModel model, float value)
    {
        UpdatePlayerMoveSpeedMultiplier();
    }

    private void PlayerNameDidChange(PlayerSyncDataModel model, string value)
    {
        UpdatePlayerName();
    }

    private void PlayerIsCanEnterNextLVDidChange(PlayerSyncDataModel model, bool value)
    {
        UpdateIsCanEnterNextLV();
    }

    private void UpdatePlayerHP() 
    {
        _playerHP = model.playerHP;

        Debug.Log("PlayerHP = " + _playerHP);

        GetComponent<PlayerHealthController>().ReceiveDamage(_playerHP);
        UIPlayerHealth uIPlayerHealth = GameObject.Find("HUD Canvas").GetComponent<UIPlayerHealth>();
        uIPlayerHealth.RefreshPlayerHealthUI(_playerHP);
    }

    private void UpdatePlayerScore() 
    {
        _playerScore = model.playerScore;
        if(_playerScore > 0 )
        {
            UIScore uIScore = GameObject.Find("HUD Canvas").GetComponent<UIScore>();
            uIScore.UpdateScoreText(gameObject);
        }
        Debug.Log("PlayerScore = " + _playerScore);
    }

     private void UpdatePlayerMoveInput() 
    {
        _playerMoveInput = model.playerMoveInput;

        Debug.Log("PlayerMoveInput = " + _playerMoveInput);
    }

    private void UpdatePlayerMoveSpeedMultiplier() 
    {
        _playerMoveSpeedMultiplier = model.playerMoveSpeedMultiplier;

        Debug.Log("PlayerMoveSpeedMultiplier = " + _playerMoveSpeedMultiplier);
    }

    private void UpdatePlayerName()
    {
        _playerName = model.playerName;

        Debug.Log("PlayerName = " + _playerName);

        GameObject [] playerNameTags = GameObject.FindGameObjectsWithTag("PNameTag");

        foreach (GameObject nameTag in playerNameTags)
        {
            nameTag.GetComponent<PlayerNameTag>().UpdateNameTag();
        }
        
        GameObject [] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            player.name = player.GetComponent<PlayerSyncData>()._playerName;
        }
    }

    private void UpdateIsCanEnterNextLV()
    {
        _playerIsCanEnterNextLV = model.playerIsCanEnterNextLV;
                
        Debug.Log("PlayerIsCanEnterNextLV = " + _playerIsCanEnterNextLV);
    }

    public void AddPlayerHP(int hp) 
    {
        model.playerHP += hp;
        if (model.playerHP > 100)
        {
            model.playerHP = 100;
        }
    }

    public void DecreasePlayerHP(int hp) 
    {
        model.playerHP -= hp;
    }

    public void AddPlayerScore(int scoreToAdd) 
    {
        model.playerScore += scoreToAdd;
    }

    public void ChangedPlayerMoveInput(Vector2 playerInput)
    {
        model.playerMoveInput = playerInput;
    }

    public void ChangedPlayerMoveSpeedMultiplier(float playerSpeedMultiplier)
    {
        model.playerMoveSpeedMultiplier = playerSpeedMultiplier;
    }

    public void ChangedPlayerName(string playerName)
    {
        model.playerName = playerName;
    }

    public void ChangedIsCanEnterNextLV(bool iscanEnterNextLV )
    {
        model.playerIsCanEnterNextLV = iscanEnterNextLV;
    }
}
