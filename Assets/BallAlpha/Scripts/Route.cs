using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

/// <summary>
/// 道のり、時間を含む配列
/// </summary>
public class BallRoute
{
    List<RouteNode> _nodrs = new List<RouteNode>();

    /// <summary>時間の合計</summary>
    public float AllTime { get => _nodrs.LastOrDefault().Time - _nodrs.FirstOrDefault().Time; }
    /// <summary>時間の最大値</summary>
    public float MaxTime { get => _nodrs.Max(n => n.Time); }
    /// <summary>時間の最低値</summary>
    public float MinTime { get => _nodrs.Min(n => n.Time); }
    /// <summary>道のりの合計</summary>
    public float AllWay
    {
        get
        {
            float way = 0;
            for (int i = 1; i < _nodrs.Count; i++)
            {
                way += Vector3.Distance(_nodrs[i - 1].Point, _nodrs[i].Point);
            }

            return way;
        }
    }

    /// <summary>ノードの数</summary>
    public int Count { get => _nodrs.Count; }

    /// <summary>
    /// RouteNode配列
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public RouteNode this[int index]
    {
        get
        {
            return _nodrs[index];
        }
    }

    /// <summary>
    /// Potisionの配列
    /// </summary>
    public Vector3[] Positons
    {
        get
        {
            Vector3[] positions = new Vector3[_nodrs.Count];
            for(int i = 0; i < positions.Length; i++)
            {
                positions[i] = _nodrs[i].Point;
            }

            return positions;
        }
    }


    /// <summary>
    /// ノードを追加する
    /// </summary>
    /// <param name="point"></param>
    /// <param name="time"></param>
    public void AddNode(Vector3 point, float time)
    {
        _nodrs.Add(new RouteNode(point, time));
        _nodrs.Sort();
    }

    /// <summary>
    /// 時間に対応した座標を返す
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public Vector3? PointInCaseTime(float time)
    {
        if(time < MinTime)
        {
            return _nodrs.FirstOrDefault().Point;
        }
        else if(time > MaxTime)
        {
            return _nodrs.LastOrDefault().Point;
        }
        Vector3? point = null;
        for(int i = 1; i < _nodrs.Count; i++)
        {
            if(_nodrs[i].Time > time)
            {
                point = Vector3.Lerp(_nodrs[i - 1].Point, _nodrs[i].Point, (time - _nodrs[i - 1].Time) / (_nodrs[i].Time - _nodrs[i - 1].Time));
                break;
            }
        }
        return point;
    }

    /// <summary>
    /// 時間に対応した座標を返す
    /// </summary>
    /// <param name="time"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool TryPointInCaseTime(float time, out Vector3 point)
    {
        Vector3? buf = PointInCaseTime(time);
        if (buf == null)
        {
            point = Vector3.zero;
            return false;
        }
        else
        {
            point = buf.Value;
            return true;
        }
    }

    /// <summary>
    /// 距離に対応した座標を返す
    /// </summary>
    /// <param name="way"></param>
    /// <returns></returns>
    public Vector3? PointInCaseDistance(float way)
    {
        if (way < 0)
        {
            return _nodrs.FirstOrDefault().Point;
        }
        else if(way > AllWay)
        {
            return _nodrs.LastOrDefault().Point;
        }
        Vector3? point = null;
        float distance = 0;
        for (int i = 1; i < _nodrs.Count; i++)
        {
            distance += Vector3.Distance(_nodrs[i - 1].Point, _nodrs[i].Point);
            if (distance > way)
            {
                point = Vector3.Lerp(_nodrs[i].Point, _nodrs[i - 1].Point, (distance - way) / (Vector3.Distance(_nodrs[i].Point ,_nodrs[i - 1].Point)));
                break;
            }
        }
        return point;
    }

    /// <summary>
    /// 時間に対応した座標を返す
    /// </summary>
    /// <param name="way"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool TryPointInCaseDistance(float way, out Vector3 point)
    {
        Vector3? buf = PointInCaseDistance(way);
        if (buf == null)
        {
            point = Vector3.zero;
            return false;
        }
        else
        {
            point = buf.Value;
            return true;
        }
    }
}

/// <summary>
/// 道を記録するノード
/// </summary>
public struct RouteNode : IComparable<RouteNode>
{

    public RouteNode(Vector3 point, float time)
    {
        Point = point;
        Time = time;
    }
    public Vector3 Point;
    public float Time;

    int IComparable<RouteNode>.CompareTo(RouteNode other)
    {
        if (Time > other.Time)
        {
            return 1;
        }
        else if (Time < other.Time)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}