﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KathyAndTyke : Enemy
{
    // Start is called before the first frame update
    public ProyectilLimo limo_gameObject;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (limo_gameObject.gameObject.activeSelf)
        {
            delaySelectMovement = 0.1f;
        }
        CheckInSpecialAttack();
        if (transform.position.y > InitialPosition.y && enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecial)
        {
            enumsEnemy.SetMovement(EnumsEnemy.Movimiento.Nulo);
            SpeedJump = -1f;
            CheckMovement();
            delaySelectMovement = 0.7f;
        }
    }

    public void CheckInSpecialAttack()
    {
        if (!limo_gameObject.gameObject.activeSelf)
        {
            if (spriteEnemy != null)
            {
                if (spriteEnemy.GetAnimator() != null)
                {
                    //spriteEnemy.GetAnimator().SetBool("EnPlenoAtaqueEspecial", false);
                    spriteEnemy.GetAnimator().SetBool("FinalAtaqueEspecial", true);

                }
            }
        }
        else
        {
            //spriteEnemy.GetAnimator().SetBool("EnPlenoAtaqueEspecial", true);
            spriteEnemy.GetAnimator().SetBool("FinalAtaqueEspecial", false);
            //spriteEnemy.disableSpecialAttack = false;
        }
    }
    public override void CheckDelayAttack(bool specialAttack)
    {
        if (delayAttack > 0)
        {
            delayAttack = delayAttack - Time.deltaTime;
            if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.Saltar || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoAtaque)
            {
                spriteEnemy.PlayAnimation("Salto famosa");
            }
        }
        else if (delayAttack <= 0)
        {
            AnimationAttack();
        }
    }
    public override void AnimationAttack()
    {

        if (enemyPrefab.activeSelf == true)
        {
            if (!inAttack)
            {
                valueAttack = Random.Range(0, 100);
            }
            if (valueAttack >= parabolaAttack || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecial
                || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecialAgachado
                || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecialSalto
                || !enableMecanicParabolaAttack)
            {
                if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtacarEnElLugar
                    && !GetIsJamping() && SpeedJump >= GetAuxSpeedJamp()
                    && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AgacharseAtaque
                    && !GetIsDuck())
                {
                    spriteEnemy.GetAnimator().Play("Ataque parado famosa");
                    inAttack = true;
                    SetIsDuck(false);
                }
                else if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoAtaque
                    || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.Nulo)
                {
                    spriteEnemy.GetAnimator().Play("Ataque salto famosa");
                    inAttack = true;
                    SetIsDuck(false);
                }
                else if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AgacharseAtaque && GetIsDuck() && !GetIsJamping() && SpeedJump >= GetAuxSpeedJamp())
                {
                    spriteEnemy.GetAnimator().Play("Ataque agachado famosa");
                    inAttack = true;
                    SetIsDuck(true);
                }
                else if ((enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecial
                    || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecialAgachado
                    || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecialSalto
                    || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.Nulo) && transform.position.y <= InitialPosition.y)
                {
                    switch (enumsEnemy.GetMovement())
                    {
                        case EnumsEnemy.Movimiento.AtaqueEspecial:
                            spriteEnemy.GetAnimator().SetBool("AtaqueEspecial", true);
                            enumsEnemy.SetMovement(EnumsEnemy.Movimiento.AtaqueEspecial);
                            inAttack = true;
                            break;
                        case EnumsEnemy.Movimiento.AtaqueEspecialAgachado:
                            break;
                        case EnumsEnemy.Movimiento.AtaqueEspecialSalto:
                            break;
                    }
                }
            }
            else if (valueAttack < parabolaAttack)
            {
                if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtacarEnElLugar
                    && !GetIsJamping() && SpeedJump >= GetAuxSpeedJamp() && delayAttack <= 0)
                {
                    spriteEnemy.PlayAnimation("Ataque Parabola parado famosa");
                    inAttack = true;
                    SetIsDuck(false);
                }
                else if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoAtaque && delayAttack <= 0)
                {
                    spriteEnemy.PlayAnimation("Ataque Parabola salto famosa");
                    inAttack = true;
                    SetIsDuck(false);
                }
                else if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AgacharseAtaque && delayAttack <= 0)
                {
                    spriteEnemy.PlayAnimation("Ataque Parabola agachado famosa");
                    inAttack = true;
                    SetIsDuck(true);
                }
            }
        }
    }
    public override void Attack(bool jampAttack, bool specialAttack, bool _doubleDamage)
    {
        bool shootDown = false;
        GameObject go = null;
        Proyectil proyectil = null;
        Proyectil.typeProyectil tipoProyectil = Proyectil.typeProyectil.Nulo;
        if (specialAttack && transform.position.y <= InitialPosition.y)
        {
            limo_gameObject.SetEnemy(this);
            limo_gameObject.gameObject.SetActive(true);
            limo_gameObject.disparadorDelProyectil = Proyectil.DisparadorDelProyectil.Enemigo;
            spriteEnemy.GetAnimator().SetBool("AtaqueEspecial", false);
            enumsEnemy.SetMovement(EnumsEnemy.Movimiento.AtaqueEspecial);
        }
        else if (transform.position.y > InitialPosition.y)
        {
            enumsEnemy.SetMovement(EnumsEnemy.Movimiento.Nulo);
            SpeedJump = -1f;
            CheckMovement();
            delaySelectMovement = 0.7f;
        }
        if (!limo_gameObject.gameObject.activeSelf)
        {
            if (!specialAttack)
            {
                go = poolObjectAttack.GetObject();
                proyectil = go.GetComponent<Proyectil>();
                proyectil.SetEnemy(gameObject.GetComponent<Enemy>());
                proyectil.SetDobleDamage(_doubleDamage);
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
                if (_doubleDamage)
                {
                    proyectil.damage = proyectil.damageCounterAttack;
                }
            }
            if (!GetIsDuck() && !specialAttack && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AgacharseAtaque)
            {
                tipoProyectil = Proyectil.typeProyectil.ProyectilNormal;
                if (jampAttack)
                {
                    tipoProyectil = Proyectil.typeProyectil.ProyectilAereo;
                    shootDown = true;
                }
                go.transform.rotation = generadoresProyectiles.transform.rotation;
                go.transform.position = generadoresProyectiles.transform.position;
                proyectil.posicionDisparo = Proyectil.PosicionDisparo.PosicionMedia;
            }
            else if (!specialAttack && GetIsDuck() || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AgacharseAtaque)
            {
                tipoProyectil = Proyectil.typeProyectil.ProyectilBajo;
                go.transform.rotation = generadorProyectilesAgachado.transform.rotation;
                go.transform.position = generadorProyectilesAgachado.transform.position;
                proyectil.posicionDisparo = Proyectil.PosicionDisparo.PosicionBaja;
            }
            if (!specialAttack)
            {
                if (applyColorShoot == ApplyColorShoot.None || applyColorShoot == ApplyColorShoot.Stela)
                {
                    proyectil.On(tipoProyectil, false);
                }
                else
                {
                    proyectil.On(tipoProyectil, true);
                }

                if (!shootDown)
                {
                    proyectil.ShootForward();
                }
                else
                {
                    proyectil.ShootForwardDown();
                }
            }
        }
    }
}
