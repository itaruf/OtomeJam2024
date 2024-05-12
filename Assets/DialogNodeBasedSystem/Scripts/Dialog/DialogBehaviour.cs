using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace cherrydev
{
    public class DialogBehaviour : MonoBehaviour
    {
        [SerializeField] private float dialogCharDelay;
        [SerializeField] private List<KeyCode> nextSentenceKeyCodes;
        [SerializeField] private bool isCanSkippingText = true;

        [Space(10)]
        [SerializeField] private UnityEvent onDialogStarted;
        [SerializeField] private UnityEvent onDialogFinished;

        private DialogNodeGraph currentNodeGraph;
        private Node currentNode;

        private int maxAmountOfAnswerButtons;

        private bool isDialogStarted;
        private bool isCurrentSentenceSkipped;

        public bool IsCanSkippingText
        {   get
            {
                return isCanSkippingText;
            }
            set
            {
                isCanSkippingText = value;
            }
        }

        public event Action OnSentenceNodeActive;
        public event Action<string, string, Sprite> OnSentenceNodeActiveWithParameter;

        public event Action OnAnswerNodeActive;
        public event Action<int, AnswerNode> OnAnswerButtonSetUp;
        public event Action<int> OnMaxAmountOfAnswerButtonsCalculated;
        public event Action<int> OnAnswerNodeActiveWithParameter;
        public event Action<int, string> OnAnswerNodeSetUp;

        public event Action OnDialogTextCharWrote;
        public event Action<string> OnDialogTextSkipped;

        public DialogExternalFunctionsHandler ExternalFunctionsHandler { get; private set; }

        private void Awake()
        {
            ExternalFunctionsHandler = new DialogExternalFunctionsHandler();
        }

        private void Update()
        {
            HandleSentenceSkipping();
        }

        public void SetCharDelay(float value)
        {
            dialogCharDelay = value;
        }

        public void SetNextSentenceKeyCodes(List<KeyCode> keyCodes)
        {
            nextSentenceKeyCodes = keyCodes;
        }

        public void StartDialog(DialogNodeGraph dialogNodeGraph)
        {
            isDialogStarted = true;

            if (dialogNodeGraph.nodesList == null)
            {
                Debug.LogWarning("Dialog Graph's node list is empty");
                return;
            }

            onDialogStarted?.Invoke();

            currentNodeGraph = dialogNodeGraph;

            DefineFirstNode(dialogNodeGraph);
            CalculateMaxAmountOfAnswerButtons();
            HandleDialogGraphCurrentNode(currentNode);
        }

        public void BindExternalFunction(string funcName, Action function)
        {
            ExternalFunctionsHandler.BindExternalFunction(funcName, function);
        }

        public void AddListenerToDialogFinishedEvent(UnityAction action)
        {
            onDialogFinished.AddListener(action);
        }

        public void SetCurrentNodeAndHandleDialogGraph(Node node)
        {
            currentNode = node;
            HandleDialogGraphCurrentNode(this.currentNode);
        }

        private void HandleDialogGraphCurrentNode(Node currentNode)
        {
            StopAllCoroutines();

            if (currentNode.GetType() == typeof(SentenceNode))
            {
                HandleSentenceNode(currentNode);
            }
            else if (currentNode.GetType() == typeof(AnswerNode))
            {
                HandleAnswerNode(currentNode);
            }
        }

        private void HandleSentenceNode(Node currentNode)
        {
            SentenceNode sentenceNode = (SentenceNode)currentNode;

            isCurrentSentenceSkipped = false;

            foreach (var evts in sentenceNode.EDI_OnNodeActivated)
                evts.TriggerEvent(null);

            OnSentenceNodeActive?.Invoke();

            OnSentenceNodeActiveWithParameter?.Invoke(sentenceNode.GetSentenceCharacterName(), sentenceNode.GetSentenceText(),
                sentenceNode.GetCharacterSprite());

            WriteDialogText(sentenceNode.GetSentenceText());
        }

        private void HandleAnswerNode(Node currentNode)
        {
            AnswerNode answerNode = (AnswerNode)currentNode;

            int amountOfActiveButtons = 0;

            OnAnswerNodeActive?.Invoke();

            foreach (var evts in answerNode.EDI_OnNodeActivated)
                evts.TriggerEvent(null);
            
            foreach (var answer in answerNode.AnswersData) 
                foreach (var e in answer.events)
                    e.TriggerEvent(null);

            for (int i = 0; i < answerNode.childSentenceNodes.Count; ++i)
            {
                if (answerNode.childSentenceNodes[i] != null && answerNode.AnswersData[i] != null)
                {
                    OnAnswerNodeSetUp?.Invoke(i, answerNode.AnswersData[i].dialogue);
                    OnAnswerButtonSetUp?.Invoke(i, answerNode);

                    amountOfActiveButtons++;
                }
                else
                {
                    break;
                }
            }

            if (amountOfActiveButtons == 0)
            {
                isDialogStarted = false;

                onDialogFinished?.Invoke();
                return;
            }

            OnAnswerNodeActiveWithParameter?.Invoke(amountOfActiveButtons);
        }

        private void DefineFirstNode(DialogNodeGraph dialogNodeGraph)
        {
            if (dialogNodeGraph.nodesList.Count == 0)
            {
                Debug.LogWarning("The list of nodes in the DialogNodeGraph is empty");
                return;
            }

            currentNode = dialogNodeGraph.nodesList[0];
        }

        private void WriteDialogText(string text)
        {
            StartCoroutine(WriteDialogTextRoutine(text));
        }

        private IEnumerator WriteDialogTextRoutine(string text)
        {
            foreach (char textChar in text)
            {
                if (isCurrentSentenceSkipped)
                {
                    OnDialogTextSkipped?.Invoke(text);
                    break;
                }

                OnDialogTextCharWrote?.Invoke();

                yield return new WaitForSeconds(dialogCharDelay);
            }

            yield return new WaitUntil(CheckNextSentenceKeyCodes);

            CheckForDialogNextNode();
        }

        private void CheckForDialogNextNode()
        {
            foreach (var events in currentNode.EDI_OnNodeDeactivated)
                events.TriggerEvent(null);

            if (currentNode.GetType() == typeof(SentenceNode))
            {
                SentenceNode sentenceNode = (SentenceNode)currentNode;

                if (sentenceNode.childNode != null)
                {
                    currentNode = sentenceNode.childNode;
                    HandleDialogGraphCurrentNode(currentNode);
                }
                else
                {
                    isDialogStarted = false;

                    onDialogFinished?.Invoke();
                }
            }
        }

        private void CalculateMaxAmountOfAnswerButtons()
        {
            foreach (Node node in currentNodeGraph.nodesList)
            {
                if (node.GetType() == typeof(AnswerNode))
                {
                    AnswerNode answerNode = (AnswerNode)node;

                    /*if (answerNode.Answers.Count > maxAmountOfAnswerButtons)
                    {*/
                        maxAmountOfAnswerButtons = answerNode.Answers.Count;
                    /*}*/
                }
            }

            OnMaxAmountOfAnswerButtonsCalculated?.Invoke(maxAmountOfAnswerButtons);
        }

        private void HandleSentenceSkipping()
        {
            if (!isDialogStarted || !isCanSkippingText)
            {
                return;
            }

            if (CheckNextSentenceKeyCodes() && !isCurrentSentenceSkipped)
            {
                isCurrentSentenceSkipped = true;
            }
        }

        private bool CheckNextSentenceKeyCodes()
        {
            for (int i = 0; i < nextSentenceKeyCodes.Count; ++i)
            { 
                if (Input.GetKeyDown(nextSentenceKeyCodes[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}