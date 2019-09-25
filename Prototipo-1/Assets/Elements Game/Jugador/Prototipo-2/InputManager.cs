﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototipo_2 {
    public class InputManager : MonoBehaviour
    {
        public Player player1;
        public Player player2;
        private bool moveHorizontalPlayer1;
        private bool moveVerticalPlayer1;
        private bool moveVerticalPlayer2;
        private bool moveHorizontalPlayer2;
        // Update is called once per frame
        private void Start()
        {
            moveHorizontalPlayer1 = true;
            moveVerticalPlayer1 = true;
            moveHorizontalPlayer2 = true;
            moveVerticalPlayer2 = true;
        }
        void Update()
        {
            CheckInputPlayer1();
            CheckInputPlayer2();
        }
        public void CheckInputPlayer1()
        {
            if (InputPlayerController.Vertical_Button_P1() > 0 && moveVerticalPlayer1)
            {
                if (player1.enumsPlayers.movimiento == EnumsPlayers.Movimiento.Nulo)
                {
                    player1.MovementJump();
                    moveVerticalPlayer1 = false;
                }
            }
            else if (InputPlayerController.Vertical_Button_P1() < 0)
            {
                if (player1.enumsPlayers.movimiento == EnumsPlayers.Movimiento.Nulo)
                {
                    player1.MovementDuck();
                    player1.enumsPlayers.movimiento = EnumsPlayers.Movimiento.Agacharse;
                    player1.spritePlayerActual.ActualSprite = SpritePlayer.SpriteActual.Agachado;
                }
            }
            else if (InputPlayerController.Vertical_Button_P1() == 0 &&
                (player1.enumsPlayers.movimiento == EnumsPlayers.Movimiento.Agacharse
                || player1.enumsPlayers.movimiento == EnumsPlayers.Movimiento.AgacharseAtaque
                || player1.enumsPlayers.movimiento == EnumsPlayers.Movimiento.AgacheDefensa))
            {
                player1.enumsPlayers.movimiento = EnumsPlayers.Movimiento.Nulo;
            }
            else if (InputPlayerController.Vertical_Button_P1() == 0)
            {
                moveVerticalPlayer1 = true;
            }

            if (InputPlayerController.Horizontal_Button_P1() < 0 && moveHorizontalPlayer1)
            {
                if (player1.enumsPlayers.movimiento == EnumsPlayers.Movimiento.Nulo)
                {
                    moveHorizontalPlayer1 = false;
                    player1.MovementLeft();
                }
            }
            else if (InputPlayerController.Horizontal_Button_P1() > 0 && moveHorizontalPlayer1)
            {
                if (player1.enumsPlayers.movimiento == EnumsPlayers.Movimiento.Nulo)
                {
                    moveHorizontalPlayer1 = false;
                    player1.MovementRight();
                }
            }
            else if (InputPlayerController.Horizontal_Button_P1() == 0)
            {
                moveHorizontalPlayer1 = true;
            }

            if (InputPlayerController.AttackButton_P1())
            {
                if (player1.enumsPlayers.movimiento == EnumsPlayers.Movimiento.Saltar && InputPlayerController.Vertical_Button_P1() < 0)
                {
                    player1.AttackDown();
                }
                else
                {
                    player1.Attack();
                }
            }

            if (InputPlayerController.CheckPressDeffenseButton_P1())
            {
                player1.Deffence();
            }
        }
        public void CheckInputPlayer2()
        {

        }
    }
}
