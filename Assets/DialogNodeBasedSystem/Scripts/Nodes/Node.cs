using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace cherrydev
{
    public class Node : ScriptableObject
    {
        [HideInInspector] public List<Node> connectedNodesList;
        [HideInInspector] public DialogNodeGraph nodeGraph;
        [HideInInspector] public Rect rect = new Rect();

        [HideInInspector] public bool isDragging;
        [HideInInspector] public bool isSelected;

        private string nodeType = "Node Type";
        [SerializeField] private string nodeName = "Node Name";
        protected float standartHeight;

        private const float nodeTypeFieldWith = 150f;

        // Event Dispatchers
        [SerializeField] private List<CustomEventDispatcher<UnityAction>> _EDI_OnNodeActivated;
        [SerializeField] private List<CustomEventDispatcher<UnityAction>> _EDI_OnNodeDeactivated;

        public string NodeType { get { return nodeType; } set { nodeType = value; } }
        public string NodeName { get => nodeName; set => nodeName = value; }

        public List<CustomEventDispatcher<UnityAction>> EDI_OnNodeActivated { get => _EDI_OnNodeActivated; set => _EDI_OnNodeActivated = value; }
        public List<CustomEventDispatcher<UnityAction>> EDI_OnNodeDeactivated { get => _EDI_OnNodeDeactivated; set => _EDI_OnNodeDeactivated = value; }

#if UNITY_EDITOR

        public virtual void Initialise(Rect rect, string nodeType, DialogNodeGraph nodeGraph)
        {
            name = nodeType;
            standartHeight = rect.height;
            this.nodeType = nodeType;
            this.rect = rect;
            this.nodeGraph = nodeGraph;
        }

        public virtual void Draw(GUIStyle nodeStyle, GUIStyle lableStyle)
        { }

        public virtual void DrawNodeTypeFieldHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            NodeName = EditorGUILayout.TextField(NodeName, GUILayout.Width(nodeTypeFieldWith));
            name = NodeName;
            EditorGUILayout.EndHorizontal();
        }

        public virtual void DrawNodeTypeHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(nodeType);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(2.5f);
        }

        public virtual void DrawCharacterSpriteHorizontal() {}

        public virtual bool AddToParentConnectedNode(Node nodeToAdd)
        { return true; }

        public virtual bool AddToChildConnectedNode(Node nodeToAdd)
        { return true; }

        public void ProcessNodeEvents(Event currentEvent)
        {
            switch (currentEvent.type)
            {
                case EventType.MouseDown:
                    ProcessMouseDownEvent(currentEvent);
                    break;

                case EventType.MouseUp:
                    ProcessMouseUpEvent(currentEvent);
                    break;

                case EventType.MouseDrag:
                    ProcessMouseDragEvent(currentEvent);
                    break;

                default:
                    break;
            }
        }

        private void ProcessMouseDownEvent(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                ProcessLeftMouseDownEvent(currentEvent);
            }
            else if (currentEvent.button == 1)
            {
                ProcessRightMouseDownEvent(currentEvent);
            }
        }

        private void ProcessLeftMouseDownEvent(Event currentEvent)
        {
            OnNodeLeftClick();
        }

        private void ProcessRightMouseDownEvent(Event currentEvent)
        {
            nodeGraph.SetNodeToDrawLineFromAndLinePosition(this, currentEvent.mousePosition);
        }

        private void ProcessMouseUpEvent(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                ProcessLeftMouseUpEvent(currentEvent);
            }
        }

        private void ProcessLeftMouseUpEvent(Event currentEvent)
        {
            isDragging = false;
        }

        private void ProcessMouseDragEvent(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                ProcessLeftMouseDragEvent(currentEvent);
            }
        }

        private void ProcessLeftMouseDragEvent(Event currentEvent)
        {
            isDragging = true;
            DragNode(currentEvent.delta);
            GUI.changed = true;
        }

        public void OnNodeLeftClick()
        {
            Selection.activeObject = this;

            if (isSelected)
            {
                isSelected = false;
            }
            else
            {
                isSelected = true;
            }
        }

        public void DragNode(Vector2 delta)
        {
            rect.position += delta;
            EditorUtility.SetDirty(this);
        }

        // Method to rename the node
        public void Rename(string newName)
        {
            this.name = newName;
            EditorUtility.SetDirty(this);  // Mark the node as dirty to ensure changes are saved
        }

        virtual public void RemoveNodeFromParents(Node node)
        {

        }

        virtual public void RemoveNodeFromChilds(Node nodeToRemove)
        {

        }
#endif
    }
}