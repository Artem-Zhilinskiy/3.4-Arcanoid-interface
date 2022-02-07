using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Arcanoid
{
    public class Platform2InputControls : MonoBehaviour
    {
        //Импорт настроек из GameManager
        //Объявление переменной, чтобы можно было получить доступ в других методах. Надо в инспекторе Unity добавить.
        [SerializeField] private GameManager GameManager;

        //1. Первое что нужно сделать при создании скрипта управления в NewInputSystem
        public PlatformControls1 controls;

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
            //controls.ActionMap.Movement.performed += OnMovement;
            controls.ActionMap.Launch.performed += OnLaunch;
        }

        //4. Дезактивировать в OnDisable
        private void OnDisable()
        {
            controls.ActionMap.Launch.performed -= OnLaunch;
            controls.ActionMap.Disable();
        }

        //5. Написать обработчики событий
        // В OnMovement ничего не писать, потому что движение обрабатывается в Update
        // Если нужна инерция, то надо работать через Rigidbody
        /*
        public void OnMovement (CallbackContext context)
        {
            //Дальше перейти в Update.
            //Для инерции надо сделать
            //GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            var value = controls.ActionMap.Movement2Platform.ReadValue<Vector2>();
            Vector3 direction = new Vector3(0, value.y, value.x);
            //GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            GetComponent<Rigidbody>().AddForce(direction, GameManager._platformAcceleration);
        }
        */

        public void OnLaunch(CallbackContext context)
        {

        }

        // Start is called before the first frame update
        public void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Movement();
            /*
            //6. Обработка движения
            var value = controls.ActionMap.Movement.ReadValue<Vector2>();
            transform.position += new Vector3(0, value.y, value.x) * Time.deltaTime * GameManager._platformSpeed;
            //transform.position += new Vector3(0, value.y, value.x) * Time.deltaTime * GameObject.Find("GameManager").GetComponent<GameManager>()._platformSpeed;
            */
        }

        private void Movement()
        {
            //Дальше перейти в Update.
            //Для инерции надо сделать
            //GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            var value = controls.ActionMap.Movement2Platform.ReadValue<Vector2>();
            //-z ставить, потому что платформа расположена зеркально к первой.
            Vector3 direction = new Vector3(0, value.y, -value.x);
            //GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            GetComponent<Rigidbody>().AddForce(direction, GameManager._platformAcceleration);
        }
    }
}
