using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace cherrydev
{
    public class Node : ScriptableObject
    {
        [HideInInspector] public List<Node> connectedNodesList;
        [HideInInspector] public DialogNodeGraph nodeGraph;
        [HideInInspector] public Rect rect = new Rect();

        [HideInInspector] public bool isDragging;
        [HideInInspector] public bool isSelected;

        [SerializeField] private string nodeName = "Node";

        protected float standartHeight;

        public string NodeName { get { return nodeName; } set { nodeName = value; } }

#if UNITY_EDITOR

        public virtual void Initialise(Rect rect, string nodeName, DialogNodeGraph nodeGraph)
        {
            name = nodeName;
            this.nodeName = nodeName;
            standartHeight = rect.height;
            this.rect = rect;
            this.nodeGraph = nodeGraph;
        }

        public virtual void Draw(GUIStyle nodeStyle, GUIStyle lableStyle)
        { }

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