#if UNITY_EDITOR
namespace SuperstarDJ.CustomEditors
{
    using UnityEditor;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities.Editor;
    using Sirenix.Utilities;
    using SuperstarDJ.Audio;

    public class GameStatsWindow : OdinEditorWindow
    {
        [MenuItem("Tools/SuperstarDJ/GameStats")]
        private static void OpenWindow()
        {
            var window = GetWindow<GameStatsWindow> ();

            // Nifty little trick to quickly position the window in the middle of the editor.
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
            //RythmManager = RythmManager.instance;
        }

        //protected override object GetTarget()
        //{
        //    return MySingleton.Instance;
        //}

        [EnumToggleButtons]
        [InfoBox("Inherit from OdinEditorWindow instead of EditorWindow in order to create editor windows like you would inspectors - by exposing members and using attributes.")]
        public ViewTool SomeField;

        public RythmManager RythmManager = RythmManager.instance;
    }
}
#endif
