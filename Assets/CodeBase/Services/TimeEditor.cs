using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace CodeBase.Services
{
    public class TimeEditor : MonoBehaviour
    {
        private const string Edit = "Edit";
        private const string Set = "Close";
        private const string Wrong = "Incorrect Time format ! Try Again !";
        private const int MaxMinAndSec = 60;
        private const int MaxHours = 24;
        private const Char CharSeparator = ':';
        
        [SerializeField] private Button _editButton;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private TMP_Text _textWrong;
        [SerializeField] private TMP_InputField _inputField;

        private string _userInput;
        private Time _time;
        private float _fadeTime = 3f;

        public event Action<Time> TimeEdited;

        private void OnEnable()
        {
            _editButton.onClick.AddListener(OnEditButtonPressed);
        }

        private void Start()
        {
            _inputField.gameObject.SetActive(false);
            _time = new Time();
            
            SetText();
        }

        public void ReadInput(string input)
        {
            if (CanTransformInput(input))
            {
                TimeEdited?.Invoke(_time);
            }
            else
            {
                _textWrong.alpha = 1f;
                _textWrong.DOFade(0, _fadeTime);
            }
        }

        private void SetText()
        {
            _textWrong.alpha = 0f;
            _textWrong.text = Wrong;
            _buttonText.text = Edit;
        }

        private void OnDisable()
        {
            _editButton.onClick.RemoveListener(OnEditButtonPressed);
        }

        private void OnEditButtonPressed()
        {
            _inputField.gameObject.SetActive(!_inputField.gameObject.activeSelf);
            _buttonText.text = _buttonText.text == Edit ? Set : Edit;
        }

        private bool CanTransformInput(string input)
        {
            string[] numbers = input.Split(CharSeparator, StringSplitOptions.RemoveEmptyEntries);
            int[] numbersInt = new int[numbers.Length];
            int num = 0;
            int minIndex = 1;
            int secIndex = 2;
            int minimumLenght = 3;

            if (numbers.Length < minimumLenght)
                return false;
            
            for(int i = 0; i < numbers.Length; i++)
            {
                if (int.TryParse(numbers[i], out num))
                    numbersInt[i] = num;
                else
                    return false;
            }

            if (numbersInt[0] < MaxHours && numbersInt[minIndex] < MaxMinAndSec && numbersInt[secIndex] < MaxMinAndSec)
            {
                _time.Synchronize(numbersInt[0], numbersInt[minIndex], numbersInt[secIndex]);
            } 
            
            return true;
        }
    }
}