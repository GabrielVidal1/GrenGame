using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class controlsMenu : MonoBehaviour {

    public bool noTransition;

    public GameObject menu;
    public GameObject parent;

    [HideInInspector] public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI fwd, lft, bwd, rgt, jmp, sprt, invtry, inter;

    private GameObject curKey;
    private Color32 normal = new Color32(149, 255, 0, 133);
    private Color32 selected = new Color32(0, 255, 115, 133);

    [SerializeField] private MainMainMenu mainMainMenu;

    // Use this for initialization
    void Start () {
        keys.Add("Fwd", KeyCode.Z);
        keys.Add("Lft", KeyCode.Q);
        keys.Add("Bwd", KeyCode.S);
        keys.Add("Rgt", KeyCode.D);
        keys.Add("Jmp", KeyCode.Space);
        keys.Add("Sprt", KeyCode.LeftShift);
        keys.Add("Invtry", KeyCode.Tab);
        keys.Add("Inter", KeyCode.E);

        fwd.text = keys["Fwd"].ToString();
        lft.text = keys["Lft"].ToString();
        bwd.text = keys["Bwd"].ToString();
        rgt.text = keys["Rgt"].ToString();
        jmp.text = keys["Jmp"].ToString();
        sprt.text = keys["Sprt"].ToString();
        invtry.text = keys["Invtry"].ToString();
        inter.text = keys["Inter"].ToString();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnGUI()
    {
        if (curKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[curKey.name] = e.keyCode;
                curKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                curKey.GetComponent<Image>().color = normal;
                curKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        if (curKey != null)
        {
            curKey.GetComponent<Image>().color = normal;
        }
        curKey = clicked;
        curKey.GetComponent<Image>().color = selected;
    }

    public void Save()
    {

    }

    public void Back()
    {
        if (!noTransition)
        {
            mainMainMenu.transition = VoidBack;
            mainMainMenu.Transit();
        }
        else
        {
            VoidBack();
        }
    }

    void VoidBack()
    {
        menu.SetActive(false);
        parent.SetActive(true);
    }
}
