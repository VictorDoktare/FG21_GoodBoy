using UnityEngine;

public class UIBillboard : MonoBehaviour
{
        private Camera _mainCam;

        private void Start()
        {
                _mainCam = Camera.main;
        }

        private void Update()
        {
                transform.rotation = Quaternion.LookRotation(transform.position - _mainCam.transform.position);
        }
}
