using System.Reflection;
using UnityEngine;

namespace Infrastructure.TestMono
{
    public abstract class UIHelper : MonoBehaviour
    {
        // https://stackoverflow.com/questions/6394921/get-fields-with-reflection
        protected void Start()
        {
            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                if (field.GetValue(this) == null)
                    Debug.LogWarning($"{field.Name} missing");
        }
    }
}