using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardControl : MonoBehaviour
{
    public static BoardControl instance = null;

    public int width;

    public int height;

    public Transform tilePrefabs;

    public GameObject[,] matrix;

    public List<GameObject> list = new List<GameObject>();

    public const int BARRIER = 2;
    public const int NEED = 1;

    int[,] getMaxtrix()
    {
        int[,] m = new int[height, width];
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                m[j, i] = matrix[j, i].GetComponent<TileControl>().barrier;
            }
        }
        return m;
    }

    // Use this for initialization
    void Start()
    {
        matrix = new GameObject[height , width];

        for (int j = 0; j < height ; j++)
        {
            for (int i = 0; i < width; i++)
            {
                var tile = Instantiate(tilePrefabs, transform.position, transform.rotation);
                tile.transform.SetParent(transform);
                tile.localScale = Vector3.one;
                var component = tile.GetComponent<TileControl>();
                component.x = j;
                component.y = i;
                matrix[j, i] = tile.gameObject;
           
                
                if (i == 0 || j == 0 || i == width-1 || j == height-1)
                {
                    component.barrier = 0;
                    component.Hide();
                }
                else
                {
                    component.barrier = BARRIER;
                    list.Add(tile.gameObject);
                }

            }
            

        }






        data();



        Invoke("Disable", 1.0f);

    }

    public void Disable()
    {
        GetComponent<GridLayoutGroup>().enabled = false;
    }

    public void data()
    {
        List<GameObject> temp = new List<GameObject>();
        int length = list.Count / 4;
        for (int i = 0; i < length; i++)
        {
            int r1 = Random.Range(0, list.Count);
            list[r1].GetComponent<TileControl>().type = i;
            temp.Add(list[r1]);
            list.RemoveAt(r1);

            int r2 = Random.Range(0, list.Count);
            list[r2].GetComponent<TileControl>().type = i;
            temp.Add(list[r2]);
            list.RemoveAt(r2);

            int r3 = Random.Range(0, list.Count);
            list[r3].GetComponent<TileControl>().type = i;
            temp.Add(list[r3]);
            list.RemoveAt(r3);

            int r4 = Random.Range(0, list.Count);
            list[r4].GetComponent<TileControl>().type = i;
            temp.Add(list[r4]);
            list.RemoveAt(r4);
        }

        list = temp;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

    }

    public GameObject current;

    public void Eat(GameObject go)
    {
        Debug.Log("v");
        if (current == null)
            current = go;
        else
        {
            CheckEat(current, go);
            current = null;
        }
    }



    public void CheckEat(GameObject t1, GameObject t2)
    {
        var c1 = t1.GetComponent<TileControl>();
        var c2 = t2.GetComponent<TileControl>();
        c1.barrier = 1;
        c2.barrier = 1;
        if (isConnect(c1,c2))
        {
            c1.barrier = 0;
            c2.barrier = 0;
            c1.gameObject.SetActive(false);
            c2.gameObject.SetActive(false);
        }
        else
        {
            c1.barrier = BARRIER;
            c2.barrier = BARRIER;
            c1.Revert();
            c2.Revert();
            c1.Up();
            c2.Up();
        }
    }

    public bool isConnect(TileControl t1,TileControl t2)
    {
        Algorithm al = new Algorithm(width,height);
        al.setMatrix(getMaxtrix());
        bool find = al.checkTwoPoint(t1.getPoint(), t2.getPoint()) != null;
        Debug.Log("find" + find);
        return  find && t1.type==t2.type;
    }
}
