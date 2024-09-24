using DG.Tweening;
using UnityEngine;

namespace CodeBase.Hands
{
    public class Hand : MonoBehaviour
    {
        private const float RotateDuration = 0.7f;

        public void Rotate(Vector3 rotation)
        {
            transform.DOLocalRotate(rotation, RotateDuration);
        }
    }
}