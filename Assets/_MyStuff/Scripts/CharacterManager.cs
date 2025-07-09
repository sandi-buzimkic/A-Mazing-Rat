using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    public SkinDatabase skinsDB;
    public Sprite RatSprite;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
        RatSprite = skinsDB.skins[PlayerPrefs.GetInt("skinindex", 0)].sprite;
    }
}
