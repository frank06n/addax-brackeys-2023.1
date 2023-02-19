using UnityEngine;

public class TrashSpriteSetter : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    private void Awake()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
