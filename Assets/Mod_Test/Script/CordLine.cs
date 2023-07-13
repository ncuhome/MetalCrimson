using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 物理链条
/// </summary>
public class CordLine : MonoBehaviour
{
    /// <summary>
    /// 曲线拟合点的数量
    /// </summary>
    [Range(1,99)]
    public int NodeCount = 50;
    /// <summary>
    /// p1的水平偏移
    /// </summary>
    public float P1HDrift;
    /// <summary>
    /// p2的水平偏移
    /// </summary>
    public float P2HDrift;
    /// <summary>
    /// p2的垂直偏移
    /// </summary>
    public float P2VDrift;

    /// <summary>
    /// 链条点
    /// </summary>
    private List<Transform> nodes;
    /// <summary>
    /// 线绘制器
    /// </summary>
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        #region 获取子物体（节点）的位置
        nodes = GetComponentsInChildren<Transform>().ToList();
        nodes.RemoveAt(0);//移除自身(父物体)的位置

        lr= GetComponent<LineRenderer>();
        #endregion
    }
    /// <summary>
    /// 获取画线的节点坐标
    /// </summary>
    /// <returns></returns>
    private List<Vector3> GetLineNodes()
    {
        List<Vector3> linePosList = new List<Vector3>();//曲线上的点
        List<Vector3> chainPosList = nodes.Select(x=>x.position).ToList();//链条上的点
        Vector3? preP2 = null;
        for(int i=0;i<chainPosList.Count-1;i++)
        {
            //和邻三高点形成的夹角
            float angle = 0;
            ///交接影响系数
            float angleEfect = 0;

            if(i<chainPosList.Count-2)
            {
                angle = Vector2.Angle(chainPosList[i] - chainPosList[i + 1], chainPosList[i + 2] - chainPosList[i + 1]);
                angleEfect = 1 - (angle/ 180f);
            }

            var ctrlPos = GetCtrlPoint(chainPosList[i], chainPosList[i+1],angleEfect,preP2);
            preP2 = ctrlPos[1];
            //通过贝塞尔曲线拟合曲线，获得拟合点坐标
            for(int j=0;j<NodeCount;j++)
            {
                Vector3 pos = Bezier(chainPosList[i], ctrlPos[0], ctrlPos[1], chainPosList[i + 1], j / (float)NodeCount);
                linePosList.Add(pos);
            }
        }
        return linePosList;
    }
    /// <summary>
    /// 绘制线
    /// </summary>
    private void DrawLine()
    {
        var nodes = GetLineNodes();
        lr.positionCount= nodes.Count;
        lr.SetPositions(nodes.ToArray());
    }
    /// <summary>
    /// 三阶贝塞尔曲线推导公式
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3,float t)
    {
        Vector3 temp;
        var p0p1 = (1 - t) * p0 + t * p1;
        var p1p2 = (1 - t) * p1 + t * p2;
        var p2p3 = (1 - t) * p2 + t * p3;
        var p0p1p2 = (1 - t) * p0p1 + t * p1p2;
        var p1p2p3 = (1 - t) * p1p2 + t * p2p3;
        temp = (1 - t) * p0p1p2 + t * p1p2p3;
        return temp;
    }

    private Vector3[] GetCtrlPoint(Vector3 p0,Vector3 p3,float angleEffect,Vector3? preP2 = null)
    {
        //判断控制dian p2,垂直偏移的正负方向
        int dir = 0;
        if(Vector3.Cross(Vector2.up,p3-p0).x>0) dir = 1;
        else dir = -1;

        //当前两个点(p0;p3)所在向量和垂直方向的夹角
        float vAngle = Vector2.Angle(p3-p0,Vector3.down);
        //向量重合度越高，影响系数越小
        float vAngleEffect = vAngle > 90 ? (180 - vAngle) / 90f : vAngle / 90f;


        Vector3 p1, p2;

        p1 = p0 + (p3 - p0) * P1HDrift;
        p2 = p0 + (p3 - p0) * P2HDrift;

        if(preP2!=null) p1 = p0 + (p0 - (Vector3)preP2);
        p2 = (Vector2)p2 + Vector2.Perpendicular(p3 - p0).normalized*angleEffect*P2VDrift*dir*vAngleEffect;
        return new Vector3[] { p1, p2 };

    }

    // Update is called once per frame
    void Update() 
    {
        DrawLine(); 
    }
}
