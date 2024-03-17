using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ER.Items;
public class TipScript : MonoBehaviour
{
    public Image tipImage;
    public Vector3 oldVec;
    public Vector3 targetVec;
    public float moveTime;
    public bool startMove;
    // Start is called before the first frame update
    void Start()
    {
        oldVec = transform.localPosition;
        targetVec = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Magnitude(transform.localPosition - targetVec) < 0.5f)
        {
            transform.localPosition = targetVec;
            moveTime = 0f;
            startMove = false;
        }
        else
        {
            startMove = true;
            moveTime += Time.deltaTime * 5;
            Vector3 newVec = Vector3.Lerp(oldVec, targetVec, moveTime);
            transform.localPosition = newVec;
        }
    }

    public void ShowTips(int componentID)
    {
        StopCoroutine("WaitHideTip");
        tipImage.sprite = Resources.Load<Sprite>(TemplateStoreManager.Instance["Item"][componentID].GetText("StaticAddress"));
        tipImage.SetNativeSize();
        oldVec = new Vector3(-1200, 100, 0);
        targetVec = new Vector3(-720, 100, 0);
        startMove = true;
        StartCoroutine("WaitHideTip");
    }

    public IEnumerator WaitHideTip()
    {
        yield return new WaitForSeconds(2f);
        HideTip();
    }

    public void HideTip()
    {
        oldVec = new Vector3(-720, 100, 0);
        targetVec = new Vector3(-1200, 100, 0);
        startMove = true;
    }
}
