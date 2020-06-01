﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour
{
    public ApplyColorShoot applyColorShoot;
    public enum ApplyColorShoot
    {
        Stela,
        Proyectil,
        StelaAndProyectil,
        None,
    }
    public Color colorShoot;
    public string namePlayer;
    //BOOLEANOS DE MOVIMIENTO
    [HideInInspector]
    public bool enableMovement;
    [HideInInspector]
    public bool enableMoveHorizontalPlayer;
    [HideInInspector]
    public bool enableMoveVerticalPlayer;
    [HideInInspector]
    public bool enableMovementPlayer;
    //----------------------//
    //CODIGO DE INPUT
    public string inputHorizontal;
    public string inputHorizontal_Analogico;
    public string inputVertical;
    public string inputVertical_Analogico;
    public string inputAttackButton;
    public string inputDeffenseButton;
    public string inputSpecialAttackButton;
    public string inputJumpButton;
    public string inputParabolaAttack;
    public string inputPauseButton;
    //-----------------------//

    //DATOS PARA EL MOVIMIENTO
    public GameObject alturaMaxima;
    public GameObject[] posicionesDeMovimiento;
    //-------------------------------------------//
    public BarraDeEscudo barraDeEscudo;
    public GameObject PrefabPlayer;
    public bool resetPlayer;
    public bool resetScore;
    public PlayerData PD;
    public Grid gridPlayer;
    private float auxLife;
    public StructsPlayer structsPlayer;
    public List<SpritePlayer> spritePlayers;
    [HideInInspector]
    public SpritePlayer spritePlayerActual;
    public EnumsPlayers enumsPlayers;

    public GameObject generadorProyectiles;
    public GameObject generadorProyectilesAgachado;
    public GameObject generadorProyectilesParabola;
    public GameObject generadorProyectilesParabolaAgachado;
    private Animator animator;

    [HideInInspector]
    public float xpActual;
    public float xpNededSpecialAttack;
    public float xpForHit;
    public float SpeedJump;
    public float Speed;
    public float Resistace;
    public float Gravity;
    private float auxSpeedJump;
    private GameManager gm;
    private bool doubleDamage;
    private bool isJumping;
    private bool isDuck;
    private bool EnableCounterAttack;
    private Vector3 InitialPosition;
    public string ButtonDeffence;
    public string ButtonAttack;
    public string ButtonSpecialAttack;
    public float delayCounterAttack;
    public bool SpecialAttackEnabelEveryMoment;
    private float auxDelayCounterAttack;
    private bool controllerJoystick;
    public bool DoubleSpeed;
    public bool LookingForward;
    public bool LookingBack;
    public float delayAttack;
    public float delayParabolaAttack;
    private float auxDelayParabolaAttack;
    private float auxDelayAttack;
    [HideInInspector]
    public bool enableAttack;
    [HideInInspector]
    public bool enableSpecialAttack;
    public BoxColliderController boxColliderPiernas;
    public BoxColliderController boxColliderSprite;
    public BoxColliderController boxColliderParado;
    public BoxColliderController boxColliderAgachado;
    public BoxColliderController boxColliderSaltando;
    public string NameInputManager;
    private InputManager inputManager;
    private Player_PvP player_PvP;
    private bool enableParabolaAttack;
    public bool enableMecanicParabolaAttack;
    [HideInInspector]
    public float timeStuned = 0;

    [HideInInspector]
    public EventWise eventWise;
    private bool InFuegoEmpieza;

    private void Awake()
    {
        player_PvP = GetComponent<Player_PvP>();
    }
    void Start()
    {
        delayParabolaAttack = 0;
        enableMovementPlayer = false;
        enableMovement = false;
        InFuegoEmpieza = false;
        eventWise = GameObject.Find("EventWise").GetComponent<EventWise>();
        enableParabolaAttack = true;
        GameObject go = GameObject.Find(NameInputManager);
        inputManager = go.GetComponent<InputManager>();
        xpActual = 0;
        enableSpecialAttack = false;
        enableAttack = true;
        auxDelayAttack = delayAttack;
        auxDelayParabolaAttack = delayParabolaAttack;
        delayAttack = 0;
        controllerJoystick = false;
        if (resetPlayer)
        {
            ResetPlayer();
        }
        if (resetScore)
        {
            PD.score = 0;
        }
        CheckSpritePlayerActual();
        auxDelayCounterAttack = delayCounterAttack;
        isDuck = false;
        auxSpeedJump = SpeedJump;
        InitialPosition = transform.position;
        isJumping = false;
        enumsPlayers.movimiento = EnumsPlayers.Movimiento.Nulo;
        structsPlayer.dataPlayer.CantCasillasOcupadas_X = 1;
        structsPlayer.dataPlayer.CantCasillasOcupadas_Y = 2;
        structsPlayer.dataPlayer.CantCasillasOcupadasAgachado = structsPlayer.dataPlayer.CantCasillasOcupadas_Y /2;
        structsPlayer.dataPlayer.CantCasillasOcupadasParado = structsPlayer.dataPlayer.CantCasillasOcupadas_Y;
        structsPlayer.dataPlayer.columnaActual = 1;
        doubleDamage = false;
        enumsPlayers.movimiento = EnumsPlayers.Movimiento.Nulo;
        enumsPlayers.estadoJugador = EnumsPlayers.EstadoJugador.vivo;
        if (GameManager.instanceGameManager != null)
        {
            gm = GameManager.instanceGameManager;
        }
        animator = GetComponent<Animator>();
        //DrawScore();

    }

    void Update()
    {
        //Sacar esto al terminar de testear
        //PD.lifePlayer = PD.maxLifePlayer;
        //--------------------------------

        CheckOutLimit();
        CheckDead();
        CheckState();

        if (enableMovement)
        {
            DelayEnableAttack();
            DelayEnableParabolaAttack();
            CheckMovementInSpecialAttack();
            CheckBoxColliders2D();
        }
        /*if (Input.GetKey(KeyCode.Space))
        {
            xpActual = xpNededSpecialAttack;
            PD.lifePlayer = PD.maxLifePlayer;
        }*/
    }
    public void CheckState()
    {
        switch (enumsPlayers.estadoJugador)
        {
            case EnumsPlayers.EstadoJugador.Atrapado:
                CheckStune(timeStuned);
                break;
        }
    }

    public void CheckStune(float timeStuned)
    {
        if (timeStuned > 0)
        {
            timeStuned = timeStuned - Time.deltaTime;
            //hacer que el color del player se vea azul;
            enableMovement = false;
        }
        else if (timeStuned <= 0)
        {
            //hacer que el color del player se vea blanco;
            enableMovement = true;
            if (PD.lifePlayer > 0)
            {
                enumsPlayers.estadoJugador = EnumsPlayers.EstadoJugador.vivo;
            }
            else
            {
                enumsPlayers.estadoJugador = EnumsPlayers.EstadoJugador.muerto;
            }

        }
    }
    public void CheckBoxColliders2D()
    {
        if (!isDuck && spritePlayerActual.ActualSprite != SpritePlayer.SpriteActual.Salto
            || !isDuck && spritePlayerActual.ActualSprite != SpritePlayer.SpriteActual.SaltoAtaque
            || !isDuck && spritePlayerActual.ActualSprite != SpritePlayer.SpriteActual.SaltoDefensa)
        {
                boxColliderAgachado.GetBoxCollider2D().enabled = false;
                boxColliderParado.GetBoxCollider2D().enabled = true;
                boxColliderSaltando.GetBoxCollider2D().enabled = false;
                if (spritePlayerActual.ActualSprite == SpritePlayer.SpriteActual.ParadoDefensa)
                {
                    if (boxColliderPiernas != null)
                    {
                        boxColliderPiernas.GetBoxCollider2D().enabled = true;
                    }
                }
                else
                {
                    if (boxColliderPiernas != null)
                    {
                        boxColliderPiernas.GetBoxCollider2D().enabled = false;
                    }
                }
        }
        else if (isDuck || enumsPlayers.movimiento == EnumsPlayers.Movimiento.Agacharse
            || enumsPlayers.movimiento == EnumsPlayers.Movimiento.AgacharseAtaque
            || enumsPlayers.movimiento == EnumsPlayers.Movimiento.AgacheDefensa)
        {
                boxColliderAgachado.GetBoxCollider2D().enabled = true;
                boxColliderParado.GetBoxCollider2D().enabled = false;
                boxColliderSaltando.GetBoxCollider2D().enabled = false;
                if (spritePlayerActual.ActualSprite == SpritePlayer.SpriteActual.ParadoDefensa)
                {
                    if (boxColliderPiernas != null)
                    {
                        boxColliderPiernas.GetBoxCollider2D().enabled = true;
                    }
                }
                else
                {
                    if (boxColliderPiernas != null)
                    {
                        boxColliderPiernas.GetBoxCollider2D().enabled = false;
                    }
                }
        }
        else if (isJumping || enumsPlayers.movimiento == EnumsPlayers.Movimiento.Saltar
            || enumsPlayers.movimiento == EnumsPlayers.Movimiento.SaltoAtaque
            || enumsPlayers.movimiento == EnumsPlayers.Movimiento.SaltoDefensa)
        {
                if (spritePlayerActual.ActualSprite == SpritePlayer.SpriteActual.ParadoDefensa)
                {
                    if (boxColliderPiernas != null)
                    {
                        boxColliderPiernas.GetBoxCollider2D().enabled = true;
                    }
                }
                else
                {
                    if (boxColliderPiernas != null)
                    {
                        boxColliderPiernas.GetBoxCollider2D().enabled = false;
                    }
                }
                boxColliderAgachado.GetBoxCollider2D().enabled = false;
                boxColliderParado.GetBoxCollider2D().enabled = false;
                boxColliderSaltando.GetBoxCollider2D().enabled = true;
        }
            
    }
        
    public void CheckMovementInSpecialAttack()
    {
        switch (enumsPlayers.specialAttackEquipped)
        {
            case EnumsPlayers.SpecialAttackEquipped.DisparoDeCarga:
                if (structsPlayer.dataAttack.DisparoDeCarga.activeSelf)
                {
                    enableMovementPlayer = false;
                }
                break;
        }
    }
    public void ResetPlayer()
    {
        PD.lifePlayer = PD.maxLifePlayer;
    }

    public void CheckSpritePlayerActual()
    {
        for (int i = 0; i < spritePlayers.Count; i++)
        {
            if (spritePlayers[i].gameObject.activeSelf)
            {
                spritePlayerActual = spritePlayers[i];
            }
        }
    }
    

    public void CheckDead()
    {
        if (PD.lifePlayer <= 0)
        {
            enableMovementPlayer = false;

            if (transform.position.y <= InitialPosition.y)
            {
                spritePlayerActual.PlayAnimation("Death");
            }
            else if (transform.position.y > InitialPosition.y)
            {
                spritePlayerActual.ActualSprite = SpritePlayer.SpriteActual.Salto;
            }
        }
    }
    public void Dead()
    {
        enumsPlayers.estadoJugador = EnumsPlayers.EstadoJugador.muerto;
        if (SceneManager.GetActiveScene().name == "Supervivencia")
        {
            gm.GameOver("GameOverSupervivencia");
        }
        else if (SceneManager.GetActiveScene().name != "PvP" && SceneManager.GetActiveScene().name != "TiroAlBlanco")
        {
            gm.GameOver("GameOverHistoria");
        }
        else if (SceneManager.GetActiveScene().name == "PvP" || SceneManager.GetActiveScene().name == "TiroAlBlanco")
        {
            enableMovementPlayer = false;
        }
        gm.ResetRoundCombat(true);
    }
    
    public void DelayEnableAttack()
    {
        if (delayAttack > 0)
        {
            delayAttack = delayAttack - Time.deltaTime;
            enableAttack = false;
        }
        else
        {
            enableAttack = true;
        }
    }
    public void DelayEnableParabolaAttack()
    {
        if (delayParabolaAttack > 0)
        {
            delayParabolaAttack = delayParabolaAttack - Time.deltaTime;
            enableParabolaAttack = false;
        }
        else
        {
            enableParabolaAttack = true;
        }
    }
    public void AttackDown(Proyectil.DisparadorDelProyectil disparador)
    {
        if (enableAttack)
        {
            Proyectil.typeProyectil tipoProyectil = Proyectil.typeProyectil.ProyectilAereo;
            GameObject go = structsPlayer.dataAttack.poolProyectil.GetObject();
            Proyectil proyectil = go.GetComponent<Proyectil>();
            switch (enumsPlayers.numberPlayer)
            {
                case EnumsPlayers.NumberPlayer.player1:
                    proyectil.SetPlayer(gameObject.GetComponent<Player>());
                    disparador = Proyectil.DisparadorDelProyectil.Jugador1;
                    break;
                case EnumsPlayers.NumberPlayer.player2:
                    proyectil.SetPlayer2(gameObject.GetComponent<Player>());
                    disparador = Proyectil.DisparadorDelProyectil.Jugador2;
                    break;
            }
            proyectil.SetDobleDamage(doubleDamage);
            if (doubleDamage)
            {
                proyectil.damage = proyectil.damage * 2;
            }
            if (!isDuck)
            {
                go.transform.position = generadorProyectiles.transform.position;
                go.transform.rotation = generadorProyectiles.transform.rotation;
                proyectil.posicionDisparo = Proyectil.PosicionDisparo.PosicionMedia;
            }
            else
            {
                go.transform.position = generadorProyectilesAgachado.transform.position;
                go.transform.rotation = generadorProyectilesAgachado.transform.rotation;
                proyectil.posicionDisparo = Proyectil.PosicionDisparo.PosicionBaja;
            }
            proyectil.disparadorDelProyectil = disparador;
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
            if (applyColorShoot == ApplyColorShoot.None || applyColorShoot == ApplyColorShoot.Stela)
            {
                proyectil.On(tipoProyectil, false);
            }
            else
            {
                proyectil.On(tipoProyectil, true);
            }
            proyectil.ShootForwardDown();
            delayAttack = auxDelayAttack;
        }
    }
    public void Attack(Proyectil.DisparadorDelProyectil disparador)
    {
        if (enableAttack)
        {
            GameObject go = structsPlayer.dataAttack.poolProyectil.GetObject();
            Proyectil proyectil = go.GetComponent<Proyectil>();
            Proyectil.typeProyectil tipoProyectil = Proyectil.typeProyectil.ProyectilAereo;
            switch (enumsPlayers.numberPlayer)
            {
                case EnumsPlayers.NumberPlayer.player1:
                    proyectil.SetPlayer(gameObject.GetComponent<Player>());
                    disparador = Proyectil.DisparadorDelProyectil.Jugador1;
                    break;
                case EnumsPlayers.NumberPlayer.player2:
                    proyectil.SetPlayer2(gameObject.GetComponent<Player>());
                    disparador = Proyectil.DisparadorDelProyectil.Jugador2;
                    break;
            }
            proyectil.SetDobleDamage(doubleDamage);
            if (doubleDamage)
            {
                proyectil.damage = proyectil.damage * 2;
            }
            if (!isDuck)
            {
                if (enumsPlayers.movimiento != EnumsPlayers.Movimiento.Saltar
                    && enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoAtaque
                    && enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoDefensa)
                {
                    tipoProyectil = Proyectil.typeProyectil.ProyectilNormal;
                }
                go.transform.position = generadorProyectiles.transform.position;
                go.transform.rotation = generadorProyectiles.transform.rotation;
                proyectil.posicionDisparo = Proyectil.PosicionDisparo.PosicionMedia;
            }
            else
            {
                if (isDuck)
                {
                    tipoProyectil = Proyectil.typeProyectil.ProyectilBajo;
                }
                go.transform.position = generadorProyectilesAgachado.transform.position;
                go.transform.rotation = generadorProyectilesAgachado.transform.rotation;
                proyectil.posicionDisparo = Proyectil.PosicionDisparo.PosicionBaja;
            }
            if (applyColorShoot == ApplyColorShoot.None || applyColorShoot == ApplyColorShoot.Stela)
            {
                proyectil.On(tipoProyectil, false);
            }
            else
            {
                proyectil.On(tipoProyectil, true);
            }
            proyectil.disparadorDelProyectil = disparador;
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
            proyectil.ShootForward();
            delayAttack = auxDelayAttack;
        }
    }

    //ATAQUE EN PARABOLA.
    public void ParabolaAttack(Proyectil.DisparadorDelProyectil disparador)
    {
        //Debug.Log("EnableParabolaAttack: "+enableParabolaAttack);
        //Debug.Log("EnableMecanicParabolaAttack: " + enableMecanicParabolaAttack);
        if (enableParabolaAttack && enableMecanicParabolaAttack)
        {
            GameObject go = structsPlayer.dataAttack.poolProyectilParabola.GetObject();
            ProyectilParabola proyectil = go.GetComponent<ProyectilParabola>();
            proyectil.SetDobleDamage(doubleDamage);
            proyectil.disparadorDelProyectil = disparador;
            if (enumsPlayers.numberPlayer == EnumsPlayers.NumberPlayer.player1)
            {
                proyectil.SetPlayer(this);
            }
            else if (enumsPlayers.numberPlayer == EnumsPlayers.NumberPlayer.player2)
            {
                proyectil.SetPlayer2(this);
            }
            if (doubleDamage)
            {
                proyectil.damage = proyectil.damage * 2;
            }
            if (!isDuck)
            {
                proyectil.TypeRoot = 1;
                go.transform.position = generadorProyectilesParabola.transform.position;
            }
            else
            {
                proyectil.TypeRoot = 2;
                go.transform.position = generadorProyectilesParabolaAgachado.transform.position;
            }
            switch (proyectil.TypeRoot)
            {
                case 1:
                    proyectil.rutaParabola_AtaqueJugador = structsPlayer.ruta;
                    break;
                case 2:
                    proyectil.rutaParabolaAgachado_AtaqueJugador = structsPlayer.rutaAgachado;
                    break;
            }
            proyectil.rutaParabola_AtaqueJugador = structsPlayer.ruta;
            proyectil.OnParabola(null,this, Proyectil.typeProyectil.Nulo);
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
            enableParabolaAttack = false;
            delayParabolaAttack = auxDelayParabolaAttack;
        }
    }
    public void SpecialAttack(Proyectil.DisparadorDelProyectil disparador)
    {
        switch (enumsPlayers.specialAttackEquipped)
        {
            case EnumsPlayers.SpecialAttackEquipped.Default:
                if (enableSpecialAttack)
                {
                    GameObject go = structsPlayer.dataAttack.poolProyectilParabola.GetObject();
                    ProyectilParabola proyectil = go.GetComponent<ProyectilParabola>();
                    switch (applyColorShoot)
                    {
                        case ApplyColorShoot.None:
                            break;
                        case ApplyColorShoot.Stela:
                            proyectil.SetColorStela(colorShoot);
                            break;
                    }
                    proyectil.SetDobleDamage(doubleDamage);
                    proyectil.disparadorDelProyectil = disparador;
                    if (enumsPlayers.numberPlayer == EnumsPlayers.NumberPlayer.player1)
                    {
                        proyectil.SetPlayer(this);
                    }
                    else if (enumsPlayers.numberPlayer == EnumsPlayers.NumberPlayer.player2)
                    {
                        proyectil.SetPlayer2(this);
                    }
                    if (doubleDamage)
                    {
                        proyectil.damage = proyectil.damage * 2;
                    }
                    if (!isDuck)
                    {
                        proyectil.TypeRoot = 1;
                        go.transform.position = generadorProyectilesParabola.transform.position;
                    }
                    else
                    {
                        proyectil.TypeRoot = 2;
                        go.transform.position = generadorProyectilesParabolaAgachado.transform.position;
                    }
                    switch (proyectil.TypeRoot)
                    {
                        case 1:
                            proyectil.rutaParabola_AtaqueJugador = structsPlayer.ruta;
                            break;
                        case 2:
                            proyectil.rutaParabolaAgachado_AtaqueJugador = structsPlayer.rutaAgachado;
                            break;
                    }
                    proyectil.rutaParabola_AtaqueJugador = structsPlayer.ruta;
                    proyectil.OnParabola(null,this,Proyectil.typeProyectil.AtaqueEspecial);
                    enableSpecialAttack = false;
                    xpActual = 0;
                }
                break;
            case EnumsPlayers.SpecialAttackEquipped.GranadaGaseosa:
                if (enableSpecialAttack)
                {
                    GameObject go = structsPlayer.dataAttack.poolGranadaGaseosa.GetObject();
                    ProyectilParabola proyectil = go.GetComponent<ProyectilParabola>();
                    switch (applyColorShoot)
                    {
                        case ApplyColorShoot.None:
                            break;
                        case ApplyColorShoot.Stela:
                            proyectil.SetColorStela(colorShoot);
                            break;
                    }
                    proyectil.SetDobleDamage(doubleDamage);
                    proyectil.disparadorDelProyectil = disparador;

                    switch (player_PvP.playerSelected)
                    {
                        case Player_PvP.PlayerSelected.Protagonista:
                            proyectil.spriteRenderer.sprite = proyectil.GetComponent<GranadaGaseosa>().propBotella;
                            break;
                        case Player_PvP.PlayerSelected.Balanceado:
                            proyectil.spriteRenderer.sprite = proyectil.GetComponent<GranadaGaseosa>().propLata;
                            break;
                    }
                    if (enumsPlayers.numberPlayer == EnumsPlayers.NumberPlayer.player1)
                    {
                        proyectil.SetPlayer(this);
                    }
                    else if (enumsPlayers.numberPlayer == EnumsPlayers.NumberPlayer.player2)
                    {
                        proyectil.SetPlayer2(this);
                    }
                    if (doubleDamage)
                    {
                        proyectil.damage = proyectil.damage * 2;
                    }
                    if (!isDuck)
                    {
                        proyectil.TypeRoot = 1;
                        go.transform.position = generadorProyectilesParabola.transform.position;
                    }
                    else
                    {
                        proyectil.TypeRoot = 2;
                        go.transform.position = generadorProyectilesParabolaAgachado.transform.position;
                    }
                    switch (proyectil.TypeRoot)
                    {
                        case 1:
                            proyectil.rutaParabola_AtaqueJugador = structsPlayer.ruta;
                            break;
                        case 2:
                            proyectil.rutaParabolaAgachado_AtaqueJugador = structsPlayer.rutaAgachado;
                            break;
                    }
                    proyectil.rutaParabola_AtaqueJugador = structsPlayer.ruta;
                    proyectil.OnParabola(null, this, Proyectil.typeProyectil.AtaqueEspecial);
                    enableSpecialAttack = false;
                    xpActual = 0;
                }
                break;
            case EnumsPlayers.SpecialAttackEquipped.DisparoDeCarga:
                if (!InFuegoEmpieza)
                {
                    InFuegoEmpieza = true;
                    eventWise.StartEvent("fuego_empieza");
                }
                if (enableSpecialAttack)
                {
                    if (!isJumping && !isDuck
                    && enumsPlayers.movimiento != EnumsPlayers.Movimiento.Saltar
                    && enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoAtaque
                    && enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoDefensa)
                    {
                        structsPlayer.dataAttack.DisparoDeCarga.SetActive(true);
                        enableSpecialAttack = false;
                        xpActual = 0;
                    }
                }
                break;
            case EnumsPlayers.SpecialAttackEquipped.ProyectilImparable:
                if (enableSpecialAttack)
                {
                    if (!isJumping && !isDuck
                    && enumsPlayers.movimiento != EnumsPlayers.Movimiento.Saltar
                    && enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoAtaque
                    && enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoDefensa)
                    {
                        GameObject go = structsPlayer.dataAttack.poolProyectilImparable.GetObject();
                        ProyectilInparable proyectilInparable = go.GetComponent<ProyectilInparable>();
                        proyectilInparable.SetEnemy(gameObject.GetComponent<Enemy>());
                        switch (applyColorShoot)
                        {
                            case ApplyColorShoot.None:
                                break;
                            case ApplyColorShoot.Stela:
                                proyectilInparable.SetColorStela(colorShoot);
                                break;
                        }
                        proyectilInparable.disparadorDelProyectil = Proyectil.DisparadorDelProyectil.Enemigo;
                        if (enumsPlayers.numberPlayer == EnumsPlayers.NumberPlayer.player1)
                        {
                            proyectilInparable.SetPlayer(gameObject.GetComponent<Player>());
                        }
                        else if (enumsPlayers.numberPlayer == EnumsPlayers.NumberPlayer.player2)
                        {
                            proyectilInparable.SetPlayer2(gameObject.GetComponent<Player>());
                        }
                        go.transform.position = generadorProyectilesAgachado.transform.position;
                        go.transform.rotation = generadorProyectilesAgachado.transform.rotation;
                        proyectilInparable.ShootForward();
                        enableSpecialAttack = false;
                        xpActual = 0;
                    }
                }
                break;
        }
    }
    public void CheckOutLimit()
    {
        if (transform.position.y <= InitialPosition.y)
        {
            transform.position = new Vector3(transform.position.x, InitialPosition.y, transform.position.z);
        }
    }
        
    public void MovementLeft()
    {
        if (LookingForward)
        {
            if (structsPlayer.dataPlayer.columnaActual > 0)
            {
                MoveLeft(posicionesDeMovimiento[structsPlayer.dataPlayer.columnaActual - 1].transform.position);
            }
        }
        else if (LookingBack)
        {
            if (structsPlayer.dataPlayer.columnaActual > 0)
            {
                MoveLeft(posicionesDeMovimiento[structsPlayer.dataPlayer.columnaActual - 1].transform.position);
            }
        }
    }
    public void MovementRight()
    {
        if (LookingForward)
        {
            if (structsPlayer.dataPlayer.columnaActual < posicionesDeMovimiento.Length - 1)
            {
                MoveRight(posicionesDeMovimiento[structsPlayer.dataPlayer.columnaActual + 1].transform.position);
            }
        }
        else if (LookingBack)
        {
            if (structsPlayer.dataPlayer.columnaActual < posicionesDeMovimiento.Length - 1)
            {
                MoveRight(posicionesDeMovimiento[structsPlayer.dataPlayer.columnaActual + 1].transform.position);
            }
        }
    }
    public void MovementJump()
    {
        if (enumsPlayers.movimiento == EnumsPlayers.Movimiento.Nulo)
        {
            structsPlayer.particleMovement.particleJump.transform.position = new Vector3(transform.position.x, structsPlayer.particleMovement.particleJump.transform.position.y, structsPlayer.particleMovement.particleJump.transform.position.z);
            structsPlayer.particleMovement.particleJump.gameObject.SetActive(true);
            isJumping = true;
            SpeedJump = auxSpeedJump;
        }
        Jump(alturaMaxima.transform.position);
    }
    public void MovementDuck()
    {
        Duck(structsPlayer.dataPlayer.CantCasillasOcupadas_Y);
    }
    public bool CheckMove(Vector3 PosicionDestino)
    {
        Vector3 distaciaObjetivo = transform.position - PosicionDestino;
        bool mover = false;
        if (distaciaObjetivo.magnitude > 0f)
        {
            mover = true;
        }
        return mover;
    }
    public void Move(Vector3 direccion)
    {
        transform.Translate(direccion * Speed * Time.deltaTime);
    }
    public void MoveJamp(Vector3 direccion)
    {
        if (direccion == Vector3.up)
        {
            transform.Translate(direccion * SpeedJump * Time.deltaTime);
            SpeedJump = SpeedJump - Time.deltaTime * Resistace;
        }
        else if (direccion == Vector3.down)
        {
            transform.Translate(direccion * SpeedJump * Time.deltaTime);
            SpeedJump = SpeedJump + Time.deltaTime * Gravity;
        }
    }

    public void MoveLeft(Vector3 cuadrillaDestino)
    {
        if (LookingForward)
        {
            if (CheckMove(new Vector3(posicionesDeMovimiento[0].transform.position.x, transform.position.y, transform.position.z)) && transform.position.x > cuadrillaDestino.x)
            {
                Move(Vector3.left);
                if (enumsPlayers.movimiento != EnumsPlayers.Movimiento.MoverAtras)
                {
                    eventWise.StartEvent("moverse");
                }
                enumsPlayers.movimiento = EnumsPlayers.Movimiento.MoverAtras;
            }
            else if (enumsPlayers.movimiento != EnumsPlayers.Movimiento.Nulo)
            {
                structsPlayer.dataPlayer.columnaActual--;
                enumsPlayers.movimiento = EnumsPlayers.Movimiento.Nulo;
            }
        }
        else if (LookingBack)
        {
            if (CheckMove(new Vector3(posicionesDeMovimiento[0].transform.position.x, transform.position.y, transform.position.z)) && transform.position.x > cuadrillaDestino.x)
            {
                Move(-Vector3.left);
                if (enumsPlayers.movimiento != EnumsPlayers.Movimiento.MoverAtras)
                {
                    eventWise.StartEvent("moverse");
                }
                enumsPlayers.movimiento = EnumsPlayers.Movimiento.MoverAtras;
            }
            else if (enumsPlayers.movimiento != EnumsPlayers.Movimiento.Nulo)
            {
                structsPlayer.dataPlayer.columnaActual--;
                enumsPlayers.movimiento = EnumsPlayers.Movimiento.Nulo;
            }
        }
    }
    public void MoveRight(Vector3 cuadrillaDestino)
    {
        if (LookingForward)
        {
            if (CheckMove(new Vector3(posicionesDeMovimiento[posicionesDeMovimiento.Length-1].transform.position.x, transform.position.y, transform.position.z)) && transform.position.x < cuadrillaDestino.x)
            {
                Move(Vector3.right);
                if (enumsPlayers.movimiento != EnumsPlayers.Movimiento.MoverAdelante)
                {
                    eventWise.StartEvent("moverse");
                }
                enumsPlayers.movimiento = EnumsPlayers.Movimiento.MoverAdelante;
            }
            else if (enumsPlayers.movimiento != EnumsPlayers.Movimiento.Nulo)
            {
                structsPlayer.dataPlayer.columnaActual++;
                enumsPlayers.movimiento = EnumsPlayers.Movimiento.Nulo;
            }
        }
        else if (LookingBack)
        {
            if (CheckMove(new Vector3(posicionesDeMovimiento[posicionesDeMovimiento.Length - 1].transform.position.x, transform.position.y, transform.position.z)) && transform.position.x < cuadrillaDestino.x)
            {
                if (enumsPlayers.movimiento != EnumsPlayers.Movimiento.MoverAdelante)
                {
                    eventWise.StartEvent("moverse");
                }
                Move(-Vector3.right);
                enumsPlayers.movimiento = EnumsPlayers.Movimiento.MoverAdelante;
            }
            else 
            {
                structsPlayer.dataPlayer.columnaActual++;
                enumsPlayers.movimiento = EnumsPlayers.Movimiento.Nulo;
            }
        }
    }
    public void Jump(Vector3 alturaMaxima)
    {
        if (CheckMove(new Vector3(transform.position.x, alturaMaxima.y, transform.position.z)) && isJumping)
        {
            if(enumsPlayers.movimiento != EnumsPlayers.Movimiento.Saltar)
            {
                eventWise.StartEvent("saltar");
            }
            enumsPlayers.movimiento = EnumsPlayers.Movimiento.Saltar;
            MoveJamp(Vector3.up);
            if (SpeedJump <= 0)
            {
                isJumping = false;
            }
        }
        else
        {
            isJumping = false;
            if (CheckMove(new Vector3(transform.position.x, InitialPosition.y, transform.position.z)))
            {
                MoveJamp(Vector3.down);
            }
            else
            {
                eventWise.StartEvent("caer");
                enumsPlayers.movimiento = EnumsPlayers.Movimiento.Nulo;
                SpeedJump = auxSpeedJump;
            }
        }
    }
    public void Duck(int rangoAgachado)
    {
        isDuck = true;
    }
    public void Deffence()
    {
        if (!isDuck
            && enumsPlayers.movimiento != EnumsPlayers.Movimiento.Saltar
            && enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoAtaque
            && enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoDefensa
            && !isJumping)
        {
            boxColliderParado.state = BoxColliderController.StateBoxCollider.Defendido;
        }
        else if (isDuck 
            && enumsPlayers.movimiento != EnumsPlayers.Movimiento.Saltar
            && enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoAtaque
            && enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoDefensa
            && !isJumping)
        {
            boxColliderAgachado.state = BoxColliderController.StateBoxCollider.Defendido;
        }
        else if(enumsPlayers.movimiento == EnumsPlayers.Movimiento.Saltar 
            || enumsPlayers.movimiento == EnumsPlayers.Movimiento.SaltoAtaque 
            || enumsPlayers.movimiento == EnumsPlayers.Movimiento.SaltoDefensa
            || isJumping)
        {
            boxColliderSaltando.state = BoxColliderController.StateBoxCollider.Defendido;
        }
        boxColliderSprite.state = BoxColliderController.StateBoxCollider.Defendido;
        if (player_PvP != null)
        {
            if (player_PvP.playerSelected == Player_PvP.PlayerSelected.Defensivo)
            {
                if (player_PvP.delayCounterAttackDeffense > 0)
                {
                    player_PvP.delayCounterAttackDeffense = player_PvP.delayCounterAttackDeffense - Time.deltaTime;
                    player_PvP.stateDeffence = Player_PvP.StateDeffence.CounterAttackDeffense;
                    spritePlayerActual.spriteRenderer.color = Color.yellow;
                }
                else
                {
                    player_PvP.stateDeffence = Player_PvP.StateDeffence.NormalDeffense;
                    spritePlayerActual.spriteRenderer.color = Color.white;
                }
            }
        }
        // A MEDIDA QUE ME DEFIENDO BAJO EL PORCENTAJE A LA BARRA DE DEFENSA
        if (barraDeEscudo != null)
        {
            barraDeEscudo.SubstractPorcentageBar();
        }
            

    }
    public bool GetEnableCounterAttack()
    {
        return EnableCounterAttack;
    }
    public void SetEnableCounterAttack(bool _enableCounterAttack)
    {
        EnableCounterAttack = _enableCounterAttack;
    }
    public float GetAuxDelayAttack()
    {
        return auxDelayAttack;
    }
    public float GetAuxDelayCounterAttack()
    {
        return auxDelayCounterAttack;
    }
    public bool GetIsDuck()
    {
        return isDuck;
    }
    public void SetIsDuck(bool _isDuck)
    {
        isDuck = _isDuck;
    }
    public bool GetIsJumping()
    {
        return isJumping;
    }
    public void SetIsJumping(bool _isJumping)
    {
        isJumping = _isJumping;
    }
    public float GetAuxSpeedJump()
    {
        return auxSpeedJump;
    }
    public void SetAuxSpeedJump(float _auxSpeedJump)
    {
        auxSpeedJump = _auxSpeedJump;
    }
    public void SetControllerJoystick(bool _controllerJoystick)
    {
        controllerJoystick = _controllerJoystick;
    }
    public bool GetControllerJoystick()
    {
        return controllerJoystick;
    }
    public void SetEnableSpecialAttack(bool _enableSpecialAttack)
    {
        enableSpecialAttack = _enableSpecialAttack;
    }
    public void SetXpActual(float _xpActual)
    {
        xpActual = _xpActual;
    }
    public float GetXpActual()
    {
        return xpActual;
    }
    public bool GetEnableAttack()
    {
        return enableAttack;
    }
    public InputManager GetInputManager()
    {
        return inputManager;
    }
    public bool GetEnableSpecialAttack()
    {
        return enableSpecialAttack;
    }
    public Player_PvP GetPlayerPvP()
    {
        return player_PvP;
    }
    public Vector3 GetInitialPosition()
    {
        return InitialPosition;
    }
    public void SetInFuegoEmpieza(bool _inFuegoEmpieza)
    {
        InFuegoEmpieza = _inFuegoEmpieza;
    }
}
