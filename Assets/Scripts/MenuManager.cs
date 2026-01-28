using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Fusion;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public TMP_Dropdown colorDropdown;
    public Button startButton;

    
    public static string LocalPlayerName;
    public static Color LocalPlayerColor;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        
        LocalPlayerName = nameInput.text;
        if (string.IsNullOrEmpty(LocalPlayerName)) LocalPlayerName = "Guest_" + Random.Range(100, 999);

        LocalPlayerColor = GetColorFromDropdown(colorDropdown.value);

        
        gameObject.SetActive(false);

        
        FindObjectOfType<NetworkRunner>().StartGame(new StartGameArgs {
            GameMode = GameMode.AutoHostOrClient, 
            SessionName = "TestRoom"
        });
    }

    private Color GetColorFromDropdown(int index)
    {
        switch (index)
        {
            case 0: return Color.red;
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.yellow;
            default: return Color.white;
        }
    }
}