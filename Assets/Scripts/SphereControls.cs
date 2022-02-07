using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Arcanoid
{
    public class SphereControls : MonoBehaviour
    {
        Coroutine _moveSphereCoroutine = null;
        Quaternion _sphereStartRotation = new Quaternion();

        //Стартовые точки шара
        static Vector3 _sphereStartLocationLevel1 = new Vector3(6, 1, 1);
        public static Vector3 _sphereStartLocationLevel2 = new Vector3(3.5f, 1, 20);
        public static Vector3 _sphereStartLocationLevel3 = new Vector3(10, 1, 41);
        public static Vector3 _sphereStartLocation = _sphereStartLocationLevel1;

        //Импорт настроек из GameManager
        //Объявление переменной, чтобы можно было получить доступ в других методах. Надо в инспекторе Unity добавить.
        [SerializeField] private GameManager GameManager;

        [Tooltip("Скорость движения шара")] public float _moveSpeed = 1;
        [Tooltip("Максимальная скорость движения шара")] public float _maxMoveSpeed = 5;

        //1. Первое что нужно сделать при создании скрипта управления в NewInputSystem
        public PlatformControls1 controls;

        //Объявить делегат для события
        public delegate void DesactivateObstacleDelegate(Transform _transform);

        //Объявление события
        public event DesactivateObstacleDelegate DesactivateObstacleEvent;

        //2. Второе, что нужно сделать - инициализировать controls в Awake
        private void Awake()
        {
            controls = new PlatformControls1();
        }

        //3. Активировать в OnEnable ActionMap
        private void OnEnable()
        {
            controls.ActionMap.Enable();

            //Подключить события
            controls.ActionMap.Launch.performed += OnLaunch;
        }

        //4. Дезактивировать в OnDisable
        private void OnDisable()
        {
            controls.ActionMap.Launch.performed -= OnLaunch;
            controls.ActionMap.Disable();
        }

        public void OnLaunch(CallbackContext context)
        {
            _moveSphereCoroutine = StartCoroutine(MoveForward());
        }

        // Start is called before the first frame update
        public void Start()
        {
           _sphereStartRotation = transform.rotation;
        }

        private IEnumerator MoveForward()
        {
            while (true)
            {
                transform.position += _moveSpeed * Time.deltaTime * transform.forward;
                yield return null;
            }
        }

        public void OnCollisionEnter(Collision _collision)
        {
            //Debug.Log("OnCollision");
            
            //Вектор, от которого поворачиваем
            Vector3 _beforeCollision = transform.forward;

            //Определение точки контакта
            ContactPoint _contact = _collision.GetContact(0);

            //Вектор, куда поворачиваем
            Vector3 _afterCollision = Vector3.Reflect(_beforeCollision, _contact.normal);

            //Присвоение поворота
            transform.forward = _afterCollision;

            //Блок отладки 
            /*
            Debug.Log("_beforeCollision " + _beforeCollision);
            Debug.Log("_afterCollision " + _afterCollision);
            Debug.Log("_contact.normal " + _contact.normal);
            */

            //Попытка отключить препятствия через делегат
            //Событие
            if (DesactivateObstacleEvent != null)
            {
                DesactivateObstacleEvent(_collision.transform);
            }
        }

        //Возращение шара после пропуска
        public void SphereReturn()
        {
            transform.position = _sphereStartLocation;
            transform.rotation = _sphereStartRotation;
            StopCoroutine(_moveSphereCoroutine);
            Debug.Log("возврат шара");
        }

        //Метод дискретного набора скорости после уничтожения препятствия до определённого уровня.
        public void IncreaseSpeed()
        {
            if (_moveSpeed < _maxMoveSpeed)
            {
                _moveSpeed = _moveSpeed + 0.1f;
                Debug.Log("Препятствие уничтожено. Скорость шара увеличена до " + _moveSpeed);
            }
            else
            {
                Debug.Log("Препятствие уничтожено. Достигнута максимальная скорость шара.");
            }
        }
    }
}
