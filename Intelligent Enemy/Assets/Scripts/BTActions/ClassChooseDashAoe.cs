using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Actions;
using BBUnity.Actions;

[Action("CustomActions/ClassChooseDashAoe")]
public class ClassChooseDashAoe : GOAction
{
    public override void OnStart()
    {
        AgentAttacks attack = gameObject.GetComponent<AgentAttacks>();
        attack.attackQueue.Add(AgentAttacks.Attacks.dash);
        attack.attackQueue.Add(AgentAttacks.Attacks.aoe);
    }
}