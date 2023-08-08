//#nullable enable
//using System;
//using UnityEngine;

//public interface IEntityViewModel
//{
//    Vector3 Position { get; set; }
//    Quaternion Rotation { get; set; }
//    [Obsolete]
//    Transform Transform { get; }

//    event ElapsedEventHandler? Updated;
//}

//public interface IAnimalViewModel : IEntityViewModel
//{
//    // ↓この辺のコンポーネントが露出しているの引っ込ませたいけど、簡便な方法が思いつかないので保留。
//    Animator Animator { get; }
//    Rigidbody Rigidbody { get; }
//    // ↑
//    float GaugeValue_sample { get; set; }
//}

//public interface ISpecies1ViewModel : IAnimalViewModel
//{
//}

//public interface IHumanoidViewModel : IAnimalViewModel
//{
//    // ↓この辺のコンポーネントが露出しているの引っ込ませたいけど、簡便な方法が思いつかないので保留。
//    TriggerSensor Foot { get; }
//    // ↑
//}

//public interface IPlayerViewModel : IHumanoidViewModel
//{
//    // ↓この辺のコンポーネントが露出しているの引っ込ませたいけど、簡便な方法が思いつかないので保留。
//    Camera Camera { get; }
//    // ↑

//    event InputEventHandler? Input;
//}