﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreAnimator : MonoBehaviour
{
    public const string MOVEMENT_STATE = "Movement";
    public const string MOVEMENT_X = "MovX";
    public const string MOVEMENT_Y = "MovY";
    public const string ATTACK_STATE = "Attack";
    public const string ATTACK_VALUE = "Attack_Value";
}

public class Locomotion : CoreAnimator
{
    public enum TLocomotion
    {
        Idle,
        Movement,
        Attack,
        Dead
    }

    public enum TSpeed
    {
        Walk,
        Run
    }

    public TLocomotion typeLocomotion = TLocomotion.Idle;
    public TSpeed typeSpeed = TSpeed.Walk;
    public Animator animControl;

    [Header("Health System")]
    public int curHealth = 100;
    public int maxHealth = 100;

    public Locomotion target;

    private Transform localTransform;
    private float factor = 1f;

    #region Core

    public void CoreUpdate()
    {
        ControlState();
    }

    public void Initialization()
    {
        localTransform = animControl.transform;
    }

    public void Movement(Vector3 direction)
    {
        Vector3 inversDirection = localTransform.InverseTransformDirection(direction);

        if (typeSpeed == TSpeed.Walk)
        {
            factor = Mathf.Lerp(factor, 1, 0.1f);
        }
        else if (typeSpeed == TSpeed.Run)
        {
            factor = Mathf.Lerp(factor, 2, 0.1f);
        }

        if(inversDirection.magnitude > 0)
        {
            animControl.SetFloat(MOVEMENT_X, Mathf.Lerp(animControl.GetFloat(MOVEMENT_X), inversDirection.x * factor, 0.05f));
            animControl.SetFloat(MOVEMENT_Y, Mathf.Lerp(animControl.GetFloat(MOVEMENT_Y), inversDirection.z * factor, 0.05f));
        }

        animControl.SetBool(MOVEMENT_STATE, inversDirection.magnitude > 0);
    }

    public void Rotate(Vector3 movDirection)
    {
        if (typeLocomotion == TLocomotion.Dead) { return; }

        float lerpFactor = 0.1f;

        if (typeLocomotion == TLocomotion.Attack) { lerpFactor /= 2; }

        Vector3 fixDir = movDirection;
        fixDir.y = 0;

        Quaternion fixRotation = Quaternion.Lerp(localTransform.rotation, Quaternion.LookRotation(fixDir, Vector3.up), lerpFactor);
        localTransform.rotation = fixRotation;
    }

    public void AttackControl()
    {
        animControl.SetTrigger(ATTACK_STATE);
    }

    private void ControlState()
    {
        if (animControl.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            typeLocomotion = TLocomotion.Idle;
        }
        else if (animControl.GetCurrentAnimatorStateInfo(0).IsTag("Movement"))
        {
            typeLocomotion = TLocomotion.Movement;
        }
        else if (animControl.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            typeLocomotion = TLocomotion.Attack;
        } else if (animControl.GetCurrentAnimatorStateInfo(0).IsTag("Dead"))
        {
            typeLocomotion = TLocomotion.Dead;
        }
    }

    public void AnimAttack()
    {
        if (!target || curHealth <= 0) { return; }

        if (Vector3.Distance(target.localTransform.position, localTransform.position) <= 2f)
        {
            target.AddDamage(Random.Range(15, 35));
        }
    }

    #endregion

    #region Health System

    private void HealthSystem()
    {

    }

    public void AddDamage(int damage)
    {
        if (curHealth == 0) { return; }

        curHealth -= damage;

        if(curHealth <= 0)
        {
            curHealth = 0;
            Death();
        }
    }

    private void Death()
    {
        animControl.SetTrigger("Death");
    }

    #endregion
}
