using NewsBoardMessaging;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace NewsBoardMessaging.TestScene
{
    public class NewsBoardTest_UI : MonoBehaviour
    {
        TextMeshProUGUI text;
        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TextMeshProUGUI> ();
            NewsBoard.Subscribe ( NewsTopics.SomeonePressedAButton_string, GetUiDisplayText );
        }

        public void GetUiDisplayText( NewsEvent news )
        {
            var message = news.Open<string> ();

            text.text = message;

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}