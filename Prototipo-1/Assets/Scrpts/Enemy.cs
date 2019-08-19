﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Enemy : MonoBehaviour
{
    public enum Categoria
    {
        Balanceado,
        Agresivo,
        Defensivo,
        Count
    }
    public enum Objetivo
    {
        Cabeza,
        Torso,
        Piernas,
    }
    private Objetivo objetivo;
    public Pool poolObjectAttack;
    public float SpeedJump;
    private int movimientoElejido;
    private int objetivoElejido;
    private int modoDeEsquiveElejido;
    public Sprite SpriteBlanco;
    public Image imagenAccion;
    public Image imagenMovimiento;
    public Sprite SpriteMovimientoAtaque;
    public Sprite SpriteMovimientoDefensa;
    public Sprite SpriteMovimientoEsquive;
    public Sprite SpriteAtaqueCabeza;
    public Sprite SpriteAtaqueTorso;
    public Sprite SpriteAtaquePies;
    public Sprite SpriteDefensaCabeza;
    public Sprite SpriteDefensaTorso;
    public Sprite SpriteDefensaPies;
    public Sprite SpriteSalto;
    public Sprite SpriteAgacharse;
    public BoxCollider2D ShildHead;
    public BoxCollider2D ShildChest;
    public BoxCollider2D ShildLegs;
    public BoxCollider2D BoxColliderHead;
    public BoxCollider2D BoxColliderChest;
    public BoxCollider2D BoxColliderLegs;
    private Rigidbody2D rg2D;
    private GameManager gm;
    public Transform tranformAtaque;
    private float DeffensePorcentage;
    private float AttackPorcentage;
    private float DodgePorcentage;
    private float AttackHeadPorcentage;
    private float AttackChestPorcentage;
    private float AttackLegsPorcentage;
    private float DeffenseHeadPorcentage;
    private float DeffenseChestPorcentage;
    private float DeffenseLegsPorcentage;
    private float JumpPorcentage;
    private float DuckPorcentage;
    private bool ContraAtaque;
    private bool ataqueCabeza;
    private bool ataqueTorso;
    private bool ataquePies;
    private bool defensaCabeza;
    private bool defensaTorso;
    private bool defensaPies;
    private bool agacharse;
    private bool saltar;
    private bool SelectDefinitive = false;
    private float MinRangeRandom = 0;
    private float MaxRangeRandom = 100;
    [HideInInspector]
    public bool mover;
    // Start is called before the first frame update
    public Categoria typeEnemy;
    void Start()
    {
        SelectDefinitive = false;
        ContraAtaque = false;
        mover = false;
        DisableShild();
        if (GameManager.instanceGameManager != null)
        {
            gm = GameManager.instanceGameManager;
        }
        rg2D = GetComponent<Rigidbody2D>();
        switch (typeEnemy)
        {
            //PANEL DE CONFIGURACION DE PORCENTAJES
            case Categoria.Balanceado:
                //----Movimiento----//
                AttackPorcentage = 45;
                DeffensePorcentage = 45;
                DodgePorcentage = 10;
                //----Objetivo Atacar----//
                AttackHeadPorcentage = 33.3f;
                AttackChestPorcentage = 33.4f;
                AttackLegsPorcentage = 33.3f;
                //----Objetivo Defender----//
                DeffenseHeadPorcentage = 33.3f;
                DeffenseChestPorcentage = 33.4f;
                DeffenseLegsPorcentage = 33.3f;
                //----Esquivar Arriba/Abajo----//
                JumpPorcentage = 50;
                DuckPorcentage = 50;
                break;
            case Categoria.Agresivo:
                //----Movimiento----//
                AttackPorcentage = 80;
                DeffensePorcentage = 20;
                DodgePorcentage = 0;
                //----Objetivo Atacar----//
                AttackLegsPorcentage = 60;
                AttackChestPorcentage = 30;
                AttackHeadPorcentage = 10;
                //----Objetivo Defender----//
                DeffenseHeadPorcentage = 33.3f;
                DeffenseChestPorcentage = 33.4f;
                DeffenseLegsPorcentage = 33.3f;
                //----Esquivar Arriba/Abajo----//
                JumpPorcentage = 0;
                DuckPorcentage = 0;
                break;
            case Categoria.Defensivo:
                //---Movimiento---//
                AttackPorcentage = 40;
                DeffensePorcentage = 60;
                DodgePorcentage = 0;
                //----Objetivo Atacar----//
                AttackHeadPorcentage = 100;
                AttackChestPorcentage = 0;
                AttackLegsPorcentage = 0;
                //----Objetivo Defender----//
                DeffenseHeadPorcentage = 33.3f;
                DeffenseChestPorcentage = 33.4f;
                DeffenseLegsPorcentage = 33.3f;
                //----Esquivar Arriba/Abajo----//
                JumpPorcentage = 0;
                DuckPorcentage = 0;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0) {
            if (gm.timeSelectionAttack > 0)
            {
                CheckMovement();
            }
            else if(gm.timeSelectionAttack <= 0 && gm.timeSelectionAttack > -1) {
                mover = true;
                gm.timeSelectionAttack = -1;
            }
            if(mover){
                SelectDefinitive = true;
                CheckMovement();
                if (ataqueCabeza)
                {
                    Debug.Log("Enemigo: ATACO CABEZA");
                    imagenAccion.sprite = SpriteAtaqueCabeza;
                    Attack(Objetivo.Cabeza);
                }
                else if (ataqueTorso)
                {
                    Debug.Log("Enemigo: ATACO TORSO");
                    imagenAccion.sprite = SpriteAtaqueTorso;
                    Attack(Objetivo.Torso);
                }
                else if (ataquePies)
                {
                    Debug.Log("Enemigo: ATACO PIERNAS");
                    imagenAccion.sprite = SpriteAtaquePies;
                    Attack(Objetivo.Piernas);
                }
                else if (defensaCabeza)
                {
                    Debug.Log("Enemigo: DEFENDIO CABEZA");
                    imagenAccion.sprite = SpriteDefensaCabeza;
                    Deffense(Objetivo.Cabeza);
                }
                else if (defensaTorso)
                {
                    Debug.Log("Enemigo: DEFENDIO TORSO");
                    imagenAccion.sprite = SpriteDefensaTorso;
                    Deffense(Objetivo.Torso);
                }
                else if (defensaPies)
                {
                    Debug.Log("Enemigo: DEFENDIO PIES");
                    imagenAccion.sprite = SpriteDefensaPies;
                    Deffense(Objetivo.Piernas);
                }
                else if (saltar)
                {
                    Debug.Log("Enemigo: SALTAR");
                    imagenAccion.sprite = SpriteSalto;
                    Jump();
                }
                else if (agacharse) {
                    Debug.Log("Enemigo: AGACHARSE");
                    imagenAccion.sprite = SpriteAgacharse;
                    Duck();
                }
                ataqueCabeza = false;
                ataqueTorso = false;
                ataquePies = false;
                defensaCabeza = false;
                defensaTorso = false;
                defensaPies = false;
                if (typeEnemy != Categoria.Balanceado)
                {
                    saltar = false;
                    agacharse = false;
                }
            }
        }
    }
    public void CounterAttack()
    {
        ShildChest.gameObject.SetActive(false);
        ShildHead.gameObject.SetActive(false);
        ShildLegs.gameObject.SetActive(false);
        imagenMovimiento.sprite = SpriteMovimientoAtaque;
        if (poolObjectAttack.count > 0)
        {
            float objetivoElejir = Random.Range(MinRangeRandom, MaxRangeRandom);
            //Debug.Log("Objetivo: " + objetivoElejir);
            if (objetivoElejir <= AttackHeadPorcentage)
            {
                //ATACAR A LA CABEZA
                //Attack(Objetivo.Cabeza);
                imagenAccion.sprite = SpriteAtaqueCabeza;
                Attack(Objetivo.Cabeza);
            }
            else if (objetivoElejir > AttackHeadPorcentage && objetivoElejir <= AttackHeadPorcentage + AttackChestPorcentage)
            {

                //ATACAR AL TORSO
                //Attack(Objetivo.Torso);
                imagenAccion.sprite = SpriteAtaqueTorso;
                Attack(Objetivo.Torso);
            }

            else if (objetivoElejir > AttackHeadPorcentage + AttackChestPorcentage)
            {
                    //ATACAR A LOS PIES
                    //Attack(Objetivo.Piernas);
                    imagenAccion.sprite = SpriteAtaquePies;
                    Attack(Objetivo.Piernas);
      
            }
        }
    }
    public void CheckMovement() {
        mover = false;
        float movimientoElejir = Random.Range(MinRangeRandom, MaxRangeRandom);
        //Debug.Log("Movimiento: "+ movimientoElejir);

        if (movimientoElejir <= AttackPorcentage)
        {
            //ATACAR
            //Debug.Log("ATACANDO");
            imagenMovimiento.sprite = SpriteMovimientoAtaque;
            if (poolObjectAttack.count > 0) {
                float objetivoElejir = Random.Range(MinRangeRandom, MaxRangeRandom);
                //Debug.Log("Objetivo: " + objetivoElejir);
                if (objetivoElejir <= AttackHeadPorcentage)
                {

                    if (Random.Range(MinRangeRandom, MaxRangeRandom) > MaxRangeRandom / 2 || SelectDefinitive)
                    {
                        //ATACAR A LA CABEZA
                        //Attack(Objetivo.Cabeza);
                        imagenAccion.sprite = SpriteAtaqueCabeza;
                        ataqueCabeza = true;
                        ataqueTorso = false;
                        ataquePies = false;
                        defensaCabeza = false;
                        defensaTorso = false;
                        defensaPies = false;
                        saltar = false;
                        agacharse = false;
                    }
                    else {
                        CheckMovement();
                    }

                }
                else if (objetivoElejir > AttackHeadPorcentage && objetivoElejir <= AttackHeadPorcentage + AttackChestPorcentage)
                {
                    if (Random.Range(MinRangeRandom, MaxRangeRandom) > MaxRangeRandom / 2 || SelectDefinitive)
                    {
                        //ATACAR AL TORSO
                        //Attack(Objetivo.Torso);
                        imagenAccion.sprite = SpriteAtaqueTorso;
                        ataqueCabeza = false;
                        ataqueTorso = true;
                        ataquePies = false;
                        defensaCabeza = false;
                        defensaTorso = false;
                        defensaPies = false;
                        saltar = false;
                        agacharse = false;
                    }
                    else {
                        CheckMovement();
                    }
                }
                else if (objetivoElejir > AttackHeadPorcentage + AttackChestPorcentage)
                {
                    if (Random.Range(MinRangeRandom, MaxRangeRandom) > MaxRangeRandom / 2 || SelectDefinitive)
                    {
                        //ATACAR A LOS PIES
                        //Attack(Objetivo.Piernas);
                        imagenAccion.sprite = SpriteAtaquePies;
                        ataqueCabeza = false;
                        ataqueTorso = false;
                        ataquePies = true;
                        defensaCabeza = false;
                        defensaTorso = false;
                        defensaPies = false;
                        saltar = false;
                        agacharse = false;

                    }
                    else {
                        CheckMovement();
                    }

                }
            }
        }
        else if (movimientoElejir > AttackPorcentage && movimientoElejir <= AttackPorcentage + DeffensePorcentage)
        {
            //DEFENDER
            //Debug.Log("DEFENDIENDO");
            imagenMovimiento.sprite = SpriteMovimientoDefensa;
            float objetivoElejir = Random.Range(0, 100);
            //Debug.Log("Objetivo: " + objetivoElejir);
            
            if (objetivoElejir <= DeffenseHeadPorcentage)
            {
                if (Random.Range(MinRangeRandom, MaxRangeRandom) > MaxRangeRandom / 2 || SelectDefinitive)
                {
                    //DEFENDER A LA CABEZA
                    imagenAccion.sprite = SpriteDefensaCabeza;
                    //Deffense(Objetivo.Cabeza);
                    ataqueCabeza = false;
                    ataqueTorso = false;
                    ataquePies = false;
                    defensaCabeza = true;
                    defensaTorso = false;
                    defensaPies = false;
                    saltar = false;
                    agacharse = false;
                }
                else {
                    CheckMovement();
                }
            }
            else if (objetivoElejir > DeffenseHeadPorcentage && objetivoElejir <= DeffenseHeadPorcentage + DeffenseChestPorcentage)
            {
                if (Random.Range(MinRangeRandom, MaxRangeRandom) > MaxRangeRandom / 2 || SelectDefinitive)
                {
                    //DEFENDER AL TORSO
                    //Deffense(Objetivo.Torso);
                    imagenAccion.sprite = SpriteDefensaTorso;
                    ataqueCabeza = false;
                    ataqueTorso = false;
                    ataquePies = false;
                    defensaCabeza = false;
                    defensaTorso = true;
                    defensaPies = false;
                    saltar = false;
                    agacharse = false;
                }
                else {
                    CheckMovement();
                }
            }
            else if (objetivoElejir > DeffenseHeadPorcentage + DeffenseChestPorcentage)
            {
                if (Random.Range(MinRangeRandom, MaxRangeRandom) > MaxRangeRandom / 2 || SelectDefinitive)
                {
                    //DEFENDER A LOS PIES
                    //Deffense(Objetivo.Piernas);
                    imagenAccion.sprite = SpriteDefensaPies;
                    ataqueCabeza = false;
                    ataqueTorso = false;
                    ataquePies = false;
                    defensaCabeza = false;
                    defensaTorso = false;
                    defensaPies = true;
                    saltar = false;
                    agacharse = false;
                }
                else {
                    CheckMovement();
                }
            }
        }
        else if (movimientoElejir > AttackPorcentage + DeffensePorcentage)
        {
            //Esquivar
            imagenMovimiento.sprite = SpriteMovimientoEsquive;
            float objetivoElejir = Random.Range(0, 100);
            objetivoElejido = Random.Range(0, 100);
            //Debug.Log("Objetivo: " + objetivoElejir);
            if (typeEnemy == Categoria.Balanceado)
            {
                //SI ESQUIVA CONTRATACA (EL BALANCEADO ES EL UNICO QUE PUEDE ESQUIVAR)
                if (objetivoElejido <= JumpPorcentage)
                {
                    if (Random.Range(MinRangeRandom, MaxRangeRandom) > MaxRangeRandom / 2 || SelectDefinitive)
                    {
                        imagenAccion.sprite = SpriteSalto;
                        ataqueCabeza = false;
                        ataqueTorso = false;
                        ataquePies = false;
                        defensaCabeza = false;
                        defensaTorso = false;
                        defensaPies = false;
                        saltar = true;
                        agacharse = false;
                        //Jump();
                    }
                    else {
                        CheckMovement();
                    }
                }
                else if (objetivoElejido > DuckPorcentage)
                {
                    if (Random.Range(MinRangeRandom, MaxRangeRandom) > MaxRangeRandom / 2 || SelectDefinitive)
                    {
                        imagenAccion.sprite = SpriteAgacharse;
                        ataqueCabeza = false;
                        ataqueTorso = false;
                        ataquePies = false;
                        defensaCabeza = false;
                        defensaTorso = false;
                        defensaPies = false;
                        saltar = false;
                        agacharse = true;
                        //Duck();
                    }
                    else {
                        CheckMovement();
                    }
                }
            }
        }

    }
    public void DisableShild()
    {
        ShildChest.gameObject.SetActive(false);
        ShildHead.gameObject.SetActive(false);
        ShildLegs.gameObject.SetActive(false);
    }
    public void ActivateShild()
    {
        ShildChest.gameObject.SetActive(true);
        ShildHead.gameObject.SetActive(true);
        ShildLegs.gameObject.SetActive(true);
    }
    public void Attack(Objetivo ob) {
        
        if (poolObjectAttack.count > 0)
        {
            DisableShild();
            GameObject go = poolObjectAttack.GetObject();
            Proyectil proyectil = go.GetComponent<Proyectil>();
            go.transform.position = tranformAtaque.position;
            go.transform.rotation = new Quaternion(go.transform.rotation.x, go.transform.rotation.y + 180, go.transform.rotation.z, go.transform.rotation.w);
            proyectil.On();
            switch (ob)
            {
                case Objetivo.Cabeza:
                    proyectil.ShootForwardUp();
                    break;
                case Objetivo.Torso:
                    proyectil.ShootForward();
                    break;
                case Objetivo.Piernas:
                    proyectil.ShootForwardDown();
                    break;
            }
        }
    }

    public void Deffense(Objetivo ob) {
        switch (ob)
        {
            case Objetivo.Cabeza:
                ShildHead.gameObject.SetActive(true);
                ShildChest.gameObject.SetActive(false);
                ShildLegs.gameObject.SetActive(false);
                break;
            case Objetivo.Torso:
                ShildHead.gameObject.SetActive(false);
                ShildChest.gameObject.SetActive(true);
                ShildLegs.gameObject.SetActive(false);
                break;
            case Objetivo.Piernas:
                ShildHead.gameObject.SetActive(false);
                ShildChest.gameObject.SetActive(false);
                ShildLegs.gameObject.SetActive(true);
                break;
        }
    }

    public void Jump()
    {
        Debug.Log("Enemigo: Animacion De Salto");
        rg2D.AddForce(transform.up * SpeedJump, ForceMode2D.Impulse);
        BoxColliderHead.gameObject.SetActive(true);
        BoxColliderChest.gameObject.SetActive(true);
        BoxColliderLegs.gameObject.SetActive(true);
    }
    public void Duck()
    {
        Debug.Log("Enemigo: Animacion De Agacharse");
        BoxColliderHead.gameObject.SetActive(false);
        BoxColliderChest.gameObject.SetActive(true);
        BoxColliderLegs.gameObject.SetActive(true);
    }
    public void SetAtaqueCabeza(bool _ataqueCabeza)
    {
        ataqueCabeza = _ataqueCabeza;
    }
    public void SetAtaqueTorso(bool _ataqueTorso) {
        ataqueTorso = _ataqueTorso;
    }
    public void SetAtaquePies(bool _ataquePies) {
        ataquePies = _ataquePies;
    }
    public void SetDefensaCabeza(bool _defensaCabeza) {
        defensaCabeza = _defensaCabeza;
    }
    public void SetDefensaTorso(bool _defensaTorso) {
        defensaTorso = _defensaTorso;
    }
    public void SetDefensaPies(bool _defensaPies) {
        defensaPies = _defensaPies;
    }
    public void SetSaltar(bool _saltar) {
        saltar = _saltar;
    }
    public void SetAgacharse(bool _agacharse) {
        agacharse = _agacharse;
    }
    public bool GetAtaqueCabeza() {
        return ataqueCabeza;
    }
    public bool GetAtaqueTorso() {
        return ataqueTorso;
    }
    public bool GetAtaquePies() {
        return ataquePies;
    }
    public bool GetDefensaCabeza() {
        return defensaCabeza;
    }
    public bool GetDefensaTorso() {
        return defensaTorso;
    }
    public bool GetDefensaPies() {
        return defensaPies;
    }
    public bool GetAgacharse() {
        return agacharse;
    }
    public bool GetSaltar() {
        return saltar;
    }
}
