using UnityEngine;
using System.Runtime.InteropServices;

public class Yandexs : MonoBehaviour
{
    public static Yandexs Instance;
    public PlayerDATAs Data = new PlayerDATAs();
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            transform.parent = null;
            Instance = this;
            LoadExtern();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [DllImport("__Internal")]
    private static extern void SaveExtern (string data);
    [DllImport("__Internal")]
    public static extern void LoadExtern ();

    public void SaveData ()
    {
        string jsonFile = JsonUtility.ToJson (Data);
        SaveExtern (jsonFile);
    }
    private void LoadDataFromServer(string value)
    {
        Debug.Log("LOADED DATA");
        Data = JsonUtility.FromJson<PlayerDATAs>(value);
    }
}
[System.Serializable]
public class PlayerDATAs
{
    public int Level;
    public float MusicVolume;
    public float SoundVolume;
}