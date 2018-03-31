using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Utilities;
using TD.Enemy;
using UnityEngine;
using UnityEngine.UI;

public class TDManager : MonoBehaviour {
    // Not used yet, might delete
    public enum State {Playing, Pause, Win, Lose}

    public Spawn Spawn;

    public State GameState;
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

    public void StartGame() {
        // Might add more things here
        Spawn = FindObjectOfType<Spawn> ();
        Spawn.SpawnInterval = _spawnInterval;
        CurrentWave = 0;
        StartPausedPhase ();
    }

    private void StartPlayingPhase() {
        // Change state
        GameState = State.Playing;
        // Hide go button
        GoButton.gameObject.SetActive (false);
        // Update current wave
        WaveText.text = "Wave : " + CurrentWave + "/" + WavesTotal;
        // Lives
        LivesText.text = "Lives : " + LivesLeft;
        // Call spawner
        StartNextWave();
        // Check if enemies are all dead or player is
        // THIS SHOULD BE CHECKED WHEN A TOWER FIRES AND WHEN AN ENEMY TAKES A LIFE POINT OFF
        // Call CallNextPhase or StartPausePhase
    }

    private void StartPausedPhase() {
        CurrentWave++;
        // Change state
        GameState = State.Pause;
        // Display next wave
        WaveText.text = "Next Wave: " + Environment.NewLine + ShowNextWave(_enemyWaves[CurrentWave - 1]);
        // Show button Go
        GoButton.gameObject.SetActive (true);
    }

    string ShowNextWave(IEnumerable<RayColor> wave) {
        int red, white, blue, yellow, green, cyan, magenta, none;
        red = white = blue = yellow = green = cyan = magenta = none = 0;
        foreach (var color in wave) {
            if (color == RayColor.RED) {
                red++;
                continue;
            }
            if (color == RayColor.WHITE) {
                white++;
                continue;
            }
            if (color == RayColor.BLUE) {
                blue++;
                continue;
            }
            if (color == RayColor.YELLOW) {
                yellow++;
                continue;
            }
            if (color == RayColor.GREEN) {
                green++;
                continue;
            }
            if (color == RayColor.CYAN) {
                cyan++;
                continue;
            }
            if (color == RayColor.MAGENTA) {
                magenta++;
                continue;
            }
            if (color == RayColor.NONE) {
                none++;
            }
        }

        StringBuilder sb = new StringBuilder ();
        if (red > 0) sb.Append ("RED x").Append (red).Append (Environment.NewLine);
        if (blue > 0) sb.Append ("BLUE x").Append (blue).Append (Environment.NewLine);
        if (yellow > 0) sb.Append ("YELLOW x").Append (yellow).Append (Environment.NewLine);
        if (cyan > 0) sb.Append ("CYAN x").Append (cyan).Append (Environment.NewLine);
        if (green > 0) sb.Append ("GREEN x").Append(green).Append (Environment.NewLine);
        if (white > 0) sb.Append ("WHITE x").Append (white).Append (Environment.NewLine);
        if (magenta > 0) sb.Append ("MAGENTA x").Append (magenta).Append (Environment.NewLine);
        if (none > 0) sb.Append ("COLOR-IMMUNE x").Append (none).Append (Environment.NewLine);
        Debug.Log (sb);
        return sb.ToString ();
    }

    private void StartNextWave() {
        // Might add more things
        Spawn.StartWave (_enemyWaves[CurrentWave - 1]);
    }

    // Might delete if utility is low
    public void CallNextPhase()
    {
        switch (GameState)
        {
            case State.Playing:
                StartPausedPhase ();
                break;
            case State.Pause:
                StartPlayingPhase ();
                break;
            case State.Lose:
                // TODO
                break;
            case State.Win:
                // TODO
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetUpWaves(JSONObject data)
    {
        // Init the dictionary with all char and corresponding colors
        InitializeWavesDico ();

        // Add GoButton Listener
        GoButton.GetComponent <Button>().onClick.AddListener (OnGoButtonClick);
        
        foreach (var jsonEntity in data["Waves"].list)
        {
            DecodeEnemyWaves(jsonEntity ["Enemies"].str);
        }

        LivesLeft = (int)data ["Info"].GetField ("Lives").n;
        _spawnInterval = data ["Info"].GetField ("SpawnInterval").n;
        WavesTotal = _enemyWaves.Count;
    }
        
    private void DecodeEnemyWaves(string encodedWave) {
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
        if (LivesLeft <= 0) {
            //TODO
        } else {
            LivesText.text = string.Format("Lives : {0}", LivesLeft);
        }
    }
}
