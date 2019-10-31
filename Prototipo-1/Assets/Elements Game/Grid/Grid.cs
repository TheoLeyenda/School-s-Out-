﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototipo_2;
using UnityEngine.SceneManagement;
public class Grid : MonoBehaviour
{
    public enum IdPlataforma
    {
        PlataformaAnatomia,
        PlataformaHistoria,
        PlataformaEducacionFisica,
        PlataformaArte,
        PlataformaMatematica,
        PlataformaQuimica,
        PlataformaProgramacion,
        PlataformaTESIS,
        PlatafomaCafeteria,
        Count,

    }
    private IdPlataforma idPlataforma;
    public List<GameObject> Plataformas;
    public Cuadrilla[] cuadrilla;
    public List<List<Cuadrilla>> matrizCuadrilla;
    private int cuadrilla_columnas = 3;
    private int cuadrilla_filas = 3;
    public Cuadrilla leftCuadrilla;
    public Cuadrilla rightCuadrilla;
    public int baseGrild = 2;
    private GameManager gm;
    private void Awake()
    {
        matrizCuadrilla = new List<List<Cuadrilla>>();
        InitGrid();
        InitMatrizCuadrilla();
        if (GameManager.instanceGameManager != null)
        {
            gm = GameManager.instanceGameManager;
        }
    }
    private void Start()
    {
        ActivatePlataforma();
        
    }
    public void ActivatePlataforma()
    {
        if (Plataformas.Count > 0)
        {
            for (int i = 0; i < Plataformas.Count; i++)
            {
                if (Plataformas[i] != null)
                {
                    Plataformas[i].SetActive(false);
                }
            }
            if (SceneManager.GetActiveScene().name != "PvP")
            {
                //CUANDO AYA MAS NIVELES AGREGARLOS ACA PARA ACTIVAR LAS PLATAFORMAS DEPENDIENDO DEL NIVEL
                switch (SceneManager.GetActiveScene().name)
                {
                    case "Nivel 1":
                        idPlataforma = IdPlataforma.PlataformaAnatomia;
                        break;
                    case "Nivel 2":
                        idPlataforma = IdPlataforma.PlataformaHistoria;
                        break;
                    case "Nivel 3":
                        idPlataforma = IdPlataforma.PlataformaEducacionFisica;
                        break;
                    case "Supervivencia":
                        idPlataforma = IdPlataforma.PlatafomaCafeteria;
                        break;
                }
                Plataformas[(int)idPlataforma].SetActive(true);
            }
            else if (SceneManager.GetActiveScene().name == "PvP")
            {
                if (gm != null)
                {
                    Plataformas[(int)gm.structGameManager.gm_dataCombatPvP.level_selected].SetActive(true);
                }
            }
        }
    }
    public void InitGrid()
    {
        for (int i = 0; i < cuadrilla_filas; i++)
        {
            matrizCuadrilla.Add(new List<Cuadrilla>());
            for (int j = 0; j < cuadrilla_columnas; j++)
            {
                matrizCuadrilla[i].Add(null);
            }
        }
    }
    public int GetCuadrilla_columnas()
    {
        return cuadrilla_columnas;
    }
    public int GetCuadrilla_filas()
    {
        return cuadrilla_filas;
    }
    public void CheckCuadrillaOrden()
    {
        for (int i = 0; i < cuadrilla_filas; i++)
        {
            for (int j = 0; j < cuadrilla_columnas; j++)
            {
                Debug.Log(matrizCuadrilla[i][j].name);
            }
        }
    }
    public void RestartCuadrillas()
    {
        if (matrizCuadrilla != null)
        {
            for (int i = 0; i < matrizCuadrilla.Count; i++)
            {
                for (int j = 0; j < matrizCuadrilla[i].Count; j++)
                {
                    matrizCuadrilla[i][j].ResetCuadrilla();
                }
            }
        }
    }
    public void InitMatrizCuadrilla()
    {
        int aux = 0;
        for (int i = 0; i < cuadrilla_filas; i++)
        {
            for (int j = 0; j < cuadrilla_columnas; j++)
            {
                matrizCuadrilla[i][j] = cuadrilla[aux];
                aux++;
            }
        }
    }
    public void CheckCuadrillaOcupada(int columnaActual, int CasillasBaseOcupadas,int CasillasAltasOcupadas)
    {
        if (matrizCuadrilla != null)
        {
            int casillaBase = 2;
            RestartCuadrillas();
            matrizCuadrilla[casillaBase][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
            switch (CasillasBaseOcupadas)
            {
                case 1:

                    switch (CasillasAltasOcupadas)
                    {
                        case 2:
                            matrizCuadrilla[casillaBase - 1][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            break;
                        case 3:
                            matrizCuadrilla[casillaBase - 1][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            matrizCuadrilla[casillaBase - 2][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            break;
                    }
                    break;

                case 2:
                    switch (columnaActual)
                    {
                        case 0:

                            matrizCuadrilla[casillaBase][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            switch (CasillasAltasOcupadas)
                            {
                                case 2:
                                    matrizCuadrilla[casillaBase - 1][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    matrizCuadrilla[casillaBase - 1][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    break;
                                case 3:
                                    matrizCuadrilla[casillaBase - 1][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    matrizCuadrilla[casillaBase - 1][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    matrizCuadrilla[casillaBase - 2][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    matrizCuadrilla[casillaBase - 2][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    break;
                            }

                            break;
                        case 1:
                            matrizCuadrilla[casillaBase][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            switch (CasillasAltasOcupadas)
                            {
                                case 2:
                                    matrizCuadrilla[casillaBase - 1][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    matrizCuadrilla[casillaBase - 1][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    break;
                                case 3:
                                    matrizCuadrilla[casillaBase - 1][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    matrizCuadrilla[casillaBase - 1][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    matrizCuadrilla[casillaBase - 2][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    matrizCuadrilla[casillaBase - 2][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                                    break;
                            }
                            break;
                    }
                    break;
                case 3:
                    matrizCuadrilla[casillaBase][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                    matrizCuadrilla[casillaBase][columnaActual + 2].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                    switch (CasillasAltasOcupadas)
                    {
                        case 2:
                            matrizCuadrilla[casillaBase + 1][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            matrizCuadrilla[casillaBase + 1][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            matrizCuadrilla[casillaBase + 1][columnaActual + 2].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            break;
                        case 3:
                            matrizCuadrilla[casillaBase + 1][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            matrizCuadrilla[casillaBase + 1][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            matrizCuadrilla[casillaBase + 1][columnaActual + 2].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            matrizCuadrilla[casillaBase + 2][columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            matrizCuadrilla[casillaBase + 2][columnaActual + 1].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            matrizCuadrilla[casillaBase + 2][columnaActual + 2].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                            break;
                    }
                    break;
            }
        }
    }
}
