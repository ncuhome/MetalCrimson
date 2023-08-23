using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TypeClick : MonoBehaviour, IPointerClickHandler
{
    public ComponentTypeScript componentTypeScript;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        componentTypeScript.ChooseType();
    }
}
