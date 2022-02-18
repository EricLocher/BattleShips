using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginUser : MonoBehaviour
{
    [SerializeField] TMP_InputField email, password;
    [SerializeField] TMP_Text errorText;

    public async void SignIn()
    {
        if (email.text == "") { errorText.text = "Invalid Email Adress."; return; }
        if (password.text == "") { errorText.text = "Invalid Password."; return; }

        await User.SignIn(email.text, password.text);

        SceneManager.LoadScene("MainMenu");
    }

}
