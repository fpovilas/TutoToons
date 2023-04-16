using UnityEngine;
using TMPro;

public class Fade : MonoBehaviour
{
    TextMeshProUGUI tekstas;
    float laikmatis;
    float laikasIsnykimui = 3f;
    float isnyksta;
    bool isVisable = true;

    private void Awake()
    {
        tekstas = FindObjectOfType<TextMeshProUGUI>();
    }

    void Start()
    {
        laikmatis = laikasIsnykimui;
    }

    void Update()
    {
        letaiPanaikintiTeksta();
    }

    void letaiPanaikintiTeksta()
    {
        laikmatis -= Time.deltaTime;
        isnyksta = laikmatis / laikasIsnykimui;
        if(isVisable && isnyksta >= 0)
        {
            tekstas.alpha = isnyksta;
        }
    }
}