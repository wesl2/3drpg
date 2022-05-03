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
        Anim.SetFloat("Speed", NMA.velocity.sqrMagnitude);//navmesh�Դ���
                                                          //������velocity
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

        if(target !=null)
        {
          AttackTarget = target;
          StartCoroutine(MoveToEnemy());
        }
    }
    //ʲôʱ����Э�̣� 1.��Ҫ��ʱֹͣ���涨ֹͣ��ʱ��ʱ�������ƶ���ĳ��
    //2.update����if̫�����if����߼�̫����
    //3.�ӳ�
    //��ʵֱ����updateҲһ��

    IEnumerator MoveToEnemy()
    {
        NMA.isStopped = false;
        //update����ֱ��дѭ������
        
        //TODO:����Ҫ��������̶�ֵ��
        while (Vector3.Distance(transform.position, AttackTarget.transform.position) > characterstates.attackRange)
        {
            transform.LookAt(AttackTarget.transform);
            NMA.destination = AttackTarget.transform.position;
            yield return null;
            //��һ��д����
            //yield return (Vector3.Distance(transform.position, //AttackTarget.transform.position) > 1.2f);
            //yield return : ����yield�����ʱ���𣬵ȵ�yield return�����������ż���ִ��yield����������ݡ�
            //��ֱ��return nullʱ������д��ѭ���
            //��ʱreturn�˾Ͳ�ִ�к�����ˣ�����ѭ��(����)��ɡ�
        }
        NMA.isStopped = true;

        if (lastAttackTime < 0f)
        {
            Anim.SetTrigger("Attack");
            lastAttackTime = 0.5f;
        }
    }
}
