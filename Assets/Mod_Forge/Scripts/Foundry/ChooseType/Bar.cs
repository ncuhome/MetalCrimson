using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bar : MonoBehaviour
{
    public int barNum;
    public Image[] blocks;
    public Color barColor;
    public Transform blockParentTrans;
    // Start is called before the first frame update

    void Awake()
    {
        blocks = new Image[10];
        for (int i = 0; i < 10; i++)
        {
            blocks[i] = blockParentTrans.GetChild(i).GetComponent<Image>();
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshBar()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            if (i < barNum)
            {
                blocks[i].color = barColor;
            }
            else
            {
                blocks[i].color = Color.gray;
            }
        }
    }
}
