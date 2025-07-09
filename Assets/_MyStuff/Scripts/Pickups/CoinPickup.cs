using UnityEngine;

public class CoinPickup : Pickup
{
    public override float SpawnChance => 0.5f;
    protected override void Collect(GameObject player)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0)+1);
        GameFlowController.Instance.ScoreManager.UpdateCoinsUI();
        Destroy(gameObject);
    }
}
