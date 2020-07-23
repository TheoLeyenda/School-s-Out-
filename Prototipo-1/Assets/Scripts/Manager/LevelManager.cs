﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public bool NivelFinal;
    public string NameFinishSceneStoryMode;
    public GameObject marcoTexto;
    public Image imageHabladorActual;
    //public GameObject imageEnemigoHablando;
    public GameObject CamvasInicioPelea;
    public TextMeshProUGUI textDialog;
    //public TextMeshProUGUI textHabladorActual;
    public List<Dialogos> DialogoInicial;
    private GameManager gm;
    public bool InitDialog;
    [HideInInspector]
    public int ObjectiveOfPassLevel;
    public float delayPassLevel = 4f;
    private float auxDelayPassLevel;
    private int Level;
    private bool inDialog;
    private int idDialogo;
    public bool inSurvival;
    private bool goToGameOver;
    private bool disableOnlyOnce = false;
    
    [System.Serializable]
    public class Dialogos
    {
        public string nombreHabladorActual;
        public string habladorActual;
        public string dialogoPersonaje;
        public Sprite spriteHabladorActual;
    }
    private void OnEnable()
    {
        Player.OnDie += EnableGameOver;
    }
    private void OnDisable()
    {
        Player.OnDie -= EnableGameOver;
    }
    private void Start()
    {
        auxDelayPassLevel = delayPassLevel;
        if (InitDialog)
        {
            inDialog = true;
        }
        if (GameManager.instanceGameManager != null)
        {
            gm = GameManager.instanceGameManager;
        }
        Level = 1;
        ObjectiveOfPassLevel = 1;
        if (InitDialog)
        {
            CheckDiagolos();
        }
    }
    void Update()
    {
        CheckDiagolos();
        if (inSurvival)
        {
            CheckPassGameOver();
        }
        else
        {
            CheckPassGameOver();
            CheckPassLevel();
        }
        if (InputPlayerController.GetInputButtonDown("SelectButton_P1"))
        {
            NextId();
        }
    }
    public void CheckPassLevel()
    {
        if (ObjectiveOfPassLevel <= 0)
        {
            NextLevel();
            gm.playerData_P1.auxScore = gm.playerData_P1.score;
            ObjectiveOfPassLevel = 1;
        }
    }
    public void EnableGameOver(Player p) 
    {
        goToGameOver = true;
    }
    public void CheckPassGameOver()
    {
        if (goToGameOver)
        {
            if (delayPassLevel <= 0)
            {
                goToGameOver = false;
                delayPassLevel = auxDelayPassLevel;
                if (SceneManager.GetActiveScene().name == "Supervivencia")
                {
                    gm.GameOver("GameOverSupervivencia");
                }
                else if (SceneManager.GetActiveScene().name != "PvP" && SceneManager.GetActiveScene().name != "TiroAlBlanco")
                {
                    gm.GameOver("GameOverHistoria");
                }
            }
            else
            {
                delayPassLevel = delayPassLevel - Time.deltaTime;
            }
        }
    }
    public void CheckDiagolos()
    {
        if (idDialogo < DialogoInicial.Count && inDialog)
        {
            Time.timeScale = 0;
            inDialog = true;
            imageHabladorActual.sprite = DialogoInicial[idDialogo].spriteHabladorActual;
            //textHabladorActual.text = " "; ;
            textDialog.text = DialogoInicial[idDialogo].nombreHabladorActual + " " + DialogoInicial[idDialogo].dialogoPersonaje;
        }
        else
        {
            if (inDialog && !InputPlayerController.GetInputButtonDown("JumpButton_P1"))
            {
                Time.timeScale = 1;
            }
            inDialog = false;
            if (!disableOnlyOnce)
            {
                disableOnlyOnce = true;
                DisableChat();
                if (CamvasInicioPelea != null)
                {
                    CamvasInicioPelea.SetActive(true);
                }
            }
           
        }
    }
    public void NextId()
    {
        idDialogo++;
    }
    public void DisableChat()
    { 
            idDialogo = 0;
            marcoTexto.SetActive(false);
    }
    public void NextLevel()
    {
        if (delayPassLevel <= 0)
        {
            delayPassLevel = auxDelayPassLevel;
            if (!NivelFinal)
            {
                gm.screenManager.LoadLevel(gm.screenManager.GetIdListLevel() + 1);
            }
            else
            {
                gm.GameOver(NameFinishSceneStoryMode);
            }
        }
        else 
        {
            delayPassLevel = delayPassLevel - Time.deltaTime;
        }
    }
    public bool GetInDialog()
    {
        return inDialog;
    }
    public void SetInDialog(bool _inDialog)
    {
        inDialog = _inDialog;
    }
}
