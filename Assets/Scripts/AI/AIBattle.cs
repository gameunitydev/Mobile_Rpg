using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TDC;

public class AIBattle : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public Locomotion locomotion;
    public CoreTrigger viewTrigger;

    public EnemyUI enemyUI;

    public bool isBattle;

    #region Unity

    private void Start()
    {
        Initialization();
    }

    private void Update()
    {
        CoreUpdate();
    }

    #endregion

    #region Core

    private void Initialization()
    {
        locomotion.Initialization();
        locomotion.animControl.SetBool("Enemy", true);
        SetRagdoll(false);
        enemyUI = GetComponent<EnemyUI>();
    }

    private void CoreUpdate()
    {
        ViewControl();
        locomotion.CoreUpdate();

        if (target)
        {
            enemyUI.SetHealthBarStatus(true);
            agent.SetDestination(target.position);
            agent.isStopped = false;
            isBattle = true;
        }
        else
        {
            enemyUI.SetHealthBarStatus(false);
            agent.isStopped = true;
            isBattle = false;
        }

        Locomotion();
    }

    private void Locomotion()
    {
        if (!target)
        {
            locomotion.Movement(Vector3.zero);
            return;
        }

        Vector3 fixDirection = Vector3.zero;

        if (Vector3.Distance(target.position, transform.position) >= agent.stoppingDistance)
        {
            fixDirection = (agent.steeringTarget - transform.position).normalized;
            locomotion.Rotate(fixDirection);
            locomotion.target = null;
        }
        else if (target.GetComponent<Health>().currentHealth > 0)
        {
            locomotion.target = target.GetComponent<Locomotion>();
            locomotion.AttackControl();
        }

        locomotion.Movement(fixDirection);
    }

    private void ViewControl()
    {
        foreach (Transform tar in viewTrigger.listObject)
        {
            if (tar)
            {
                target = tar;
                locomotion.target = target.GetComponent<Locomotion>();
                return;
            }
        }

        locomotion.target = null;
        target = null;
    }

    private void SetKinematic(bool newValue)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = newValue;
        }
    }

    public void SetRagdoll(bool value)
    {
        SetKinematic(!value);
        GetComponent<Animator>().enabled = !value;
    }

    #endregion
}