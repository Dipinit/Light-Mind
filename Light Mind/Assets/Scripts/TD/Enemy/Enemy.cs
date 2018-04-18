using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utilities;

/**
 * Contains basic information for Enemy creations.
 **/
public class Enemy {
    public RayColor Color;
    public float Speed;
    public int Hitpoints;
    public float SpawnTime;

    public Enemy (int hitpoints, float speed, RayColor color, float spawnTime) {
        this.Hitpoints = hitpoints;
        this.Speed = speed;
        this.Color = color;
        this.SpawnTime = spawnTime;
    }

    public string toString() {
        return "Hitpoints: " + this.Hitpoints + ", Speed: " + this.Speed + ", Color: " + this.Color.GetName () + ", SpawnTime: " + this.SpawnTime;
    }
}
