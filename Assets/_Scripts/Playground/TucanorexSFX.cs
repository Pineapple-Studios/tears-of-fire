using FMOD.Studio;
using UnityEngine;

public class TucanorexSFX : MonoBehaviour
{
    EventInstance _bossMusic;

    private void OnEnable()
    {
        PlayerProps.onPlayerDead += StopMusic;
        TucanoRexProps.onTucanoRexDead += OnBossDead;
    }

    private void OnDisable()
    {
        PlayerProps.onPlayerDead -= StopMusic;
        TucanoRexProps.onTucanoRexDead -= OnBossDead;
    }

    private void StopMusic(GameObject obj)
    {
        _bossMusic.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void StartMusic()
    {
        _bossMusic = FMODAudioManager.Instance.PlayOneShotWithParameters("event:/Tutorial/Music_Tutorial/OST_Boss-Cave", transform.position);
    }

    public void IntroductionScreaming()
    {
        FMODAudioManager.Instance.PlayOneShot("event:/Tutorial/Boss/SFX_StartBattle", transform.position);
    }

    public void AttackT1()
    {
        FMODAudioManager.Instance.PlayOneShot("event:/Tutorial/Boss/SFX_atk-Fase 1", transform.position);
    }

    public void AttackT2()
    {
        FMODAudioManager.Instance.PlayOneShot("event:/Tutorial/Boss/SFX_atk-Fase 2", transform.position);
    }

    private void OnBossDead(GameObject obj)
    {
        _bossMusic.setParameterByName("Stop", 1);
    }
}