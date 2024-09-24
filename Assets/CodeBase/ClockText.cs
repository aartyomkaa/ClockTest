using UnityEngine;
using TMPro;

namespace CodeBase
{
    public class ClockText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void SetTimeText(Time time)
        {
            _text.text = $"{time.Hour}:{time.Minute}:{time.Second}";
        }
    }
}