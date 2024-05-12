using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace cherrydev
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Nodes/Answer Node", fileName = "New Answer Node")]
    public class AnswerNode : Node
    {
        [SerializeField] private Answer answer;

        [SerializeField] private List<AnswerScriptableObject> answersData;

        private List<string> answers = new List<string>();

        public List<Node> parentSentenceNode;
        public List<SentenceNode> childSentenceNodes = new List<SentenceNode>();

        private const float lableFieldSpace = 15f;
        private const float lableSpriteFieldSpace = 40f;

        private const float textFieldWidth = 120f;
        private const float lableSpriteFieldWidth = 107f;
        private const float nodeNameFieldWith = 150f;

        private const float answerNodeWidth = 190f;
        private const float answerNodeHeight = 135f;

        private float currentAnswerNodeHeight = 135f;
        private float additionalAnswerNodeHeight = 20f;

        // Property accessors
        public List<string> Answers { get => answers; set => answers = value; }
        public List<AnswerScriptableObject> AnswersData { get => answersData; set => answersData = value; }

        public string GetSentenceCharacterName()
        {
            return answer.characterName;
        }

        public Sprite GetCharacterSprite()
        {
            return answer.characterSprite;
        }

#if UNITY_EDITOR

        public override void Initialise(Rect rect, string nodeName, DialogNodeGraph nodeGraph)
        {
            base.Initialise(rect, nodeName, nodeGraph);

            GenerateInitialAnswer();

            parentSentenceNode = new List<Node>();
            childSentenceNodes = new List<SentenceNode>();
        }

        void GenerateInitialAnswer()
        {
            answersData ??= new List<AnswerScriptableObject>();
            AnswersData.Add(null);

            Answers ??= new List<string>();
            Answers.Add(string.Empty);
        }

        public override void Draw(GUIStyle nodeStyle, GUIStyle lableStyle)
        {
            base.Draw(nodeStyle, lableStyle);

            childSentenceNodes.RemoveAll(item => item == null);

            rect.size = new Vector2(answerNodeWidth, currentAnswerNodeHeight);

            GUILayout.BeginArea(rect, nodeStyle);

            DrawNodeNameFieldHorizontal();
            GUILayout.Space(10);

            for (int i = 0; i < AnswersData.Count; ++i)
                DrawAnswerLine(i + 1, StringConstants.GreenDot);

            DrawAnswerNodeButtons();
            DrawCharacterSpriteHorizontal();
            GUILayout.EndArea();
        }

        private void DrawNodeNameFieldHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            NodeName = EditorGUILayout.TextField(NodeName, GUILayout.Width(nodeNameFieldWith));
            EditorGUILayout.EndHorizontal();
        }

        public override void DrawCharacterSpriteHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Sprite ", GUILayout.Width(lableSpriteFieldSpace));
            answer.characterSprite = (Sprite)EditorGUILayout.ObjectField(answer.characterSprite, typeof(Sprite), false, GUILayout.Width(lableSpriteFieldWidth));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAnswerLine(int answerNumber, string iconPathOrName)
        {
            if (answersData == null)
                return;

            if (Answers == null || Answers[answerNumber - 1] == null)
                return;

            if (Answers.Count > answersData.Count)
                DecreaseAmountOfAnswers();

            if (Answers.Count < answersData.Count)
                IncreaseAmountOfAnswers();
      
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{answerNumber}. ", GUILayout.Width(lableFieldSpace));

            if (answersData[answerNumber - 1] != null)
                Answers[answerNumber - 1] = EditorGUILayout.TextField(AnswersData[answerNumber - 1].dialogue, GUILayout.Width(textFieldWidth));

            else
                Answers[answerNumber - 1] = EditorGUILayout.TextField(string.Empty, GUILayout.Width(textFieldWidth));

            EditorGUILayout.LabelField(EditorGUIUtility.IconContent(iconPathOrName), GUILayout.Width(lableFieldSpace));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAnswerNodeButtons()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add"))
                AddAnswer();

            if (GUILayout.Button("Remove"))
                RemoveAnswer();

            EditorGUILayout.EndHorizontal();
        }

        private void IncreaseAmountOfAnswers()
        {
            Answers.Add(string.Empty);

            currentAnswerNodeHeight += additionalAnswerNodeHeight;
        }

        private void DecreaseAmountOfAnswers()
        {
            Answers.RemoveAt(AnswersData.Count - 1);

            if (childSentenceNodes.Count > AnswersData.Count)
            {
                childSentenceNodes[childSentenceNodes.Count - 1].parentNodes.Remove(this);
                childSentenceNodes.RemoveAt(childSentenceNodes.Count - 1);
            }

            currentAnswerNodeHeight -= additionalAnswerNodeHeight;
        }

        private void AddAnswer()
        {
            answersData ??= new List<AnswerScriptableObject>();

            AnswersData.Add(null);
        }

        private void RemoveAnswer()
        {
            if (AnswersData.Count == 1)
                return;

            AnswersData.RemoveAt(AnswersData.Count - 1);
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

            for (int i = 0; i < AnswersData.Count - 1; i++)
            {
                currentAnswerNodeHeight += additionalAnswerNodeHeight;
            }
        }

        private bool IsCanAddToChildConnectedNode(SentenceNode sentenceNodeToAdd)
        {
            if (sentenceNodeToAdd.parentNodes == null && sentenceNodeToAdd.childNode != this && childSentenceNodes.Count < answersData.Count)
                return true;

            if (sentenceNodeToAdd.parentNodes != null && !sentenceNodeToAdd.parentNodes.Contains(this) && sentenceNodeToAdd.childNode != this && childSentenceNodes.Count < answersData.Count)
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