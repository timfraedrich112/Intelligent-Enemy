using BBUnity.Conditions;
using Pada1.BBCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Condition("CustomCondition/ClassTakeMostlyRange")]
public class ClassTakeMostlyRange : GOCondition
{
    public override bool Check()
    {
        AgentMemory memory = gameObject.GetComponent<AgentMemory>();
        return memory.meleeDmgTaken < memory.rangeDmgTaken;
    }
}