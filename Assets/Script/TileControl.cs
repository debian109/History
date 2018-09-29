using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileControl : MonoBehaviour {

    public List<Sprite> list = new List<Sprite>();

    public int type;

    public int x;

    public int y;

    public Image image;

    public bool active;

    public bool pressing;

    public int barrier;

	// Use this for initialization
	void Start () {
        if(type>=0 && type<list.Count){
            GetComponent<Image>().sprite = list[type];
        }
        GetComponent<Button>().onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick(){
        if (!pressing)
        {
            pressing = true;
            Revert();
            Invoke("Eat", 0.5f);
        }
       
    }

    void Eat(){
        BoardControl.instance.Eat(gameObject);
    }

    public void Revert(){
        active = !active;
        image.gameObject.SetActive(active);
    }

    public void Up()
    {
        pressing = false;
    }

    public void In()
    {
        barrier = 0;
    }

    public void Hide()
    {
        GetComponent<Image>().enabled = false;
    }

    public Point getPoint()
    {
        Debug.Log(x + "," + y);
        return new Point(x, y);
    }
}
