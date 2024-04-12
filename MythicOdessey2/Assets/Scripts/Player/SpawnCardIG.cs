using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCardIG : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.instance.AddAction("OnCardTrigger", (objects => { BuildObject((CardsTypeSO)objects[0]); }));
    }

    private void BuildObject(CardsTypeSO prefab)
    {
        if(!PlayerManager.Instance.CheckMana(prefab.manaCost))return;
        var built = Instantiate(prefab.prefab, transform.position + Vector3.forward*5, Quaternion.identity);
        if (built is ObjectiveTrap objectiveTrap)
        {
            objectiveTrap.objective = PlayerManager.Instance.NearEnemy.transform;
        }
        EventManager.instance.TriggerEvent("OnCardBuilt", prefab.manaCost);
    }
}
