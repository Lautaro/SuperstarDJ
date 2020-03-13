#if UNITY_EDITOR
namespace SuperstarDJ.CustomEditors
{
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using SuperstarDJ.Audio.PatternDetection;
    using UnityEditor;
    using UnityEngine;

    public class GameControlCenter : OdinMenuEditorWindow
    {
        [MenuItem ( "Tools/SuperstarDJ/ControlCenter" )]
        private static void OpenWindow()
        {
            var window = GetWindow<GameControlCenter> ();

            window.position = GUIHelper.GetEditorWindowRect ().AlignCenter ( 700, 700 );
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree ( true );

            tree.AddObjectAtPath ( "Game Settings", GameSettings.Instance );

            var patterns = Resources.LoadAll<Pattern> ( "RythmPatterns" );

            foreach ( var pattern in patterns )
            {
                tree.Add ( $"Patterns/{pattern.PatternName}", pattern );
            }

            tree.EnumerateTree ()
                .AddThumbnailIcons ()
                .SortMenuItemsByName ();

            return tree;

        }
    }
}
#endif
