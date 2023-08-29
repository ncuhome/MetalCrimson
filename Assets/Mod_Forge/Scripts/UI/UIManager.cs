using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI管理器
/// </summary>
public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Button ReturnButton, CancelButton, FinishButton;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}