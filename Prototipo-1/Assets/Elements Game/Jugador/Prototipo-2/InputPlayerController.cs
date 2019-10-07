﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputPlayerController
{
    // -------CONTROLES JUGADOR 1--------- //
    public static float Vertical_Button_P1()
    {
        float r = 0.0f;
        r += Input.GetAxis("Vertical");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }
    public static float Horizontal_Button_P1()
    {
        float r = 0.0f;
        r += Input.GetAxis("Horizontal");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }
    public static bool SpecialAttackButton_P1()
    {
        return Input.GetButtonDown("SpecialAttackButton_P1");
    }
    public static bool CheckSpecialAttackButton_P1()
    {
        return Input.GetButton("SpecialAttackButton_P1");
    }
    public static Vector3 MainJostick_P1()
    {
        return new Vector3(Horizontal_Button_P1(), 0, Vertical_Button_P1());
    }
    public static bool AttackButton_P1()
    {
        Debug.Log("Jugador 1 ha atacado");
        return Input.GetButtonDown("AttackButton_P1");
    }
    public static bool CheckPressAttackButton_P1()
    {
        return Input.GetButton("AttackButton_P1");
    }
    public static bool DeffenseButton_P1()
    {
        return Input.GetButtonDown("DeffenseButton_P1");
    }
    public static bool CheckPressDeffenseButton_P1()
    {
        return Input.GetButton("DeffenseButton_P1");
    }
    public static bool UpDeffenseButton_P1()
    {
        return Input.GetButtonUp("DeffenseButton_P1");
    }
    public static bool SelectButton_P1()
    {
        return Input.GetButtonDown("SelectButton_P1");
    }
    //--------------------------------------//



    // -------CONTROLES JUGADOR 2--------- //
    public static float Vertical_Button_P2()
    {
        float r = 0.0f;
        r += Input.GetAxis("Vertical_P2");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }
    public static float Horizontal_Button_P2()
    {
        float r = 0.0f;
        r += Input.GetAxis("Horizontal_P2");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }
    public static bool SpecialAttackButton_P2()
    {
        return Input.GetButtonDown("SpecialAttackButton_P2");
    }
    public static bool CheckSpecialAttackButton_P2()
    {
        return Input.GetButton("SpecialAttackButton_P2");
    }
    public static Vector3 MainJostick_P2()
    {
        return new Vector3(Horizontal_Button_P2(), 0, Vertical_Button_P2());
    }
    public static bool AttackButton_P2()
    {
        Debug.Log("Jugador 2 ha atacado");
        return Input.GetButtonDown("AttackButton_P2");
    }
    public static bool CheckPressAttackButton_P2()
    {
        return Input.GetButton("AttackButton_P2");
    }
    public static bool DeffenseButton_P2()
    {
        return Input.GetButtonDown("DeffenseButton_P2");
    }
    public static bool CheckPressDeffenseButton_P2()
    {
        return Input.GetButton("DeffenseButton_P2");
    }
    public static bool UpDeffenseButton_P2()
    {
        return Input.GetButtonUp("DeffenseButton_P2");
    }
    public static bool SelectButton_P2()
    {
        return Input.GetButtonDown("SelectButton_P2");
    }

    //--------------------------------------//
}
