using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroEnd : MonoBehaviour
{
    public void EndIntro()
    {
        SceneManager.LoadScene(2);
    }
}
