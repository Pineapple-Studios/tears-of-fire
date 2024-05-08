using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEventsTutorial : MonoBehaviour
{
    public static FMODEventsTutorial Instance;

    [Header("Attack")]
    [SerializeField] public EventReference attack;

    [Header("Footsteps")]
    [SerializeField] public EventReference footsteps;

    [Header("Jump Svart")]
    [SerializeField] public EventReference jumpSvart;
    [SerializeField] public EventReference fallSvart;


    [Header("Hit")]
    [SerializeField] public EventReference hitSvart;

    [Header("Jump Spider")]
    [SerializeField] public EventReference jumpSpider;
    [SerializeField] public EventReference fallSpider;

    [Header("Bat")]
    [SerializeField] public EventReference batShoot;

    [Header("BreakableWall")]
    [SerializeField] public EventReference breakableWall;
    [SerializeField] public EventReference lastHitBreakableWall;

    [Header("Magnetism")]
    [SerializeField] public EventReference magnetism;

    [Header("LifeItem")]
    [SerializeField] public EventReference catchLifeItem;
    [SerializeField] public EventReference denyLifeItem;

    [Header("Gotinha")]
    [SerializeField] public EventReference leak;

    [Header("Death")]
    [SerializeField] public EventReference death;

    [Header("Checkpoint")]
    [SerializeField] public EventReference checkpoint;

    [Header("Typing")]
    [SerializeField] public EventReference typing;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Mais de um FMODEvents");
        }

        Instance = this;
    }
}
