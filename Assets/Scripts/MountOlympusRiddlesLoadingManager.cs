using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MountOlympusRiddlesLoadingManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        GameObject loadCanvas = Resources.Load("Prefabs/Load") as GameObject;
 	    Instantiate(loadCanvas);
        yield return new WaitForSeconds(3);
        Scene nextScene = SceneManager.CreateScene("MountRiddlesMenuScene");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(nextScene);
        GameObject menuCanvas = Resources.Load("Prefabs/Menu") as GameObject;
	    Instantiate(menuCanvas);
        SceneManager.UnloadScene(currentScene);
    }
}