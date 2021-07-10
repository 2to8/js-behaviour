using UnityEngine;
using System.Collections;

public class PointsKeeper
{
    private int total;

    public void addPoints(int points)
    {
        total += points;
    }

    public int getTotal()
    {
        return total;
    }
}