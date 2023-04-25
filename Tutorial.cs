using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class Tutorial : MonoBehaviour
{
    [HideInInspector] public bool ignore = false;
    [HideInInspector] public bool onDemand = false;

    [HideInInspector] public Button skipButton;
    [HideInInspector] public GameObject bigPopup;
    [HideInInspector] public RectTransform popup;
    [HideInInspector] public RectTransform popupArrow;
    [HideInInspector] public TextMeshProUGUI bigPopUpText;
    [HideInInspector] public TextMeshProUGUI popUpText;

    public int currentStep;

    public TutorialStep skipTutorialSettings;

    public GameObject[] harvestable;

    public List<TutorialStep> steps;

    [Header("Edit Current Tutorial Step")]
    [TextArea] public string textEditor;

    private bool isActive = false;

    [System.Serializable]
    public class TutorialStep
    {
        [TextArea] public string content;

        [HideInInspector] public Vector3 position;
        [HideInInspector] public Vector3 arrowPosition;

        [HideInInspector] public Vector2 anchoredPosition;
        [HideInInspector] public Vector2 anchorMin;
        [HideInInspector] public Vector2 anchorMax;
        [HideInInspector] public Vector2 pivot;

        public GameObject[] enable;
        public GameObject[] disable;

        public Button[] activeButtons;
        public Button[] inactiveButtons;
        public TutorialCondition[] tutorialConditions;

        public bool clickToContinue = true;

        public int genericBonus = 0;

        public bool replacePhraseOnDemand;
        public bool appendPhraseOnDemand;
        public string phraseToReplace;
        public bool skipOnDemand = false;
        public bool onDemandClickSkip = false;

        [Header("Arcade Mode:")]
        public bool arcadeEnd = false;
        public bool useArcadeContent = false;

        [TextArea] public string arcadeContent;
    }

    [System.Serializable]
    public class JSONRoot
    {
        public List<string> items;
    }

    [System.Serializable]
    public class TutorialCondition
    {
        public enum Type { None, UnitA, UnitB, UnitC, UnitD, UnitE, UnitF, UnitG, UnitH, UnitI, UnitJ, UnitK, UnitL };

        [Header("IF()")]
        public Type type;
        [Header("IS EQUAL TO")]
        public int target;
        [Header("IS()")]
        public GameObject targetObject;
        [Header("Active State:")]
        public bool state;

        public bool Evaluate()
        {
            if (targetObject != null)
            {
                if (targetObject.activeInHierarchy == state) return true;
            }
            // Add conditions here to evaluate each type of unit
            // Example:
            if (type == Type.UnitA)
            {
                if (Game.instance.playerOPCO[0].accessUnits.amount == target) return true;
            }
            // Add more conditions for other unit types

            return false;
        }
    }

    void OnEnable()
    {
        Game.instance.ui.tutorialButton.GetComponent<Button>().interactable = false;
        Game.instance.tutorial = true;
    }

    void OnDisable()
    {
        Game.instance.ui.tutorialButton.GetComponent<Button>().interactable = true;
        onDemand = false;
    }

    public void Export()
    {
        JSONRoot root = new JSONRoot();
        root.items = new List<string>();

        foreach (TutorialStep step in steps)
        {
            root.items.Add(step.content);
        }

        string json = JsonUtility.ToJson(root);
        string file = Application.persistentDataPath + "/json.txt";
        File.WriteAllText(file, json);
    }

       public void Import()
    {
        string file = Application.persistentDataPath + "/json.txt";
        if (File.Exists(file))
        {
            string json = File.ReadAllText(file);
            JSONRoot root = JsonUtility.FromJson<JSONRoot>(json);

            for (int i = 0; i < root.items.Count; i++)
            {
                if (i < steps.Count)
                {
                    steps[i].content = root.items[i];
                }
            }
        }
    }

    void Start()
    {
        // Initialize components and set initial values
        bigPopup = Game.instance.ui.bigPopup;
        popup = bigPopup.GetComponent<RectTransform>();
        popupArrow = Game.instance.ui.bigPopupArrow.GetComponent<RectTransform>();
        bigPopUpText = Game.instance.ui.bigPopUpText;
        popUpText = Game.instance.ui.popUpText;

        skipButton = Game.instance.ui.skipButton.GetComponent<Button>();
        skipButton.onClick.AddListener(DelegateSkipButton);

        Game.instance.ui.tutorialButton.GetComponent<Button>().onClick.AddListener(OnTutorialButtonClicked);
        if (onDemand) Game.instance.ui.tutorialButton.SetActive(true);

        for (int i = 0; i < steps.Count; i++)
        {
            steps[i].anchoredPosition = popup.anchoredPosition;
            steps[i].anchorMin = popup.anchorMin;
            steps[i].anchorMax = popup.anchorMax;
            steps[i].pivot = popup.pivot;
        }

        if (!onDemand)
        {
            NextStep();
        }
        else
        {
            isActive = false;
            gameObject.SetActive(false);
        }
    }

    public void OnTutorialButtonClicked()
    {
        gameObject.SetActive(true);
        NextStep();
    }

    public void DelegateSkipButton()
    {
        if (!onDemand) StartCoroutine(SkipTutorial());
    }

    public void NextStep()
    {
        if (isActive) return;
        
        isActive = true;

        if (currentStep < steps.Count)
        {
            StartCoroutine(ExecuteStep(steps[currentStep]));
        }
        else
        {
            EndTutorial();
        }
    }

    IEnumerator ExecuteStep(TutorialStep step)
    {
        // Check for tutorial conditions
        foreach (TutorialCondition condition in step.tutorialConditions)
        {
            if (!condition.Evaluate())
            {
                isActive = false;
                yield break;
            }
        }

        // Enable and disable game objects as necessary
        foreach (GameObject obj in step.enable)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in step.disable)
        {
            obj.SetActive(false);
        }

        // Set active and inactive buttons
        foreach (Button button in step.activeButtons)
        {
            button.interactable = true;
        }
        foreach (Button button in step.inactiveButtons)
        {
            button.interactable = false;
        }

        // Update text and popup properties
        bigPopUpText.text = step.content;
        popup.anchoredPosition = step.anchoredPosition;
        popup.anchorMin = step.anchorMin;
        popup.anchorMax = step.anchorMax;
        popup.pivot = step.pivot;

        // Update arrow position
        popupArrow.anchoredPosition = step.arrowPosition;

        if (step.clickToContinue)
        {
            yield return new WaitForSeconds(0.5f);
            
            while (!Input.GetMouseButtonUp(0))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }

        // Move to the next step
        currentStep++;
        isActive = false;
        NextStep();
    }

    IEnumerator SkipTutorial()
    {
        // Perform any additional actions needed for skipping the tutorial.
        // Your custom code for skipping the tutorial can be added here.
        yield return new WaitForSeconds(1f);

        EndTutorial();
    }

  public void EndTutorial()
  {
        // Clean up and deactivate the tutorial
        for (int i = 0; i < steps.Count; i++)
        {
            foreach (GameObject obj in steps[i].enable)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in steps[i].disable)
            {
                obj.SetActive(true);
            }

            foreach (Button button in steps[i].activeButtons)
            {
                button.interactable = true;
            }
            foreach (Button button in steps[i].inactiveButtons)
            {
                button.interactable = true;
            }
        }

        gameObject.SetActive(false);
        
        if (onDemand) Game.instance.ui.tutorialButton.SetActive(false);

        // Invoke the OnTutorialEnded event if there are any listeners
        if (OnTutorialEnded != null)
        {
            OnTutorialEnded.Invoke();
        }
    }
}
