using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    public void PlayContinue()
    {
        SceneManager.LoadScene("TestScene_Seo");
    }
}
