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
     Vector3 guardposset;
    Quaternion guardqua;
    [Header("------- guard settings---------")]
    private Vector3 GuardPos;
    private Quaternion GuardQua;
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
    bool isDead;
     bool isAttack;
    CharacterStates EnemyData;

   public bool IsGuard;

      void Awake()
    {
        guardposset = transform.position;
        //TODO:这么写有什么问题吗？
        //guardqua =Quaternion.Euler(transform.eulerAngles);
        guardqua = transform.rotation;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        basicPoint = transform.position;
        speed = agent.speed;
        EnemyData = GetComponent<CharacterStates>();
        remainLookAtTime = lookAtTime;
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
    
    private void Update()
    {

        if (EnemyData.currentHealth == 0)
        {
            isDead = true;
         }

        SwitchStates();
        SwitchAnimations();
         
    }

    void SwitchAnimations()
    { 
        
        anim.SetBool("Walk", IsWalk);
        anim.SetBool("Chase", IsChase);
        anim.SetBool("Follow", IsFollow);
        anim.SetBool("Critical", EnemyData.IsCritical);
         anim.SetBool("Dead",isDead);
    }

    void SwitchStates()
    {
         if(isDead == true)
        {
             
            estate = EnemyStates.DEAD;
        }
       else if (FoundPlayer())
        {
            estate = EnemyStates.CHASE;
        }
        switch (estate)
        {
            case EnemyStates.GUARD:
                anim.SetLayerWeight(0, 1);
                IsChase = false;
                IsWalk = false;
                //agent.isStopped = false;
               // GuardPos = guardposset;
                //GuardQua = guardqua;
                if(transform.position != guardposset)
                {
                    IsWalk = true;
                    
                    agent.destination = guardposset;
                    if(Vector3.Distance(transform.position, guardposset) <=agent.stoppingDistance)
                    {
                        IsFollow = false;
                       // agent.isStopped = true;
                       IsWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation,guardqua,0.01f);
                    }
                }

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
                    else if (IsGuard == true)
                    { estate = EnemyStates.GUARD; }
                    else
                    { estate = EnemyStates.PATROL; }
                }
                else
                {

                    IsFollow = true;
                    agent.isStopped = false;
                    agent.destination = AttackTarget.transform.position;

                    //TODO:check the effection of these two lines
                    //IsFollow = false;
                    //agent.destination = transform.position;
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
                anim.SetLayerWeight(2, 1);
                agent.enabled = false;

                Destroy(gameObject, 2f);
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
        if (Vector3.Distance(transform.position, AttackTarget.transform.position) < EnemyData.attackData.AttackRange)
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

    void Hit()
    {
        if(AttackTarget != null)
        {
            var playerstates = AttackTarget.gameObject.GetComponent<CharacterStates>();
            EnemyData.TakeDemage(EnemyData, playerstates);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SightRaius);
    }
}
