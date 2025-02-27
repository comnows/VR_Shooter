using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class CheckPointDataModel
{
    [RealtimeProperty(1, true, true)]
    private Vector3 _respawnPointPos;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class CheckPointDataModel : RealtimeModel {
    public UnityEngine.Vector3 respawnPointPos {
        get {
            return _respawnPointPosProperty.value;
        }
        set {
            if (_respawnPointPosProperty.value == value) return;
            _respawnPointPosProperty.value = value;
            InvalidateReliableLength();
            FireRespawnPointPosDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(CheckPointDataModel model, T value);
    public event PropertyChangedHandler<UnityEngine.Vector3> respawnPointPosDidChange;
    
    public enum PropertyID : uint {
        RespawnPointPos = 1,
    }
    
    #region Properties
    
    private ReliableProperty<UnityEngine.Vector3> _respawnPointPosProperty;
    
    #endregion
    
    public CheckPointDataModel() : base(null) {
        _respawnPointPosProperty = new ReliableProperty<UnityEngine.Vector3>(1, _respawnPointPos);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _respawnPointPosProperty.UnsubscribeCallback();
    }
    
    private void FireRespawnPointPosDidChange(UnityEngine.Vector3 value) {
        try {
            respawnPointPosDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _respawnPointPosProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _respawnPointPosProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.RespawnPointPos: {
                    changed = _respawnPointPosProperty.Read(stream, context);
                    if (changed) FireRespawnPointPosDidChange(respawnPointPos);
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
            anyPropertiesChanged |= changed;
        }
        if (anyPropertiesChanged) {
            UpdateBackingFields();
        }
    }
    
    private void UpdateBackingFields() {
        _respawnPointPos = respawnPointPos;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */
