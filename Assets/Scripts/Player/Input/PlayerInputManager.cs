using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
        public InputActions PlayerControls {  get; private set; }

        private void Awake()
        {
            /*if (Instance != null && Instance != this)
            {
               // Destroy(gameObject);
                return;
            }*/

            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            PlayerControls = new InputActions();
            PlayerControls.Enable();
        }

        private void OnDisable()
        {
            PlayerControls.Disable();
        }
    }

