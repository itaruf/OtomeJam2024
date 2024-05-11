using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace cherrydev
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Nodes/Answer Node", fileName = "New Answer Node")]
    public class AnswerNode : Node
    {
        public string characterName = "MC";
        public Sprite characterSprite;

        [SerializeField] public List<AnswerScriptableObject> answersData;

        private List<string> answers = new List<string>();

        public List<Node> parentSentenceNode;
        public List<SentenceNode> childSentenceNodes = new List<SentenceNode>();

        private const float lableFieldSpace = 18f;
        private const float textFieldWidth = 120f;

        private const float answerNodeWidth = 190f;
        private const float answerNodeHeight = 115f;

        private float currentAnswerNodeHeight = 115f;
        private float additionalAnswerNodeHeight = 20f;

        // Property accessors
        public List<string> Answers { get => answers; set => answers = value; }

#if UNITY_EDITOR

        public override void Initialise(Rect rect, string nodeName, DialogNodeGraph nodeGraph)
        {
            base.Initialise(rect, nodeName, nodeGraph);

            GenerateAnswerDataOnInitialization();

            parentSentenceNode = new List<Node>();
            childSentenceNodes = new List<SentenceNode>(answersData.Count);
        }

        public override void Draw(GUIStyle nodeStyle, GUIStyle lableStyle)
        {
            base.Draw(nodeStyle, lableStyle);

            childSentenceNodes.RemoveAll(item => item == null);

            rect.size = new Vector2(answerNodeWidth, currentAnswerNodeHeight);

            GUILayout.BeginArea(rect, nodeStyle);
            EditorGUILayout.LabelField("MC Answer Node", lableStyle);

            GenerateAnswerDataOnInitialization();

            for (int i = 0; i < answersData.Count; ++i)
            {
                DrawAnswerLine(i + 1, StringConstants.GreenDot);
            }

            DrawAnswerNodeButtons();

            GUILayout.EndArea();
        }

        public void GenerateAnswerDataOnInitialization()
        {
            if  (answersData == null)
            {      
                answersData = new List<AnswerScriptableObject>();

                var dialogue = new AnswerScriptableObject();
                dialogue.dialogue = "";

                answersData.Add(dialogue);
            }
        }

        public void GenerateAnswers()
        {
            Answers.Capacity = answersData.Count;

            for (int i = 0; i < answersData.Count; ++i)
            {
                Answers[i] = answersData[i].dialogue;
            }
        }

        private void DrawAnswerLine(int answerNumber, string iconPathOrName)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField($"{answerNumber}. ", 
                GUILayout.Width(lableFieldSpace));

            if (Answers.Count > answersData.Count)
            {
                DecreaseAmountOfAnswers();
            }

            if (Answers.Count < answersData.Count)
            {
                IncreaseAmountOfAnswers();
            }

            if (answersData[answerNumber - 1] == null)
            {
                Answers[answerNumber - 1] = "";
            }

            else
            {
                Answers[answerNumber - 1] = EditorGUILayout.TextField(answersData[answerNumber - 1].dialogue,
                    GUILayout.Width(textFieldWidth));
            }

            EditorGUILayout.LabelField(EditorGUIUtility.IconContent(iconPathOrName),
                GUILayout.Width(lableFieldSpace));

            EditorGUILayout.EndHorizontal();
        }

        private void DrawAnswerNodeButtons()
        {
            if (GUILayout.Button("Add answer"))
            {
                AddAnswer();
            }

            if (GUILayout.Button("Remove answer"))
            {
                RemoveAnswer();
            }
        }

        private void IncreaseAmountOfAnswers()
        {
            Answers.Add(string.Empty);

            currentAnswerNodeHeight += additionalAnswerNodeHeight;
        }

        private void DecreaseAmountOfAnswers()
        {
            /*answers.RemoveRange(answersData.Count - 1, answersData.Count - 1);*/

            Answers.RemoveAt(answersData.Count - 1);

            if (childSentenceNodes.Count == answersData.Count)
            {
                childSentenceNodes[answersData.Count - 1].parentNodes = null;
                childSentenceNodes.RemoveAt(answersData.Count - 1);
            }

            currentAnswerNodeHeight -= additionalAnswerNodeHeight;
        }

        private void AddAnswer()
        {
            var dialogue = new AnswerScriptableObject();
            dialogue.dialogue = "";

            answersData.Add(dialogue);
        }

        private void RemoveAnswer()
        {
            if (answersData.Count == 1)
                return;

            answersData.RemoveAt(answersData.Count - 1);
        }

        public override bool AddToParentConnectedNode(Node nodeToAdd)
        {
            SentenceNode sentenceNodeToAdd;

            if (nodeToAdd.GetType() == typeof(AnswerNode))
            {
                return false;
            }

            if (nodeToAdd.GetType() == typeof(SentenceNode))
            {
                nodeToAdd = (SentenceNode)nodeToAdd;

                if (nodeToAdd == this)
                {
                    return false;
                }
            }

            parentSentenceNode.Add(nodeToAdd);

            if (nodeToAdd.GetType() == typeof(SentenceNode))
            {
                sentenceNodeToAdd = (SentenceNode)nodeToAdd;

                if (sentenceNodeToAdd.childNode == this)
                {
                    return true;
                }
                else
                {
                    parentSentenceNode.Remove(nodeToAdd);
                }
            }

            return true;
        }

        public override bool AddToChildConnectedNode(Node nodeToAdd)
        {
            SentenceNode sentenceNodeToAdd;

            if (nodeToAdd.GetType() != typeof(AnswerNode))
            {
                sentenceNodeToAdd = (SentenceNode)nodeToAdd;
            }
            else
            {
                return false;
            }

            if (IsCanAddToChildConnectedNode(sentenceNodeToAdd))
            {
                childSentenceNodes.Add(sentenceNodeToAdd);

                if (sentenceNodeToAdd.parentNodes == null)
                    sentenceNodeToAdd.parentNodes = new List<Node>();

                sentenceNodeToAdd.parentNodes.Add(this);

                return true;
            }

            return false;
        }

        public void CalculateAnswerNodeHeight()
        {
            currentAnswerNodeHeight = answerNodeHeight;

            for (int i = 0; i < answersData.Count - 1; i++)
            {
                currentAnswerNodeHeight += additionalAnswerNodeHeight;
            }
        }

        private bool IsCanAddToChildConnectedNode(SentenceNode sentenceNodeToAdd)
        {
            if (sentenceNodeToAdd.parentNodes == null && sentenceNodeToAdd.childNode != this /*&& childSentenceNodes.Count >= answersData.Count*/)
                return true;

            if (!sentenceNodeToAdd.parentNodes.Contains(this) && sentenceNodeToAdd.childNode != this/* && childSentenceNodes.Count >= answersData.Count*/)
                return true;

            return false;
        }

        public override void RemoveNodeFromParents(Node nodeToRemove)
        {
            if (parentSentenceNode == null)
                return;

            foreach (SentenceNode sn in parentSentenceNode)
            {
                if (sn.childNode == nodeToRemove)
                    sn.childNode = null;
            }
        }

        public override void RemoveNodeFromChilds(Node nodeToRemove)
        {
            if (childSentenceNodes == null)
                return;

            foreach (SentenceNode sn in childSentenceNodes)
            {
                if (sn.parentNodes != null && sn.parentNodes.Contains(nodeToRemove))
                    sn.parentNodes.Remove(nodeToRemove);
            }
        }
#endif
    }
}