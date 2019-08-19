﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//TRADUCIDO(FALTA TRADUCIR EL NOMBRE DE LA CLASE)

public class GameManager : MonoBehaviour
{


    // Use this for initialization
    public Text TextTimeOfAttack;
    public static GameManager instanceGameManager;
    public float timeSelectionAttack;
    [HideInInspector]
    public float auxTimeSelectionAttack;
    private List<Enemy> enemies;
    private Player player1;
    private Player player2;
    private Player player3;
    private Player player4;
    [HideInInspector]
    public float auxTimeOFF;
    public bool SiglePlayer;
    public bool MultiPlayer;//(EN CASO DE TENER MULTYPLAYER EL JUEGO SE TRANFORMA EN UN JUEGO POR TURNOS)



    private void Awake()
    {
        if (instanceGameManager == null)
        {
            instanceGameManager = this;
        }
        else if (instanceGameManager != null)
        {
            gameObject.SetActive(false);
        }

    }
    private void Start()
    {
        auxTimeSelectionAttack = timeSelectionAttack;
        enemies = new List<Enemy>();
        
        for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++) {
            if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Enemy")
            {
                if (SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Enemy>() != null)
                {
                    enemies.Add(SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Enemy>());
                }
            }
            if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Player")
            {
                if (SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>() != null)
                {
                    player1 = SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>();
                }
            }
            if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Player2")
            {
                if (SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>() != null)
                {
                    player2 = SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>();
                }

            }
            if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Player3")
            {
                if (SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>() != null)
                {
                    player3 = SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>();
                }
            }
            if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Player4")
            {
                if (SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>() != null)
                {
                    player4 = SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>();
                }
            }
        }
        for (int i = 0; i < enemies.Count; i++) {
            enemies[i].mover = false;
        }
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        if (TextTimeOfAttack != null)
        {
            CheckTimeAttackCharacters();
        }
        if(enemies.Count > 0 && timeSelectionAttack > -2 && timeSelectionAttack <= 0)
        {
            enemies.Clear();
            timeSelectionAttack = -2;
            
            if (timeSelectionAttack <= 0)
            {
                for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++)
                {
                    if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Enemy")
                    {
                        if (SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Enemy>() != null)
                        {
                            enemies.Add(SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Enemy>());
                        }
                    }
                    if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Player")
                    {
                        if (SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>() != null)
                        {
                            player1 = SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>();
                        }
                    }
                    if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Player2")
                    {
                        if (SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>() != null)
                        {
                            player2 = SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>();
                        }

                    }
                    if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Player3")
                    {
                        if (SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>() != null)
                        {
                            player3 = SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>();
                        }
                    }
                    if (SceneManager.GetActiveScene().GetRootGameObjects()[i].tag == "Player4")
                    {
                        if (SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>() != null)
                        {
                            player4 = SceneManager.GetActiveScene().GetRootGameObjects()[i].GetComponent<Player>();
                        }
                    }

                }
                CheckMovementCharcaters();
            }
        }

    }
    private void CheckTimeAttackCharacters() {
        if (timeSelectionAttack > 0)
        {
            timeSelectionAttack = timeSelectionAttack - Time.deltaTime;
            TextTimeOfAttack.text = "" + (int)timeSelectionAttack;
        }
    }
    public void CheckMovementCharcaters()
    {
        if (SiglePlayer)
        {
            //Debug.Log("ENTRE");
            for (int i = 0; i < enemies.Count; i++)
            {
                if (player1 != null && enemies[i] != null)
                {
                    if (enemies[i].typeEnemy == Enemy.Categoria.Balanceado)
                    {
                        if (enemies[i].gameObject.activeSelf)
                        {
                            //Debug.Log("ATAQUE CABEZA JUGADOR: " + player1.GetAtaqueCabeza());
                            //Debug.Log("AGACHARSE ENEMIGO: " + enemies[i].GetAgacharse());
                            if (player1.GetAtaqueCabeza() && enemies[i].GetAgacharse())
                            {
                                //Debug.Log("CONTRA ATAQUE");
                                enemies[i].CounterAttack();
                            }
                            else if (player1.GetAtaquePies() && enemies[i].GetSaltar())
                            {
                                
                                enemies[i].CounterAttack();
                            }
                        }
                    }
                }
            }
        }
        if (MultiPlayer)
        {
            //PROGRAMAR LA REACCION DEL ENEMIGO ANTE UN ESQUIVE DE ATAQUE SI FUERAN DOS JUGADORES 
        }
    }

}
    

//TRADUCIDO(FALTA TRADUCIR EL NOMBRE DE LA CLASE)
