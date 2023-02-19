using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform bulletsHolder;
    public Transform pickupsHolder;
    [SerializeField] private LayerMask raycastLayers;
    private readonly int[] raycastMinDepths = { 0, -5 };
    private readonly int[] raycastMaxDepths = { 5,  0 };

    public Transform player;
    [SerializeField] private FurnaceLogic furnace;
    [HideInInspector] public int LAYER_VULNERABLE;

    public AudioManager audioPlayer;



    [HideInInspector] public bool reverse;



    [Header("UI Elements")]

    public ObjectivesHandler objsHandler;
    private HintHandler hintHandler;
    public TimerHandler timerHandler;
    public AmmoDisplayHandler ammoDisplay;

    [SerializeField] private RectTransform playerHealthFill;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);

        instance = this;

        objsHandler = FindObjectOfType<ObjectivesHandler>();
        hintHandler = FindObjectOfType<HintHandler>();
        timerHandler = FindObjectOfType<TimerHandler>();
        ammoDisplay = FindObjectOfType<AmmoDisplayHandler>();

        LAYER_VULNERABLE = LayerMask.NameToLayer("Vulnerable");
    }

    public void ToggleObjectivesPanel()
    {
        objsHandler.Toggle();
    }

    public void ShowHint(string text, float duration=3f)
    {
        hintHandler.ShowHint(text, duration);
    }

    public void SetAmmoGun(GunLogic gun)
    {
        ammoDisplay.SetGun(gun);
    }

    public void ShowAmmo(bool show)
    {
        ammoDisplay.gameObject.SetActive(show);
    }

    public RaycastHit2D UnFireRaycast(Vector2 origin, Vector2 direction, string shooterTag)
    {
        int i = shooterTag == "Player" ? 0 : 1;
        return Physics2D.Raycast(origin, direction, 50,
            raycastLayers, raycastMinDepths[i], raycastMaxDepths[i]);
    }
    
    public void SetPlayerHealthFill(float fill)
    {
        playerHealthFill.localScale = new Vector3(fill, 1, 1);
        float width = playerHealthFill.rect.width;
        playerHealthFill.anchoredPosition = new Vector2(width * fill / 2, 0);
    }

    public FurnaceState GetFurnaceState()
    {
        return furnace.GetState();
    }

    public void FurnaceInteract()
    {
        if (!furnace) return;
        if (reverse)
            furnace.PrevState();
        else
            furnace.NextState();
    }

    public bool IsReverse()
    {
        return reverse;
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
