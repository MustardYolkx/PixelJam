using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTriggerEvent : MonoBehaviour
{

    PlayerScr player;
    // Start is called before the first frame update
    void Start()
    {
        player= GetComponentInParent<PlayerScr>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnWaterCapSprite()
    {
        player.SetWaterCapSpriteTrue();
    }

    public void TurnOffWaterCapSprite()
    {
        player.SetWaterCapSpriteFalse();
    }

    public void TurnOnLargeWaterCapSprite()
    {
        player.SetWaterLargeCapSpriteTrue();
    }

    public void TurnOffLargeWaterCapSprite()
    {
        player.SetWaterLargeCapSpriteFalse();
    }
}
