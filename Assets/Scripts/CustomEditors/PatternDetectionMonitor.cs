using SuperstarDJ.Audio;
using SuperstarDJ.Audio.PatternDetection;
using SuperstarDJ.MessageSystem;
using UnityEditor;
using UnityEngine;

public class PatternDetectionMonitor : EditorWindow
{
    [MenuItem ( "SuperStarDJ/Pattern Detection Monitor " )]
    static void Init()
    {
        PatternDetectionMonitor window =
            ( PatternDetectionMonitor )EditorWindow.GetWindow ( typeof ( PatternDetectionMonitor ) );
        window.patterns = Resources.LoadAll<Pattern> ( "RythmPatterns/" );
        //RythmManager.OnFrameUpdate += window.MyUpdate;
    }

    Pattern[] patterns;

    #region DrawArea
    float drawAreaWidth;
    float drawAreaheight;
    float drawAreaXpadding = 30f;
    float drawAreaYpadding = 100;
    float timeLineHeight = 50f;
    Rect rect;
    #endregion

    float lineLength;
    float amountOfSteps = (RythmManager.Settings?.StepsInPattern) ?? 64;
    float stepLength;

    private void OnGUI()
    {
        drawAreaWidth = position.width - ( drawAreaXpadding * 2 );
        drawAreaheight = position.height - ( drawAreaYpadding * 2 );
        var y = position.height - drawAreaYpadding;
        rect = new Rect ( 0 + drawAreaXpadding, y, drawAreaWidth - ( drawAreaXpadding * 2 ), 50 );
        lineLength = rect.xMax - rect.xMin;
        stepLength = lineLength / amountOfSteps;

        //   DrawDrawingAreaBox ();
        
        for ( int i = 0; i < patterns.Length; i++ )
        {
            DrawTimeLine ( i );
        }

        UpdateTracker ();
    }

    private void DrawSteps( int patternIndex )
    {
        for ( int i = 0; i < amountOfSteps; i++ )
        {
            var x = rect.xMin + ( i * stepLength );
            var y = rect.yMax - ( timeLineHeight * patternIndex );
            var pattern = patterns[patternIndex];
            var step = pattern.Steps[i];
            PatternStepAction stepAction = step.Action;

            GUI.color = Color.black;
            var thickness = 2;

            if ( stepAction == PatternStepAction.Hit )
            {
                GUI.color = new Color ( 0, 0, 256 );
                thickness = 4;
                x -= 2;
            }

            if ( step.IsCurrent )
            {
                GUI.color = new Color ( 50, 100, 256 );
                thickness = 6;
                x -= 3;
            }

            GUI.Box ( new Rect ( x, y, thickness, 5 ), "" );

            if ( i % 4 == 0 )
            {
                GUI.Label ( new Rect ( x - 8, y + 5, 20, 15 ), i.ToString ( "00" ) );
            }

            if ( RythmManager.WasHitButMissedThisFrame())
            {
                GUI.Label ( new Rect ( x - 8, y + 15, 20, 15 ), i.ToString ( "X" ) );
            }
        }
    }

    //private void DrawDrawingAreaBox()
    //{
    //    // Draw draw area box
    //    rect = new Rect ( drawAreaXpadding, drawAreaYpadding, drawAreaWidth, drawAreaheight + drawAreaYpadding );
    //    Handles.color = Color.gray;
    //    Handles.DrawLine ( new Vector3 ( rect.xMin, rect.yMin ), new Vector3 ( rect.xMax, rect.yMin ) );
    //    Handles.DrawLine ( new Vector3 ( rect.xMin, rect.yMin ), new Vector3 ( rect.xMin, rect.yMax ) );
    //}

    void DrawTimeLine( int index )
    {
        var y = index * timeLineHeight;
        Handles.color = Color.white;
        //Timeline
        Handles.DrawLine ( new Vector3 ( rect.xMin, rect.yMax - y ), new Vector3 ( rect.xMax, rect.yMax - y ) );

        DrawSteps ( index );
    }

    void UpdateTracker()
    {
        GUI.color = Color.yellow;
        var percentage = (RythmManager.instance?.PositionInPercentage ()) ?? 0;
        var trackerPosition = percentage * drawAreaWidth; 
        //       var trackerPosition = ( ( float )DateTime.Now.Second / 60 ) * drawAreaWidth;
        GUI.Box ( new Rect ( rect.xMin + trackerPosition, rect.yMax, 2, -( patterns.Length + 1 ) * timeLineHeight ), "" );
    }

    void Update()
    {
        this.Repaint ();
    }

   

}