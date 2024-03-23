using ER.Resource;
using ER.Template;
using UnityEngine;

public class GameInit_1 : MonoBehaviour, MonoInit
{
    public void Init()
    {
        GR.LoadForce(MonoLoader.InitCallback,"pack:mc:init/global");
    }

    
}