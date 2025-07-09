using System.Collections;
using UnityEngine;

public class TimeSlowPickup : Pickup
{
    public override float SpawnChance => 0.3f;
    protected override void Collect(GameObject player)
    {
        GameFlowController.Instance.StartTimeSlow();
        Destroy(gameObject);
    }
}
