using SuperstarDJ;
using SuperstarDJ.Audio;
using SuperstarDJ.Audio.PatternDetection;
using SuperstarDJ.Audio.PositionTracking;
using SuperstarDJ.MessageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PatternDetectionMonitor : EditorWindow
{
    Color drawAreaColor, borderColor, hitColor, trackerRed, successColor, failColor;

    [MenuItem("SuperStarDJ/Pattern Detection Monitor")]
    static void Init()
    {
        PatternDetectionMonitor window = (PatternDetectionMonitor)EditorWindow.GetWindow<PatternDetectionMonitor>();
        window.LoadPatterns();
        window.SetColors();
    }

    private void SetColors()
    {
        drawAreaColor = new Color(0.3f, 0.3f, 0.3f, 1f);

        borderColor = new Color(0.6f, 0.6f, 0.8f);

        successColor = new Color(0.3f, 1, 0.3f);
        failColor = new Color(1, 0.3f, 0.3f);
        hitColor = new Color(1, 1, 0, 1f);
        trackerRed = new Color(1, 0.3f, 0.3f, 1f);
    }


    private void LoadPatterns()
    {
        patterns = Resources.LoadAll<Pattern>("RythmPatterns/");
        gameSettings = Resources.Load<GameSettings>("Settings/GameSettings");
    }

    Pattern[] patterns;
    List<DjAct> djActs;
    #region DrawArea
    float drawAreaXpadding = 30;
    float drawAreaYpadding = 50;
    float timelineSpacing = 50;
    float timeLineHeight = 30f;
    Rect drawArea;
    #endregion

    float lineLength;
    float amountOfSteps = 64;
    float stepLength;

    GameSettings gameSettings;

    private void OnGUI()
    {
        if (patterns == null || patterns.Length == 0) return;

        #region SETUP
        GUI.color = drawAreaColor;
        GUI.Box(new Rect(0, 0, position.width, position.height), "");

        SetColors();

        var drawAreaTop = drawAreaYpadding;
        var drawAreaBottom = position.height - drawAreaYpadding;
        var drawAreaLeft = 0 + drawAreaXpadding;
        var drawAreaRight = position.width - drawAreaXpadding;

        drawArea = new Rect(drawAreaLeft, drawAreaTop, drawAreaRight - drawAreaLeft, drawAreaBottom - drawAreaTop);

        lineLength = this.drawArea.xMax - this.drawArea.xMin;
        stepLength = lineLength / amountOfSteps;
        #endregion


        // DRAWAREA BG
        GUI.color = drawAreaColor;
        GUI.Box(drawArea, "");

        for (int patternIndex = 0; patternIndex < patterns.Length; patternIndex++)
        {
            var pattern = patterns[patternIndex];
            var currentStepIndex = Array.FindIndex(pattern.Steps, p => p.IsCurrent);

            var y = drawArea.yMax - (timeLineHeight * patternIndex) - (timelineSpacing * patternIndex);

            DrawSteps(patternIndex, y);
            DrawTimeLines(y, pattern);
        }
    }



    void DrawTimeLines(float y, Pattern pattern)
    {
        GUI.color = borderColor;
        GUI.Box(new Rect(drawArea.xMin, y - 5, drawArea.width, 5), "");

        foreach (var hitPosition in pattern.HitPositions)
        {
            var positionInPercentage = RythmManager.instance?.CalculatePositionInPercentage(hitPosition.RawPosition) ?? 0;
            var trackerX = positionInPercentage * drawArea.width;
            DrawTracker(trackerX, y, Color.yellow);
        }
        var currentPositionInPercentage = RythmManager.instance?.CurrentPositionInPercentage() ?? 0;
        var currentTrackerX = currentPositionInPercentage * drawArea.width;
        DrawTracker(currentTrackerX, y, Color.red);

    }

    // Updates the player head tracker line that shows current position
    void DrawTracker(float x, float y, Color color)
    {
        GUI.color = color;
        GUI.Box(new Rect(drawArea.xMin + x, y-5, 2, -(timeLineHeight * 2)), "");
    }

    private void DrawSteps(int patternIndex, float y)
    {
        var hitrangePercentage = (float)gameSettings.HitRangePaddingInPercentage;
        float halfHitRangeWidth = (stepLength * (hitrangePercentage / 100)) / 2;

        for (int i = 0; i < amountOfSteps; i++)
        {
            // SETUP
            var x = drawArea.xMin + (i * stepLength);
            var pattern = patterns[patternIndex];
            var step = pattern.Steps[i];
            PatternStepAction stepAction = step.Action;
            PatternStepStatus stepStatus = step.Status;

            var startY = y - 5;

            // SET COLOR
            Color hitareaColor = borderColor;
            if (stepAction == PatternStepAction.Hit) hitareaColor = hitColor;
            if (stepStatus == PatternStepStatus.Sucess && stepAction == PatternStepAction.Hit) hitareaColor = successColor;
            if (stepStatus == PatternStepStatus.Failed) hitareaColor = failColor;
            if (step.IsCurrent) hitareaColor = Color.white;

            GUI.color = hitareaColor;

            // DRAW HIT AREA
            GUI.Box(new Rect(x - halfHitRangeWidth, startY, halfHitRangeWidth, -timeLineHeight), "");
            GUI.Box(new Rect(x, startY, halfHitRangeWidth, -timeLineHeight), "");


            // DRAW MARKER

            var thickness = 6;
            x -= thickness / 2;
            GUI.Box(new Rect(x, y, thickness, 8), "");

            // DRAW STEP NR
            if (i % 4 == 0)
            {
                var labelStyle = new GUIStyle();
                labelStyle.normal.textColor = hitareaColor;
                labelStyle.fontSize = 14;

                GUI.Label(new Rect(x - 8, y + 10, 20, 15), i.ToString("00"), labelStyle);
            }
        }
    }



    public void Update()
    {
        // This is necessary to make the framerate normal for the editor window.
        Repaint();
    }
}
