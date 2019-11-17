using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using System;

[Serializable]
[ProtoContract]
public class ProtoGameDetail
{
    [ProtoMember(1)]
    public List<float> rotationClawArm { get; set; } = new List<float>();
    [ProtoMember(2)]
    public List<float> rotationStickArm { get; set; } = new List<float>();
    [ProtoMember(3)]
    public List<float> ClawBodyPosition { get; set; } = new List<float>();
    [ProtoMember(4)]
    public List<float> StickBodyPosition { get; set; } = new List<float>();
    [ProtoMember(5)]
    public List<bool> ClawController { get; set; } = new List<bool>();
    [ProtoMember(6)]
    public List<byte[]> ImageByteBlock { get; set; } = new List<byte[]>();
    [ProtoMember(7)]
    public List<byte[]> ImageByteBP { get; set; } = new List<byte[]>();
    [ProtoMember(8)]
    public List<Int32> CurrentBPScore { get; set; } = new List<Int32>();
    [ProtoMember(9)]
    public List<Int32> CurrentScore { get; set; } = new List<Int32>();
    [ProtoMember(10)]
    public List<bool> PlayerControls { get; set; } = new List<bool>();
    [ProtoMember(11)]
    public List<Int32> BlockCount { get; set; } = new List<Int32>();
    [ProtoMember(12)]
    public List<float> BlocksPosition { get; set; } = new List<float>();
    [ProtoMember(13)]
    public List<Int32> BunnyCount { get; set; } = new List<Int32>();
    [ProtoMember(14)]
    public List<float> BunniesPosition { get; set; } = new List<float>();
    [ProtoMember(15)]
    public string Name { get; set; }
    [ProtoMember(16)]
    public string FeedBack { get; set; }
    [ProtoMember(17)]
    public Int32 FinalScore { get; set; }
    [ProtoMember(18)]
    public List<bool> switchScene { get; set; } = new List<bool>();

}
