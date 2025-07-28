using UnityEngine;

public class PlayerSkinSelect : MonoBehaviour
{
    public GameObject player;
    private SpriteRenderer playerSprite;
    public Sprite[] customSprite;


    void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
    }

    public void SelectKaan()
    {
        playerSprite.sprite = customSprite[0];
    }

    public void SelectKerem()
    {
        playerSprite.sprite = customSprite[1];
    }

    public void SelectKuzey()
    {
        playerSprite.sprite = customSprite[2];
    }
}
