using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


public class Options : MonoBehaviour
{
    public Canvas canvas;
    public TMP_Text tmptxt;
    public List<Button> button;
    private List<TMP_Text> butontext;
    public GameObject coob;
    

    private Button clickedButton;

    private void OnButtonClick(Button button)
    {
        clickedButton = button;
        // This will print the text of the button that was clicked
        Debug.Log(clickedButton.GetComponentInChildren<TMP_Text>().text);
    }


    // Start is called before the first frame update
    void Start()
    {
        foreach (Button button in button)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
        butontext = new List<TMP_Text>();

        for (int i = 0; i < button.Count; i++)
        {
            Button buttonInstance = button[i];
            if (buttonInstance == null)
            {
                Debug.LogError("Button at index " + i + " is null!");
                continue;
            }

            TMP_Text textComponent = buttonInstance.GetComponentInChildren<TMP_Text>();
            if (textComponent == null)
            {
                Debug.LogError("TMP_Text component is null for button at index " + i);
                continue;
            }

            butontext.Add(textComponent);
        }
        KillButtons();
        Scenario1();
        
    }

    // Update is called once per frame
    public void Scenario1()
    {
        KillButtons();
        tmptxt.text = "Could  you help me get my material";
        button[0].gameObject.SetActive(true);
        button[1].gameObject.SetActive(true);
        butontext[0].text = "Okay, Sir";
        butontext[1].text = "NO.";
        
    }

    public void Scenario2()
    {
        KillButtons();
        tmptxt.text = "Turn a left and then turn 2 rights and there you can find me my material";
        button[2].gameObject.SetActive(true);
        button[3].gameObject.SetActive(true);
        button[4].gameObject.SetActive(true);
        butontext[2].text = "Sure professor";
        butontext[3].text = "Okay Sir";
        butontext[4].text = "NOO.";

    }
    public void Scenario3()
    {
        KillButtons();
        tmptxt.text = "Get me the material plase, I beg of you";
        button[5].gameObject.SetActive(true);
        button[6].gameObject.SetActive(true);
        butontext[5].text = "Okay, sure";
        butontext[6].text = "NOOO.";
    }
    public void FinishD()
    {
        KillButtons();
        tmptxt.text = "Thank you very very much";
        button[7].gameObject.SetActive(true);
        butontext[7].text = "continue";
        
    }

    public void DelteCanvas()
    {
        
        canvas.gameObject.SetActive(false); 
    }

    public void KillButtons()
    {
        for (int j = 0; j < 8; j++){
            button[j].gameObject.SetActive(false);
        }
    }

    public void DelteCube()
    {
       Destroy(coob);
    }
    

    void Update()
    {
        
    }
}
