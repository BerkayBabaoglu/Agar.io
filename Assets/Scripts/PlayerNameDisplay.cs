using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameDisplay : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TextMeshProUGUI nameText;
    public GameObject player;

    private void Start()
    {
        UpdatePlayerName();

        nameInputField.onValueChanged.AddListener(delegate { UpdatePlayerName(); });
    }

    void UpdatePlayerName()
    {
        if (!string.IsNullOrEmpty(nameInputField.text))
        {
            nameText.text = nameInputField.text;
        }
    }

    private void Update()
    {
        Vector3 abovePlayer = player.transform.position;
        nameText.transform.position = Camera.main.WorldToScreenPoint(abovePlayer);
    }
}
