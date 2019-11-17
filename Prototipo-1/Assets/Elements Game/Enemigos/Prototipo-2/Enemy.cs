﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Prototipo_2
{
    public class Enemy : MonoBehaviour
    {
        //DATOS PARA EL MOVIMIENTO
        public GameObject alturaMaxima;
        public GameObject[] posicionesDeMovimiento;
        //-------------------------------------------//
        public BarraDeEscudo barraDeEscudo;
        private bool enableSpecialAttack;
        public SpriteEnemy spriteEnemy;
        public GameObject enemyPrefab;
        public Grid gridEnemy;
        public EnumsEnemy enumsEnemy;
        public StructsEnemys structsEnemys;
        public SpriteRenderer SpriteRendererEnemigo;
        public SpecialAttackEnemyController specialAttackEnemyController;
        public GameObject BARRA_DE_VIDA;
        private float auxLife;
        private Animator animator;
        public bool InPool;
        private PoolObject poolObjectEnemy;
        public float life;
        public float maxLife;
        public Image ImageHP;
        public Image ImageCarga;
        private float xpActual;
        public float xpNededSpecialAttack;
        public float xpForHit;
        public Pool poolParabolaAttack;
        public Pool poolObjectAttack;
        private Rigidbody2D rg2D;
        private GameManager gm;
        public GameObject generadoresProyectiles;
        public GameObject generadorProyectilesAgachado;
        public GameObject generadorProyectilParabola;
        public GameObject generadorProyectilParabolaAgachado;
        private float MinRangeRandom = 0;
        private float MaxRangeRandom = 100;
        private float TypeRandom = 3;
        protected float delaySelectMovement;
        public float maxRandomDelayMovement;
        public float minRandomDelayMovement;
        public float delayAttack;
        protected float auxDelayAttack;
        //protected float auxDelayParabolaAttack;
        private bool doubleDamage;
        private bool isDuck;
        private bool isDeffended;
        public float anguloAtaqueSalto;
        public float Speed;
        public float SpeedJump;
        private float auxSpeedJump;
        public float Resistace;
        public float Gravity;
        public float delayAttackJumping;
        private bool isJamping;
        public Collider2D colliderSprites;
        [HideInInspector]
        public Vector3 InitialPosition;
        [HideInInspector]
        public Vector3 pointOfDeath;
        [HideInInspector]
        public Vector3 pointOfCombat;
        public bool damageCounterAttack;
        public bool activateComportamiento;
        public BoxColliderController boxColliderPiernas;
        public BoxColliderController boxColliderSprite;
        public BoxColliderController boxColliderControllerAgachado;
        public BoxColliderController boxColliderControllerParado;
        public BoxColliderController boxColliderControllerSaltando;
        public bool enableMecanicParabolaAttack;

        [Header("Porcentage: Movimiento")]
        public float MovePorcentage;
        public float JumpPorcentage;
        public float DuckPorcentage;
        public float IdlePorcentage;

        [Header("Porcentaje: Direccion de Movimiento")]
        public float MoveForwardPorcentage;
        public float MoveBackPorcentage;

        [Header("Porcentaje: Accion De Salto")]
        public float AttackJumpPorcentage;
        public float DefenceJumpPorcentage;
        public float SimpleJumpPorcentage;

        [Header("Porcentaje: Accion Agacharse")]
        public float AttackDuckPorcentage;
        public float DefenceDuckPorcentage;
        public float SimpleDuckPorcentage;

        [Header("Porcentaje: Accion Quieto")]
        public float AttackPorcentage;
        public float DeffensePorcentage;

        [Header("Porcentaje: Ataque Especial")]
        public float AttackSpecialPorcentage;

        [Header("Porcentaje: Tipo de ataque")]
        public float parabolaAttack;

        [Header("Datos del personaje para la grilla")]
        public int CantCasillasOcupadas_X;
        public int CantCasillasOcupadas_Y;
        public int ColumnaActual;
        private void OnEnable()
        {
            delaySelectMovement = 0.2f;
        }
        public virtual void Start()
        {
            enableSpecialAttack = false;
            auxSpeedJump = SpeedJump;
            InitialPosition = transform.position;
            auxDelayAttack = delayAttack;
            //auxDelayParabolaAttack = delayParabolaAttack;
            delaySelectMovement = 0.2f;
            auxLife = life;
            poolObjectEnemy = GetComponent<PoolObject>();
            animator = GetComponent<Animator>();
            if (GameManager.instanceGameManager != null)
            {
                gm = GameManager.instanceGameManager;
            }
            rg2D = GetComponent<Rigidbody2D>();
            CheckInitialCharacter();
        }
        public virtual void Update()
        {
            //Debug.Log(delaySelectMovement);
            //spriteEnemy.animator.SetBool("AtaqueEspecial", true);
            //enumsEnemy.SetMovement(EnumsEnemy.Movimiento.AtaqueEspecial);
            CheckDeffense();
            CheckBoxColliders2D();
            CheckLifeBar();
            CheckLoadSpecialAttackBar();
            CheckDead();
            if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AtaqueEspecial 
                /*&& enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AtaqueEspecialAgachado 
                && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AtaqueEspecialSalto*/)
            {
                IA();
            }
            CheckOutLimit();
        }
        public Enemy(){}
        
        public void CheckInitialCharacter()
        {
            structsEnemys.dataEnemy.CantCasillasOcupadas_X = CantCasillasOcupadas_X;
            structsEnemys.dataEnemy.CantCasillasOcupadas_Y = CantCasillasOcupadas_Y;
            structsEnemys.dataEnemy.columnaActual = ColumnaActual;
            //gridEnemy.CheckCuadrillaOcupada(structsEnemys.dataEnemy.columnaActual, structsEnemys.dataEnemy.CantCasillasOcupadas_X, structsEnemys.dataEnemy.CantCasillasOcupadas_Y);
        }
        public void CheckOutLimit()
        {
            if (transform.position.y <= InitialPosition.y && enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.Saltar && !isJamping 
                || transform.position.y <= InitialPosition.y && enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoAtaque && !isJamping
                || transform.position.y <= InitialPosition.y && enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoDefensa && !isJamping
                || transform.position.y <= InitialPosition.y && enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecialSalto && !isJamping)
            {
                transform.position = new Vector3(transform.position.x, InitialPosition.y, transform.position.z);
                delaySelectMovement = 0.1f;// SI OCURRE DE NUEVO EL BUG DEL SALTO CAMBIAR ESTA VARIABLE POR EL VALOR 0.2f
                enumsEnemy.SetMovement(EnumsEnemy.Movimiento.Nulo);
                //gridEnemy.CheckCuadrillaOcupada(structsEnemys.dataEnemy.columnaActual, structsEnemys.dataEnemy.CantCasillasOcupadas_X, structsEnemys.dataEnemy.CantCasillasOcupadas_Y);
                SpeedJump = auxSpeedJump;
            }
            else if (transform.position.y > InitialPosition.y && !isJamping)
            {
                MoveJamp(new Vector3(transform.position.x, InitialPosition.y, transform.position.z));
                delaySelectMovement = 0.1f;// SI OCURRE DE NUEVO EL BUG DEL SALTO CAMBIAR ESTA VARIABLE POR EL VALOR 0.2f
            }
            
        }
        public void OnEnemy()
        {
            boxColliderControllerParado.GetBoxCollider2D().enabled = true;
            boxColliderControllerAgachado.GetBoxCollider2D().enabled = false;
            boxColliderControllerSaltando.GetBoxCollider2D().enabled = false;
            enumsEnemy.SetStateEnemy(EnumsEnemy.EstadoEnemigo.vivo);
            poolObjectEnemy = GetComponent<PoolObject>();
            if (GameManager.instanceGameManager != null)
            {
                gm = GameManager.instanceGameManager;
            }
            CheckInitialCharacter();
            delaySelectMovement = 0;
        }
        public void IA()
        {
            if (life > 0)
            {
                if (delaySelectMovement <= 0 && (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.Saltar || enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoAtaque))
                {
                    if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AtaqueEspecial 
                        && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AtaqueEspecialAgachado
                        && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AtaqueEspecialSalto
                        && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.MoverAdelante 
                        && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.MoverAtras)
                    {
                        CheckComportamiento();
                    }
                }
                if (delaySelectMovement > 0)
                {
                    CheckMovement();
                    delaySelectMovement = delaySelectMovement - Time.deltaTime;
                }
            }
        }
        public void CheckBoxColliders2D()
        {
            if (isDuck || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.Agacharse
                || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AgacharseAtaque
                || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AgacheDefensa
                || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecialAgachado)
            {
                if (boxColliderPiernas != null)
                {
                    boxColliderPiernas.GetBoxCollider2D().enabled = false;
                }
                boxColliderControllerAgachado.GetBoxCollider2D().enabled = true;
                boxColliderControllerParado.GetBoxCollider2D().enabled = false;
                boxColliderControllerSaltando.GetBoxCollider2D().enabled = false;
            }
            else if (isJamping || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.Saltar
                || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoAtaque
                || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoDefensa
                || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecialSalto)
            {
                if (boxColliderPiernas != null)
                {
                    boxColliderPiernas.GetBoxCollider2D().enabled = false;
                }
                boxColliderControllerAgachado.GetBoxCollider2D().enabled = false;
                boxColliderControllerParado.GetBoxCollider2D().enabled = false;
                boxColliderControllerSaltando.GetBoxCollider2D().enabled = true;
            }
            else if(enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.Saltar
                || enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoAtaque
                || enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoDefensa
                || enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AtaqueEspecialSalto)
            {
                if (boxColliderPiernas != null)
                {
                    boxColliderPiernas.GetBoxCollider2D().enabled = true;
                }
                boxColliderControllerAgachado.GetBoxCollider2D().enabled = false;
                boxColliderControllerParado.GetBoxCollider2D().enabled = true;
                boxColliderControllerSaltando.GetBoxCollider2D().enabled = false;
            }
        }
        // VA A FALTAR CREAR UNA BARRA DE ATAQUE ESPECIAL Y QUE CUANDO LA BARRA ESTE LLENA LLAME A LA FUNCION DE CHECK ATAQUE ESPECIAL
        public void CheckComportamiento()
        {
            EnumsEnemy.Movimiento movimiento = EnumsEnemy.Movimiento.Nulo;
            if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.MoveToPointCombat && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.MoveToPointDeath)
            {
                if (activateComportamiento)
                {
                    float opcionMovement = Random.Range(MinRangeRandom, MaxRangeRandom);

                    if (opcionMovement < MovePorcentage)
                    {
                        //MOVIMIENTO 
                        if (structsEnemys.dataEnemy.columnaActual >= gridEnemy.GetCuadrilla_columnas() - 1)
                        {
                            movimiento = EnumsEnemy.Movimiento.MoverAdelante;
                        }
                        else if (structsEnemys.dataEnemy.columnaActual <= 0)
                        {
                            movimiento = EnumsEnemy.Movimiento.MoverAtras;
                        }
                        else
                        {
                            opcionMovement = Random.Range(MinRangeRandom, MaxRangeRandom);
                            if (opcionMovement < MoveBackPorcentage)
                            {
                                movimiento = EnumsEnemy.Movimiento.MoverAtras;
                            }
                            else
                            {
                                movimiento = EnumsEnemy.Movimiento.MoverAdelante;
                            }
                        }
                    }
                    else if (opcionMovement >= MovePorcentage && opcionMovement < (MovePorcentage + JumpPorcentage))
                    {
                        //SALTO
                        opcionMovement = Random.Range(MinRangeRandom, MaxRangeRandom);
                        if (opcionMovement < AttackJumpPorcentage)
                        {
                            movimiento = EnumsEnemy.Movimiento.SaltoAtaque;
                        }
                        else if (opcionMovement >= AttackJumpPorcentage && opcionMovement < (AttackJumpPorcentage + DefenceJumpPorcentage))
                        {
                            movimiento = EnumsEnemy.Movimiento.SaltoDefensa;
                        }
                        else if (opcionMovement >= (AttackJumpPorcentage + DefenceJumpPorcentage))
                        {
                            movimiento = EnumsEnemy.Movimiento.Saltar;
                        }
                    }
                    else if (opcionMovement >= (MovePorcentage + JumpPorcentage) && opcionMovement < (MovePorcentage + JumpPorcentage + DuckPorcentage))
                    {
                        //AGACHARSE
                        opcionMovement = Random.Range(MinRangeRandom, MaxRangeRandom);
                        if (opcionMovement < AttackDuckPorcentage)
                        {
                            movimiento = EnumsEnemy.Movimiento.AgacharseAtaque;
                        }
                        else if (opcionMovement >= AttackDuckPorcentage && opcionMovement < (AttackDuckPorcentage + DefenceDuckPorcentage))
                        {
                            movimiento = EnumsEnemy.Movimiento.AgacheDefensa;
                        }
                        else if (opcionMovement >= (AttackDuckPorcentage + DefenceDuckPorcentage))
                        {
                            movimiento = EnumsEnemy.Movimiento.Agacharse;
                        }
                    }
                    else if (opcionMovement >= (MovePorcentage + JumpPorcentage + DuckPorcentage))
                    {
                        //QUIETO EN EL LUGAR
                        opcionMovement = Random.Range(MinRangeRandom, MaxRangeRandom);
                        if (opcionMovement < AttackPorcentage)
                        {
                            movimiento = EnumsEnemy.Movimiento.AtacarEnElLugar;
                        }
                        else if (opcionMovement >= AttackPorcentage && opcionMovement < (AttackPorcentage + DeffensePorcentage))
                        {
                            movimiento = EnumsEnemy.Movimiento.DefensaEnElLugar;
                        }
                    }
                }
            }
            else
            {
                movimiento = enumsEnemy.GetMovement();
            }
            delaySelectMovement = Random.Range(minRandomDelayMovement, maxRandomDelayMovement);
            enumsEnemy.SetMovement(movimiento);
            //Debug.Log(movimiento.ToString());
            if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoAtaque || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecialSalto)
            {
                delayAttack = delayAttackJumping;
            }
            if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoDefensa || enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.DefensaEnElLugar)
            {
                isDeffended = false;
            }

            //SACAR LA CONDICION QUE SEA IGUAL AL JEFE CUANDO ESTE TENGA UN COMPORTAMIENTO
            if (enumsEnemy.typeEnemy == EnumsEnemy.TiposDeEnemigo.Jefe || !activateComportamiento)
            {
                DefaultBehavior();
            }
        }
        public EnumsEnemy.Movimiento CheckSpecialAttack(EnumsEnemy.Movimiento _movimiento)
        {
            float specialMovement = Random.Range(MinRangeRandom, MaxRangeRandom);
            EnumsEnemy.Movimiento movimiento = _movimiento;
            if (specialMovement < AttackSpecialPorcentage)
            {
                if (_movimiento == EnumsEnemy.Movimiento.Agacharse || _movimiento == EnumsEnemy.Movimiento.AgacharseAtaque || _movimiento == EnumsEnemy.Movimiento.AgacheDefensa)
                {
                    movimiento = EnumsEnemy.Movimiento.AtaqueEspecialAgachado;
                }
                else if (_movimiento == EnumsEnemy.Movimiento.Saltar || _movimiento == EnumsEnemy.Movimiento.SaltoAtaque || _movimiento == EnumsEnemy.Movimiento.SaltoDefensa)
                {
                    movimiento = EnumsEnemy.Movimiento.AtaqueEspecialSalto;
                }
                else if (_movimiento == EnumsEnemy.Movimiento.AtacarEnElLugar || _movimiento == EnumsEnemy.Movimiento.DefensaEnElLugar)
                {
                    movimiento = EnumsEnemy.Movimiento.AtaqueEspecial;
                }
            }
            return movimiento;
        }
        public void DefaultBehavior()
        {
            EnumsEnemy.Movimiento movimiento;
            //gridEnemy.CheckCuadrillaOcupada(structsEnemys.dataEnemy.columnaActual, structsEnemys.dataEnemy.CantCasillasOcupadas_X, structsEnemys.dataEnemy.CantCasillasOcupadas_Y);
            int min = (int)EnumsEnemy.Movimiento.Nulo + 1;
            int max = (int)EnumsEnemy.Movimiento.Count - 3;
            if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.MoveToPointCombat && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.MoveToPointDeath)
            {
                movimiento = (EnumsEnemy.Movimiento)Random.Range(min, max);
                switch (movimiento)
                {
                    case EnumsEnemy.Movimiento.AgacheDefensa:
                        movimiento = EnumsEnemy.Movimiento.Agacharse;
                        break;
                    case EnumsEnemy.Movimiento.DefensaEnElLugar:
                        movimiento = EnumsEnemy.Movimiento.AtacarEnElLugar;
                        break;
                    case EnumsEnemy.Movimiento.SaltoDefensa:
                        movimiento = EnumsEnemy.Movimiento.Saltar;
                        break;
                }
            }
            else
            {
                movimiento = enumsEnemy.GetMovement();
            }
            delaySelectMovement = Random.Range(minRandomDelayMovement, maxRandomDelayMovement);
            enumsEnemy.SetMovement(movimiento);
            //Debug.Log(movimiento.ToString());
            if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoAtaque || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecialSalto)
            {
                delayAttack = delayAttackJumping;
            }
            if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoDefensa || enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.DefensaEnElLugar)
            {
                isDeffended = false;
            }
        }
        public void CheckLifeBar()
        {
            if (life <= maxLife)
            {
                ImageHP.fillAmount = life / maxLife;
            }
            else if (life > maxLife)
            {
                life = maxLife;
            }
            else if (life < 0)
            {
                life = 0;
            }
        }
        public void CheckLoadSpecialAttackBar()
        {
            if (ImageCarga != null)
            {
                if (xpActual >= xpNededSpecialAttack)
                {
                    xpActual = xpNededSpecialAttack;
                    enableSpecialAttack = true;
                }
                if (xpActual <= xpNededSpecialAttack)
                {
                    ImageCarga.fillAmount = xpActual / xpNededSpecialAttack;
                }
                if (xpActual < 0)
                {
                    xpActual = 0;
                }
                if (enableSpecialAttack)
                {
                    boxColliderControllerAgachado.state = BoxColliderController.StateBoxCollider.Normal;
                    boxColliderControllerParado.state = BoxColliderController.StateBoxCollider.Normal;
                    boxColliderControllerSaltando.state = BoxColliderController.StateBoxCollider.Normal;
                    //boxColliderPiernas.state = BoxColliderController.StateBoxCollider.Normal;
                    boxColliderSprite.state = BoxColliderController.StateBoxCollider.Normal;

                    if (!isJamping && !isDuck
                        && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.Saltar
                        && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoAtaque
                        && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoDefensa)
                    {
                        delaySelectMovement = 0.1f;
                        enableSpecialAttack = false;
                        enumsEnemy.SetMovement(EnumsEnemy.Movimiento.AtaqueEspecial);
                        AnimationAttack();
                        xpActual = 0;
                    }
                    else if (enumsEnemy.typeEnemy == EnumsEnemy.TiposDeEnemigo.Balanceado)
                    {
                        if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.Saltar
                            || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoAtaque
                            || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoDefensa)
                        {
                            enumsEnemy.SetMovement(EnumsEnemy.Movimiento.AtaqueEspecialSalto);
                        }
                        else if (isDuck)
                        {
                            enumsEnemy.SetMovement(EnumsEnemy.Movimiento.AtaqueEspecialAgachado);
                        }
                        else
                        {
                            enumsEnemy.SetMovement(EnumsEnemy.Movimiento.AtaqueEspecial);
                        }
                        AnimationAttack();
                        xpActual = 0;

                    }
                }
            }
        }
        public void MoveToPoint(Vector3 pointCombat)
        {
            if (CheckMove(pointCombat))
            {
                delaySelectMovement = 999;
                if (pointCombat.x < transform.position.x)
                {
                    enemyPrefab.transform.Translate(Vector3.left * Speed * Time.deltaTime);
                }
                else if (pointCombat.x > transform.position.x)
                {
                    enemyPrefab.transform.Translate(Vector3.right * Speed * Time.deltaTime);
                }
            }
            else
            {
                delaySelectMovement = 0;
                enumsEnemy.SetMovement(EnumsEnemy.Movimiento.Nulo);
            }
        }
        //INTERACTUA CON GAME MANAGER
        public void CheckDead()
        {
            if (!InPool)
            {
                if (life <= 0)
                {
                    // SI SU VIDA ES IGUAL A 0 POS MUERE DESACTIVADO
                    enumsEnemy.SetStateEnemy(EnumsEnemy.EstadoEnemigo.muerto);
                    gm.countEnemysDead++;
                    gm.playerData_P1.score = gm.playerData_P1.score + gm.playerData_P1.scoreForEnemyDead;
                    gm.ResetRoundCombat(false);
                    ResetEnemy();
                    enemyPrefab.gameObject.SetActive(false);
                    xpActual = 0;
                }
            }
            else if (InPool)
            {
                switch (gm.enumsGameManager.modoDeJuego)
                {
                    case EnumsGameManager.ModosDeJuego.Supervivencia:
                        if (life <= 0)
                        {
                            life = auxLife;
                            gm.generateEnemy = true;
                            enumsEnemy.SetStateEnemy(EnumsEnemy.EstadoEnemigo.muerto);
                            gm.countEnemysDead++;
                            gm.playerData_P1.score = gm.playerData_P1.score + gm.playerData_P1.scoreForEnemyDead;
                            gm.ResetRoundCombat(false);
                            ResetEnemy();
                            poolObjectEnemy.Recycle();
                            xpActual = 0;
                        }
                        break;
                    case EnumsGameManager.ModosDeJuego.Historia:
                        if (life <= 0)
                        {
                            life = auxLife;
                            gm.generateEnemy = true;
                            enumsEnemy.SetStateEnemy(EnumsEnemy.EstadoEnemigo.muerto);
                            gm.countEnemysDead++;
                            gm.playerData_P1.score = gm.playerData_P1.score + gm.playerData_P1.scoreForEnemyDead;
                            gm.ResetRoundCombat(false);
                            ResetEnemy();
                            poolObjectEnemy.Recycle();
                            xpActual = 0;
                        }
                        break;
                    case EnumsGameManager.ModosDeJuego.Nulo:
                        if (life <= 0)
                        {
                            gm.countEnemysDead++;
                            gm.playerData_P1.score = gm.playerData_P1.score + gm.playerData_P1.scoreForEnemyDead;
                            gm.ResetRoundCombat(false);
                            ResetEnemy();
                            enumsEnemy.SetStateEnemy(EnumsEnemy.EstadoEnemigo.muerto);
                            enemyPrefab.gameObject.SetActive(false);
                            xpActual = 0;
                        }
                        break;
                }
            }
        }
        public void CheckMovement()
        {
            if (barraDeEscudo != null)
            {
                if ((enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.DefensaEnElLugar
                && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AgacheDefensa
                && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoDefensa)
                || barraDeEscudo.nededBarMaxPorcentage)
                {

                    barraDeEscudo.AddPorcentageBar();

                    if (barraDeEscudo.GetValueShild() <= barraDeEscudo.porcentageNededForDeffence)
                    {
                        //Debug.Log("ENTRE");
                        barraDeEscudo.SetEnableDeffence(false);
                        //if (spriteEnemy.ActualSprite != SpriteEnemy.SpriteActual.RecibirDanio)
                        //{
                        if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AgacheDefensa)
                        {
                            enumsEnemy.SetMovement(EnumsEnemy.Movimiento.Agacharse);
                        }
                        else if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoDefensa)
                        {
                            enumsEnemy.SetMovement(EnumsEnemy.Movimiento.Saltar);
                        }
                        else if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoDefensa
                            && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoAtaque
                            && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.Saltar
                            && !isDuck)
                        {
                            enumsEnemy.SetMovement(EnumsEnemy.Movimiento.Nulo);
                        }
                        //}
                    }
                }
            }
            switch (enumsEnemy.GetMovement())
            {
                case EnumsEnemy.Movimiento.AtacarEnElLugar:
                    CheckDelayAttack(false);
                    isDeffended = false;
                    break;
                case EnumsEnemy.Movimiento.AgacharseAtaque:
                    Duck(structsEnemys.dataEnemy.CantCasillasOcupadas_Y);
                    CheckDelayAttack(false);
                    isDeffended = false;
                    break;
                case EnumsEnemy.Movimiento.SaltoAtaque:
                    if (!isJamping && SpeedJump > 0 && structsEnemys.particleMovement.particleJump != null)
                    {
                        structsEnemys.particleMovement.particleJump.transform.position = new Vector3(transform.position.x, structsEnemys.particleMovement.particleJump.transform.position.y, structsEnemys.particleMovement.particleJump.transform.position.z);
                        structsEnemys.particleMovement.particleJump.SetActive(true);
                    }
                    CheckDelayAttack(false);
                    isJamping = true;
                    Jump(alturaMaxima.transform.position);
                    isDeffended = false;
                    break;
                case EnumsEnemy.Movimiento.MoverAtras:
                    isDeffended = false;
                    if (structsEnemys.dataEnemy.columnaActual < gridEnemy.GetCuadrilla_columnas() - 1)
                    {
                        MoveRight(posicionesDeMovimiento[structsEnemys.dataEnemy.columnaActual + 1].transform.position);
                    }
                    else
                    {
                        delaySelectMovement = 0;
                    }
                    break;
                case EnumsEnemy.Movimiento.MoverAdelante:
                    isDeffended = false;
                    if (structsEnemys.dataEnemy.columnaActual > 0)
                    {
                        MoveLeft(posicionesDeMovimiento[structsEnemys.dataEnemy.columnaActual - 1].transform.position);
                    }
                    else
                    {
                        delaySelectMovement = 0;
                    }
                    break;
                case EnumsEnemy.Movimiento.Saltar:
                    if (!isJamping && SpeedJump > 0 && structsEnemys.particleMovement.particleJump != null)
                    {
                        structsEnemys.particleMovement.particleJump.transform.position = new Vector3(transform.position.x, structsEnemys.particleMovement.particleJump.transform.position.y, structsEnemys.particleMovement.particleJump.transform.position.z);
                        structsEnemys.particleMovement.particleJump.SetActive(true);
                    }
                    isDeffended = false;
                    isJamping = true;
                    Jump(alturaMaxima.transform.position);
                    break;
                case EnumsEnemy.Movimiento.DefensaEnElLugar:
                    Deffence();
                    break;
                case EnumsEnemy.Movimiento.SaltoDefensa:
                    if (!isJamping && SpeedJump > 0 && structsEnemys.particleMovement.particleJump != null)
                    {
                        structsEnemys.particleMovement.particleJump.transform.position = new Vector3(transform.position.x, structsEnemys.particleMovement.particleJump.transform.position.y, structsEnemys.particleMovement.particleJump.transform.position.z);
                        structsEnemys.particleMovement.particleJump.SetActive(true);
                    }
                    isJamping = true;
                    Jump(alturaMaxima.transform.position);
                    Deffence();
                    break;
                case EnumsEnemy.Movimiento.AgacheDefensa:
                    Duck(structsEnemys.dataEnemy.CantCasillasOcupadas_Y);
                    Deffence();
                    break;
                case EnumsEnemy.Movimiento.Agacharse:
                    isDeffended = false;
                    Duck(structsEnemys.dataEnemy.CantCasillasOcupadas_Y);
                    break;
                case EnumsEnemy.Movimiento.AtaqueEspecial:
                    //CheckDelayAttack(true);
                    isDeffended = false;
                    break;
                case EnumsEnemy.Movimiento.AtaqueEspecialAgachado:
                    Duck(structsEnemys.dataEnemy.CantCasillasOcupadas_Y);
                    //CheckDelayAttack(true);
                    isDeffended = false;
                    break;
                case EnumsEnemy.Movimiento.AtaqueEspecialSalto:
                    //CheckDelayAttack(true);
                    if (!isJamping && SpeedJump > 0 && structsEnemys.particleMovement.particleJump != null)
                    {
                        structsEnemys.particleMovement.particleJump.transform.position = new Vector3(transform.position.x, structsEnemys.particleMovement.particleJump.transform.position.y, structsEnemys.particleMovement.particleJump.transform.position.z);
                        structsEnemys.particleMovement.particleJump.SetActive(true);
                    }
                    isDeffended = false;
                    isJamping = true;
                    Jump(alturaMaxima.transform.position);
                    break;
                case EnumsEnemy.Movimiento.MoveToPointCombat:
                    isDeffended = false;
                    MoveToPoint(pointOfCombat);
                    break;
                case EnumsEnemy.Movimiento.MoveToPointDeath:
                    isDeffended = false;
                    MoveToPoint(pointOfDeath);
                    break;
            }
            if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AgacharseAtaque)
            {
                colliderSprites.enabled = true;
                isDuck = false;
            }
            if (barraDeEscudo != null)
            {
                if (spriteEnemy.ActualSprite != SpriteEnemy.SpriteActual.AgachadoDefensa
                    && spriteEnemy.ActualSprite != SpriteEnemy.SpriteActual.ParadoDefensa
                    && spriteEnemy.ActualSprite != SpriteEnemy.SpriteActual.SaltoDefensa
                    && barraDeEscudo.GetValueShild() > barraDeEscudo.porcentageNededForDeffence
                        && barraDeEscudo.GetEnableDeffence())
                {
                    isDeffended = false;
                }
            }
            //CHEKEA EL MOVIMIENTO DEL ENEMIGO
        }
        public void CheckDeffense()
        {
            //Debug.Log(isDeffended);
            if (barraDeEscudo != null)
            {
                if (isDeffended && barraDeEscudo.GetValueShild() > barraDeEscudo.porcentageNededForDeffence
                        && barraDeEscudo.GetEnableDeffence())
                {
                    boxColliderControllerAgachado.state = BoxColliderController.StateBoxCollider.Defendido;
                    boxColliderControllerParado.state = BoxColliderController.StateBoxCollider.Defendido;
                    boxColliderControllerSaltando.state = BoxColliderController.StateBoxCollider.Defendido;
                    boxColliderSprite.state = BoxColliderController.StateBoxCollider.Defendido;
                }
                else
                {
                    boxColliderControllerAgachado.state = BoxColliderController.StateBoxCollider.Normal;
                    boxColliderControllerParado.state = BoxColliderController.StateBoxCollider.Normal;
                    boxColliderControllerSaltando.state = BoxColliderController.StateBoxCollider.Normal;
                    boxColliderSprite.state = BoxColliderController.StateBoxCollider.Normal;
                    if (barraDeEscudo != null)
                    {
                        barraDeEscudo.AddPorcentageBar();

                        if (barraDeEscudo.GetValueShild() <= barraDeEscudo.porcentageNededForDeffence)
                        {
                            //Debug.Log("ENTRE");
                            barraDeEscudo.SetEnableDeffence(false);
                        }
                    }
                }
            }
        }
        public void Deffence()
        {
            isDeffended = true;
            if (!isDuck
                && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.Saltar
                && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoAtaque
                && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoDefensa
                && !isJamping)
            {
                boxColliderControllerParado.state = BoxColliderController.StateBoxCollider.Defendido;
            }
            else if (isDuck
                && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.Saltar
                && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoAtaque
                && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoDefensa
                && !isJamping)
            {
                boxColliderControllerAgachado.state = BoxColliderController.StateBoxCollider.Defendido;
            }
            else if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.Saltar
                || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoAtaque
                || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoDefensa
                || isJamping)
            {
                boxColliderControllerSaltando.state = BoxColliderController.StateBoxCollider.Defendido;
            }
            boxColliderSprite.state = BoxColliderController.StateBoxCollider.Defendido;
            if (barraDeEscudo != null)
            {
                barraDeEscudo.SubstractPorcentageBar();
                if (barraDeEscudo.nededBarMaxPorcentage && barraDeEscudo.ValueShild < barraDeEscudo.MaxValueShild)
                {
                    delaySelectMovement = 0.0f;
                }
            }
            
        }
        public virtual void CheckDelayAttack(bool specialAttack)
        {
            if (delayAttack > 0)
            {
                delayAttack = delayAttack - Time.deltaTime;
            }
            else if (delayAttack <= 0)
            {
                delayAttack = auxDelayAttack;
                if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoAtaque || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.Nulo || enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.AtaqueEspecialSalto)
                {
                    delayAttack = delayAttackJumping;
                    Attack(true, specialAttack,false);
                }
                else
                {
                    Attack(false, specialAttack,false);
                }
            }
        }
        public void ResetEnemy()
        {
            life = maxLife;
            transform.position = InitialPosition;
            enemyPrefab.transform.position = InitialPosition;
        }
        public void MoveLeft(Vector3 cuadrillaDestino)
        {
            if (CheckMove(new Vector3(posicionesDeMovimiento[0].transform.position.x, transform.position.y, transform.position.z)) && transform.position.x > cuadrillaDestino.x)
            {
                Move(Vector3.left);
                enumsEnemy.SetMovement(EnumsEnemy.Movimiento.MoverAdelante);
            }
            else if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.Nulo)
            {
                structsEnemys.dataEnemy.columnaActual--;
                enumsEnemy.SetMovement(EnumsEnemy.Movimiento.Nulo);
                //gridEnemy.CheckCuadrillaOcupada(structsEnemys.dataEnemy.columnaActual, structsEnemys.dataEnemy.CantCasillasOcupadas_X, structsEnemys.dataEnemy.CantCasillasOcupadas_Y);
                delaySelectMovement = 0;
            }
        }
        public void MoveRight(Vector3 cuadrillaDestino)
        {
            if (CheckMove(new Vector3(posicionesDeMovimiento[posicionesDeMovimiento.Length-1].transform.position.x, transform.position.y, transform.position.z)) && transform.position.x < cuadrillaDestino.x)
            {
                Move(Vector3.right);
                enumsEnemy.SetMovement(EnumsEnemy.Movimiento.MoverAtras);
            }
            else if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.Nulo)
            {
                structsEnemys.dataEnemy.columnaActual++;
                enumsEnemy.SetMovement(EnumsEnemy.Movimiento.Nulo);
                //gridEnemy.CheckCuadrillaOcupada(structsEnemys.dataEnemy.columnaActual, structsEnemys.dataEnemy.CantCasillasOcupadas_X, structsEnemys.dataEnemy.CantCasillasOcupadas_Y);
                delaySelectMovement = 0;
            }
        }
        public virtual void AnimationAttack(){ }
        public virtual void Attack(bool jampAttack, bool specialAttack, bool _doubleDamage){ }
        public virtual void Attack(bool jampAttack, bool specialAttack, bool _doubleDamage, Proyectil cuadrilla) { }
        public void CheckSpecialAttackEnemyController(int minRandomRootShoot, int maxRandomRootShoot, GameObject generador)
        {
            if (!isDuck)
            {
                if (generador != null)
                {
                    specialAttackEnemyController.SpecialAttack(doubleDamage, isDuck, generador, generador, enumsEnemy, structsEnemys, maxRandomRootShoot, minRandomRootShoot);
                }
            }
            else
            {
                if (generador != null)
                {
                    specialAttackEnemyController.SpecialAttack(doubleDamage, isDuck, generador, generador, enumsEnemy, structsEnemys, maxRandomRootShoot, minRandomRootShoot);
                }
            }
            
        }
        public void CounterAttack(bool dobleDamage)
        {
            Attack(false,false, dobleDamage);
        }
        public void Jump(Vector3 alturaMaxima)
        {
            if (CheckMove(new Vector3(transform.position.x, alturaMaxima.y, transform.position.z)) && isJamping)
            {
                if (enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoAtaque && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.SaltoDefensa && enumsEnemy.GetMovement() != EnumsEnemy.Movimiento.AtaqueEspecialSalto)
                {
                    enumsEnemy.SetMovement(EnumsEnemy.Movimiento.Saltar);
                }
                MoveJamp(Vector3.up);
                if (SpeedJump <= 0)
                {
                    isJamping = false;
                }
                if (delaySelectMovement <= 0)
                {
                    delaySelectMovement = 0.1f;
                }
                //gridEnemy.matrizCuadrilla[gridEnemy.baseGrild][structsEnemys.dataEnemy.columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Libre);

                //Debug.Log(gridPlayer.matrizCuadrilla[gridPlayer.baseGrild][structsPlayer.dataPlayer.columnaActual].name);
                if (enumsEnemy.GetMovement() == EnumsEnemy.Movimiento.SaltoDefensa)
                {
                    //gridEnemy.matrizCuadrilla[gridEnemy.baseGrild - 1][structsEnemys.dataEnemy.columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                    //gridEnemy.matrizCuadrilla[gridEnemy.baseGrild - 2][structsEnemys.dataEnemy.columnaActual].SetStateCuadrilla(Cuadrilla.StateCuadrilla.Ocupado);
                    Deffence();
                }
            }

            //CheckOutLimit();
        }
        public void ParabolaAttack()
        {
            if (enableMecanicParabolaAttack)
            {
                GameObject go = poolParabolaAttack.GetObject();
                ProyectilParabola proyectil = go.GetComponent<ProyectilParabola>();
                proyectil.SetDobleDamage(false);
                proyectil.disparadorDelProyectil = Proyectil.DisparadorDelProyectil.Enemigo;
                proyectil.SetEnemy(this);
                if (!GetIsDuck())
                {
                    proyectil.TypeRoot = 1;
                    go.transform.position = generadorProyectilParabola.transform.position;
                }
                else
                {
                    proyectil.TypeRoot = 2;
                    go.transform.position = generadorProyectilParabolaAgachado.transform.position;
                }
                switch (proyectil.TypeRoot)
                {
                    case 1:
                        proyectil.rutaParabola_AtaqueEnemigo = structsEnemys.ruta;
                        break;
                    case 2:
                        proyectil.rutaParabolaAgachado_AtaqueEnemigo = structsEnemys.rutaAgachado;
                        break;
                }
                proyectil.rutaParabola_AtaqueEnemigo = structsEnemys.ruta;
                proyectil.OnParabola();
                delayAttack = auxDelayAttack;
            }
        }
        public bool CheckMove(Vector3 PosicionDestino)
        {
            Vector3 distaciaObjetivo = transform.position - PosicionDestino;
            bool mover = false;
            if (distaciaObjetivo.magnitude > 0.2f)
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
        public void Duck(int rangoAgachado)
        {
            isDuck = true;
        }
        public bool GetIsDeffended()
        {
            return isDeffended;
        }
        public bool GetIsJamping()
        {
            return isJamping;
        }
        public bool GetIsDuck()
        {
            return isDuck;
        }
        public void SetXpActual(float xp)
        {
            xpActual = xp;
        }
        public float GetXpActual()
        {
            return xpActual;
        }
        public float GetAuxDelayAttack()
        {
            return auxDelayAttack;
        }
        public float GetAuxSpeedJamp()
        {
            return auxSpeedJump;
        }
        public void SetDelaySelectMovement(float delay)
        {
            delaySelectMovement = delay;
        }
        public void SetEnableSpecialAttack(bool _enableSpecialAttack)
        {
            enableSpecialAttack = _enableSpecialAttack;
        }
    }
}
