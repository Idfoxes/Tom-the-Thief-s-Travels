using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button[] lvls;
    void Start()
    {
        for (int i = 0; i < lvls.Length; i++)
        {
            if (i <= Yandexs.Instance.Data.Level)
                lvls[i].interactable = true;
             else  
                lvls[i].interactable = false;
        }
    }
    public void OpenScane(int index)
    {
        SceneManager.LoadScene(index);
    }
}
