using UnityEngine;

public class LifePickup : Pickup
{
    public override float SpawnChance => 0.1f;
    protected override void Collect(GameObject player)
    {
        player.GetComponent<Mouse>().currentLives++;
        player.GetComponent<Mouse>().UpdateUI();
        Destroy(gameObject);
    }
}
