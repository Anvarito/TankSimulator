using UnityEngine;
using UnityEngine.UI;


namespace ChobiAssets.PTM
{
	
	public class UI_Speed_Indicator_Control_CS : MonoBehaviour
	{
        /*
		 * This script is attached to the "Canvas_Speed_Indicator" in the scene.
		 * This script controls the texts for displaying the current selected tank speed.
		 * This script works in combination with "Drive_Control_CS" in the tank.
		*/

        // User options >>
        [SerializeField] Text Speed_Text = default;
        // << User options
        const string textFormat = "{0} km/h";
        Drive_Control_CS driveScript;

        public void Update_Speed_Text(int currentSpeed)
        {
            Speed_Text.text = string.Format(textFormat, currentSpeed);
        }
    }
}
