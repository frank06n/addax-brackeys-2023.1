using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FurnaceState
{
    BROKEN, REPAIRING, REPAIRED, WORKING, WORKED
}

public class FurnaceLogic : MonoBehaviour
{
    
    [SerializeField] private Sprite[] sprites;

    [Header("State Settings")]
    [SerializeField] private SpriteRenderer barFill;
    [SerializeField] private Color repairingBarColor;
    [SerializeField] private float repairingTime;
    [SerializeField] private Color workingBarColor;
    [SerializeField] private float workingTime;

    private SpriteRenderer sr;
    private HealthBarScript bar;
    private FurnaceState state;

    // Start is called before the first frame update
    void Awake()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        bar = GetComponentInChildren<HealthBarScript>();
        state = FurnaceState.BROKEN;
    }

    private void Start()
    {
        bar.visibility = HealthBarVisibility.HIDDEN;
    }

    public FurnaceState GetState()
    {
        return state;
    }

    public bool NextState()
    {
        if (state == FurnaceState.WORKING || state == FurnaceState.REPAIRING)
            return false;
        StartCoroutine(NextStateAnim());
        return true;
    }
    public bool PrevState()
    {
        if (state == FurnaceState.WORKING || state == FurnaceState.REPAIRING)
            return false;
        StartCoroutine(PrevStateAnim());
        return true;
    }

    private IEnumerator NextStateAnim()
    {
        float _time;
        int _spr_process, _spr_done;
        FurnaceState _state_process, _state_done;

        if (state == FurnaceState.BROKEN)
        {
            barFill.color = repairingBarColor;
            _time = repairingTime;
            _spr_process = 0;
            _spr_done = 1;
            _state_process = FurnaceState.REPAIRING;
            _state_done = FurnaceState.REPAIRED;
        }
        else if (state == FurnaceState.REPAIRED)
        {
            barFill.color = workingBarColor;
            _time = workingTime;
            _spr_process = 2;
            _spr_done = 1;
            _state_process = FurnaceState.WORKING;
            _state_done = FurnaceState.WORKED;
        }
        else
        {
            throw new System.InvalidOperationException();
        }

        sr.sprite = sprites[_spr_process];
        state = _state_process;
        bar.visibility = HealthBarVisibility.VISIBLE;

        float delta = 0;
        while (delta <= 1)
        {
            delta += Time.deltaTime / _time;
            bar.UpdateValue(delta);
            yield return null;
        }

        sr.sprite = sprites[_spr_done];
        state = _state_done;
        bar.visibility = HealthBarVisibility.HIDDEN;
        LevelManager.instance.player.GetComponent<PlayerMovement>().OnFurnaceProcessComplete();
    }

    private IEnumerator PrevStateAnim()
    {
        float _time;
        int _spr_process, _spr_done;
        FurnaceState _state_process, _state_done;

        if (state == FurnaceState.REPAIRED)
        {
            barFill.color = repairingBarColor;
            _time = repairingTime;
            _spr_process = 1;
            _spr_done = 0;
            _state_process = FurnaceState.REPAIRING;
            _state_done = FurnaceState.BROKEN;
        }
        else if (state == FurnaceState.WORKED)
        {
            barFill.color = workingBarColor;
            _time = workingTime;
            _spr_process = 2;
            _spr_done = 1;
            _state_process = FurnaceState.WORKING;
            _state_done = FurnaceState.REPAIRED;
        }
        else
        {
            throw new System.InvalidOperationException();
        }

        sr.sprite = sprites[_spr_process];
        state = _state_process;
        bar.visibility = HealthBarVisibility.VISIBLE;

        float delta = 0;
        while (delta <= 1)
        {
            delta += Time.deltaTime / _time;
            bar.UpdateValue(1f - delta);
            yield return null;
        }

        sr.sprite = sprites[_spr_done];
        state = _state_done;
        bar.visibility = HealthBarVisibility.HIDDEN;
        LevelManager.instance.player.GetComponent<PlayerMovement>().OnFurnaceProcessComplete();
    }

}
