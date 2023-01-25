using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class Main : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdv(); 

    public Player player;
    public Image[] hearts;
    public Sprite isLife, nonLife;
    public GameObject PauseScreen;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    float timer = 0f;
    public Text timeText;
    public TimeWork timeWork;
    public float countdown;
    public GameObject heart, timers, pause; 

    private void Start()
    {
        ShowAdv();

        if ((int)timeWork == 2)
            timer = countdown;
    }

    public void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (player.GetHp() > i)
                hearts[i].sprite = isLife;
            else
                hearts[i].sprite = nonLife;
        }
        if ((int)timeWork == 1)
        {
            timer += Time.deltaTime;
            timeText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - ((int)timer / 60) * 60).ToString("D2");
        }
     else 
            timeText.gameObject.SetActive(false);
}

    public void ReloadLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseOn()
    {
        heart.SetActive(false);
        timers.SetActive(false);
        pause.SetActive(false);
        Time.timeScale = 0f;
        player.enabled = false;
        PauseScreen.SetActive(true);
    }

    public void PauseOff()
    {
        heart.SetActive(true);
        timers.SetActive(true);
        pause.SetActive(true);
        Time.timeScale = 1f;
        player.enabled = true;
        PauseScreen.SetActive(false);
    }

    public void Win()
    {
        heart.SetActive(false);
        timers.SetActive(false);
        pause.SetActive(false);
        SoundEffector.Instance.PlaywinSound();
        Time.timeScale = 0f;
        player.enabled = false;
        WinScreen.SetActive(true);
        if (Yandexs.Instance.Data.Level < SceneManager.GetActiveScene().buildIndex)
            Yandexs.Instance.Data.Level = SceneManager.GetActiveScene().buildIndex;
        Yandexs.Instance.SaveData();
    }

    public void Lose()
    {
        heart.SetActive(false);
        timers.SetActive(false);
        pause.SetActive(false);
        SoundEffector.Instance.PlayloseSound();
        Time.timeScale = 0f;
        player.enabled = false;
        LoseScreen.SetActive(true);
        Yandexs.Instance.SaveData();
    }

    public void MenuLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        Yandexs.Instance.SaveData();
        SceneManager.LoadScene("Menu");
    }

    public void NextLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        Yandexs.Instance.SaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
}
public enum TimeWork
{
    None,
    StopWatch
}
