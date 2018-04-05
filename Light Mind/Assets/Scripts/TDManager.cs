using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Utilities;
using Behaviors;
using UnityEngine;
using UnityEngine.UI;

public class TDManager : MonoBehaviour
{
    // Not used yet, might delete
    public enum State
    {
        Playing,
        Pause,
        Win,
        Lose
    }

    public State GameState;
    public int WavesTotal;
    public int LivesLeft;
    public int CurrentWave;
    public Dictionary<char, RayColor> WavesDico;

    public float SpawnInterval;

    private GameObject _spawnPoint;
    private List<List<RayColor>> _enemyWaves = new List<List<RayColor>>();
    private int _enemiesSpawned;

    // GUI
    public GameObject GoButton;
    public Text LivesText;
    public Text WaveText;

    public void SetUpWaves(JSONObject data)
    {
        // Init the dictionary with all char and corresponding colors
        InitializeWavesDico();

        // Set listener to wave launcher button
        GoButton.GetComponent<Button>().onClick.AddListener(OnGoButtonClick);

        foreach (var jsonEntity in data["Waves"].list)
        {
            DecodeEnemyWaves(jsonEntity["Enemies"].str);
        }

        LivesLeft = (int) data["Info"].GetField("Lives").i;
        SpawnInterval = data["Info"].GetField("SpawnInterval").n;
        WavesTotal = _enemyWaves.Count;
        _spawnPoint = GameObject.FindGameObjectWithTag("Spawn Point");
    }

    public void StartGame()
    {
        StartPausedPhase();
    }

    private void StartPlayingPhase()
    {
        // Change state
        GameState = State.Playing;
        // Hide go button
        GoButton.gameObject.SetActive(false);
        // Update current wave
        WaveText.text = string.Format("Wave : {0}/{1}", CurrentWave, WavesTotal);
        // Lives
        LivesText.text = string.Format("Lives : {0}", LivesLeft);
        // Call spawner
        StartNextWave();
        // Check if enemies are all dead or player is
        // THIS SHOULD BE CHECKED WHEN A TOWER FIRES AND WHEN AN ENEMY TAKES A LIFE POINT OFF
        // Call CallNextPhase or StartPausePhase
    }

    private void StartPausedPhase()
    {
        CurrentWave++;
        // Change state
        GameState = State.Pause;
        // Display next wave
        WaveText.text = string.Format("Next Wave: {0}{1}", Environment.NewLine,
            ShowNextWave(_enemyWaves[CurrentWave - 1]));
        // Show lives left
        LivesText.text = string.Format("Lives : {0}", LivesLeft);
        // Show button Go
        GoButton.gameObject.SetActive(true);
    }

    public void DecreaseLives()
    {
        LivesLeft--;
        LivesText.text = string.Format("Lives : {0}", LivesLeft);

        if (LivesLeft > 0) return;
        
        // TODO Stop the game and display loss screen
        Debug.Log("Player loses the game!");
        LivesLeft = 0;
    }

    public void StartWave(List<RayColor> wave)
    {
        StartCoroutine(SpawnEnemies(wave));
    }

    private void Update()
    {
        if (_enemiesSpawned <= 0 || GameState != State.Playing) return;

        var enemies = GameObject.FindGameObjectsWithTag("enemy");
        if (enemies.Length == 0)
        {
            CallNextPhase();
        }
    }

    private string ShowNextWave(IEnumerable<RayColor> wave)
    {
        int red, white, blue, yellow, green, cyan, magenta, none;
        red = white = blue = yellow = green = cyan = magenta = none = 0;
        foreach (var color in wave)
        {
            if (color == RayColor.RED)
            {
                red++;
                continue;
            }

            if (color == RayColor.WHITE)
            {
                white++;
                continue;
            }

            if (color == RayColor.BLUE)
            {
                blue++;
                continue;
            }

            if (color == RayColor.YELLOW)
            {
                yellow++;
                continue;
            }

            if (color == RayColor.GREEN)
            {
                green++;
                continue;
            }

            if (color == RayColor.CYAN)
            {
                cyan++;
                continue;
            }

            if (color == RayColor.MAGENTA)
            {
                magenta++;
                continue;
            }

            if (color == RayColor.NONE)
            {
                none++;
            }
        }

        StringBuilder sb = new StringBuilder();
        if (red > 0) sb.Append("RED x").Append(red).Append(Environment.NewLine);
        if (blue > 0) sb.Append("BLUE x").Append(blue).Append(Environment.NewLine);
        if (yellow > 0) sb.Append("YELLOW x").Append(yellow).Append(Environment.NewLine);
        if (cyan > 0) sb.Append("CYAN x").Append(cyan).Append(Environment.NewLine);
        if (green > 0) sb.Append("GREEN x").Append(green).Append(Environment.NewLine);
        if (white > 0) sb.Append("WHITE x").Append(white).Append(Environment.NewLine);
        if (magenta > 0) sb.Append("MAGENTA x").Append(magenta).Append(Environment.NewLine);
        if (none > 0) sb.Append("COLOR-IMMUNE x").Append(none).Append(Environment.NewLine);
        Debug.Log(sb);
        return sb.ToString();
    }

    private void StartNextWave()
    {
        // Might add more things
        StartWave(_enemyWaves[CurrentWave - 1]);
    }

    // Might delete if utility is low
    public void CallNextPhase()
    {
        switch (GameState)
        {
            case State.Playing:
                StartPausedPhase();
                break;
            case State.Pause:
                StartPlayingPhase();
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

    private void InitializeWavesDico()
    {
        WavesDico = new Dictionary<char, RayColor>
        {
            {'R', RayColor.RED},
            {'B', RayColor.BLUE},
            {'W', RayColor.WHITE},
            {'C', RayColor.CYAN},
            {'G', RayColor.GREEN},
            {'M', RayColor.MAGENTA},
            {'Y', RayColor.YELLOW},
            {'N', RayColor.NONE}
        };
    }

    private void DecodeEnemyWaves(string encodedWave)
    {
        List<RayColor> waveColors = new List<RayColor>();
        RayColor previousColor = RayColor.NONE;

        for (int i = 0; i < encodedWave.Length; i++)
        {
            char currChar = encodedWave[i];
            if (WavesDico.ContainsKey(currChar))
            {
                WavesDico.TryGetValue(currChar, out previousColor);
                waveColors.Add(previousColor);
            }
            else
            {
                string countStr = "";
                while (i < encodedWave.Length && !WavesDico.ContainsKey(encodedWave[i]))
                {
                    countStr += currChar;
                    if (++i < encodedWave.Length)
                    {
                        currChar = encodedWave[i];
                    }
                }

                i--;
                int count = 0;
                if (countStr.Length > 0) count = Int32.Parse(countStr);
                while (count > 1)
                {
                    //R2 = R + R since we already added R before, only go to > 1
                    waveColors.Add(previousColor);
                    count--;
                }
            }
        }

        _enemyWaves.Add(waveColors);
    }

    private void OnGoButtonClick()
    {
        // Go to next state
        StartPlayingPhase();
    }

    private IEnumerator SpawnEnemies(IList<RayColor> wave)
    {
        _enemiesSpawned = 0;
        Debug.Log(string.Format("Wave Count: {0}", wave.Count));
        while (_enemiesSpawned < wave.Count)
        {
            var enemyGo = Instantiate(GameManager.Instance.EnemyPrefab, _spawnPoint.transform.position,
                Quaternion.identity);
            Debug.Log("Spawned");
            enemyGo.GetComponent<EnemyBehaviour>().Color = wave[_enemiesSpawned];
            _enemiesSpawned++;
            yield return new WaitForSeconds(SpawnInterval);
        }

        StopAllCoroutines();
    }
}