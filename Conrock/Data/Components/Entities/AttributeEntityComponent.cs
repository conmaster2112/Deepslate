using ConMaster.Bedrock.Packets;

namespace ConMaster.Bedrock.Data.Components.Entities
{
    public abstract class AttributeEntityComponent: EntityComponent, IAttributeComponent, INetworkType
    {
        public AttributeEntityComponent(string id, Entity entity) : base(id, entity) { }
        public bool HasChanged { get; protected set; } = true;
        private float _MinValue;
        private float _MaxValue;
        private float _DefaultMinValue;
        private float _DefaultMaxValue;
        private float _CurrentValue;
        private float _DefaultValue;
        public readonly List<AttributeModifier> Modifiers = [];
        public float DefaultMaxValue { get => _DefaultMaxValue; init => _MaxValue = _DefaultMaxValue = value; }
        public float DefaultMinValue { get => _DefaultMinValue; init => _MinValue = _DefaultMinValue = value; }
        public float EffectiveMin { get => _MinValue; init => _MinValue = value; }
        public float EffectiveMax { get => _MaxValue; init => _MaxValue = value; }
        public float Default { get => _DefaultValue; init => _DefaultValue = value; }
        public float Current { get => _CurrentValue; init => _CurrentValue = value; }
        public void ResetToDefaultValue() => SetCurrentValue(_DefaultValue);
        public void ResetToMaxValue() => SetCurrentValue(_MaxValue);
        public void ResetToMinValue() => SetCurrentValue(_MinValue);
        public void SetCurrentValue(float value)
        {
            if(value > _MaxValue) value = _MaxValue;
            else if(value < _MinValue) value = _MinValue;
            Interlocked.Exchange(ref _CurrentValue, value);
            HasChanged = true;
            Entity.SetUpdateBitFor(Entity, Entity.UPDATE_ATTRIBUES_BIT);
        }
        public void ResetAll()
        {
            Interlocked.Exchange(ref _MinValue, _DefaultMinValue);
            Interlocked.Exchange(ref _MaxValue, _DefaultMaxValue);
            SetCurrentValue(_DefaultValue);
        }




        public void Read(ProtocolMemoryReader reader)
        {
            _MinValue = reader.ReadFloat();
            _MaxValue = reader.ReadFloat();
            _CurrentValue = reader.ReadFloat();
            _DefaultMinValue = reader.ReadFloat();
            _DefaultMaxValue = reader.ReadFloat();
            _DefaultValue = reader.ReadFloat();
            Id = reader.ReadVarString();
            Modifiers.Clear();
            reader.ReadVarArray(Modifiers);
        }
        public void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(_MinValue);
            writer.Write(_MaxValue);
            writer.Write(_CurrentValue);

            writer.Write(DefaultMinValue);
            writer.Write(DefaultMaxValue);
            writer.Write(_DefaultValue);
            writer.WriteVarString(Id);
            writer.WriteVarArray(Modifiers);
        }

        string IAttributeComponent.Id { get => Id; set => throw new NotImplementedException(); }
        float IAttributeComponent.MinValue { get => _MinValue; set => throw new NotImplementedException(); }
        float IAttributeComponent.MaxValue { get => _MaxValue; set => throw new NotImplementedException(); }
        float IAttributeComponent.CurrentValue { get => _CurrentValue; set => throw new NotImplementedException(); }
        float IAttributeComponent.DefaultValue { get => _DefaultValue; set => throw new NotImplementedException(); }
        float IAttributeComponent.DefaultMaxValue { get => _DefaultMaxValue; set => throw new NotImplementedException(); }
        float IAttributeComponent.DefaultMinValue { get => _DefaultMinValue; set => throw new NotImplementedException(); }
        IReadOnlyCollection<AttributeModifier> IAttributeComponent.Modifiers { get => Modifiers; set => throw new NotImplementedException(); }
        public override T CopyTo<T>(T component)
        {
            if(component is AttributeEntityComponent att)
            {
                MoveData(att);
                return component;
            }
            throw new Exception("Attribute types do not match");
        }
        public virtual void MoveData(AttributeEntityComponent component)
        {
            component.Id = Id;
            component._CurrentValue = _CurrentValue;
            component._DefaultValue = _DefaultValue;
            component._DefaultMaxValue = _DefaultMaxValue;
            component._DefaultMinValue = _DefaultMinValue;
            component._MaxValue = _MaxValue;
            component._MinValue = _MinValue;
        }
    }

}
