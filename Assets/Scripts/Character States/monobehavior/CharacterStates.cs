using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    [HideInInspector]
    //是在具体的controller脚本里改变的
    public bool IsCritical;
    //小写的都是属性
    #region 属性
    public float maxHealth
    {
        get
        {
            if (characterData != null)
                return characterData.MaxHealth;
            else
                return 0f;
        }
        set
        {
            characterData.MaxHealth = value;
        }
    }

    public float currentHealth
    {
        get
        {
            if (characterData != null)
                return characterData.CurrentHealth;
            else
                return 0f;
        }
        set
        {
            characterData.CurrentHealth = value;
        }
    }

    public float baseDefence
    {
        get
        {
            if (characterData != null)
                return characterData.BaseDefence;
            else
                return 0f;
        }
        set
        {
            characterData.BaseDefence = value;
        }
    }

    public int currentDefence
    {
        get
        {
            if (characterData != null)
                return characterData.CurrentDefence;
            else
                return 0;
        }
        set
        {
            characterData.CurrentDefence = value;
        }
    }

    public float attackRange
    {
        get
        {
            if (attackData != null)
                return attackData.AttackRange;
            else
                return 0f;
        }
        set
        {
            attackData.AttackRange = value;
        }
    }

    public float skillRange
    {
        get
        {
            if (attackData != null)
                return attackData.SkillRange;
            else
                return 0f;
        }
        set
        {
            attackData.SkillRange = value;
        }
    }
    public float coolDown
    {
        get
        {
            if (attackData != null)
                return attackData.CoolDown;
            else
                return 0f;
        }
        set
        {
            attackData.CoolDown = value;
        }
    }
    public float minDemage
    {
        get
        {
            if (attackData != null)
                return attackData.MinDemage;
            else
                return 0f;
        }
        set
        {
            attackData.MinDemage = value;
        }

    }
    public float maxDemage
    {
        get
        {
            if (attackData != null)
                return attackData.MaxDemage;
            else
                return 0f;
        }
        set
        {
            attackData.MaxDemage = value;
        }
    }
    public float criticalMultipler
    {
        get
        {
            if (attackData != null)
                return attackData.CriticalMultipler;
            else
                return 0f;
        }
        set
        {
            attackData.CriticalMultipler = value;
        }
    }
    public float criticalChance
    {
        get
        {
            if (attackData != null)
                return attackData.CriticalChance;
            else
                return 0f;
        }
        set
        {
            attackData.CriticalChance = value;
        }
    }

    #endregion
   public void TakeDemage(CharacterStates Attacker,CharacterStates Attackee)
    {
        int demage =Mathf.Max(Attacker.currentAttack() - Attackee.currentDefence);
        Attackee.currentHealth = Mathf.Max(Attackee.currentHealth - demage, 0);
        Debug.Log("demage: " + demage);
        Debug.Log("health: " + Attackee.currentHealth);
        //写反了：在这个函数里自己反而是受攻击的一方
        if(Attacker.IsCritical)
        {
            Attackee.gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
            Attackee.gameObject.GetComponent<Animator>().SetTrigger("Hit");
        }
        
        //TODO: update UI
        //TODO: 升级
    }

    int currentAttack()
    {
        float coreDemage = Random.Range(attackData.MinDemage, attackData.MaxDemage);
        if(IsCritical)
        {
            coreDemage *= attackData.CriticalMultipler;
            Debug.Log("Critical! " + coreDemage);
        }
        return (int)coreDemage;
    }
}
