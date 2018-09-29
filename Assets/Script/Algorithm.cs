using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm
{

    private int row;
    private int col;
    private int notBarrier = 0;
    private int[,] matrix;

    public Algorithm(int row, int col)
    {
        this.row = row;
        this.col = col;
        //createMatrix();
       // showMatrix();
    }

    // show matrix
    public void showMatrix()
    {
        for (int i = 1; i < row - 1; i++)
        {
            for (int j = 1; j < col - 1; j++)
            {
                Debug.Log(matrix[i, j]);
            }
            Debug.Log("");
        }
    }

    // check with line x, from column y1 to y2
    private bool checkLineX(int y1, int y2, int x)
    {
        Debug.Log("check line x");
        // find point have column max and min
        int min = Mathf.Min(y1, y2);
        int max = Mathf.Max(y1, y2);
        // run column
        for (int y = min + 1; y < max; y++)
        {
            if (matrix[x, y] > notBarrier)
            { // if see barrier then die
                Debug.Log("die: " + x + "" + y);
                return false;
            }
            Debug.Log("ok: " + x + "" + y);
        }
        // not die -> success
        return true;
    }

    private bool checkLineY(int x1, int x2, int y)
    {
        Debug.Log("check line y");
        int min = Mathf.Min(x1, x2);
        int max = Mathf.Max(x1, x2);
        for (int x = min + 1; x < max; x++)
        {
            if (matrix[x, y] > notBarrier)
            {
                Debug.Log("die: " + x + "" + y);
                return false;
            }
            Debug.Log("ok: " + x + "" + y);
        }
        return true;
    }

    // check in rectangle
    private int checkRectX(Point p1, Point p2)
    {
        Debug.Log("check rect x");
        // find point have y min and max
        Point pMinY = p1, pMaxY = p2;
        if (p1.y > p2.y)
        {
            pMinY = p2;
            pMaxY = p1;
        }
        for (int y = pMinY.y; y <= pMaxY.y; y++)
        {
            if (y > pMinY.y && matrix[pMinY.x, y] > notBarrier)
            {
                return -1;
            }
            // check two line
            if ((matrix[pMaxY.x, y] == notBarrier || y == pMaxY.y)
                    && checkLineY(pMinY.x, pMaxY.x, y)
                    && checkLineX(y, pMaxY.y, pMaxY.x))
            {

                Debug.Log("Rect x");
                Debug.Log("(" + pMinY.x + "," + pMinY.y + ") -> ("
                       + pMinY.x + "," + y + ") -> (" + pMaxY.x + "," + y
                       + ") -> (" + pMaxY.x + "," + pMaxY.y + ")");
                // if three line is true return column y
                return y;
            }
        }
        // have a line in three line not true then return -1
        return -1;
    }

    private int checkRectY(Point p1, Point p2)
    {
        Debug.Log("check rect y");
        // find point have y min
        Point pMinX = p1, pMaxX = p2;
        if (p1.x > p2.x)
        {
            pMinX = p2;
            pMaxX = p1;
        }
        // find line and y begin
        for (int x = pMinX.x; x <= pMaxX.x; x++)
        {
            if (x > pMinX.x && matrix[x, pMinX.y] > notBarrier)
            {
                return -1;
            }
            if ((matrix[x, pMaxX.y] == notBarrier || x == pMaxX.x)
                    && checkLineX(pMinX.y, pMaxX.y, x)
                    && checkLineY(x, pMaxX.x, pMaxX.y))
            {

                Debug.Log("Rect y");
                Debug.Log("(" + pMinX.x + "," + pMinX.y + ") -> (" + x
                       + "," + pMinX.y + ") -> (" + x + "," + pMaxX.y
                       + ") -> (" + pMaxX.x + "," + pMaxX.y + ")");
                return x;
            }
        }
        return -1;
    }

    /**
	 * p1 and p2 are Points want check
	 * 
	 * @param type
	 *            : true is check with increase, false is decrease return column
	 *            can connect p1 and p2
	 */
    private int checkMoreLineX(Point p1, Point p2, int type)
    {
        Debug.Log("check chec more x");
        // find point have y min
        Point pMinY = p1, pMaxY = p2;
        if (p1.y > p2.y)
        {
            pMinY = p2;
            pMaxY = p1;
        }
        // find line and y begin
        int y = pMaxY.y + type;
        int row = pMinY.x;
        int colFinish = pMaxY.y;
        if (type == -1)
        {
            colFinish = pMinY.y;
            y = pMinY.y + type;
            row = pMaxY.x;
            Debug.Log("colFinish = " + colFinish);
        }

        // find column finish of line

        // check more
        if ((matrix[row, colFinish] == notBarrier || pMinY.y == pMaxY.y)
                && checkLineX(pMinY.y, pMaxY.y, row))
        {
            while (matrix[pMinY.x, y] == notBarrier
                    && matrix[pMaxY.x, y] == notBarrier)
            {
                if (checkLineY(pMinY.x, pMaxY.x, y))
                {

                    Debug.Log("TH X " + type);
                    Debug.Log("(" + pMinY.x + "," + pMinY.y + ") -> ("
                           + pMinY.x + "," + y + ") -> (" + pMaxY.x + "," + y
                           + ") -> (" + pMaxY.x + "," + pMaxY.y + ")");
                    return y;
                }
                y += type;
            }
        }
        return -1;
    }

    private int checkMoreLineY(Point p1, Point p2, int type)
    {
        Debug.Log("check more y");
        Point pMinX = p1, pMaxX = p2;
        if (p1.x > p2.x)
        {
            pMinX = p2;
            pMaxX = p1;
        }
        int x = pMaxX.x + type;
        int col = pMinX.y;
        int rowFinish = pMaxX.x;
        if (type == -1)
        {
            rowFinish = pMinX.x;
            x = pMinX.x + type;
            col = pMaxX.y;
        }
        if ((matrix[rowFinish, col] == notBarrier || pMinX.x == pMaxX.x)
                && checkLineY(pMinX.x, pMaxX.x, col))
        {
            while (matrix[x, pMinX.y] == notBarrier
                    && matrix[x, pMaxX.y] == notBarrier)
            {
                if (checkLineX(pMinX.y, pMaxX.y, x))
                {
                    Debug.Log("TH Y " + type);
                    Debug.Log("(" + pMinX.x + "," + pMinX.y + ") -> ("
                           + x + "," + pMinX.y + ") -> (" + x + "," + pMaxX.y
                           + ") -> (" + pMaxX.x + "," + pMaxX.y + ")");
                    return x;
                }
                x += type;
            }
        }
        return -1;
    }

    public MyLine checkTwoPoint(Point p1, Point p2)
    {
        if (!p1.Equals(p2) && matrix[p1.x, p1.y] == matrix[p2.x, p2.y])
        {
            // check line with x
            if (p1.x == p2.x)
            {
                Debug.Log("line x");
                if (checkLineX(p1.y, p2.y, p1.x))
                {
                    return new MyLine(p1, p2);
                }
            }
            // check line with y
            if (p1.y == p2.y)
            {
                Debug.Log("line y");
                if (checkLineY(p1.x, p2.x, p1.y))
                {
                    Debug.Log("ok line y");
                    return new MyLine(p1, p2);
                }
            }

            int t = -1; // t is column find

            // check in rectangle with x
            if ((t = checkRectX(p1, p2)) != -1)
            {
                Debug.Log("rect x");
                return new MyLine(new Point(p1.x, t), new Point(p2.x, t));
            }

            // check in rectangle with y
            if ((t = checkRectY(p1, p2)) != -1)
            {
                Debug.Log("rect y");
                return new MyLine(new Point(t, p1.y), new Point(t, p2.y));
            }
            // check more right
            if ((t = checkMoreLineX(p1, p2, 1)) != -1)
            {
                Debug.Log("more right");
                return new MyLine(new Point(p1.x, t), new Point(p2.x, t));
            }
            // check more left
            if ((t = checkMoreLineX(p1, p2, -1)) != -1)
            {
                Debug.Log("more left");
                return new MyLine(new Point(p1.x, t), new Point(p2.x, t));
            }
            // check more down
            if ((t = checkMoreLineY(p1, p2, 1)) != -1)
            {
                Debug.Log("more down");
                return new MyLine(new Point(t, p1.y), new Point(t, p2.y));
            }
            // check more up
            if ((t = checkMoreLineY(p1, p2, -1)) != -1)
            {
                Debug.Log("more up");
                return new MyLine(new Point(t, p1.y), new Point(t, p2.y));
            }
        }
        return null;
    }

    private void createMatrix()
    {

        matrix = new int[row, col];
        {
            for (int i = 0; i < col; i++)
            {
                matrix[0, i] = matrix[row - 1, i] = 0;
            }
        }
        {
            for (int i = 0; i < row; i++)
            {
                matrix[i, 0] = matrix[i, col - 1] = 0;
            }

        }


        Random rand = new Random();
        // int imgNumber = row * col / 4;
        int imgNumber = 37;
        int maxDouble = imgNumber / 4;
        int[] imgArr = new int[imgNumber + 1];
        List<Point> listPoint = new List<Point>();
        {
            for (int i = 1; i < row - 1; i++)
            {
                for (int j = 1; j < col - 1; j++)
                {
                    listPoint.Add(new Point(i, j));
                }
            }
        }

        int k = 0;
        do
        {
            int imgIndex = Random.Range(0, imgNumber) + 1;
            if (imgArr[imgIndex] < maxDouble)
            {
                imgArr[imgIndex] += 2;
                for (int j = 0; j < 2; j++)
                {

                    try
                    {
                        int size = listPoint.Count;
                        int pointIndex = Random.Range(0, size);
                        matrix[listPoint[pointIndex].x, listPoint[pointIndex].y] = imgIndex;
                        listPoint.RemoveAt(pointIndex);
                    }
                    catch (System.Exception)
                    {


                    }




                }
                k++;
            }
        } while (k < row * col / 2);

        // for (i = 1; i < row - 1; i++) {
        // for (int j = 1; j < col - 1; j++) {
        // if (imgArr[matrix[i][j]] == 0 || imgArr[matrix[i][j]] % 2 > 0) {
        //  Debug.Log(i + "," + j + "-" + imgArr[matrix[i][j]]
        // + "-" + matrix[i][j]);
        // }
        // }
        // }
    }

    public int getRow()
    {
        return row;
    }

    public void setRow(int row)
    {
        this.row = row;
    }

    public int getCol()
    {
        return col;
    }

    public void setCol(int col)
    {
        this.col = col;
    }

    public int[,] getMatrix()
    {
        return matrix;
    }

    public void setMatrix(int[,] matrix)
    {
        this.matrix = matrix;
    }

}
