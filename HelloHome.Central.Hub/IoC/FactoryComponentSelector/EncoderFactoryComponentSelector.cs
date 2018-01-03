using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Facilities.TypedFactory;
using HelloHome.Central.Common.Extensions;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders;

namespace HelloHome.Central.Hub.IoC.FactoryComponentSelector
{
    public class EncoderFactoryComponentSelector : DefaultTypedFactoryComponentSelector
    {
        private readonly Dictionary<Type, Type> _cache;

        public EncoderFactoryComponentSelector()
        {
            _cache = typeof(IMessageEncoder).Assembly.GetTypes()
                .Where(x => x.IsSubclassOfRawGeneric(typeof(MessageEncoder<>)) && x != typeof(MessageEncoder<>))
                .ToDictionary(x => x.BaseType.GetGenericArguments().Single());
        }

        protected override Type GetComponentType(MethodInfo method, object[] arguments)
        {
            var type = arguments[0].GetType();
            if(_cache.ContainsKey(type))
                return _cache[type];
            throw new Exception($"No encoder found for {type.Name}.");
        }
    }
}