//#nullable enable
//using System;
//using System.Linq;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UI;
//using static System.MathF;
//using static ConstantValues;
//using static UnityEngine.Input;
//using static Utils;
//using vec = UnityEngine.Vector3;
//using qtn = UnityEngine.Quaternion;
//using System.Collections.Generic;

//public class LocalPlayerViewModel : HumanoidViewModel<IPlayer>
//{
//    public new Camera camera;

//    protected new void Start()
//    {
//        base.Start();

//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    protected new void Update()
//    {
//        base.Update();

//        Model.IsRunning = GetKey(KeyCode.LeftShift);
//        Model.Turn(GetAxis("Mouse X"), GetAxis("Mouse Y"));
//        Model.Seek(GetKey(KeyCode.W), GetKey(KeyCode.S), GetKey(KeyCode.D), GetKey(KeyCode.A), Time.deltaTime);
//        if (GetKeyDown(KeyCode.Space)) Model.Jump(1);
//        if (GetMouseButtonDown(0)) Model.Fire(Time.deltaTime);
//    }
//}