
using System.Collections;
using System.Collections.Generic;
using ER.Items;
using TMPro;
using UnityEngine;

public class TetherSystem : MonoBehaviour
{
    #region 单例封装

    private static TetherSystem instance;

    public static TetherSystem Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    public List<Tether> tethers = new List<Tether>();
    public Vector2[] tethersPosition;
    public GameObject tetherPrefab;
    public Transform tetherParent;

    public GameObject tetherInfo;
    public TMP_Text tetherName;
    public TMP_Text tetherDescription;
    public TMP_Text tetherIntroduction;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    public void InitTethers()
    {
        for (int id = 800001; id < 800008; id++)
        {
            ItemTemplate tetherTemplate = TemplateStoreManager.Instance["Tether"][id];
            Tether tether = Instantiate(tetherPrefab).GetComponent<Tether>();
            tether.InitTether(tetherTemplate);
            tether.transform.parent = tetherParent;
            tether.transform.localPosition = tethersPosition[id - 800001];
            tether.transform.localScale = Vector3.one;
            tethers.Add(tether);
        }
        RefreshTether();
    }

    public void RefreshTether()
    {
        RefreshTetherInComponents();
        int i = 0;
        foreach (var tether in tethers)
        {
            tether.RefreshTether();
            if (tether.show && (tether.num != 0))
            {

                tether.gameObject.SetActive(true);
                tether.transform.localPosition = tethersPosition[i];
                i++;
            }
            else
            {
                tether.gameObject.SetActive(false);
            }
        }
    }

    public void ShowInfo(Tether tether)
    {
        tetherInfo.SetActive(true);
        tetherName.text = tether.Name + " " + tether.num;
        tetherDescription.text = tether.description;
        tetherIntroduction.text = tether.introduction;
    }

    public void HideInfo()
    {
        tetherInfo.SetActive(false);
    }

    public void MoveInfo(Vector2 vector2)
    {
        tetherInfo.transform.localPosition = vector2;
    }

    public Tether GetTether(int id)
    {
        foreach (var tether in tethers)
        {
            if (tether.tetherID == id)
            {
                return tether;
            }
        }
        return null;
    }

    public void RefreshTetherInComponents()
    {
        List<ComponentScript> components = ComponentSystem.Instance.componentInAnvil;
        foreach (var tether in tethers)
        {
            tether.num = 0;
            foreach (var component in components)
            {
                int productID = component.ComponentItem.GetInt("MaterialID");
                int value;
                TemplateStoreManager.Instance["Item"][productID].TryGetInt(tether.NameTmp, out value);
                tether.num += value;
            }
        }
    }

}
