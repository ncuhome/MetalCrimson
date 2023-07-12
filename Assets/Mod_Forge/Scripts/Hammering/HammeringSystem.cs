using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammeringSystem : MonoBehaviour
{
    static public HammeringSystem Instance = null;
    public int AddedMaterialNum = 0;
    public MaterialScript[] materialScripts = null;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            materialScripts = new MaterialScript[3];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMaterialJudgement(MaterialScript materialScript)
    {
        if (AddedMaterialNum < 3)
        {
            AddMaterial(materialScript);
        }
    }
    void AddMaterial(MaterialScript materialScript)
    {
        materialScripts[AddedMaterialNum] = materialScript;
        materialScript.materialNum --;
        AddedMaterialNum ++;
    }
}
