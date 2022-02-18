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
        text = $"{GameController.gameState}";

        if (GameController.gameState == GameStates.PlaceShips || GameController.gameState == GameStates.Attack) {
            text += $"\n{GameController.turnState}";
        }

        _text.text = text;
    }


}
