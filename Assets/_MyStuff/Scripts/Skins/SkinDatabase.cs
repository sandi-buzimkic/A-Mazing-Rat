using UnityEngine;


[CreateAssetMenu]
public class SkinDatabase : ScriptableObject
{
    public Skin[] skins;

    public Skin getSkin(int index)
    {
        return skins[index];
    }
}
