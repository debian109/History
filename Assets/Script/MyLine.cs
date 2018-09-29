using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLine  {

    public Point p1;
    public Point p2;

    public MyLine(Point p1, Point p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    public string toString()
    {
        string str = "(" + p1.x + "," + p1.y + ") and (" + p2.x + "," + p2.y + ")";
        return str;
    }
}
