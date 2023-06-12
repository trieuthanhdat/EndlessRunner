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


    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;

        SetupSkyBox(PlayerPrefs.GetInt("SkyBoxSetting"));

        //LoadColor();
    }


    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
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
    }
    public void UnlockPlayer() => player.playerUnlocked = true;
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveInfo()
    {
        ScoreInfo scoreInfo = new ScoreInfo();
        string backupFolder = "DataResources";
        string backupTextAsset = SaveAndLoad<string>.Load(backupFolder, "ScoreInfo");

        int savedCoins = PlayerPrefs.GetInt("Coins", 0);

        PlayerPrefs.SetInt("Coins", savedCoins + coins);

        score = distance * coins;

        PlayerPrefs.SetFloat("LastScore", score);

        if (PlayerPrefs.GetFloat("HighScore") < score)
            PlayerPrefs.SetFloat("HighScore", score);
    }

    public void GameEnded()
    {
        SaveInfo();
        ui.OpenEndGameUI();
    }


}
[System.Serializable]
public class ScoreInfo
{
    public int coins = 0 ;
    public float distance = 0;
    public float score = 0;
}
