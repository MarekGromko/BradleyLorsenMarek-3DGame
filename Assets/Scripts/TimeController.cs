//using UnityEngine;
//using TMPro;
//using System.Diagnostics;

//public class TimeController : MonoBehaviour
//{
//    [SerializeField] TextMeshProUGUI Timer;
//    public float startTime = 60f;
//    private float currentTime;
//    private bool timeActive = false;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        currentTime = startTime;
//# UpdateTimerDisplay();
//        timeActive = true;
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (timeActive)
//        {
//            if (currentTime > 0)
//            {
//                currentTime -= Time.deltaTime;
//# UpdateTimerDisplay();
//            }
//            else
//            {
//                currentTime = 0;
//                timeActive= false;
//# UpdateTimerDisplay();
//                Debug.Log("bbakbkafb;");
                

//            }
           
//        }
        
//    }

   
//}
