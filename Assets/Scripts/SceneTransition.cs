using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;

    [SerializeField] private GameObject FOS;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        if (instance != null)
            Destroy(instance);
        
        instance = this;
        DontDestroyOnLoad(this);
    }

    public static void ChangeScene(string scene)
    {
        instance.LoadScene(scene);
    }

    public static void ChangeScene(string scene, Vector3 position)
    {
        instance.LoadScene(scene, position);
    }

    private void LoadScene(string scene)
    {
        //make the scene fadeout
        //- create an object that becomes completely black over two seconds and covers the screen
        //- change scene after two seconds 
        GameObject fos = Instantiate(FOS, OverworldMovement.PlayerPosition, Quaternion.identity);
        fos.GetComponent<FadeOutSquare>().StartFadeIn();
        StartCoroutine(LoadSceneRoutine(scene));
    }

    private void LoadScene(string scene, Vector3 position)
    {
        //make the scene fadeout
        //- create an object that becomes completely black over two seconds and covers the screen
        //- change scene after two seconds 
        GameObject fos = Instantiate(FOS, position, Quaternion.identity);
        fos.GetComponent<FadeOutSquare>().StartFadeIn();
        StartCoroutine(LoadSceneRoutine(scene));
    }

    private IEnumerator LoadSceneRoutine(string scene)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scene);
    }
}
