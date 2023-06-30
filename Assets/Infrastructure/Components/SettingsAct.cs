using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Components
{
    public class SettingsAct : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public void Launch()
        {
            _button.Select();
        }
    }
}
