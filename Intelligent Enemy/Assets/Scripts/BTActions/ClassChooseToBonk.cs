using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Pada1.BBCore;
using Pada1.BBCore.Actions;
using BBUnity.Actions;

[Action("CustomActions/ClassChooseToBonk")]
public class ClassChooseToBonk : GOAction
{
    public override void OnStart()
    {
        gameObject.GetComponent<AgentAttacks>().attackQueue.Add(AgentAttacks.Attacks.bonk);
    }
}