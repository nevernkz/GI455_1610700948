using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Name : MonoBehaviour
{
    
    public string[] MMD = new string[5];
    public InputField inputField;
    public Text Check;
    public Text OutPut;
    public string NameText;
    


    

    // Update is called once per frame
    void Update()
    {
        NameText = inputField.text;
    }

    public void EventClick()
    {
        for(int i=0; i<MMD.Length; i++)
        {
            if(NameText == MMD[i])
            {

                OutPut.text = (" [ " + $"<color=green>{ NameText}</color>" + " ] " + " Is Found. ");
                //OutPut.text = (" [ "+NameText+ " ] " + " Is Found. ");
                Debug.Log(NameText+ " test ");
                break;
            }
            else 
            {
                OutPut.text = (" [ " +$"<color=red>{ NameText}</color>" +" ] " + " Isn't Found. ");
                Debug.Log(NameText + " F ");
                
            }
            
        }




        /* if (NameText == MMD[0])
         {

             Check.text = (NameText + " Is Found ");
             Debug.Log(NameText + " Is Found ");

         }
         else if (NameText == MMD[1])
         {
             Check.text = (NameText + " Is Found ");
         }
         else if (NameText == MMD[2])
         {

             Check.text = (NameText + " Is Found ");
         }
         else if (NameText == MMD[3])
         {

             Check.text = (NameText + " Is Found ");
         }
         else if (NameText == MMD[4])
         { 
             Check.text = (NameText + " Is Found ");
         }

         else
         { 
             Check.text = (NameText + " Isn't Found");
         } */

    }
}
