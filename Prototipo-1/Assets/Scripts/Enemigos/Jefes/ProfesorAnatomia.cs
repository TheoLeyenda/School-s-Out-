﻿using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProfesorAnatomia : Enemy
{
    public enum MyAnimations
    {
        MasiveAttack,
        Terremoto,
        PunietazoDeFuria,
        Braggart,
        FinishBraggart,
        Death,
        Count,
    }

    [Header("Config Profesor Anatomia")]
    public bool ChargeInBraggartState;
    public float speedChargeSpecialAttack;
    public float delayBraggart;
    public float delayFinishBraggart;
    public float delayAttackPunietazoDeFuria;
    //private float auxDelayAttackPunietazoDeFuria;
    private float auxDelayBraggart;
    private float auxDelayFinishBraggart;
    public SpriteBoss_ProfesorAnatomia spriteBoss_ProfesorAnatomia;
    public List<string> NameAnimations;
    public GameObject GeneratorSpecialAttack;
    public int countRepetitionSpecialAttack;
    private int auxCountRepetitionSpecialAttack;
    [HideInInspector]
    public bool NextSpecialAttack;
    public Pool PoolAnatomiaPunch;
    public ObjectTerremoto ObjectTerremoto;
    public int DamageAnatomiaPunch;
    public int DamageTerremoto;
    public float timeLifeTerremoto;

    [Header("Type Attack")]
    public float PorcentageHorizontalAttack;
    public float PorcentageParabolaAttack;

    [Header("Height Attack")]
    public float PorcentageMediumHeightAttack;
    public float PorcentageDownHeightAttack;

    [Header("Target Parabola")]
    public float porcentageLeftTarget;
    public float porcentageCentralTarget;
    public float porcentageRightTarget;

    [Header("Porcentage Seleccion Special Attack")]
    public float porcentageSpecialAttack_1;
    public float porcentageSpecialAttack_2;

    [Header("Effects - CameraShake")]
    public CameraShake cameraShake;
    float durationCameraShake = 0.45f;
    float magnitudeCameraShake = 0.4f;

    public static event Action<ProfesorAnatomia, float, int,bool> OnInitTrowSpecialAttackTerremoto;

    [HideInInspector]
    public bool initBraggert = false;
    public enum EstadoProfesorAnatomia
    {
        Idle,
        MasiveAttack,
        ThrowSpecialAttack,
        Braggart,
        FinishBraggart,
        Death,
        Count
    }
    //HAGO UN ENUM DE Eventos
    public enum EventosProfesorAnatomia
    {
        StartMasiveAttack,
        SpecialAttackReady,
        FinishSpecialAttack,
        LifeOut,
        Count
    }
    [HideInInspector]
    public FSM fsmProfesorAnatomia;

    private void Awake()
    {
        /*
        PATRÓN: Aparece detrás de una cortina de humo y comienza el diálogo. 
       Una vez termina el diálogo: 
       -Comienza a disparar frenéticamente sus proyectiles al azar 
       (en parábola por arriba, en parábola por abajo, adelante carril del medio adelante carril abajo)
       -A medida que pasa el tiempo se carga lentamente con una velocidad X su barra de ataque especial.
       -Al cargar su barra especial elije uno de sus dos ataques especial al azar y lo tira.
       -Una vez tirado se pone en pose epica y presume un poco, luego de que pasan X cantidad de segundos grita 
       “TE APLASTARE MOCOSO” y se pone en pose agresivo y vuelve al ataque.
       -Solo podes hacerle daño y cargar tu ataque especial si le disparas a la cabeza disparos al pecho 
       no cargas nada de tu ataque especial y te rebotan los proyectiles

       */
        fsmProfesorAnatomia = new FSM((int)EstadoProfesorAnatomia.Count, (int)EventosProfesorAnatomia.Count, (int)EstadoProfesorAnatomia.Idle);

        fsmProfesorAnatomia.SetRelations((int)EstadoProfesorAnatomia.Idle, (int)EstadoProfesorAnatomia.MasiveAttack, (int)EventosProfesorAnatomia.StartMasiveAttack);
        fsmProfesorAnatomia.SetRelations((int)EstadoProfesorAnatomia.MasiveAttack, (int)EstadoProfesorAnatomia.ThrowSpecialAttack, (int)EventosProfesorAnatomia.SpecialAttackReady);
        fsmProfesorAnatomia.SetRelations((int)EstadoProfesorAnatomia.ThrowSpecialAttack, (int)EstadoProfesorAnatomia.Braggart, (int)EventosProfesorAnatomia.FinishSpecialAttack);
        fsmProfesorAnatomia.SetRelations((int)EstadoProfesorAnatomia.Braggart, (int)EstadoProfesorAnatomia.MasiveAttack, (int)EventosProfesorAnatomia.StartMasiveAttack);

        fsmProfesorAnatomia.SetRelations((int)EstadoProfesorAnatomia.MasiveAttack, (int)EstadoProfesorAnatomia.Death, (int)EventosProfesorAnatomia.LifeOut);
        fsmProfesorAnatomia.SetRelations((int)EstadoProfesorAnatomia.ThrowSpecialAttack, (int)EstadoProfesorAnatomia.Death, (int)EventosProfesorAnatomia.LifeOut);
        fsmProfesorAnatomia.SetRelations((int)EstadoProfesorAnatomia.Braggart, (int)EstadoProfesorAnatomia.Death, (int)EventosProfesorAnatomia.LifeOut);

    }
    private void OnEnable()
    {
        Grid.OnSettingTitileo += SetTargetGrid;
    }
    private void OnDisable()
    {
        Grid.OnSettingTitileo -= SetTargetGrid;
    }
    public override void Start()
    {
        NextSpecialAttack = true;
        auxDelayBraggart = delayBraggart;
        auxDelayFinishBraggart = delayFinishBraggart;
        //auxDelayAttackPunietazoDeFuria = delayAttackPunietazoDeFuria;
        auxCountRepetitionSpecialAttack = countRepetitionSpecialAttack;
        base.Start();
        if (speedChargeSpecialAttack <= 0)
        {
            speedChargeSpecialAttack = 1;
        }

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        switch (fsmProfesorAnatomia.GetCurrentState())
        {
            case (int)EstadoProfesorAnatomia.Idle:
                Idle();
                break;
            case (int)EstadoProfesorAnatomia.MasiveAttack:
                MasiveAttack();
                break;
            case (int)EstadoProfesorAnatomia.ThrowSpecialAttack:
                ThrowSpecialAttack();
                break;
            case (int)EstadoProfesorAnatomia.Braggart:
                Braggart();
                break;
            case (int)EstadoProfesorAnatomia.Death:
                Death();
                break;
        }

        if (fsmProfesorAnatomia.GetCurrentState() != (int)EstadoProfesorAnatomia.Braggart || ChargeInBraggartState)
        {
            ChargeSpecialAttack();
        }

        if (life <= 0)
        {
            fsmProfesorAnatomia.SendEvent((int)EventosProfesorAnatomia.LifeOut);
        }
    }

    protected void MasiveAttack()
    {
        if (delayAttack > 0)
        {
            delayAttack = delayAttack - Time.deltaTime;
        }
        else
        {
            delayAttack = auxDelayAttack;
            spriteBoss_ProfesorAnatomia.PlayAnimation(NameAnimations[(int)MyAnimations.MasiveAttack]);
        }

        if (xpActual >= xpNededSpecialAttack) 
        {
            fsmProfesorAnatomia.SendEvent((int)EventosProfesorAnatomia.SpecialAttackReady);
        }
    }
    public void Idle() 
    {
        //CAMBIAR ESTO POR LAS ANIMACIONES DE LAS POSES QUE ESTARAN DURANTE EL DIALOGO ANTES DE LA PELEA
        fsmProfesorAnatomia.SendEvent((int)EventosProfesorAnatomia.StartMasiveAttack);
    }
    public void ThrowSpecialAttack()
    {
        //ARMAR LAS ANIMACIONES
        float porcentage = UnityEngine.Random.Range(0, 100);
        //SACAR LUEGO
        //porcentage = 10;
        //-------------//
        if (porcentage <= porcentageSpecialAttack_1)
        {
            //Ataque especial 1
            if (countRepetitionSpecialAttack >= 0)
            {
                if (NextSpecialAttack)
                {
                    spriteBoss_ProfesorAnatomia.PlayAnimation(NameAnimations[(int)MyAnimations.PunietazoDeFuria]);
                    NextSpecialAttack = false;
                    countRepetitionSpecialAttack--;
                }
            }
            else 
            {
                fsmProfesorAnatomia.SendEvent((int)EventosProfesorAnatomia.FinishSpecialAttack);
                NextSpecialAttack = true;
                countRepetitionSpecialAttack = auxCountRepetitionSpecialAttack;
            }
        }
        else
        {
            //Ataque especial 2
            spriteBoss_ProfesorAnatomia.PlayAnimation(NameAnimations[(int)MyAnimations.Terremoto]);
            //Se llama al evento FinishSpecialAttack una vez termino la animacion de ataque especial.
        }

    }
    public void Braggart()
    {
        if (!initBraggert)
        {
            if (delayBraggart > 0)
            {
                delayBraggart = delayBraggart - Time.deltaTime;
                spriteBoss_ProfesorAnatomia.PlayAnimation(NameAnimations[(int)MyAnimations.Braggart]);
            }
            else
            {
                if (delayFinishBraggart > 0)
                {
                    delayFinishBraggart = delayFinishBraggart - Time.deltaTime;
                    spriteBoss_ProfesorAnatomia.PlayAnimation(NameAnimations[(int)MyAnimations.FinishBraggart]);
                }
                else
                {
                    delayBraggart = auxDelayBraggart;
                    delayFinishBraggart = auxDelayFinishBraggart;
                    fsmProfesorAnatomia.SendEvent((int)EventosProfesorAnatomia.StartMasiveAttack);
                }
            }
        }
        else 
        {
            spriteBoss_ProfesorAnatomia.PlayAnimation("InitBraggart");
        }
    }
    public void Death()
    {
        spriteBoss_ProfesorAnatomia.PlayAnimation(NameAnimations[(int)MyAnimations.Death]);
    }
    public void SetTargetGrid(Grid g, Vector3 target)
    {
        if (g != null) 
        {
            GeneratorSpecialAttack.transform.position = target;
        }
    }
    public void InitSpecialAttack_PunietazoDeFuria() 
    {
        int numberCasilla = UnityEngine.Random.Range(1, 4);
        if (OnInitTrowSpecialAttackTerremoto != null)
        {
            OnInitTrowSpecialAttackTerremoto(this, delayAttackPunietazoDeFuria, numberCasilla,false);
        }
        xpActual = 0;
    }
    public void InitSpecialAttack_Terremoto()
    {
        int numberCasilla = 3;
        if (OnInitTrowSpecialAttackTerremoto != null)
        {
            OnInitTrowSpecialAttackTerremoto(this, delayAttackPunietazoDeFuria, numberCasilla, true);
        }
        xpActual = 0;
    }
    public void SpecialAttack_PunietazoDeFuria()
    {
        //GENERA LA MANO EN LA POSICION GeneratorAttackTerremoto
        GameObject go = PoolAnatomiaPunch.GetObject();
        if (go != null) 
        {
            AnatomiaPunch anatomiaPunch = go.GetComponent<AnatomiaPunch>();
            go.transform.position = GeneratorSpecialAttack.transform.position;
            anatomiaPunch.damage = DamageAnatomiaPunch;
            anatomiaPunch.PunchAnimation();
        }
    }
    public void SpecialAttack_Terremoto()
    {
        
        ObjectTerremoto.transform.position = GeneratorSpecialAttack.transform.position;
        ObjectTerremoto.damage = DamageTerremoto;
        ObjectTerremoto.timeLife = timeLifeTerremoto;
        ObjectTerremoto.gameObject.SetActive(true);
        StartCoroutine(cameraShake.Shake(durationCameraShake, magnitudeCameraShake));
    }
    public void BossAttack()
    {

        float porcentage = UnityEngine.Random.Range(0, 100);
        //SACAR LUEGO
        //porcentage = 99;
        //-------------//
        if (porcentage <= PorcentageHorizontalAttack)
        {
            //ENTRO AL HORIZONTAL ATTACK
            porcentage = UnityEngine.Random.Range(0, 100);
            if (porcentage <= PorcentageMediumHeightAttack)
            {
                //ENTRO A ATAQUE POR EL MEDIO
                Attack(false);

            }
            else
            {
                //ENTRO A ATAQUE POR ABAJO
                Attack(true);

            }
        }
        else
        {
            //ENTRO AL PARABOLA ATTACK
            porcentage = UnityEngine.Random.Range(0, 100);
            if (porcentage <= PorcentageMediumHeightAttack)
            {
                //ENTRO A ATAQUE POR EL MEDIO
                porcentage = UnityEngine.Random.Range(0, 100);
                if (porcentage <= porcentageLeftTarget)
                {
                    //TARGET IZQUIERDO
                    SetIsJumping(false);
                    SetIsDuck(false);
                    CheckSpecialAttackEnemyController(0, 0, generadorProyectilParabola);
                }
                else if (porcentage > porcentageLeftTarget && porcentage <= porcentageLeftTarget + porcentageCentralTarget)
                {
                    //TARGET CENTRAL
                    SetIsJumping(false);
                    SetIsDuck(false);
                    CheckSpecialAttackEnemyController(1, 1, generadorProyectilParabola);
                }
                else
                {
                    //TARGET DERECHO
                    SetIsJumping(false);
                    SetIsDuck(false);
                    CheckSpecialAttackEnemyController(2, 2, generadorProyectilParabola);
                }
            }
            else
            {
                //ENTRO A ATAQUE POR ABAJO
                porcentage = UnityEngine.Random.Range(0, 100);
                if (porcentage <= porcentageLeftTarget)
                {
                    //TARGET IZQUIERDO
                    SetIsJumping(false);
                    SetIsDuck(true);
                    CheckSpecialAttackEnemyController(0, 0, generadorProyectilParabolaAgachado);
                }
                else if (porcentage > porcentageLeftTarget && porcentage <= porcentageLeftTarget + porcentageCentralTarget)
                {
                    //TARGET CENTRAL
                    SetIsJumping(false);
                    SetIsDuck(true);
                    CheckSpecialAttackEnemyController(1, 1, generadorProyectilParabolaAgachado);
                }
                else
                {
                    //TARGET DERECHO
                    SetIsJumping(false);
                    SetIsDuck(true);
                    CheckSpecialAttackEnemyController(2, 2, generadorProyectilParabolaAgachado);
                }
            }
        }
    }
    public void Attack(bool DuckActtack)
    {
        GameObject go = null;
        Proyectil proyectil = null;
        Proyectil.typeProyectil tipoProyectil = Proyectil.typeProyectil.Nulo;

        go = poolObjectAttack.GetObject();
        proyectil = go.GetComponent<Proyectil>();
        proyectil.SetEnemy(gameObject.GetComponent<Enemy>());
        proyectil.SetDobleDamage(false);
        proyectil.disparadorDelProyectil = Proyectil.DisparadorDelProyectil.Enemigo;

        switch (applyColorShoot)
        {
            case ApplyColorShoot.None:
                break;
            case ApplyColorShoot.Proyectil:
                proyectil.SetColorProyectil(colorShoot);
                break;
            case ApplyColorShoot.Stela:
                proyectil.SetColorStela(colorShoot);
                break;
            case ApplyColorShoot.StelaAndProyectil:
                proyectil.SetColorProyectil(colorShoot);
                proyectil.SetColorStela(colorShoot);
                break;
        }
        if (DuckActtack)
        {
            tipoProyectil = Proyectil.typeProyectil.ProyectilBajo;
            go.transform.rotation = generadorProyectilesAgachado.transform.rotation;
            go.transform.position = generadorProyectilesAgachado.transform.position;
            proyectil.posicionDisparo = Proyectil.PosicionDisparo.PosicionBaja;
        }
        else 
        {
            tipoProyectil = Proyectil.typeProyectil.ProyectilNormal;
            go.transform.rotation = generadoresProyectiles.transform.rotation;
            go.transform.position = generadoresProyectiles.transform.position;
            proyectil.posicionDisparo = Proyectil.PosicionDisparo.PosicionMedia;
        }
        if (applyColorShoot == ApplyColorShoot.None || applyColorShoot == ApplyColorShoot.Stela)
        {
            proyectil.On(tipoProyectil, false);
        }
        else
        {
            proyectil.On(tipoProyectil, true);
        }
        proyectil.ShootForward();
    }
    public void ChargeSpecialAttack() 
    {
        xpActual = xpActual + Time.deltaTime * speedChargeSpecialAttack;
    }
    public void CounterAttack() 
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

}
