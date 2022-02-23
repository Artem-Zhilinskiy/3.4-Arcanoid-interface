using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcanoid
{
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Ускорение движения платформ"), SerializeField]
        public UnityEngine.ForceMode _platformAcceleration = (ForceMode)1;

        //Здесь заданы препятствия всех уровней
        [Header("Массив препятствий 1 уровня"), SerializeField]
        public Transform[] _obstaclesLevel1;

        [Header("Массив препятствий 2 уровня"), SerializeField]
        public Transform[] _obstaclesLevel2;

        [Header("Массив препятствий 3 уровня"), SerializeField]
        public Transform[] _obstaclesLevel3;

        //Задать шар
        [Header("Шар"), SerializeField]
        public Transform _sphere;

        //Определить ворота
        [Header("Ворота, уровень 1"), SerializeField]
        public Transform _gate11;
        public Transform _gate12;

        [Header("Ворота, уровень 2"), SerializeField]
        public Transform _gate21;
        public Transform _gate22;

        [Header("Ворота, уровень 3"), SerializeField]
        public Transform _gate31;
        public Transform _gate32;

        //Счётчик жизней
        [Header("Счётчик жизней"), SerializeField]
        private int _lifeCounter = 10;

        //Камеры
        [Header("Камеры, уровень 1"), SerializeField]
        public Transform _camera11;
        public Transform _camera12;

        [Header("Камеры, уровень 2"), SerializeField]
        public Transform _camera21;
        public Transform _camera22;

        [Header("Камеры, уровень 3"), SerializeField]
        public Transform _camera31;
        public Transform _camera32;

        //Счётчик текущего уровня
        private byte _currentLevel = 1;

        // Start is called before the first frame update
        void Start()
        {
            RotateObstacles();
            ObstacleEventHandler();
            LifeEventHandler();
            RestartLevelHandler();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void RotateObstacles()
        {
            foreach (var _obstacle in _obstaclesLevel1)
            {
                _obstacle.rotation = Random.rotation;
            }
            foreach (var _obstacle in _obstaclesLevel2)
            {
                _obstacle.rotation = Random.rotation;
            }
            foreach (var _obstacle in _obstaclesLevel3)
            {
                _obstacle.rotation = Random.rotation;
            }
        }
        
        private void DesactivateObstacleInMassive (Transform _transform)
        {
            //Определение к какому массиву должно принадлежать препятствие
            switch (_currentLevel)
            {
                case 1: 
                    DesactivateObstacle(_transform ,_obstaclesLevel1);
                    LevelCheck(_obstaclesLevel1);
                    break;
                case 2: 
                    DesactivateObstacle(_transform ,_obstaclesLevel2);
                    LevelCheck(_obstaclesLevel2);
                    break;
                case 3: 
                    DesactivateObstacle(_transform ,_obstaclesLevel3);
                    LevelCheck(_obstaclesLevel3);
                    break;
            }
        }

        private void DesactivateObstacle (Transform _transform, Transform[] _obstacles)
        {
            foreach (var _obstacle in _obstacles)
            {
                if (_transform == _obstacle)
                {
                    _transform.gameObject.SetActive(false);
                    //Увеличение скорость движения шара
                    _sphere.GetComponent<SphereControls>().IncreaseSpeed();
                    return;
                }
            }
        }

        private void LevelCheck(Transform[] _obstacles)
        {
            //bool _keepPlaying = false;
            foreach (var _obstacle in _obstacles)
            {
                if (_obstacle.gameObject.activeSelf == true)
                {
                    //_keepPlaying = true;
                    //Debug.Log("debug 3");
                    return;
                }
            }
            //if (_keepPlaying == false && _currentLevel < 3)
            if (_currentLevel < 3)
            {
                Debug.Log("Уровень пройден! Поздравляем с переходом на следующий уровень.");
                LevelUp();
            }
            else 
            {
                Debug.Log("Вы победили!");
                UnityEditor.EditorApplication.isPaused = true;
            }
        }

        private void LevelUp()
        {
            switch (_currentLevel)
            {
                case 0: //В случае перезапуска первого уровня
                    _sphere.GetComponent<SphereControls>().SphereReturn();
                    _currentLevel++;
                    break;
                case 1:
                    //Задание новой стартовой позиции шара
                    SphereControls._sphereStartLocation = SphereControls._sphereStartLocationLevel2;
                    //Перемещение шара на новую стартовую позицию
                    _sphere.GetComponent<SphereControls>().SphereReturn();
                    //Переключение камер
                    _camera11.gameObject.SetActive(false);
                    _camera12.gameObject.SetActive(false);
                    _camera21.gameObject.SetActive(true);
                    _camera22.gameObject.SetActive(true);
                    _currentLevel++;
                    break;
                case 2:
                    //Задание новой стартовой позиции шара
                    SphereControls._sphereStartLocation = SphereControls._sphereStartLocationLevel3;
                    //Перемещение шара на новую стартовую позицию
                    _sphere.GetComponent<SphereControls>().SphereReturn();
                    //Переключение камер
                    _camera21.gameObject.SetActive(false);
                    _camera22.gameObject.SetActive(false);
                    _camera31.gameObject.SetActive(true);
                    _camera32.gameObject.SetActive(true);
                    _currentLevel++;
                    break;
            }
        }


        private void ObstacleEventHandler()
        {
            _sphere.GetComponent<SphereControls>().DesactivateObstacleEvent += DesactivateObstacleInMassive;
        }

        //Счёт жизней
        private void LifeCounter()
        {
            _lifeCounter -= 1;
            if (_lifeCounter > 0)
            {
                Debug.Log("Шар упущен. Осталось жизней: " + _lifeCounter);
            }
            else
            {
                Debug.Log("Шар упущен. Осталось жизней:" + _lifeCounter + " Игра окончена.");
                UnityEditor.EditorApplication.isPaused = true;
            }
            //Возврат и остановка шара
            _sphere.GetComponent<SphereControls>().SphereReturn();
        }

        private void LifeEventHandler()
        {
            _gate11.GetComponent<GateControls>().SphereLeftEvent += LifeCounter;
            _gate12.GetComponent<GateControls>().SphereLeftEvent += LifeCounter;
            _gate21.GetComponent<GateControls>().SphereLeftEvent += LifeCounter;
            _gate22.GetComponent<GateControls>().SphereLeftEvent += LifeCounter;
            _gate31.GetComponent<GateControls>().SphereLeftEvent += LifeCounter;
            _gate32.GetComponent<GateControls>().SphereLeftEvent += LifeCounter;
        }

        //Модуль перезапуска уровня
        void RestartLevelHandler()
        {
            MenuManager.RestartLevelEvent += RestartLevel;
        }

        void RestartLevel()
        {
            ReactivateObstacleInMassive(_currentLevel);
            _currentLevel--;
            LevelUp();
        }

        private void ReactivateObstacle(Transform[] _obstacles)
        {
            foreach (var _obstacle in _obstacles)
            {
                    _obstacle.gameObject.SetActive(true);
            }
        }

        private void ReactivateObstacleInMassive(byte _currentLevel)
        {
            //Определение к какому массиву должно принадлежать препятствие
            switch (_currentLevel)
            {
                case 1:
                    ReactivateObstacle(_obstaclesLevel1);
                    break;
                case 2:
                    ReactivateObstacle(_obstaclesLevel2);
                    break;
                case 3:
                    ReactivateObstacle(_obstaclesLevel3);
                    break;
            }
        }
    }
}