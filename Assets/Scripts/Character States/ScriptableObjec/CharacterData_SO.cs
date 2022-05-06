using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data",menuName ="Character StatesData")]
public class CharacterData_SO : ScriptableObject
{
    public float MaxHealth;
    public float CurrentHealth;
    public float BaseDefence;
    public int CurrentDefence;
}
