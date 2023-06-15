using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;



    public UI_Main ui;
    public Player player;
    public bool colorEntierPlatform;

    [Header("Skybox materials")]
    [SerializeField] private Material[] skyBoxMat;

    [Header("Purchased color")]
    public Color platformColor;


    [Header("Score info")]
    public int coins;
    public float distance;
    public float score;

    bool isEnded = false;
    private SaveWrapData saveData;
    public SaveWrapData SaveData { get => saveData; }
    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;

        SetupSkyBox(PlayerPrefs.GetInt("SkyBoxSetting"));
        LoadSaveData();
        //LoadColor();
    }

    private void LoadSaveData()
    {
        string backupFolder = "DataResources";
        SaveWrapData saveInfo = SaveAndLoad<SaveWrapData>.Load(backupFolder, "ScoreInfoData");
        if (saveInfo == null)
            saveData = new SaveWrapData();
        else
            saveData = saveInfo;
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        ui.Init();
    }

    public void SetupSkyBox(int i)
    {
        if(i <= 1)
            RenderSettings.skybox = skyBoxMat[i];
        else 
            RenderSettings.skybox = skyBoxMat[Random.Range(0,skyBoxMat.Length)];

        PlayerPrefs.SetInt("SkyBoxSetting", i);
    }

    public void SaveColor(float r, float g, float b)
    {
        PlayerPrefs.SetFloat("ColorR", r);
        PlayerPrefs.SetFloat("ColorG", g);
        PlayerPrefs.SetFloat("ColorB", b);
    }
    private void ClearData()
    {
        PlayerPrefs.DeleteAll();
        ScoreInfo newInfo = new ScoreInfo(0,0,0,0);
        saveData = new SaveWrapData();
        saveData.info = newInfo;
        coins = 0;
        SaveInfo();
    }
    private void LoadColor()
    {
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();

        Color newColor = new Color(PlayerPrefs.GetFloat("ColorR"),
                                   PlayerPrefs.GetFloat("ColorG"),
                                   PlayerPrefs.GetFloat("ColorB"),
                                   PlayerPrefs.GetFloat("ColorA", 1));

        sr.color = newColor;
    }

    private void Update()
    {
        if (player.transform.position.x > distance)
            distance = player.transform.position.x;

        if (Input.GetKeyDown(KeyCode.D))
        {
            ClearData();
            ui.Init();
        }
    }
    public void UnlockPlayer() => player.playerUnlocked = true;
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveInfo()
    {
        string backupFolder = "DataResources";
        int savedCoins = PlayerPrefs.GetInt("Coins", 0);

        PlayerPrefs.SetInt("Coins", savedCoins + coins);

        score = distance * coins;

        PlayerPrefs.SetFloat("LastDistance", distance);

        PlayerPrefs.SetFloat("LastScore", score);

        if (PlayerPrefs.GetFloat("HighScore") < score)
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }
        //Save Data Info
        saveData.info.lastDistance = distance;
        saveData.info.coins = PlayerPrefs.GetInt("Coins");
        saveData.info.highScore = PlayerPrefs.GetFloat("HighScore");
        saveData.info.lastScore = PlayerPrefs.GetFloat("LastScore");
        Debug.Log("GAMEMANAGER: save info "+ saveData.info.ToString());
        SaveAndLoad<SaveWrapData>.Save(saveData, backupFolder, "ScoreInfoData");
    }

    public void GameEnded()
    {
        if (isEnded) return;
        SaveInfo();
        ui.OpenEndGameUI();
        isEnded = true;
    }


}
[System.Serializable]
public class SaveWrapData
{
    public ScoreInfo info;
    public SaveWrapData() { info = new ScoreInfo(); }
}
[System.Serializable]
public class ScoreInfo
{
    public int coins = 0;
    public float highScore = 0;
    public float lastScore = 0;
    public float lastDistance = 0;
    public ScoreInfo()
    {
        coins = 0;
        highScore = 0;
        lastScore = 0;
        lastDistance = 0;
    }
    public ScoreInfo (int coin, float hScore, float lScore, float lDis)
    {
        coins = coin;
        highScore = hScore;
        lastScore = lScore;
        lastDistance = lDis;
    }
    public override string ToString()
    {
        return "coins " + coins + " high score " + highScore + " last score " + lastScore + " last distance " + lastDistance ;
    }
}
