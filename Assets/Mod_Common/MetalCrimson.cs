// Ignore Spelling: Collider

using UnityEngine;

public static class MetalCrimson
{
    /// <summary>
    /// 自动归正UI的Collider大小
    /// </summary>
    /// <param name="transform"></param>
    public static void AutoColliderSize(RectTransform transform)
    {
        BoxCollider2D collider= transform.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(transform.sizeDelta.x,transform.sizeDelta.y);
    }
}