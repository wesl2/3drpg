using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Data", menuName = "Attack Data")]
public class AttackData_SO : ScriptableObject
{
     public float  AttackRange;
     public float  SkillRange;
     public float  CoolDown ;
     public float  MinDemage ;
     public float  MaxDemage ;
     public float  CriticalMultipler ;
     public float  CriticalChance;
}
