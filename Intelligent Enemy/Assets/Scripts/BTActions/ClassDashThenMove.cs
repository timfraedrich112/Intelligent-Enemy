using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Actions;
using BBUnity.Actions;

[Action("CustomActions/ClassDashThenMove")]
public class ClassDashThenMove : GOAction
{
    public override void OnStart()
    {
        AgentAttacks attack = gameObject.GetComponent<AgentAttacks>();
        attack.attackQueue.Add(AgentAttacks.Attacks.dash);
        attack.GoToTargetLocation();
    }
}