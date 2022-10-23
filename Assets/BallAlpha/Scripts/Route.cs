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
    List<RouteNode> _nodrs;

    /// <summary>時間の合計</summary>
    public float AllTime { get => _nodrs.Sum(n => n.Time); }
    public float MaxTime { get => _nodrs.Max(n => n.Time); }
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

    public void AddNode(Vector3 point, float time)
    {
        _nodrs.Add(new RouteNode(point, time));
        _nodrs.Sort();
    }

    public Vector3? PointInCaseTime(float time)
    {
        if(time < MinTime || time > MaxTime)
        {
            return null;
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
    public bool TryPointInCaseTime(float time, out Vector3 point)
    {
        point = Vector3.zero;
        if (time < MinTime || time > MaxTime)
        {
            return false;
        }
        for (int i = 1; i < _nodrs.Count; i++)
        {
            if (_nodrs[i].Time > time)
            {
                point = Vector3.Lerp(_nodrs[i - 1].Point, _nodrs[i].Point, (time - _nodrs[i - 1].Time) / (_nodrs[i].Time - _nodrs[i - 1].Time));
                return true;
            }
        }
        return false;
    }
}

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