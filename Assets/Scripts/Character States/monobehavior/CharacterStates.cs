using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    [HideInInspector]
    public bool IsCritical;
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

    public float currentDefence
    {
        get
        {
            if (characterData != null)
                return characterData.CurrentDefence;
            else
                return 0f;
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



}
