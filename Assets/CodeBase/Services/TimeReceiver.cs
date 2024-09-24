using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CodeBase.Services
{
    public class TimeReceiver
    {
        private const string URL = "https://yandex.com/time/sync.json";
        
        private TimeConverter _timeConverter;
        private ICoroutineRunner _coroutineRunner;
        private Coroutine _getTimeCoroutine;
        private Time _time;

        public TimeReceiver(ICoroutineRunner coroutineRunner)
        {
	        _coroutineRunner = coroutineRunner;
            _timeConverter = new TimeConverter();
            _time = new Time();
        }

        public void ReceiveTime(Action<Time> onSccess = null)
        {
            _coroutineRunner.StartCoroutine(GetTime(onSccess));
        }

        private IEnumerator GetTime(Action<Time> onSccess = null)
        {
            using UnityWebRequest request = UnityWebRequest.Get(URL);
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                _timeConverter.Convert(request.downloadHandler.text, _time);

                onSccess?.Invoke(_time);
            }
            else
            {
                Debug.Log("NOOOOOOOOOOOOOO");
            }
        }
    }
}