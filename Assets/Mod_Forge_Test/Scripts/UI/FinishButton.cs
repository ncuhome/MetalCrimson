using UnityEngine;

public class FinishButton : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void FinishQTE()
    {
        QTE.Instance.FinishQTE();
    }
}