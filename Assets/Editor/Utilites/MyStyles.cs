
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Utilites
{
    internal static class MyStyles
    {
        private static GUIStyle _normal;
        private static GUIStyle _selected;

        private static Texture2D _selectedTexture;

        public static GUIStyle Normal
        {
            get
            {
                if (_normal == null)
                {
                    _normal = new GUIStyle(EditorStyles.miniButton)
                    {
                        fontSize = 12,
                        fixedHeight = 24,
                        alignment = TextAnchor.MiddleCenter
                    };
                }

                return _normal;
            }
        }

        public static GUIStyle Selected
        {
            get
            {
                if (_selected == null)
                {
                    _selected = new GUIStyle(EditorStyles.miniButton)
                    {
                        fontSize = 14,
                        fontStyle = FontStyle.Bold,
                        fixedHeight = 32,
                        alignment = TextAnchor.MiddleCenter
                    };

                    _selectedTexture = MakeTex(new Color(0.2f, 0.55f, 0.95f));

                    _selected.normal.background = _selectedTexture;
                    _selected.hover.background = _selectedTexture;
                    _selected.active.background = _selectedTexture;

                    _selected.normal.textColor = Color.white;
                    _selected.hover.textColor = Color.white;
                    _selected.active.textColor = Color.white;
                }

                return _selected;
            }
        }

        private static Texture2D MakeTex(Color col)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, col);
            tex.Apply();
            return tex;
        }
    }
}
