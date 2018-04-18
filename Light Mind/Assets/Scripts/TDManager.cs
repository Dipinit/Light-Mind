using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Utilities;
using Behaviors;
using UnityEngine;
using UnityEngine.UI;
using UI;

/**
 * Handles all mechanics linked with:
 * Enemy Waves (Parse JSON Instanciation, ...), Waves UI and Game States
 **/
public class TDManager : MonoBehaviour
{
    public enum State
    {
        NotStartedYet,
        Playing,
        Spawning,
        Pause,
        Win,
        Lose
    }
        
    public State GameState;
    public int WavesTotal;
    public int LivesLeft;
    public int CurrentWave;

    public GameManager GameManager;
    public GameObject Inventory;

    private GameObject _spawnPoint;
    private List<List<Enemy>> _enemyWaves = new List<List<Enemy>>();
    private int _enemiesSpawned;

    private float _defaultSpawnInterval;
    private int _defaultHitpoints;
    private float _defaultSpeed;
    private RayColor _defaultColor;

    // GUI
    public GameObject GoButton;
    public Text LivesText;
    public Text WaveText;

    public void StartGame(GameManager gameManager)
    {
        GameState = State.NotStartedYet;
        Inventory = GameObject.Find("Inventory");
        GameManager = gameManager;
        CallNextPhase();
    }

    public void SetUpGame(JSONObject data)
    {
        SetUpGameInfo (data);
        SetUpWaves (data);
    }

    /**
     * Parses the given JSON Object to fill the EnemyWaves.
     **/
    private void SetUpWaves(JSONObject data) {
        
        // JSON Structure is as followed: Waves > (Multiple) Enemies > (Array) OBJECT (HP, SPEED, COLOR)
        foreach (var enemies in data["Waves"].list)
        {
            List<Enemy> currentWave = new List<Enemy> ();
            foreach (JSONObject enemy in enemies["Enemies"].list) {
                Debug.Log (enemy.GetType () + " : " + enemy);
                currentWave.Add (CreateEnemyFromJSON (enemy));
            }
            _enemyWaves.Add (currentWave);
        }

        WavesTotal = _enemyWaves.Count;
    }

    private Enemy CreateEnemyFromJSON(JSONObject enemy) {
        int hitpoints = enemy ["Hitpoints"] != null ? (int) enemy ["Hitpoints"].n : _defaultHitpoints;
        float speed = enemy ["Speed"] != null ? enemy ["Speed"].n : _defaultSpeed;
        RayColor color = enemy ["Color"] != null ? RayColor.Parse (enemy ["Color"].str) : _defaultColor;
        float spawnTime = enemy ["SpawnTime"] != null ? enemy ["SpawnTime"].n : _defaultSpawnInterval;

        return new Enemy (hitpoints, speed, color, spawnTime);
    }

    /**
     * Sets up specific settings (Players Lives, Spawn Interval, ...)
     **/
    private void SetUpGameInfo(JSONObject data) {
        _defaultSpawnInterval = data["Info"].GetField("DefaultSpawnInterval").n;
        _defaultHitpoints = (int) data["Info"].GetField("DefaultHitpoints").i;
        _defaultSpeed = data["Info"].GetField("DefaultSpeed").n;
        _defaultColor = RayColor.Parse(data["Info"].GetField("DefaultColor").str);

        LivesLeft = (int) data["Info"].GetField("Lives").i;
        _spawnPoint = GameObject.FindGameObjectWithTag("Spawn Point");

        GoButton.GetComponent<Button>().onClick.AddListener(CallNextPhase);
    }

    /**
     * Updates GUI for wave-fighting gameplay and calls the Enemy Spawner
     **/
    private void StartPlayingPhase()
    {
        //Prevent User from placing tower while State is Spawning/Playing
        Inventory.SetActive (false);
        GoButton.gameObject.SetActive(false);

        WaveText.text = string.Format("Wave : {0}/{1}", CurrentWave, WavesTotal);
        LivesText.text = string.Format("Lives : {0}", LivesLeft);

        StartWave(_enemyWaves[CurrentWave - 1]);
    }

    /**
     * Update GUI for the "Building" phase of that game.
     **/
    private void StartPausedPhase()
    {
        GoButton.gameObject.SetActive(true);
        Inventory.SetActive (true);

        CurrentWave++;

        GameState = State.Pause;
        WaveText.text = string.Format("Next Wave: {0}{1}", Environment.NewLine,
            GetNextWaveColors(_enemyWaves[CurrentWave - 1]));
        LivesText.text = string.Format("Lives : {0}", LivesLeft);
    }

    public void DecreaseLives()
    {
        LivesLeft--;
        CameraShake.shakeAmount = 0.3f;
        CameraShake.shakeTimer = 0.5f;
        LivesText.text = string.Format("Lives : {0}", LivesLeft);

        if (LivesLeft > 0) return;

        Debug.Log("Player loses the game!");
        LivesLeft = 0;
        GameState = State.Lose;
        CallNextPhase ();
    }

    public void StartWave(List<Enemy> wave)
    {
        StartCoroutine(SpawnEnemies(wave));
    }

    private void Update()
    {
        // TODO: Fixes a bug, sometimes the inventory popups after a drag and drop
        if ((GameState == State.Playing || GameState == State.Spawning) && Inventory.activeSelf) {
            Inventory.SetActive (false);
        }

        if (_enemiesSpawned > 0 && GameState == State.Playing) {
            var enemies = GameObject.FindGameObjectsWithTag ("enemy");
            if (enemies.Length == 0) {
                if (CurrentWave == _enemyWaves.Count) {
                    GameState = State.Win;
                }
                CallNextPhase ();
            }
        }
    }

    /**
     * Determines how many enemies of each colors are in the given wave.
     * Returns a String to be when displaying the next wave message.
     **/
    private string GetNextWaveColors(IEnumerable<Enemy> wave)
    {
        int red, white, blue, yellow, green, cyan, magenta, none;
        red = white = blue = yellow = green = cyan = magenta = none = 0;
        foreach (var enemy in wave)
        {
            RayColor color = enemy.Color;
            if (color == RayColor.RED) { red++; continue; }
            if (color == RayColor.WHITE) { white++; continue; }
            if (color == RayColor.BLUE) { blue++; continue; }
            if (color == RayColor.YELLOW) { yellow++; continue; }
            if (color == RayColor.GREEN) { green++; continue; }
            if (color == RayColor.CYAN) { cyan++; continue; }
            if (color == RayColor.MAGENTA) { magenta++; continue; }
            if (color == RayColor.NONE) { none++; }
        }

        StringBuilder sb = new StringBuilder();
        if (red > 0) sb.Append("<size=30><color=red>●</color></size> x").Append(red).Append(Environment.NewLine);
        if (blue > 0) sb.Append("<size=30><color=blue>●</color></size> x").Append(blue).Append(Environment.NewLine);
        if (yellow > 0) sb.Append("<size=30><color=yellow>●</color></size> x").Append(yellow).Append(Environment.NewLine);
        if (cyan > 0) sb.Append("<size=30><color=cyan>●</color></size> x").Append(cyan).Append(Environment.NewLine);
        if (green > 0) sb.Append("<size=30><color=green>●</color></size> x").Append(green).Append(Environment.NewLine);
        if (white > 0) sb.Append("<size=30><color=white>●</color></size> x").Append(white).Append(Environment.NewLine);
        if (magenta > 0) sb.Append("<size=30><color=magenta>●</color></size> x").Append(magenta).Append(Environment.NewLine);
        if (none > 0) sb.Append("<size=30><color=black>●</color></size> x").Append(none).Append(Environment.NewLine);
        Debug.Log(sb);
        return sb.ToString();
    }
        
    /**
     * Determines which method to call depending on current GameState
     **/
    public void CallNextPhase()
    {
        switch (GameState)
        {
            case State.Playing:
            case State.Spawning:
            case State.NotStartedYet:
                StartPausedPhase ();
                break;
            case State.Pause:
                StartPlayingPhase();
                break;
            case State.Lose:
                GameManager.LoseLevel ();
                break;
            case State.Win:
                HandleLevelProgression ();
                GameManager.WinLevel ();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /**
     * Handles level being replayed.
     * Will crash if trying to "continue" on the last level. Easily handled by adding a variable maxLevel.
     **/
    private void HandleLevelProgression() {
        var levelReached = PlayerPrefs.HasKey ("levelReached") ? PlayerPrefs.GetInt ("levelReached") : 0;
        var currentLevel = Int32.Parse (PlayerPrefs.GetString ("currentLevel").Substring (5));
        if (currentLevel >= levelReached) {
            levelReached++;
            PlayerPrefs.SetInt ("levelReached", levelReached);
        }
    }

    /**
     * Converts a List of Enemy into EnemyPrefab and instanciate them with specific values.
     * During the Spawning, the GameState should be State.Spawning, this stops win condition checking.
     **/
    private IEnumerator SpawnEnemies(IList<Enemy> wave)
    {
        GameState = State.Spawning;
        _enemiesSpawned = 0;
        Debug.Log(string.Format("Wave Count: {0}", wave.Count));
        while (_enemiesSpawned < wave.Count && State.Lose != this.GameState)
        {
            Enemy currentEnemy = wave [_enemiesSpawned];
            var enemyGo = Instantiate(GameManager.Instance.EnemyPrefab, _spawnPoint.transform.position,
                Quaternion.identity);

            EnemyBehaviour currentEnemyBehaviour = enemyGo.GetComponent<EnemyBehaviour> ();
            currentEnemyBehaviour.Color = currentEnemy.Color;
            currentEnemyBehaviour.Life = currentEnemy.Hitpoints;
            currentEnemyBehaviour.Speed = currentEnemy.Speed;
            currentEnemyBehaviour.UpdateNavigationAgent ();

            _enemiesSpawned++;
            Debug.Log("Spawned Enemy [" + currentEnemy.toString () + "]");

            yield return new WaitForSeconds(currentEnemy.SpawnTime);
        }

        StopAllCoroutines();
        GameState = State.Playing;
    }
}