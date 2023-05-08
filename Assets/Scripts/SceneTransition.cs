using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;

    //[SerializeField] private GameObject FOS;
    [SerializeField] private Animator fadeAnim;
    private static Vector3 playerPos;
    public static string previousScene;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static void ChangeScene(string scene)
    {
        //if the scene is not combat, then update the player information and go back to that position if the current scene is combat
        if (SceneManager.GetActiveScene().name != "Combat")
        {
            playerPos = GameObject.Find("Player").transform.position;
            Debug.Log(playerPos);
            instance.LoadScene(scene);
            previousScene = SceneManager.GetActiveScene().name;
        }
        else if (SceneManager.GetActiveScene().name == "Combat")
        {
            
            instance.LoadScene(scene);
        }



        //later on we should keep track of the place to teleport the player back to before they after returning to a scene theyve been in before
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
        //GameObject fos = Instantiate(FOS, playerPos, Quaternion.identity);//This used to be referencing the overworld script 
        //fos.GetComponent<FadeOutSquare>().StartFadeIn();

        fadeAnim.SetTrigger("FadeIn");

        StartCoroutine(LoadSceneRoutine(scene));
    }

    private void LoadScene(string scene, Vector3 position)
    {
        //make the scene fadeout
        //- create an object that becomes completely black over two seconds and covers the screen
        //- change scene after two seconds 
        Debug.Log(position + " : Done");
        //GameObject fos = Instantiate(FOS, position, Quaternion.identity);
        //fos.GetComponent<FadeOutSquare>().StartFadeIn();

        fadeAnim.SetTrigger("FadeIn");

        StartCoroutine(LoadSceneRoutine(scene, position));
    }

    private IEnumerator LoadSceneRoutine(string scene)
    {
        Debug.Log("Changing scene now");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
    private IEnumerator LoadSceneRoutine(string scene, Vector3 position)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
        //yield return new WaitForSeconds(0.05f);
        //GameObject fos = Instantiate(FOS, position, Quaternion.identity);
        //fos.GetComponent<FadeOutSquare>().StartFadeOut();
        fadeAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.1f);
        if (GameObject.Find("Player") != null)
        {
            GameObject.Find("Player").GetComponent<Transform>().position = position;
            OverworldMovement.canMove = true;
        }
        else
        {
            Debug.Log("ERROR : PLAYER NOT FOUND ");
        }
    }
    public void onDeath()
    {
        StartCoroutine(onDeathRoutine());
    }

    private IEnumerator onDeathRoutine()
    {
        SoundManager.PlayMusic("Game Over");

        yield return new WaitForSeconds(0.5f);
        PlayerData.Health = PlayerData.MaxHealth;
        SceneManager.LoadScene("DeathScene");

    }


    public void onGameEndNoKill()
    {
        instance.StartCoroutine(LoadSceneRoutine("WinSceneJoAlive"));
    }
    public void onGameEndKill()
    {
        instance.StartCoroutine(LoadSceneRoutine("WinSceneJoDead"));
    }
}
