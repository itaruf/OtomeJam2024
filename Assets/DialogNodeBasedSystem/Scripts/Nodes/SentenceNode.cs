using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace cherrydev
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Nodes/Sentence Node", fileName = "New Sentence Node")]
    public class SentenceNode : Node
    {
        [SerializeField] private Sentence sentence;

        [Space(10)]
        public List<Node> parentNodes;
        public Node childNode;

        private const float lableFieldSpace = 40f;
        private const float textFieldWidth = 107f;
        private const float externalNodeHeight = 155f;

        public string GetSentenceCharacterName()
        {
            return sentence.characterName;
        }

        public void SetSentenceText(string text)
        {
            sentence.text = text;
        }

        public string GetSentenceText()
        {
            return sentence.text;
        }

        public Sprite GetCharacterSprite()
        {
            return sentence.characterSprite;
        }

#if UNITY_EDITOR

        public override void Initialise(Rect rect, string nodeType, DialogNodeGraph nodeGraph)
        {
            base.Initialise(rect, nodeType, nodeGraph);
            SetSentenceText("N/A");
        }

        public override void Draw(GUIStyle nodeStyle, GUIStyle lableStyle)
        {
            base.Draw(nodeStyle, lableStyle);

            GUILayout.BeginArea(rect, nodeStyle);

            DrawNodeTypeHorizontal();
            DrawNodeTypeFieldHorizontal();

            GUILayout.Space(10);

            DrawCharacterNameFieldHorizontal();
            DrawSentenceTextFieldHorizontal();
            DrawCharacterSpriteHorizontal();

            GUILayout.EndArea();
        }

        private void DrawCharacterNameFieldHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Char ", GUILayout.Width(lableFieldSpace));
            sentence.characterName = EditorGUILayout.TextField(sentence.characterName, GUILayout.Width(textFieldWidth));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSentenceTextFieldHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Text ", GUILayout.Width(lableFieldSpace));
            sentence.text = EditorGUILayout.TextField(sentence.text, GUILayout.Width(textFieldWidth));
            EditorGUILayout.EndHorizontal();
        }

        public override void DrawCharacterSpriteHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Sprite ", GUILayout.Width(lableFieldSpace));
            sentence.characterSprite = (Sprite)EditorGUILayout.ObjectField(sentence.characterSprite, typeof(Sprite), false, GUILayout.Width(textFieldWidth));
            EditorGUILayout.EndHorizontal();
        }

        public void CheckNodeSize(float width, float height)
                {
                    rect.width = width;
            
                    if (standartHeight == 0)
                    {
                        standartHeight = height;
                    }

                    rect.height = standartHeight;
                }

        public override bool AddToChildConnectedNode(Node nodeToAdd)
        {
            SentenceNode sentenceNodeToAdd;

            if (nodeToAdd.GetType() == typeof(SentenceNode))
            {
                nodeToAdd = (SentenceNode)nodeToAdd;

                if (nodeToAdd == this)
                {
                    return false;
                }
            }

            if (nodeToAdd.GetType() == typeof(SentenceNode))
            {
                sentenceNodeToAdd = (SentenceNode)nodeToAdd;

                if (sentenceNodeToAdd != null && sentenceNodeToAdd.childNode == this)
                {
                    return false;
                }
            }

            childNode = nodeToAdd;
            return true;
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

            if (parentNodes == null)
                parentNodes = new List<Node>();

            parentNodes.Add(nodeToAdd);

            if (nodeToAdd.GetType() == typeof(SentenceNode))
            {
                sentenceNodeToAdd = (SentenceNode)nodeToAdd;

                if (sentenceNodeToAdd.childNode == this)
                {
                    return true;
                }
                else
                {
                    parentNodes.Remove(nodeToAdd);
                }
            }

            return true;
        }

        public override void RemoveNodeFromParents(Node nodeToRemove)
        {
            if (parentNodes == null)
                return;

            foreach (Node n in parentNodes)
            {
                if (n == nodeToRemove)
                    parentNodes.Remove(n);
            }
        }
#endif
    }
}