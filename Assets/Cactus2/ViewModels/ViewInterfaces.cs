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
//    // �����̕ӂ̃R���|�[�l���g���I�o���Ă���̈������܂��������ǁA�ȕւȕ��@���v�����Ȃ��̂ŕۗ��B
//    Animator Animator { get; }
//    Rigidbody Rigidbody { get; }
//    // ��
//    float GaugeValue_sample { get; set; }
//}

//public interface ISpecies1ViewModel : IAnimalViewModel
//{
//}

//public interface IHumanoidViewModel : IAnimalViewModel
//{
//    // �����̕ӂ̃R���|�[�l���g���I�o���Ă���̈������܂��������ǁA�ȕւȕ��@���v�����Ȃ��̂ŕۗ��B
//    TriggerSensor Foot { get; }
//    // ��
//}

//public interface IPlayerViewModel : IHumanoidViewModel
//{
//    // �����̕ӂ̃R���|�[�l���g���I�o���Ă���̈������܂��������ǁA�ȕւȕ��@���v�����Ȃ��̂ŕۗ��B
//    Camera Camera { get; }
//    // ��

//    event InputEventHandler? Input;
//}