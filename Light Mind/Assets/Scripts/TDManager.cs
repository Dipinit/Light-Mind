using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Utilities;
using UnityEngine.UI;
using System.Text;

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
    public Dictionary<char, RayColor> WavesDico;

    // GUI
    public Button GoButton;
    public Text LivesText;
    public Text WaveText;

    public void Init() {
        InitializeWavesDico ();
        InitializeGoButton ();
        InitializeTexts ();
    }

    public void StartGame() {
        // Might add more things here
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
        WaveText.text = "Next Wave: " + ShowNextWave(_enemyWaves[CurrentWave]);
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
            if (WavesDico.ContainsKey (currChar)) {
                WavesDico.TryGetValue (currChar, out previousColor);
                waveColors.Add (previousColor);
            } else {
                string countStr = "";
                while (i < encodedWave.Length && !WavesDico.ContainsKey (encodedWave[i])) {
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
        WavesDico = new Dictionary<char, RayColor>();
        WavesDico.Add ('R', RayColor.RED);
        WavesDico.Add ('B', RayColor.BLUE);
        WavesDico.Add ('W', RayColor.WHITE);
        WavesDico.Add ('C', RayColor.CYAN);
        WavesDico.Add ('G', RayColor.GREEN);
        WavesDico.Add ('M', RayColor.MAGENTA);
        WavesDico.Add ('Y', RayColor.YELLOW);
        WavesDico.Add ('N', RayColor.NONE);
    }

    public void InitializeGoButton() {
        GameObject go = new GameObject ();
        GoButton = go.AddComponent<Button>();
        go.transform.SetParent (this.transform);
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
        LivesText = go2.AddComponent<Text>();
        go2.transform.SetParent (this.transform);
        LivesText.alignment = TextAnchor.UpperCenter;
    }

    public void DecreaseLives() {
        LivesLeft--;
        LivesText.text = "Lives : " + LivesLeft;
    }
}
