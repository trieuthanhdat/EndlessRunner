using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    private Player player;

    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private Image heartEmpty;
    [SerializeField] private Image heartFull;
    [SerializeField] private Image slideIcon;


    private float distance;
    private int coins;

    void Start()
    {
        player = GameManager.instance.player;
        InvokeRepeating("UpdateInfo", 0, .2f);
    }

    private void UpdateInfo()
    {
        slideIcon.enabled = player.slideCooldownCounter < 0;
        distance = GameManager.instance.distance;
        coins = GameManager.instance.coins;

        if(distance > 0)
            distanceText.text = distance.ToString("#,#") + "  m";

        if(coins > 0)
            coinsText.text = GameManager.instance.coins.ToString("#,#");


        heartEmpty.enabled = !player.extraLife;
        heartFull.enabled = player.extraLife;
    }
}
