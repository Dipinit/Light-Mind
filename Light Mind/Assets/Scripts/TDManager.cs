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
    public Button GoButton;
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
        GoButton.gameObject.SetActive (false);
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
        GoButton.gameObject.SetActive (true);
    }

    String ShowNextWave(List<RayColor> wave) {
        int RED, WHITE, BLUE, YELLOW, GREEN, CYAN, MAGENTA, NONE;
        RED = WHITE = BLUE = YELLOW = GREEN = CYAN = MAGENTA =  NONE = 0;
        foreach (RayColor color in wave) {
            if (color == RayColor.RED) {
                RED++;
                break;
            }
            if (color == RayColor.WHITE) {
                WHITE++;
                break;
            }
            if (color == RayColor.BLUE) {
                BLUE++;
                break;
            }
            if (color == RayColor.YELLOW) {
                YELLOW++;
                break;
            }
            if (color == RayColor.GREEN) {
                GREEN++;
                break;
            }
            if (color == RayColor.CYAN) {
                CYAN++;
                break;
            }
            if (color == RayColor.MAGENTA) {
                MAGENTA++;
                break;
            }
            if (color == RayColor.NONE) {
                NONE++;
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
        if (MAGENTA > 0) sb.Append ("MAGENTA x").Append (MAGENTA).Append (Environment.NewLine);
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

    public void InitializeGoButton() {
        GameObject go = new GameObject ();
        GoButton = go.AddComponent<Button>();
        go.transform.SetParent (this.transform);
        if (GUI.Button (new Rect(Screen.width - 100, Screen.height - 100, 50, 50), "Error 404")) {
            OnGoButtonClick ();
        }
        GoButton.gameObject.SetActive (false);
        GoButton.onClick.AddListener (OnGoButtonClick);
    }

    private void OnGoButtonClick() {
        // Go to next state
        StartPlayingPhase ();
    }

    public void InitializeTexts() {
        GameObject go = new GameObject ();
        WaveText = go.AddComponent<Text>();
        go.transform.SetParent (this.transform);
        WaveText.alignment = TextAnchor.UpperLeft;

        GameObject go2 = new GameObject ();
        Lives = go2.AddComponent<Text>();
        go2.transform.SetParent (this.transform);
        Lives.alignment = TextAnchor.UpperCenter;
    }
}
