using NewsBoardMessaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewsBoardMessaging.TestScene
{
    public class NewsBoardTest_Scene : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //NewsBoard.
        }
        int count = 0;
        // Update is called once per frame
        void Update()
        {
            if ( Input.GetKeyDown ( KeyCode.Space ) )
            {

                NewsBoard.PublishNews<string> ( NewsTopics.SomeonePressedAButton_string, $"[{count++}]Space has been pressed" );
            }

        }
    }
}