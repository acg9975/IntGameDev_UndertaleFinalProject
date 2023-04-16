using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private string scene;
    [SerializeField]
    private GameObject FOS;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            //make the scene fadeout
            //- create an object that becomes completely black over two seconds and covers the screen
            //- change scene after two seconds 
            GameObject fos = Instantiate(FOS, other.transform);
            fos.GetComponent<FadeOutSquare>().StartFadeIn();
            StartCoroutine(changeScene());
        }

    }

    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scene);
    }



}
