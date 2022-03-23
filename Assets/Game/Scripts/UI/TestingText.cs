using TMPro;
using UnityEngine;
public class TestingText : MonoBehaviour
{
    string text = "";
    TMP_Text _text;

    void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text = $"{GameController.turnState}";

        _text.text = text;
    }


}
