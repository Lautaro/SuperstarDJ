#if UNITY_EDITOR
namespace SuperstarDJ.CustomEditors
{
    using UnityEditor;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities.Editor;
    using Sirenix.Utilities;
    using SuperstarDJ.Audio;
    using UnityEngine;
    using System.Linq;
    using SuperstarDJ.Audio.PatternDetection;

    public class GameMonitorWindow : OdinMenuEditorWindow
    {
        [MenuItem ( "Tools/SuperstarDJ/Monitor" )]
        private static void OpenWindow()
        {
            var window = GetWindow<GameMonitorWindow> ();

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
