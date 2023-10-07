using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SavePrefs : MonoBehaviour
{
    [SerializeField] Slider sldGen;
    [SerializeField] Slider sldMsc;
    [SerializeField] Slider sldFX;

    //[SerializeField] TMP_Dropdown dpdResolution;

    
    // Start is called before the first frame update
    void Start()
    {
        LoadValue();
    }


    public void SaveValue()
    {
        // convertendo e setando o valor do slider geral
        float sldGenValue = sldGen.value;
        PlayerPrefs.SetFloat("GeneralSlider", sldGenValue);

        // convertendo o valor do slider music
        float sldMscValue = sldMsc.value;
        PlayerPrefs.SetFloat("MusicSlider", sldMscValue);

        // convertendo o valor do slider fx
        float sldFXValue = sldFX.value;
        PlayerPrefs.SetFloat("FXSlider", sldFXValue);

        LoadValue();

        Debug.Log("Valores Salvos");
    }
    
    public void LoadValue()
    {
        // pegando o valor pela chave
        float sldGenValue = PlayerPrefs.GetFloat("GeneralSlider");
        sldGen.value = sldGenValue;

        float sldMscValue = PlayerPrefs.GetFloat("MusicSlider");
        sldMsc.value = sldMscValue;

        float sldFXValue = PlayerPrefs.GetFloat("FXSlider");
        sldFX.value = sldFXValue;

        //AudioListener.volume = sldGenValue;
    }
}
