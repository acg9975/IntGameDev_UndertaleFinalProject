using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutSquare : MonoBehaviour
{
    Animator anim;

    [SerializeField][Tooltip("If this is checked, it will automatically play the fade in animation instead of waiting to get triggered")]
    bool createdOnSceneStart = false;
    // Start is called before the first frame update
    void Awake()
    {
        
        anim = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().rendererPriority = 200;
        Destroy(gameObject, 2f);
        if (createdOnSceneStart)
        {
            StartFadeIn();
        }

    }
    
    public void StartFadeOut()
    {
        Debug.Log("fadeout");
        anim.SetTrigger("FadeOut");
    }

    public void StartFadeIn()
    {
        Debug.Log("fadein");

        anim.SetTrigger("FadeIn");
    }
}
