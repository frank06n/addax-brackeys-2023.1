using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class ShogunEffect : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float variance;

    private GameObject mainCam;
    private DigitalGlitch dg;
    private AnalogGlitch ag;

    [Header("Max Values")]
    [SerializeField] [Range(0f, 1f)]
    private float dig_intensity;
    [SerializeField]
    [Range(0f, 1f)]
    private float colorDrift;
    [SerializeField]
    [Range(0f, 1f)]
    private float horizontalShake;
    [SerializeField]
    [Range(0f, 1f)]
    private float scanLineJitter;
    [SerializeField]
    [Range(0f, 1f)]
    private float verticalJump;

    bool off;


    // Start is called before the first frame update
    void Awake()
    {
        startPos = transform.position;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        dg = mainCam.AddComponent<DigitalGlitch>();
        ag = mainCam.AddComponent<AnalogGlitch>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (off)
        {

        }
        
        transform.position = startPos + Vector3.right * variance * Random.Range(-1f, 1f);
        float dist = Vector2.Distance(startPos, LevelManager.instance.player.position);
        float p = 1f - Mathf.Clamp01(Mathf.Atan(dist / 5));

        if (off)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.clear;
            p = 0;
        }

        dg.intensity = p * dig_intensity;
        ag.colorDrift = p * colorDrift;
        ag.horizontalShake = p * horizontalShake;
        ag.scanLineJitter = p * scanLineJitter;
        ag.verticalJump = p * verticalJump;
    }

    public void turnOff()
    {
        off = true;
    }
}
