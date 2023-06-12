using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    private bool gamePaused;
    private bool gameMuted;


    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject endGame;

    [Space]
    [SerializeField] private TextMeshProUGUI lastScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [Header("Volume info")]
    [SerializeField] private UI_VolumeSlider[] slider;
    [SerializeField] private Image muteIcon;
    [SerializeField] private Image inGameMuteIcon;

    private void Start()
    {
        for (int i = 0; i < slider.Length; i++)
        {
            slider[i].SetupSlider();
        }

        SwitchMenuTo(mainMenu);

        lastScoreText.text = "Last score:  " + PlayerPrefs.GetFloat("LastScore").ToString("#,#");
        highScoreText.text = "High score:  " + PlayerPrefs.GetFloat("HighScore").ToString("#,#");
    }


    public void SwitchMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);

        AudioManager.instance.PlaySFX(4);
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
    }

    public void SwitchSkyBox(int index)
    {
        AudioManager.instance.PlaySFX(4);
        GameManager.instance.SetupSkyBox(index);
    }

    public void MuteButton()
    {
        gameMuted = !gameMuted; // works like a switcher

        if (gameMuted)
        {
            muteIcon.color = new Color(1, 1, 1, .5f);
            AudioListener.volume = 0;
        }
        else
        {
            muteIcon.color = Color.white;
            AudioListener.volume = 1;
        }
    }

    public void StartGameButton()
    {
        muteIcon = inGameMuteIcon;

        if (gameMuted)
            muteIcon.color = new Color(1, 1, 1, .5f);

        GameManager.instance.UnlockPlayer();
    }
    public void PauseGameButton()
    {
        if (gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void RestartGameButton() => GameManager.instance.RestartLevel();

    public void OpenEndGameUI()
    {
        SwitchMenuTo(endGame);
    }
}
