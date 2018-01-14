using System;
using System.Linq;
using Castle.Facilities.TypedFactory;
using HelloHome.Central.Common.Exceptions;
using HelloHome.Central.Common.Extensions;
using HelloHome.Central.Hub.Handlers;

namespace HelloHome.Central.Hub.IoC.FactoryComponentSelector
{
	public class MessageHandlerComponentSelector : DefaultTypedFactoryComponentSelector
	{
	    readonly ILookup<Type, Type> _typeLookup = null;

		public MessageHandlerComponentSelector ()
		{
			var types = typeof (MessageHandler<>)
				.Assembly
				.GetTypes ()
				.Where (x => x.IsSubclassOfRawGeneric(typeof(MessageHandler<>)) && x != typeof(MessageHandler<>))
				.ToList ();
			_typeLookup = types.ToLookup (x => x.BaseType.GetGenericArguments ().Single (), x => x);
		}

		protected override Type GetComponentType (System.Reflection.MethodInfo method, object [] arguments)
		{
			var msgType = arguments.Single().GetType();
			var typeGroup = _typeLookup.FirstOrDefault(_ => _.Key == msgType);
			if (typeGroup == null)
				throw new HelloHomeException($"No message handler found for {arguments[0].GetType().Name}");
			return typeGroup.Single();
		}
	}
}

