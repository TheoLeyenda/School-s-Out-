﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChicleCasilla : MonoBehaviour
{
    // Start is called before the first frame update
    public Proyectil.DisparadorDelProyectil disparadorDelProyectil;

    public Player player;

    public Enemy enemy;

    public float timeStuned;

    private bool inStunedEffect;

    // Update is called once per frame
    private void OnDisable()
    {
        inStunedEffect = false;
    }
    void Update()
    {
        CheckEffect();
    }
    public void CheckEffect()
    {
        //Debug.Log(player != null);
        //Debug.Log(!player.GetIsJumping());
        //Debug.Log(player.SpeedJump >= player.GetAuxSpeedJump());
        if(player != null && player.transform.position.y <= player.GetInitialPosition().y && !player.GetIsJumping() 
            && player.enumsPlayers.movimiento != EnumsPlayers.Movimiento.Saltar 
            && player.enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoAtaque
            && player.enumsPlayers.movimiento != EnumsPlayers.Movimiento.SaltoDefensa
            && player.SpeedJump >= player.GetAuxSpeedJump())
        {
            if (!inStunedEffect)
            {
                player.timeStuned = timeStuned;
                player.enumsPlayers.estadoJugador = EnumsPlayers.EstadoJugador.Atrapado;
                inStunedEffect = true;
            }

            if (player.timeStuned <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        else if (enemy != null && enemy.transform.position.y <= enemy.InitialPosition.y && !enemy.GetIsJamping()
            && enemy.enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.Saltar
            && enemy.enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoAtaque
            && enemy.enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoDefensa
            && enemy.SpeedJump >= enemy.GetAuxSpeedJamp())
        {
            if (!inStunedEffect)
            {
                enemy.timeStuned = timeStuned;
                enemy.enumsEnemy.SetStateEnemy(EnumsEnemy.EstadoEnemigo.Atrapado);
                inStunedEffect = true;
            }
            if (enemy.timeStuned <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.GetComponent<Player>();
            if (player == null)
            {
                return;
            }
        }
        if (collision.tag == "Enemy")
        {
            enemy = collision.GetComponent<Enemy>();
            if (enemy == null)
            {
                return;
            }
        }
    }
}
