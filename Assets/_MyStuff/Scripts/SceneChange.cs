using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Skins skinManager;
    public Animator trigger;
    public void GoToShop()
    {
        StartCoroutine(Transition("Shop"));
    }
    public void GoToMaze()
    {
        PlayerPrefs.SetInt("skinindex", skinManager.currentIndex);
        CharacterManager.Instance.RatSprite = skinManager.skinsDB.skins[skinManager.currentIndex].sprite;
        StartCoroutine(Transition("Maze"));
    }
    public IEnumerator Transition(string scene)
    {
        trigger.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }

}
