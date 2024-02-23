using System.Collections;
using System.Collections.Generic;
using ER.Items;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tether : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public string NameTmp;
    public string Name;
    public int tetherID;
    public string avatarAddress;
    public int num;
    public bool show;
    public string[] introductions;
    public string introduction;
    public string description;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitTether(ItemTemplate tetherTemplate)
    {
        NameTmp = tetherTemplate.NameTmp;
        Name = tetherTemplate.GetText("Name");
        tetherID = tetherTemplate.ID;
        avatarAddress = tetherTemplate.GetText("AvatarAddress");
        num = 10;
        show = string.Equals(tetherTemplate.GetText("Show"), "TRUE");
        introductions = tetherTemplate.GetText("Introductions").Split(';');
        description = tetherTemplate.GetText("Description");
    }

    public void RefreshTether()
    {
        switch (tetherID)
        {
            case 800001:
                if (num >= 1)
                {
                    introduction = introductions[0];
                }
                if (num >= 3)
                {
                    introduction += "\n" + introductions[1];
                }
                if (num >= 7)
                {
                    introduction += "\n" + introductions[2];
                }
                if (num >= 10)
                {
                    introduction += "\n" + introductions[3];
                }
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        TetherSystem.Instance.ShowInfo(this);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        TetherSystem.Instance.HideInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransform rectTransform = UIManager.Instance.canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out pos);
        TetherSystem.Instance.MoveInfo(pos + new Vector2(140, -194));
    }
}
