using UnityEngine;
using TMPro;
using Fusion;

public class MainMenuUI : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_Dropdown colorDropdown;
    public NetworkManager networkManager;

    public void OnClickStart()
    {
        
        NetworkManager.SelectedName = nameField.text;
        NetworkManager.SelectedColor = GetColorFromIndex(colorDropdown.value);

        
        networkManager.StartGame(GameMode.AutoHostOrClient);

        
        this.gameObject.SetActive(false);
    }

    private Color GetColorFromIndex(int index)
    {
        return index switch {
            0 => Color.red,
            1 => Color.blue,
            2 => Color.green,
            _ => Color.white
        };
    }
}