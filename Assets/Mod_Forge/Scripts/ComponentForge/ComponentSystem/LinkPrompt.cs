using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkPrompt : MonoBehaviour
{

    public Image promptImage;
    public Sprite correctSprite;
    public Sprite errorSprite;

    public LinkPrompt linkedPrompt;
    public ComponentScript componentScript;
    public bool inPrompt;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Correct()
    {
        promptImage.sprite = correctSprite;
        promptImage.enabled = true;
    }

    public void Error()
    {
        promptImage.sprite = errorSprite;
        promptImage.enabled = true;
    }

    public void Hide()
    {
        promptImage.enabled = false;
    }

    public void Match(LinkPrompt linkPrompt)
    {
        linkedPrompt = linkPrompt;
        linkPrompt.linkedPrompt = this;
    }

    public void RemoveLink()
    {
        linkedPrompt.linkedPrompt = null;
        linkedPrompt = null;
    }
}
