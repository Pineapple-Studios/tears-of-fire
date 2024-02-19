using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] public TextAsset inkFile;
    [SerializeField] public GameObject textBox;
    //[SerializeField] public GameObject customButton;
    //[SerializeField] public GameObject optionPanel;
    //[SerializeField] public bool isTalking = false;

    [SerializeField] string npcName;

    static Story story;
    TMP_Text nametag;
    TMP_Text message;
    List<string> tags;
    //static Choice choiceSelected;

    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkFile.text);
        nametag = textBox.transform.GetChild(0).GetComponent<TMP_Text>();
        message = textBox.transform.GetChild(1).GetComponent<TMP_Text>();
        tags = new List<string>();
        //choiceSelected = null;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Is there more to the story?
            if (story.canContinue)
            {
                nametag.text = npcName;
                AdvanceDialogue();

                //Are there any choices?
                if (story.currentChoices.Count != 0)
                {
                    //StartCoroutine(ShowChoices());
                }
            }
            else
            {
                FinishDialogue();
                textBox.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    // Finished the Story (Dialogue)
    private void FinishDialogue()
    {
        Debug.Log("End of Dialogue!");
    }

    // Advance through the story 
    void AdvanceDialogue()
    {
        string currentSentence = story.Continue();
        ParseTags();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    // Type out the sentence letter by letter and make character idle if they were talking
    IEnumerator TypeSentence(string sentence)
    {
        message.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            message.text += letter;
            yield return null;
        }
        //CharacterScript tempSpeaker = GameObject.FindObjectOfType<CharacterScript>();
        //if (tempSpeaker.isTalking)
        //{
        //    SetAnimation("idle");
        //}
        yield return null;
    }

    // Create then show the choices on the screen until one got selected
    //IEnumerator ShowChoices()
    //{
    //    Debug.Log("There are choices need to be made here!");
    //    List<Choice> _choices = story.currentChoices;

    //    for (int i = 0; i < _choices.Count; i++)
    //    {
    //        GameObject temp = Instantiate(customButton, optionPanel.transform);
    //        temp.transform.GetChild(0).GetComponent<Text>().text = _choices[i].text;
    //        temp.AddComponent<Selectable>();
    //        temp.GetComponent<Selectable>().element = _choices[i];
    //        temp.GetComponent<Button>().onClick.AddListener(() => { temp.GetComponent<Selectable>().Decide(); });
    //    }

    //    optionPanel.SetActive(true);

    //    yield return new WaitUntil(() => { return choiceSelected != null; });

    //    AdvanceFromDecision();
    //}

    // Tells the story which branch to go to
    //public static void SetDecision(object element)
    //{
    //    choiceSelected = (Choice)element;
    //    story.ChooseChoiceIndex(choiceSelected.index);
    //}

    // After a choice was made, turn off the panel and advance from that choice
    //void AdvanceFromDecision()
    //{
    //    optionPanel.SetActive(false);
    //    for (int i = 0; i < optionPanel.transform.childCount; i++)
    //    {
    //        Destroy(optionPanel.transform.GetChild(i).gameObject);
    //    }
    //    choiceSelected = null; // Forgot to reset the choiceSelected. Otherwise, it would select an option without player intervention.
    //    AdvanceDialogue();
    //}

    /*** Tag Parser ***/
    /// In Inky, you can use tags which can be used to cue stuff in a game.
    /// This is just one way of doing it. Not the only method on how to trigger events. 
    void ParseTags()
    {
        tags = story.currentTags;
        foreach (string t in tags)
        {
            string prefix = t.Split(' ')[0];
            string param = t.Split(' ')[1];

            switch (prefix.ToLower())
            {
                case "anim":
                    SetAnimation(param);
                    break;
                case "color":
                    SetTextColor(param);
                    break;
            }
        }
    }
    void SetAnimation(string _name)
    {
        CharacterScript cs = GameObject.FindObjectOfType<CharacterScript>();
        cs.PlayAnimation(_name);
    }
    void SetTextColor(string _color)
    {
        switch (_color)
        {
            case "red":
                message.color = Color.red;
                break;
            case "blue":
                message.color = Color.cyan;
                break;
            case "green":
                message.color = Color.green;
                break;
            case "white":
                message.color = Color.white;
                break;
            default:
                Debug.Log($"{_color} is not available as a text color");
                break;
        }
    }

}
