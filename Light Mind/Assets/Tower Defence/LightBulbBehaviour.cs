using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightState
{
    public GameObject visualization;
    public GameObject bullet;
    public float fireRate;
}

public class LightBulbBehaviour : MonoBehaviour
{
    public List<LightState> states;
    private LightState currentState;



    public LightState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
            int currentStateIndex = states.IndexOf(currentState);

            GameObject levelVisualization = states[currentStateIndex].visualization;
            for (int i = 0; i < states.Count; i++)
            {
                if (levelVisualization != null)
                {
                    if (i == currentStateIndex)
                    {
                        states[i].visualization.SetActive(true);
                    }
                    else
                    {
                        states[i].visualization.SetActive(false);
                    }
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        CurrentState = states[0];
    }

    public LightState getNextState()
    {
        int currentStateIndex = states.IndexOf(currentState);
        int maxStateIndex = states.Count - 1;
        if (currentStateIndex < maxStateIndex)
        {
            return states[currentStateIndex + 1];
        }
        else
        {
            return null;
        }
    }

    public void increaseState()
    {
        int currentStateIndex = states.IndexOf(currentState);
        if (currentStateIndex < states.Count - 1)
        {
            CurrentState = states[currentStateIndex + 1];
        }
    }

}

