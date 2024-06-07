using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionsManager : MonoBehaviour
{
    public Position position;
    public BehindTheStump behindStumpPosition;
}
public enum Position
{
    Position1,
    Position2,
    Position3,
    Position4,
    Position5,
    Position6,
    Position7
}
public enum BehindTheStump
{
    True,
    False
}