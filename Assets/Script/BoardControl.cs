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

    public GameObject[,] map;

    public List<GameObject> list = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        map = new GameObject[width, height];

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                var tile = Instantiate(tilePrefabs, transform.position, transform.rotation);
                tile.transform.SetParent(transform);
                tile.localScale = Vector3.one;
                var component = tile.GetComponent<TileControl>();
                component.x = j;
                component.y = i;
                map[i, j] = tile.gameObject;
                list.Add(tile.gameObject);
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
        int length = list.Count / 2;
        for (int i = 0; i < length; i++)
        {
            int r1=Random.Range(0, list.Count);
            list[r1].GetComponent<TileControl>().type = i;
            temp.Add(list[r1]);
            list.RemoveAt(r1);

            int r2 = Random.Range(0, list.Count);
            list[r2].GetComponent<TileControl>().type = i;
            temp.Add(list[r2]);
            list.RemoveAt(r2);
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

    public void Eat(GameObject go){
        Debug.Log("v");
        if (current == null)
            current = go;
        else{
            CheckEat(current, go);
            current = null;
        }
    }

    public void CheckEat(GameObject t1,GameObject t2){
        var c1 = t1.GetComponent<TileControl>();
        var c2 = t2.GetComponent<TileControl>();
        if(c1.type==c2.type){
            c1.gameObject.SetActive(false);
            c2.gameObject.SetActive(false);
        }else{
            c1.Revert();
            c2.Revert();
            c1.Up();
            c2.Up();
        }
    }
}
