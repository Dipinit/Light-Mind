using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Utilities;
using UnityEngine.UI;
using System.Text;

public class TDManager : MonoBehaviour {
    public enum STATE {PLAYING, PAUSE};
    public Spawn Spawn;

    private STATE _state;
    private int _waves;
    private int _lives;
    private float _spawnInterval;
    private List<List<RayColor>> _enemyWaves = new List<List<RayColor>>();
    private Dictionary<char, RayColor> _wavesDico;

    // GUI
    private Button _goButton;
    public Text Lives;
    public Text WaveText;

    public void Init() {
        InitializeWavesDico ();
        InitializeGoButton ();
        InitializeTexts ();
    }

    public void StartGame() {
        // Might add more things here
        StartPausedPhase ();
    }

    private void StartPlayingPhase() {
        // Change state
        _state = STATE.PLAYING;
        // Hide go button
        _goButton.IsActive (false);
        // Update current wave
        WaveText.text = "Wave : " + Spawn.wave;
        // Call spawner
        StartNextWave();
    }

    private void StartPausedPhase() {
        // Change state
        _state = STATE.PAUSE;
        // Display next wave
        WaveText.text = "Next Wave: " + ShowNextWave(_enemyWaves[Spawn.wave]);
        // Show button Go
        _goButton.IsActive(true);
    }

    String ShowNextWave(List<RayColor> wave) {
        int RED, WHITE, BLUE, YELLOW, GREEN, CYAN, MAGENTA, NONE = 0;
        foreach (RayColor color in wave) {
            switch(color) {
            case RayColor.RED:
                RED++;
                break;
            case RayColor.WHITE:
                WHITE++;
                break;
            case RayColor.BLUE:
                BLUE++;
                break;
            case RayColor.YELLOW:
                YELLOW++;
                break;
            case RayColor.GREEN:
                GREEN++;
                break;
            case RayColor.CYAN:
                CYAN++;
                break;
            case RayColor.MAGENTA:
                MAGENTA++;
                break;
            case RayColor.NONE:
                NONE++;
                break;
            default:
                Debug.Log ("Unknown enemy color:" + color);
                break;
            }
        }

        StringBuilder sb = new StringBuilder ();
        if (RED > 0) sb.Append ("RED x").Append (RED).Append (Environment.NewLine);
        if (BLUE > 0) sb.Append ("BLUE x").Append (BLUE).Append (Environment.NewLine);
        if (YELLOW > 0) sb.Append ("YELLOW x").Append (YELLOW).Append (Environment.NewLine);
        if (CYAN > 0) sb.Append ("CYAN x").Append (CYAN).Append (Environment.NewLine);
        if (GREEN > 0) sb.Append ("GREEN x").Append(GREEN).Append (Environment.NewLine);
        if (WHITE > 0) sb.Append ("WHITE x").Append (WHITE).Append (Environment.NewLine);
        if (NONE > 0) sb.Append ("COLOR-IMMUNE x").Append (NONE).Append (Environment.NewLine);

        return sb.ToString ();
    }

    void StartNextWave() {
        // Might add more things
        Spawn.StartWave ();
    }

    public void SetUpWaves(JSONObject data) {        
        foreach (var jsonEntity in data["Waves"].list)
        {
            DecodeEnemyWaves(jsonEntity ["Enemies"].str);
        }
        _lives = Int32.Parse(data ["Info"].GetField ("Lives").str);
        _spawnInterval = float.Parse(data ["Info"].GetField ("SpawnInterval").str);
        _waves = _enemyWaves.Count;
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
                while(!_wavesDico.ContainsKey (currChar) && i < encodedWave.Length) {
                    countStr = countStr + currChar;
                    currChar = encodedWave [++i];
                }
                int count = Int32.Parse (countStr);
                while (count > 1) { //R2 = R + R since we already added R before, only go to > 1
                    waveColors.Add (previousColor);
                    count--;
                }
            }
        }

        _enemyWaves.Add (waveColors);
    }

    private void InitializeWavesDico() {
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

    private void InitializeGoButton() {
        _goButton = new Button ();
        _goButton = GUI.Button (new Rect(Screen.width - 100, Screen.height - 100, 50, 50), "Go!");
        _goButton.IsActive (false);
        _goButton.onClick.AddListener (OnGoButtonClick);
    }

    private void OnGoButtonClick() {
        // Go to next state
        StartPlayingPhase ();
    }

    private void InitializeTexts() {
        WaveText = new Text ();
        WaveText.alignment = TextAnchor.UpperLeft;

        Lives = new Text ();
        Lives.alignment = TextAnchor.UpperCenter;
    }
}
