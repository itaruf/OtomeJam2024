using UnityEngine;

namespace cherrydev
{
    [System.Serializable]
    public struct Answer
    {
        public string characterName;
        public Sprite characterSprite;

        public Answer(string characterName)
        {
            characterSprite = null;
            this.characterName = characterName;
        }
    }
}