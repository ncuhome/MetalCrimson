using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class materialInFurnace : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// 在炉子里的顺序ID
    /// </summary>
    public int ID;

    private MaterialScript materialScript;
    public HDRScript HDRScript;
    private Outline outline;
    private ShowOutLine showOutLine;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (HammeringSystem.Instance.chainTimes > 0)
        {
            outline.enabled = false;
            showOutLine.enabled = false;
        }
        else
        {
            outline.enabled = true;
            showOutLine.enabled = true;
            HDRScript.HideColor();
        }
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        materialScript = HammeringSystem.Instance.materialScripts[ID];
        HDRScript = GetComponent<HDRScript>();
        outline = GetComponent<Outline>();
        showOutLine = GetComponent<ShowOutLine>();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {

    }

    /// <summary>
    /// 点击时返还材料
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        if (HammeringSystem.Instance.chainTimes > 0) { return; }
        Debug.Log("EnableClick");
        HammeringSystem.Instance.MoveBackMaterial(ID);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

}