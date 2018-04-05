﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    public float speed = 10;
    public int damage = 50;
    public GameObject target;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public RayColor color;

    private float distance;
    private float startTime;

    private GameManager gameManager;
    // Use this for initialization
    void Start () {
        startTime = Time.time;
        distance = Vector2.Distance(startPosition, targetPosition);
        GameObject gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        // 1 
        float timeInterval = Time.time - startTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, timeInterval * speed / distance);

        // 2 
        if (gameObject.transform.position.Equals(targetPosition))
        {
            if (target != null)
            {
                // 3
                Transform healthBarTransform = target.transform.Find("HealthBar");
                HealthBar healthBar =
                    healthBarTransform.gameObject.GetComponent<HealthBar>();
                healthBar.currentHealth -= Mathf.Max(damage, 0);
                // 4
                if (healthBar.currentHealth <= 0)
                {
                    Destroy(target);
                    //AudioSource audioSource = target.GetComponent<AudioSource>();
                    //AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                }
            }
            Destroy(gameObject);
        }
    }
}
