using CodeBase.Hands;
using UnityEngine;

namespace CodeBase.Services
{
    public class ClockHandDrager : MonoBehaviour
    {
        private const int Offset = 90;
        private const int SecondsMultiplier = 60;
        private const int HoursDivider = 12;
        private const int MaxHours = 23;
        private const int NextHourDegree = -710;
        private const int LastHourDegree = -10;
        private const int DegreeOffset = 180;
        private const int HourAngleOffset = -30;
        private const int Divisions = 6;
        
        [SerializeField] private Transform _clockCenter;
        [SerializeField] private DragableHand _minuteHand;
        [SerializeField] private Hand _hourHand;
        [SerializeField] private Hand _secondHand;
        
        private Clock _clock;
        private Time _time = new Time();

        private int _count;
        private float _startRotation;
        
        private int _hour;
        private float _maxDegree;

        private void OnEnable()
        {
            _minuteHand.BeingDrag += OnMinuteHandDrag;
            _minuteHand.EndDrag += OnMinuteHandDragEnd;
        }

        private void OnDisable()
        {
            _minuteHand.BeingDrag -= OnMinuteHandDrag;
            _minuteHand.EndDrag -= OnMinuteHandDragEnd;
        }

        public void Init(Time time ,Clock clock)
        {
            _time = time;
            _clock = clock;
            _hour = _clock.Time.Hour;
        }

        private void OnMinuteHandDrag()
        {
            _clock.StopRotateHands();
            
            Vector2 direction = GetDirection();

            float minuteAngle = Mathf.Atan2( direction.x,-direction.y) * Mathf.Rad2Deg + DegreeOffset;
            
            Vector3 minuteRotation = new Vector3(0, 0, -minuteAngle);

            FixHour(minuteRotation);
                
            _minuteHand.transform.localEulerAngles = minuteRotation;
            
            SetAngles(minuteAngle);

            _clock.DragSynchronized(AngleToTime(
                _minuteHand.transform.localEulerAngles.z,
                _secondHand.transform.localEulerAngles.z));
        }

        private void FixHour(Vector3 minuteRotation)
        {
            if (minuteRotation.z - _minuteHand.transform.localEulerAngles.z < NextHourDegree)
            {
                _hour += 1;

                if (_hour > MaxHours)
                    _hour = 0;
            }
            else if (minuteRotation.z - _minuteHand.transform.localEulerAngles.z > LastHourDegree)
            {
                _hour -= 1;
                
                if (_hour < 0)
                    _hour = MaxHours;
            }
        }

        private Vector2 GetDirection()
        {
            Vector3 pointerPosition = Input.mousePosition;
            
            Vector3 clockCenterScreen = Camera.main.WorldToScreenPoint(_clockCenter.position);
            Vector2 direction = pointerPosition - clockCenterScreen;
            return direction;
        }

        private void SetAngles(float minuteAngle)
        {
            float hourAngle = minuteAngle / HoursDivider - Offset + (_hour + 1) * HourAngleOffset;
            Vector3 hourRotation = new Vector3(0, 0, -(hourAngle + Offset));
            _hourHand.transform.localEulerAngles = hourRotation;
            
            float secondAngle = minuteAngle * SecondsMultiplier;
            Vector3 secondRotation = new Vector3(0, 0, -(secondAngle - Offset));
            _secondHand.transform.localEulerAngles = secondRotation;
        }

        private Time AngleToTime( float minZ, float secZ)
        {
            if (secZ < 0)
            {
                secZ += _maxDegree;
            }

            if (minZ < 0)
            {
                minZ += _maxDegree;
            }
            
            _time.Synchronize(
                _hour, 
                Mathf.FloorToInt(minZ / Divisions), 
                Mathf.FloorToInt(secZ / Divisions));
            
            return _time;
        }

        private void OnMinuteHandDragEnd()
        {
            _clock.RotateHands();
        }
    }
}