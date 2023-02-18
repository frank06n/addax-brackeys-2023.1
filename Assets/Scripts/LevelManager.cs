using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform pickupsHolder;
    [SerializeField] private LayerMask raycastLayers;
    private readonly int[] raycastMinDepths = { 0, -5 };
    private readonly int[] raycastMaxDepths = { 5,  0 };

    public Transform player;
    [SerializeField] private FurnaceLogic furnace;
    [HideInInspector] public int LAYER_VULNERABLE;

    [Header("UI Elements")]

    [SerializeField] private RectTransform panelObjs; // Objectives Panel
    private bool panelObjs_Animating;
    private bool panelObjs_Visible;

    [SerializeField] private RectTransform playerHealthFill;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);

        instance = this;

        panelObjs_Animating = false;
        panelObjs_Visible = false;

        LAYER_VULNERABLE = LayerMask.NameToLayer("Vulnerable");
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().play("Theme");
    }

    public void ToggleObjectivesPanel()
    {
        if (panelObjs_Animating) return;
        StartCoroutine(AnimateObjectivesPanel(!panelObjs_Visible));
    }

    public bool IsObjectivesPanelVisible()
    {
        return panelObjs_Visible;
    } 

    public RaycastHit2D UnFireRaycast(Vector2 origin, Vector2 direction, string shooterTag)
    {
        int i = shooterTag == "Player" ? 0 : 1;
        return Physics2D.Raycast(origin, direction, 50,
            raycastLayers, raycastMinDepths[i], raycastMaxDepths[i]);
    }

    private IEnumerator AnimateObjectivesPanel(bool show)
    {
        panelObjs_Animating = true;
        float animProgress = 0f;

        const float ANIM_DURATION = 0.25f;
        float PANEL_Y = panelObjs.anchoredPosition.y;

        while (animProgress <= 1f)
        {
            float x = show ? Mathf.Lerp(300, -10, animProgress) : Mathf.Lerp(-10, 300, animProgress);
            panelObjs.anchoredPosition = new Vector2(x, PANEL_Y);
            animProgress += Time.deltaTime / ANIM_DURATION;
            yield return null;
        }
        panelObjs_Animating = false;
        panelObjs_Visible = show;
    }
    
    public void SetPlayerHealthFill(float fill)
    {
        playerHealthFill.localScale = new Vector3(fill, 1, 1);
        float width = playerHealthFill.rect.width;
        playerHealthFill.anchoredPosition = new Vector2(width * fill / 2, 0);
    }

    public void FurnaceInteract()
    {
        if (this.furnace)
        {
            this.furnace.NextState();
        }
    }
}
