using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{
    [SerializeField] private float powerPerQuestion = 5;
     public float maxPower = 20;
    [HideInInspector] public float currPower;
    [SerializeField] private Image img;
    [SerializeField] private Sprite ThumbsUp;
    [SerializeField] private Sprite ThumbsDown;

    PlayerChangePower UI;

    private void Start()
    {
        currPower = 0;
        UI = GetComponent<PlayerChangePower>();
    }

    public void resetPower()
    {
        currPower = 0;

        if (UI == null)
            return;
        UI.changeplayerPower(currPower / maxPower);
    }

    public void addPower()
    {
        currPower += powerPerQuestion;
        if (UI == null)
            return;

        img.sprite = ThumbsUp;
        img.enabled = true;
        StartCoroutine(hideImg());

        UI.changeplayerPower(currPower/ maxPower);
    }

    public void noAddPower()
    {
        img.sprite = ThumbsDown;
        img.enabled = true;
        StartCoroutine(hideImg());
    }


    private IEnumerator hideImg()
    {
        yield return new WaitForSeconds(2);
        img.enabled = false;
    }
}
