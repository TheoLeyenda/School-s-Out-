﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUpContainerManager_IA : PowerUpContainer
{
    // Start is called before the first frame update
    public static event Action<PowerUpContainerManager_IA> OnRefreshDataPowerUpUI;
    public static event Action<PowerUpContainerManager_IA> OnNextPowerUpAsigned;
    public int minPowerUpGenerar = 2;
    public int maxPowerUpGenerar = 5;
    public int intentosDeAsignacion;
    private int auxIntentosDeAsignacion;
    public Enemy userEnemy;
    public float minDelayThrowPowerUp = 7;
    public float maxDelayThrowPowerUp = 18;
    [SerializeField]
    private float delayThrowPowerUp;
    [HideInInspector]
    public PowerUp prevPowerUp;
    [HideInInspector]
    public int prevIndex;
    [HideInInspector]
    public bool emptyPowerUps = false;
    private void Awake()
    {
        for (int i = 0; i < powerUpContainerContent.Count; i++)
        {
            if (powerUpContainerContent[i].powerUp != null)
            {
                powerUpContainerContent[i].namePowerUp = powerUpContainerContent[i].powerUp.namePowerUp;
                powerUpContainerContent[i].powerUp.enemy = userEnemy;
            }
        }
    }
    private void OnEnable()
    {
        currentIndexPowerUp = powerUpContainerContent.Count - 1;
        delayThrowPowerUp = UnityEngine.Random.Range(minDelayThrowPowerUp, maxDelayThrowPowerUp);
        PowerUp.OnDisablePowerUpEffect += EnableShootPowerUp;
        Enemy.OnDie += CheckDropPowerUp;
        if (intentosDeAsignacion <= 1)
        {
            intentosDeAsignacion = 10;
        }
        SettingsDataPowerUpInSpawn();
    }
    private void OnDisable()
    {
        PowerUp.OnDisablePowerUpEffect -= EnableShootPowerUp;
        Enemy.OnDie -= CheckDropPowerUp;
    }
    void Start()
    {
        auxIntentosDeAsignacion = intentosDeAsignacion;
        userContainer = ThrowPowerUpController.UserPowerUpController.Enemy;
    }

    void Update()
    {
        DelayEnableThrowPowerUp();
        CheckDelayThrowPowerUp();
    }
    public void ResetCountPowerUps()
    {
        for (int i = 0; i < powerUpContainerContent.Count; i++)
        {
            powerUpContainerContent[i].countPowerUps = 0;
        }
    }
    public void CheckDelayThrowPowerUp()
    {
        if (delayThrowPowerUp > 0)
        {
            delayThrowPowerUp = delayThrowPowerUp - Time.deltaTime;
        }
        else
        {
            delayThrowPowerUp = UnityEngine.Random.Range(minDelayThrowPowerUp, maxDelayThrowPowerUp);
            ThrowPowerUp(currentIndexPowerUp);
        }
    }
    public void EnableShootPowerUp(PowerUp powerUp)
    {
        if (powerUp.userPowerUp == userContainer)
        {
            enableDelay = true;
            if (OnRefreshDataPowerUpUI != null)
            {
                OnRefreshDataPowerUpUI(this);
            }
        }
    }

    public void SettingsDataPowerUpInSpawn()
    {
        ResetCountPowerUps();
        int countPowerUp = UnityEngine.Random.Range(minPowerUpGenerar, maxPowerUpGenerar + 1);
        int countAsignedPowerUp = 0;
        while (countAsignedPowerUp < countPowerUp && intentosDeAsignacion > 0)
        {
            int randomIndexSelected = UnityEngine.Random.Range(1, powerUpContainerContent.Count);
            randomIndexSelected--;

            if (powerUpContainerContent[randomIndexSelected].countPowerUps <= 0 
                && powerUpContainerContent[randomIndexSelected].namePowerUp != "None")
            {
                DeselectAllPowerUps();
                int randomCount = UnityEngine.Random.Range(1, powerUpContainerContent[randomIndexSelected].maxCountPowerUps + 1);
                powerUpContainerContent[randomIndexSelected].countPowerUps = randomCount;
                powerUpContainerContent[randomIndexSelected].currentPowerUp = true;
                currentIndexPowerUp = randomIndexSelected;
                countAsignedPowerUp++;
                emptyPowerUps = false;
            }
            intentosDeAsignacion--;
        }

        for (int i = 0; i < powerUpContainerContent.Count; i++)
        {
            if (i < powerUpContainerContent.Count && powerUpContainerContent[i].powerUp != null)
            {
                powerUpContainerContent[i].powerUp.userPowerUp = userContainer;
            }
        }
    }
    public override void ThrowPowerUp(int index)
    {
        if (powerUpContainerContent[index].countPowerUps <= 0 || emptyPowerUps || index < 0 || index >= powerUpContainerContent.Count)
            return;

        if (powerUpContainerContent[index].namePowerUp != "None")
        {
            bool characterEnableMovement = false;

            Enemy e  = powerUpContainerContent[index].powerUp.enemy;
            if (e == null) return;

            characterEnableMovement = (e.enableMovement || e.enumsEnemy.GetStateEnemy() == EnumsEnemy.EstadoEnemigo.Atrapado);

            if (powerUpContainerContent[index].currentPowerUp
            && powerUpContainerContent[index].countPowerUps > 0
            && characterEnableMovement && enableShootPowerUp)
            {
                powerUpContainerContent[index].powerUp.ActivatedPowerUp();
                powerUpContainerContent[index].countPowerUps--;
                enableShootPowerUp = false;

                prevPowerUp = powerUpContainerContent[index].powerUp;
                prevIndex = index;
                //Debug.Log("POWER UP LANZADO");

                CheckNextPowerUpAssigned();
            }
            if (OnRefreshDataPowerUpUI != null)
            {
                OnRefreshDataPowerUpUI(this);
            }
        }
        //else
        //{
            //if (OnRefreshDataPowerUpUI != null)
            //{
                //currentIndexPowerUp = powerUpContainerContent.Count - 1;
                //OnRefreshDataPowerUpUI(this);
            //}
        //}
    }

    public void CheckNextPowerUpAssigned()
    {
        bool powerUpAsigned = false;
        if (powerUpContainerContent[currentIndexPowerUp].countPowerUps > 0) return;

        powerUpContainerContent[currentIndexPowerUp].currentPowerUp = false;
        if (currentIndexPowerUp == powerUpContainerContent.Count - 1)
        {
            currentIndexPowerUp = 0;
        }
        for (int j = currentIndexPowerUp; j < powerUpContainerContent.Count; j++)
        {
            if (powerUpContainerContent[j].countPowerUps > 0)
            {
                //Debug.Log("ASIGNO EL NUEVO POWER UP");
                powerUpAsigned = true;
                powerUpContainerContent[j].currentPowerUp = true;
                currentIndexPowerUp = j;
                j = powerUpContainerContent.Count;
                if (OnNextPowerUpAsigned != null)
                {
                    OnNextPowerUpAsigned(this);
                }
            }
        }
        if (!powerUpAsigned)
        {
            currentIndexPowerUp = powerUpContainerContent.Count - 1;
            emptyPowerUps = true;
            if (OnNextPowerUpAsigned != null)
            {
                OnNextPowerUpAsigned(this);
            }
        }
    }

    public void CheckDropPowerUp(Enemy e)
    {
        if (e != userEnemy) return;
       
        //FUNCIONALIDAD QUE DROPEA EL POWER UP
        
    }
}