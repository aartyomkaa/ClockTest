using CodeBase.Hands;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase
{
	public class Clock : MonoBehaviour, ICoroutineRunner
	{
		private const int RotationMultiplayer = 6;
		private const int HourRotationMultiplayer = 30;
		private const int HourDivisions = 12;
		private const int Hour = 3600;
		
		[SerializeField] private Hand _handHours;
		[SerializeField] private DragableHand _handMinutes;
		[SerializeField] private Hand _handSeconds;
		[SerializeField] private ClockText _clockText;
		[SerializeField] private TimeEditor _timeEditor;
		[SerializeField] private ClockHandDrager _handDrager;

		private TimeReceiver _timeReceiver;
		private Time _time;

		public Time Time => _time;

		private void OnEnable()
		{
			_timeEditor.TimeEdited += SetSynchronizedTime;
		}

		private void Awake()
		{
			_timeReceiver = new TimeReceiver(this);
			_time = new Time();

			InvokeRepeating(nameof(SynchronizeTime), 0, Hour);
			RotateHands();
		}

		private void OnDisable()
		{
			_timeEditor.TimeEdited -= SetSynchronizedTime;
			CancelInvoke();
		}

		public void RotateHands()
		{
			InvokeRepeating(nameof(UpdateTime), 0, 1);
		}

		public void StopRotateHands()
		{
			CancelInvoke(nameof(UpdateTime));
		}

		private void SynchronizeTime()
		{
			_timeReceiver.ReceiveTime(SetSynchronizedTime);
		}

		private void UpdateTime()
		{
			_time.Update(1);
			_clockText.SetTimeText(_time);
			
			UpdateHands();
		}

		public void DragSynchronized(Time time)
		{
			_time = time;
			_clockText.SetTimeText(time);
		}

		private void UpdateHands()
		{
			float handRotationHours	= _time.Hour * HourRotationMultiplayer;
			float handRotationMinutes = _time.Minute * RotationMultiplayer;
			float handRotationSeconds = _time.Second * RotationMultiplayer;
			
			Vector3 hoursVec = new Vector3(0, 0, handRotationHours + handRotationMinutes / HourDivisions);
			Vector3 minutesVec = new Vector3(0, 0, handRotationMinutes);
			Vector3 secondsVec = new Vector3(0, 0, handRotationSeconds);
			
			_handHours.Rotate(hoursVec);
			_handMinutes.Rotate(minutesVec);
			_handSeconds.Rotate(secondsVec);
		}

		private void SetSynchronizedTime(Time time)
		{
			_time = time;
			_clockText.SetTimeText(time);
			_handDrager.Init(_time, this);
			
			UpdateHands();
		}
	}
}