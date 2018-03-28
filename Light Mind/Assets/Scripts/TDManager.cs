using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Utilities;
using UnityEngine.UI;
using System.Text;
using System.Xml.Linq;

public class TDManager : MonoBehaviour {
    // Not used yet, might delete
    public enum STATE {PLAYING, PAUSE, WIN, LOSE};

    public Spawn Spawn;

    public STATE GameState;
    public int WavesTotal;
    public int LivesLeft;
    public int CurrentWave;
    private float _spawnInterval;
    private List<List<RayColor>> _enemyWaves = new List<List<RayColor>>();
    public Dictionary<char, RayColor> _wavesDico;
    private List<Vector3> _paths = new List<Vector3> ();

    // GUI
    public GameObject GoButton;
    public Text LivesText;
    public Text WaveText;

    public void Init() {
        // Init the dictionary with all char and corresponding colors
        InitializeWavesDico ();

        // Add GoButton Listener
        GoButton.GetComponent <Button>().onClick.AddListener (OnGoButtonClick);
    }

    public void StartGame() {
        // Might add more things here
        Spawn = GameObject.FindObjectOfType<Spawn> ();
        Spawn.SetUp (this, _spawnInterval, _paths);
        Debug.Log (Spawn);
        CurrentWave = 1;
        StartPausedPhase ();
    }

    private void StartPlayingPhase() {
        // Change state
        GameState = STATE.PLAYING;
        // Hide go button
        GoButton.gameObject.SetActive (false);
        // Update current wave
        WaveText.text = "Wave : " + CurrentWave;
        // Lives
        LivesText.text = "Lives : " + LivesLeft;
        // Call spawner
        StartNextWave();
        // Check if enemies are all dead or player is
        // THIS SHOULD BE CHECKED WHEN A TOWER FIRES AND WHEN AN ENEMY TAKES A LIFE POINT OFF
        // Call CallNextPhase or StartPausePhase
    }

    private void StartPausedPhase() {
        // Change state
        GameState = STATE.PAUSE;
        // Display next wave
        WaveText.text = "Next Wave: " + Environment.NewLine + ShowNextWave(_enemyWaves[CurrentWave]);
        // Show button Go
        GoButton.gameObject.SetActive (true);
    }

    String ShowNextWave(List<RayColor> wave) {
        int RED, WHITE, BLUE, YELLOW, GREEN, CYAN, MAGENTA, NONE;
        RED = WHITE = BLUE = YELLOW = GREEN = CYAN = MAGENTA =  NONE = 0;
        foreach (RayColor color in wave) {
            if (color == RayColor.RED) {
                RED++;
                continue;
            }
            if (color == RayColor.WHITE) {
                WHITE++;
                continue;
            }
            if (color == RayColor.BLUE) {
                BLUE++;
                continue;
            }
            if (color == RayColor.YELLOW) {
                YELLOW++;
                continue;
            }
            if (color == RayColor.GREEN) {
                GREEN++;
                continue;
            }
            if (color == RayColor.CYAN) {
                CYAN++;
                continue;
            }
            if (color == RayColor.MAGENTA) {
                MAGENTA++;
                continue;
            }
            if (color == RayColor.NONE) {
                NONE++;
                continue;
            }
        }

        StringBuilder sb = new StringBuilder ();
        if (RED > 0) sb.Append ("RED x").Append (RED).Append (Environment.NewLine);
        if (BLUE > 0) sb.Append ("BLUE x").Append (BLUE).Append (Environment.NewLine);
        if (YELLOW > 0) sb.Append ("YELLOW x").Append (YELLOW).Append (Environment.NewLine);
        if (CYAN > 0) sb.Append ("CYAN x").Append (CYAN).Append (Environment.NewLine);
        if (GREEN > 0) sb.Append ("GREEN x").Append(GREEN).Append (Environment.NewLine);
        if (WHITE > 0) sb.Append ("WHITE x").Append (WHITE).Append (Environment.NewLine);
        if (MAGENTA > 0) sb.Append ("MAGENTA x").Append (MAGENTA).Append (Environment.NewLine);
        if (NONE > 0) sb.Append ("COLOR-IMMUNE x").Append (NONE).Append (Environment.NewLine);
        Debug.Log (sb);
        return sb.ToString ();
    }

    void StartNextWave() {
        // Might add more things
        Spawn.StartWave (_enemyWaves[CurrentWave - 1]);
    }

    // Might delete if utility is low
    void CallNextPhase() {
        if (GameState == STATE.PLAYING) {
            StartPausedPhase ();
        } else if (GameState == STATE.PAUSE) {
            StartPlayingPhase ();
        } else if (GameState == STATE.LOSE) {
            // TODO
        } else if (GameState == STATE.WIN) {
            // TODO
        }
    }

    public void SetUpWaves(JSONObject data) {        
        foreach (var jsonEntity in data["Waves"].list)
        {
            DecodeEnemyWaves(jsonEntity ["Enemies"].str);
        }
        LivesLeft = (int)data ["Info"].GetField ("Lives").n;
        _spawnInterval = (float)data ["Info"].GetField ("SpawnInterval").n;
        WavesTotal = _enemyWaves.Count;
    }
        
    private void DecodeEnemyWaves(String encodedWave) {
        List<RayColor> waveColors = new List<RayColor>();
        RayColor previousColor = RayColor.NONE;

        for (int i = 0; i < encodedWave.Length; i++) {
            char currChar = encodedWave [i];
            if (_wavesDico.ContainsKey (currChar)) {
                _wavesDico.TryGetValue (currChar, out previousColor);
                waveColors.Add (previousColor);
            } else {
                string countStr = "";
                while (i < encodedWave.Length && !_wavesDico.ContainsKey (encodedWave[i])) {
                    countStr += currChar;
                    if (++i < encodedWave.Length) {
                        currChar = encodedWave [i];
                    }
                }
                i--;
                int count = 0;
                if (countStr.Length > 0) count = Int32.Parse (countStr);
                while (count > 1) { //R2 = R + R since we already added R before, only go to > 1
                    waveColors.Add (previousColor);
                    count--;
                }
            }
        }

        _enemyWaves.Add (waveColors);
    }

    public void InitializeWavesDico() {
        _wavesDico = new Dictionary<char, RayColor>();
        _wavesDico.Add ('R', RayColor.RED);
        _wavesDico.Add ('B', RayColor.BLUE);
        _wavesDico.Add ('W', RayColor.WHITE);
        _wavesDico.Add ('C', RayColor.CYAN);
        _wavesDico.Add ('G', RayColor.GREEN);
        _wavesDico.Add ('M', RayColor.MAGENTA);
        _wavesDico.Add ('Y', RayColor.YELLOW);
        _wavesDico.Add ('N', RayColor.NONE);
    }

    public void InitializeGoButton() {
        GoButton.GetComponent <Button>().onClick.AddListener (OnGoButtonClick);
    }

    private void OnGoButtonClick() {
        // Go to next state
        StartPlayingPhase ();
    }

    public void AddPath(Vector3 path) {
        _paths.Add (path);
    }

    public void DecreaseLives() {
        LivesLeft--;
        LivesText.text = "Lives : " + LivesLeft;
    }
}
