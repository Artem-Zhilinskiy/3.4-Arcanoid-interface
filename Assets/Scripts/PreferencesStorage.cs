using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcanoid
{
    public class PreferencesStorage : MonoBehaviour
    {


        [SerializeField]
        Toggle _muteToggle;
        [SerializeField]
        Slider _volumeSlider;
        [SerializeField]
        Toggle _easyToggle;
        [SerializeField]
        Toggle _mediumToggle;
        [SerializeField]
        Toggle _hardToggle;

        // Start is called before the first frame update
        void Awake()
        {
            //Метод загрузки сохранённых настроек
            LoadPreferences();
            DebugMethod();
        }

        private void Mute()
        {
            PlayerPrefs.SetInt("IsMute", 1);
            _muteToggle.isOn = true;
            _volumeSlider.value = 0;
            //Debug.Log("mute");
        }

        private void Unmute()
        {
            PlayerPrefs.SetInt("IsMute", 0);
            _muteToggle.isOn = false;
            //Debug.Log("Unmute");
        }

        public void MuteToggle()
        {
            if (_muteToggle.isOn)
            {
                Mute();
            }
            else
            {
                Unmute();
            }
        }

        public void VolumeSlider()
        {
            PlayerPrefs.SetFloat("Volume", _volumeSlider.value);
            if (_volumeSlider.value == 0)
            {
                Mute();
            }
            else
            {
                Unmute();
            }
            //Debug.Log("VolumeSlider");
        }

        public void SetDifficultyEasy()
        {
            if (_easyToggle.isOn)
            {
                PlayerPrefs.SetString("Difficulty", "Easy");
                //Debug.Log("SetDifficultyEasy");
            }
        }
        public void SetDifficultyMedium()
        {
            if (_mediumToggle.isOn)
            {
                PlayerPrefs.SetString("Difficulty", "Medium");
                //Debug.Log("SetDifficultyMedium");
            }
        }
        public void SetDifficultyHard()
        {
            if (_hardToggle.isOn)
            {
                PlayerPrefs.SetString("Difficulty", "Hard");
                //Debug.Log("SetDifficultyHard");
            }
        }

        private void LoadPreferences()
        {
            if (PlayerPrefs.GetInt("IsMute") == 0)
            {
                _muteToggle.isOn = true;
            }
            else
            {
                _muteToggle.isOn = false;
            }
            _volumeSlider.value = PlayerPrefs.GetFloat("Volume");
            switch (PlayerPrefs.GetString("Difficulty")) 
            {
                case "Easy":
                    _easyToggle.isOn = true;
                    _mediumToggle.isOn = false;
                    _hardToggle.isOn = false;
                    break;
                case "Medium":
                    _easyToggle.isOn = false;
                    _mediumToggle.isOn = true;
                    _hardToggle.isOn = false;
                    break;
                case "Hard":
                    _easyToggle.isOn = false;
                    _mediumToggle.isOn = false;
                    _hardToggle.isOn = true;
                    break;
                default:
                    _easyToggle.isOn = false;
                    _mediumToggle.isOn = true;
                    _hardToggle.isOn = false;
                    break;
            }
        }

        private void DebugMethod()
        {
            Debug.Log(PlayerPrefs.GetInt("IsMute").ToString());
            Debug.Log(PlayerPrefs.GetFloat("Volume").ToString());
            Debug.Log(PlayerPrefs.GetString("Difficulty"));
        }
    }
}