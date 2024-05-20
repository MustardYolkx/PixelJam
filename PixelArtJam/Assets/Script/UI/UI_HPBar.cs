using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : MonoBehaviour
{

    public PlayerScr player;
    public Image bar;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScr>();
        bar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = player.currentHP / player.maxHP;
    }
}
