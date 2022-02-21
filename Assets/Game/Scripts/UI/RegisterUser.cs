using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{
    [SerializeField] TMP_InputField username, email, password;
    [SerializeField] TMP_Text errorText;
    [SerializeField] Button backButton;

    public async void Register()
    {
        if (username.text == "") { errorText.text = "Invalid Username."; return; }
        if (email.text == "") { errorText.text = "Invalid Email Adress."; return; }
        if (password.text == "") { errorText.text = "Invalid Password."; return; }
        if (password.text.Length < 8) { errorText.text = "The password must contain atleast 8 characters."; return; }


        await User.RegisterUser(email.text, password.text, username.text);

        backButton.onClick.Invoke();
    }
}
