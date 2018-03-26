using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Utilities;

public class TDManager : MonoBehaviour {

    public Spawn Spawn;

    private int _waves;
    private int _lives;
    private float _spawnInterval;
    private List<List<RayColor>> _enemyWaves = new List<List<RayColor>>();
    private Dictionary<char, RayColor> _wavesDico;

    public void Init() {
        InitializeWavesDico ();
    }

    public void StartNextWave() {
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
}
