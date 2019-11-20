﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototipo_2 {
    public class Blancos : MonoBehaviour
    {
        // Start is called before the first frame update
        public float scoreForHit;
        public SpriteRenderer spriteRenderer;
        public int numberPlayerInThis;

        public float delayColor;
        public float auxDelayColor;

        private void Update()
        {
            CheckColor();
        }
        public void CheckColor()
        {
            if (delayColor > 0 && spriteRenderer.color == Color.green)
            {
                delayColor = delayColor - Time.deltaTime;
            }
            else if (delayColor <= 0)
            {
                delayColor = auxDelayColor;
                if (spriteRenderer.color == Color.green)
                {
                    spriteRenderer.color = Color.white;
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Proyectil proyectil = collision.GetComponent<Proyectil>();
            GranadaGaseosa granadaGaseosa = collision.GetComponent<GranadaGaseosa>();
            ProyectilInparable proyectilInparable = collision.GetComponent<ProyectilInparable>();
            DisparoDeCarga disparoDeCarga = collision.GetComponent<DisparoDeCarga>();
            if (proyectil != null && granadaGaseosa == null && proyectilInparable == null && disparoDeCarga == null)
            {
                if (proyectil.disparadorDelProyectil == Proyectil.DisparadorDelProyectil.Jugador1 && numberPlayerInThis == 2)
                {
                    spriteRenderer.color = Color.green;
                    proyectil.GetPlayer().PD.score = proyectil.GetPlayer().PD.score + scoreForHit;
                    proyectil.AnimationHit();
                }
                else if (proyectil.disparadorDelProyectil == Proyectil.DisparadorDelProyectil.Jugador2 && numberPlayerInThis == 1)
                {
                    spriteRenderer.color = Color.green;
                    proyectil.GetPlayer2().PD.score = proyectil.GetPlayer2().PD.score + scoreForHit;
                    proyectil.AnimationHit();
                }

            }
        }
    }
}