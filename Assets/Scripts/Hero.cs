﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in Inspector")]
    // These fields control the movement of the shi
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 4;

    //This variable holds a referance to the last triggering gameObject
    private GameObject lastTriggerGo = null;

    void Awake()
    {
        if (S == null)
        {
            S = this; // Set the Singleton
        }

        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        // Pull in information from the Input class
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // Rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print("Triggered: " + go.name);

        //Make sure it's the same triggering go as last time
        if (go == lastTriggerGo)
        {
            return;
        }

        lastTriggerGo = go;

        // If the shield was triggered by an enemy
        if (go.tag == "Enemy")
        {
            shieldLevel--; // Decrease the level of the shield by 1
            Destroy(go); // … and Destroy the enemy
        }

        else
        {
            print("Triggered by non-Enemy: " + go);
        }
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }

        set
        {
            _shieldLevel = Mathf.Min(value, 4);

            // If the shield is going to be set to less than zero
            if (value < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}