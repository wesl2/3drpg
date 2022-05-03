 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates {GUARD,PATROL,CHASE,DEAD, }
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("-------- basic settings--------")]
    public float SightRaius = 2.5f;
    public float speed;
    NavMeshAgent agent;
    public EnemyStates estate;
    public float lookAtTime;
    private float remainLookAtTime;

    [Header("------- patrol settings ---------")]
    public float patrolRange;
    private Vector3 patrolPoint;
    private Vector3 basicPoint;
    private GameObject AttackTarget; 
    private Animator anim;
    float CD;
    bool IsWalk;
    bool IsChase;
    bool IsFollow;

    bool isAttack;
    CharacterStates EnemyData;

   public bool IsGuard;

      void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        basicPoint = transform.position;
        speed = agent.speed;
        EnemyData = GetComponent<CharacterStates>();
        remainLookAtTime = lookAtTime;
    }
    
    private void Update()
    {
        SwitchStates();
        SwitchAnimations();
         
    }
    private void Start()
    {
        CD = EnemyData.coolDown;
        StartCoroutine(Attack_CD());
        if(IsGuard == true)
        {
            estate = EnemyStates.GUARD;
        }
        else
        {
            estate = EnemyStates.PATROL;
            GetNewPatrolPoint();
        }
     }
    void SwitchAnimations()
    { 
        anim.SetBool("Walk", IsWalk);
        anim.SetBool("Chase", IsChase);
        anim.SetBool("Follow", IsFollow);
        anim.SetBool("Critical", EnemyData.IsCritical);
    }

    void SwitchStates()
    {
        if (FoundPlayer())
        {
            estate = EnemyStates.CHASE;
        }
        switch (estate)
        {
            case EnemyStates.GUARD:
               
                break;
            case EnemyStates.PATROL:
                anim.SetLayerWeight(0, 1);
                IsChase = false;
                agent.speed = speed * 0.5f;
                //是否到达史莱姆随机选取的那个点
                if(Vector3.Distance(patrolPoint,transform.position)<= agent.stoppingDistance)
                {
                    IsWalk = false;
                    if (remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    else
                    GetNewPatrolPoint();
                }
                else
                {
                    IsWalk = true;
                    remainLookAtTime = lookAtTime;
                    agent.destination = patrolPoint;
                }
                break;
            case EnemyStates.CHASE:
                anim.SetLayerWeight(1, 1);
                IsWalk = false;
                IsChase = true;
                if (!FoundPlayer())
                {
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else
                    {
                        if (IsGuard == true)
                        { estate = EnemyStates.GUARD; }
                        else
                        { estate = EnemyStates.PATROL; }
                        IsFollow = false;
                        agent.destination = transform.position;

                    }
                }
                else//find player
                {
                    IsFollow = true;
                    agent.isStopped = false;
                    agent.destination = AttackTarget.transform.position;
                    if (TargetInAttactRange() || TargetInSkillRange())
                    {
                         IsFollow = false;
                        agent.destination = transform.position;
                         if(isAttack == true)
                        { 
                            Attack();
                            StartCoroutine(Attack_CD());
                            isAttack = false;
                        }
                    }
                }
                break;
            case EnemyStates.DEAD:

                break;
        }
    }
    
    void Attack()
    {
        
        anim.SetLayerWeight(1, 1);
        transform.LookAt(AttackTarget.transform.position);
        EnemyData.IsCritical = (Random.value) <= EnemyData.criticalChance;

        if (TargetInAttactRange())
        {
            anim.SetTrigger("Attack");
        }
        if(TargetInSkillRange())
        {
            anim.SetTrigger("Skill");
        }
    }
    IEnumerator Attack_CD()
    {
        for(;CD>0f;CD-=Time.deltaTime)
        {
            yield return null;
        }
        if (CD <= 0)
        {
            isAttack = true;
            CD = EnemyData.coolDown;
        }
    }
    
    bool FoundPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, SightRaius);

        foreach (var collider in colliders)
        {
            if (collider.transform.gameObject.tag == "Player")
            {
                AttackTarget = collider.gameObject;
                return true;
            }
        }
        AttackTarget = null;
        return false;
    }
    void GetNewPatrolPoint()
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 RandomPoint = new Vector3(basicPoint.x+randomX,transform.position.y,basicPoint.z + randomZ);

        NavMeshHit meshHit;
        if (NavMesh.SamplePosition(RandomPoint, out meshHit, patrolRange, 1))
        {
            patrolPoint = meshHit.position;
        }
        else
            patrolPoint = transform.position;
    }

    bool TargetInAttactRange()
    {
        if (Vector3.Distance(transform.position, AttackTarget.transform.position) < EnemyData.attackRange)
        {
            return true;
        }
        else
            return false;
    }

    bool TargetInSkillRange()
    {
        if (Vector3.Distance(transform.position, AttackTarget.transform.position) < EnemyData.skillRange)
        {
            return true;
        }
        else
            return false;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SightRaius);
    }
}
