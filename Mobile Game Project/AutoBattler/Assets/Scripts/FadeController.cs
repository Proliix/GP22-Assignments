using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    //const int FADE_IN_AMOUNT = 3;
    //const int FADE_OUT_AMOUNT = 3;
    const int FADE_AMOUNT = 3;

    private static FadeController _instance;
    public static FadeController Instance { get { return _instance; } }

    Animator anim;
    GameController gameController;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            anim = GetComponent<Animator>();
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void FinishedAnim()
    {
        gameController.IntermissionComplete();
    }

    public void FadeOut()
    {
        //int r = Random.Range(0, FADE_OUT_AMOUNT);
        //anim.SetInteger("Index", r);
        index = Random.Range(0, FADE_AMOUNT);
        anim.SetInteger("Index", index);
        anim.SetTrigger("FadeOut");
    }

    public void FadeIn()
    {
        //int r = Random.Range(0, FADE_IN_AMOUNT);
        // anim.SetInteger("Index", r);
        anim.SetInteger("Index", index);
        anim.SetTrigger("FadeIn");
    }

}
