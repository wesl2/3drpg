using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{           
     NavMeshAgent NMA;
     Animator Anim;
    GameObject AttackTarget;
    CharacterStates characterstates;
    float lastAttackTime = 0.5f;
    bool isDead;
     private void Awake()
    {
        NMA = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        characterstates = GetComponent<CharacterStates>();
    }
    private void Start()
    {
        MouseController.instance.OnMouseClicked += MoveToTarget;
        MouseController.instance.OnEnemyClicked += EventAttack;
    }
    private void Update()
    {
        Anim.SetFloat("Speed", NMA.velocity.sqrMagnitude);//navmesh自带的参数：velocity
    }
    private void FixedUpdate()
    {
        lastAttackTime -= Time.fixedDeltaTime;
    }
    void MoveToTarget(Vector3 Target)
    {
        NMA.isStopped = false;
        //StopAllCoroutines();
        StopCoroutine(MoveToEnemy());
        NMA.destination = Target;
    }
    void EventAttack(GameObject target)
    {
        characterstates.IsCritical = UnityEngine.Random.value <= characterstates.criticalChance; 
        if(target !=null)
        {
          AttackTarget = target;
          StartCoroutine(MoveToEnemy());
        }
    }
    //什么时候用协程？ 1.需要随时停止并规定停止的时间时，比如移动到某处
    //2.update里面if太多或者if里的逻辑太复杂
    //3.延迟
    //其实直接用update也一样

    IEnumerator MoveToEnemy()
    {
        NMA.isStopped = false;
        //update不能直接写循环！！
        
         while (Vector3.Distance(transform.position, AttackTarget.transform.position) > characterstates.attackRange)
        {
            transform.LookAt(AttackTarget.transform);
            NMA.destination = AttackTarget.transform.position;
            yield return null;
            //换一种写法：
            //yield return (Vector3.Distance(transform.position, //AttackTarget.transform.position) > 1.2f);
            //yield return : 遇到yield后会暂时挂起，等到yield return后的条件满足才继续执行yield语句后面的内容。
            //在直接return null时，条件写在循环里。
            //此时return了就不执行后面的了，除非循环(条件)完成。
        }
        NMA.isStopped = true;

        if (lastAttackTime < 0f)
        {
            Anim.SetBool("Critical", characterstates.IsCritical);
            Anim.SetTrigger("Attack");
            lastAttackTime = characterstates.coolDown;//属性直接获取
        }
    }

    void Hit()
    {
        var enemystates = AttackTarget.GetComponent<CharacterStates>();
        characterstates.TakeDemage(characterstates, enemystates);
    }
}
