using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_speed = 1f;
    [SerializeField] private Material m_normalMaterial = null;
    [SerializeField] private Material m_boostingMaterial = null;

    private Rigidbody m_playerRigidbody;

    private float m_movementX;
    private float m_movementY;

    private int m_collectablesTotalCount, m_collectabesCounter;

    private Stopwatch m_stopwatch;

    public Text scoreText;

    public Text bestTime;
    public Text timeScore;
    public bool timerActive = true;
    public float timeTaken;
    public float sceneBestTime = 1000f;

    private float boostTimer;
    private bool boosting;
    public float duration;

    private void Start()
    {
        m_playerRigidbody = GetComponent<Rigidbody>();

        m_collectablesTotalCount = m_collectabesCounter = GameObject.FindGameObjectsWithTag("Collectable").Length;

        m_stopwatch = Stopwatch.StartNew();

        UpdateScoreText();

        bestTime.text = PlayerPrefs.GetFloat("HighScore", 0).ToString();
        sceneBestTime = PlayerPrefs.GetFloat("CurrentBestTime", sceneBestTime);

        boostTimer = 0;
        boosting = false;
    }

    private void OnMove(InputValue inputValue)
    {
        Vector2 movementVector = inputValue.Get<Vector2>();

        m_movementX = movementVector.x;
        m_movementY = movementVector.y;
    }


    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(m_movementX, 0f, m_movementY);
        
        m_playerRigidbody.AddForce(movement * m_speed);

        if (timerActive)
        {
            timeTaken += Time.deltaTime;
            timeScore.text = timeTaken.ToString();
        }

        if(boosting)
        {
            boostTimer += Time.deltaTime;
            if(boostTimer >= duration)
            {
                m_speed = (m_speed + 19);
                boostTimer = 0;
                boosting = false;
                m_playerRigidbody.GetComponent<Renderer>().material = m_normalMaterial;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false);

            m_collectabesCounter--;
            if(m_collectabesCounter == 0)
            {
                UnityEngine.Debug.Log("You Win!");
                UnityEngine.Debug.Log($"It took you {m_stopwatch.Elapsed} to find all {m_collectablesTotalCount} collectables.");
                StopTimer();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#endif
            }
            else
            {
                UnityEngine.Debug.Log($"You've already found {m_collectablesTotalCount - m_collectabesCounter} of {m_collectablesTotalCount} Collectables!");
                UpdateScoreText();
            }


        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            UnityEngine.Debug.Log("Game Over!");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }

        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            boosting = true;
            m_speed = (m_speed - 19);
            Destroy(other.gameObject);
            m_playerRigidbody.GetComponent<Renderer>().material = m_boostingMaterial;
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = (m_collectablesTotalCount - m_collectabesCounter) + "/" + m_collectablesTotalCount;
    }
    void StopTimer()
    {
        timerActive = false;
        if (timeTaken < sceneBestTime)
        {
            bestTime.text = timeTaken.ToString();
            PlayerPrefs.SetFloat("CurrentBestTime", timeTaken);
            PlayerPrefs.SetFloat("HighScore", timeTaken);
            UnityEngine.Debug.Log("You got the best Time!");
        }
    }
}
