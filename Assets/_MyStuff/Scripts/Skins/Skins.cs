using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Skins : MonoBehaviour
{
    public SkinDatabase skinsDB;
    [SerializeField] private GameObject prefab;
    float pos = 0f;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Price;
    public GameObject Button;
    private List<GameObject> skinInstances = new();
    public int currentIndex { get; private set; }

    const string prefix = "Skin_";
    private void Start()
    {
        currentIndex = PlayerPrefs.GetInt("skinindex", 0);


        int counter=0;
        foreach (Skin skin in skinsDB.skins)
        {
            GameObject Skin = Instantiate(prefab, new Vector3(pos, -2f, 0f), Quaternion.identity);
            Skin.GetComponent<SpriteRenderer>().sprite = skin.sprite;
            Debug.Log(counter + ": " + PlayerPrefs.GetInt(prefix + counter, 0));
            if(PlayerPrefs.GetInt(prefix + counter, 0) == 1)
            {
                skin.unlocked = true;
            }
            if(!skin.unlocked)
            {
                Skin.GetComponent<SpriteRenderer>().color = Color.gray3;
            }
            counter++;
            pos += 1.5f;
            skinInstances.Add(Skin);
        }

        UpdateSkinPositions();
    }

    private void UpdateSkinPositions()
    {
        Name.text = skinsDB.skins[currentIndex].name;

        
        Price.text = skinsDB.skins[currentIndex].price.ToString();

        Button.SetActive(!skinsDB.skins[currentIndex].unlocked);
        for (int i = 0; i < skinInstances.Count; i++)
        {
            
            GameObject skin = skinInstances[i];
            int offset = GetOffset(i, currentIndex);
            SkinMover mover = skin.GetComponent<SkinMover>();

            float spacing = 2f;
            float scale = (offset == 0) ? 2f : 1f;
            float xPos = offset * spacing;

            mover.targetPosition = new Vector3(xPos, -1.5f, 0f);
            mover.targetScale = Vector3.one * scale;

            if (Mathf.Abs(offset) > 3f)
            {
                skin.GetComponent<SpriteRenderer>().enabled = false;
                continue;
            }
            skin.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    private int GetOffset(int index, int center)
    {
        int offset = index - center;
        int half = skinsDB.skins.Length / 2;

        // Loop around
        if (offset > half) offset -= skinsDB.skins.Length;
        if (offset < -half) offset += skinsDB.skins.Length;

        return offset;
    }

    private Vector2 touchStart;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            touchStart = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 touchEnd = Input.mousePosition;
            float delta = touchEnd.x - touchStart.x;

            if (Mathf.Abs(delta) > 50f)
            {
                if (delta > 0)
                    currentIndex = (currentIndex - 1 + skinsDB.skins.Length) % skinsDB.skins.Length;
                else
                    currentIndex = (currentIndex + 1) % skinsDB.skins.Length;

                UpdateSkinPositions();
            }
        }
    }
    public void Unlock()
    {
        skinInstances[currentIndex].GetComponent<SpriteRenderer>().color = Color.white;
        Button.SetActive(false);
        PlayerPrefs.SetInt(prefix + currentIndex, 1);
    }

}
