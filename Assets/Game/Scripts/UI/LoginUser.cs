using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginUser : MonoBehaviour
{
    [SerializeField] TMP_InputField email, password;
    [SerializeField] TMP_Text errorText;

    public async void SignIn()
    {
        if (email.text == "") { errorText.text = "Invalid Email Adress."; return; }
        if (password.text == "") { errorText.text = "Invalid Password."; return; }

        if(!await User.SignIn(email.text, password.text)) { errorText.text = "Something went wrong..."; return; }

        SceneManager.LoadScene("MainMenu");
    }

}
