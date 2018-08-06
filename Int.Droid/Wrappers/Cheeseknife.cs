/*
* Copyright 2014 Marcel Braghetto
* 
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
* 
* http://www.apache.org/licenses/LICENSE-2.0
* 
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
* 
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Int.Droid.Wrappers
{
    /// <summary>
    ///     Inject view attribute. Android widgets based on the
    ///     View super class can be resolved at runtime when
    ///     annotated with this attribute. This attribute is only
    ///     permitted on instance fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectView : Attribute
    {
        public InjectView(int resourceId)
        {
            ResourceId = resourceId;
        }

        public int ResourceId { get; }
    }

    /// <summary>
    ///     Inject click event handler onto an Android View.
    ///     Your method must have the following signature:
    ///     <para></para>
    ///     <para></para>
    ///     void SomeMethodName(object sender, EventArgs e) { ... }
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectOnClick : InjectView
    {
        public InjectOnClick(int resourceId) : base(resourceId)
        {
        }
    }

    /// <summary>
    ///     Inject touch event handler onto an Android View.
    ///     Your method must have the following signature:
    ///     <para></para>
    ///     <para></para>
    ///     void SomeMethodName(object sender, View.TouchEventArgs e) { ... }
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectOnTouch : InjectView
    {
        public InjectOnTouch(int resourceId) : base(resourceId)
        {
        }
    }

    /// <summary>
    ///     Inject item click event handler onto an Android AdapterView.
    ///     Your method must have the following signature:
    ///     <para></para>
    ///     <para></para>
    ///     void SomeMethodName(object sender, AdapterView.ItemClickEventArgs e) { ... }
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectOnItemClick : InjectView
    {
        public InjectOnItemClick(int resourceId) : base(resourceId)
        {
        }
    }

    /// <summary>
    ///     Cheeseknife exception which gets thrown when injection mappings
    ///     are wrong or fail ...
    /// </summary>
    public class CheeseknifeException : Exception
    {
        private const string Prefix = "Cheeseknife Exception: ";

        /// <summary>
        ///     Call this constructor with an Android view class type and a UI
        ///     event name to indicate that the view class is not compatible
        ///     with the particular event type specified.
        /// </summary>
        /// <param name="viewType">View type.</param>
        /// <param name="eventName">Event name.</param>
        public CheeseknifeException(Type viewType, string eventName) : base(
            GetViewTypeExceptionMessage(viewType, eventName))
        {
        }

        /// <summary>
        ///     Call this constructor with a list of required event type
        ///     parameters to indicate that the parameters couldn't be found
        ///     or matched to the signature of the user attributed method.
        /// </summary>
        /// <param name="requiredEventParameters">Required event types.</param>
        public CheeseknifeException(Type[] requiredEventParameters) : base(
            GetArgumentTypeExceptionMessage(requiredEventParameters))
        {
        }

        /// <summary>
        ///     Gets the view type exception message for an Android view class
        ///     that can't receive the specified event type.
        /// </summary>
        /// <returns>The view type exception message.</returns>
        /// <param name="viewType">View type.</param>
        /// <param name="eventName">Event name.</param>
        private static string GetViewTypeExceptionMessage(Type viewType, string eventName)
        {
            var sb = new StringBuilder();
            sb.Append(Prefix);
            sb.Append(" Incompatible Android view type specified for event '");
            sb.Append(eventName);
            sb.Append("', the Android view type '");
            sb.Append(viewType);
            sb.Append("' doesn't appear to support this event.");
            return sb.ToString();
        }

        /// <summary>
        ///     Gets the argument type exception message when the user attributed
        ///     method doesn't have the same number of parameters as the specified
        ///     event signature, or the parameter types don't match between the
        ///     event and user method.
        /// </summary>
        /// <returns>The argument type exception message.</returns>
        /// <param name="requiredEventParameters">Required event parameters.</param>
        private static string GetArgumentTypeExceptionMessage(Type[] requiredEventParameters)
        {
            var sb = new StringBuilder();
            sb.Append(Prefix);
            sb.Append(" Incorrect arguments in receiving method, should be => (");
            for (var i = 0; i < requiredEventParameters.Length; i++)
            {
                sb.Append(requiredEventParameters[i]);
                if (i < requiredEventParameters.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(")");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Cheeseknife! It's like a Butterknife with a weird shape!
    ///     <para></para>
    ///     <para></para>
    ///     Inspired by the extremely helpful Java based Butterknife
    ///     Android library, this helper class allows for easy Android
    ///     view and common event handler injections for Xamarin.Android.
    ///     This injection happens at runtime rather than compile time.
    /// </summary>
    public static class Cheeseknife
    {
        #region PRIVATE CONSTANTS

        private const BindingFlags InjectionBindingFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        #endregion

        private static readonly ConcurrentDictionary<Type, CacheObject> InternalCache =
            new ConcurrentDictionary<Type, CacheObject>();

        private static readonly MethodInfo ResolveMethod = typeof(Cheeseknife).GetMethod(
            MethodNameResolveAndroidView, BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly Type InjectType = typeof(InjectView);

        private static bool _initiated;

        public static void Initialize()
        {
            if (_initiated) return;
            _initiated = true;
            Task.Run(() =>
            {
                Parallel.ForEach(AppDomain.CurrentDomain.GetAssemblies()?.ElementAtOrDefault(1)?.GetTypes(), type =>
                {
                    var fields = GetAttributedFields(InjectType, type);
                    var properties = GetAttributedProperties(InjectType, type);

                    var fieldInfos = fields as FieldInfo[] ?? fields.ToArray();
                    var propertyInfos = properties as PropertyInfo[] ?? properties.ToArray();
                    if (!fieldInfos.Any() && !propertyInfos.Any()) return;
                    var cached = CreateCacheObject(fieldInfos, propertyInfos);
                    while (true)
                        if (InternalCache.TryAdd(type, cached)) break;
                });
            });
        }

        private struct Pair
        {
            private readonly int _resourceId;
            private readonly MethodInfo _methodInfo;
            private readonly Action<object, object> _setValue;

            public Pair(FieldInfo fieldInfo, int resourceId)
            {
                _resourceId = resourceId;
                _methodInfo = ResolveMethod.MakeGenericMethod(fieldInfo.FieldType);
                _setValue = fieldInfo.SetValue;
            }

            public Pair(PropertyInfo propertyInfo, int resourceId)
            {
                _resourceId = resourceId;
                _methodInfo = ResolveMethod.MakeGenericMethod(propertyInfo.PropertyType);
                _setValue = propertyInfo.SetValue;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void SetWidget(object parent, IDisposable view)
            {
                _setValue(parent, _methodInfo?.Invoke(parent, new object[] {view, _resourceId}));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void ResetWidget(object parent)
            {
                _setValue(parent, null);
            }
        }

        private class CacheObject
        {
            public readonly IReadOnlyList<Pair> Items = new List<Pair>();

            public void AddProperties(IEnumerable<PropertyInfo> properties)
            {
                (Items as List<Pair>)?.AddRange(properties.Select(prop =>
                    new Pair(prop, prop.GetCustomAttribute<InjectView>().ResourceId)));
            }

            public void AddFields(IEnumerable<FieldInfo> fields)
            {
                (Items as List<Pair>)?.AddRange(fields.Select(field =>
                    new Pair(field, field.GetCustomAttribute<InjectView>().ResourceId)));
            }
        }

        #region EVENT / METHOD CONSTANTS

        private const string MethodNameInvoke = "Invoke";
        private const string MethodNameResolveAndroidView = "ResolveAndroidView";

        #endregion

        #region PUBLIC API

        /// <summary>
        ///     Inject the specified parent activity, scanning all class
        ///     member fields and methods for injection attributions. The
        ///     assumption is that the activitie's 'Window.DecorView.RootView'
        ///     represents the root view in the layout hierarchy for the
        ///     given activity.
        ///     <para></para>
        ///     <para></para>
        ///     Sample activity usage:
        ///     <para></para>
        ///     <para></para>
        ///     [InjectView(Resource.Id.my_text_view)]
        ///     <para></para>
        ///     TextView myTextView;
        ///     <para></para>
        ///     <para></para>
        ///     [InjectOnClick(Resource.Id.my_button)]
        ///     <para></para>
        ///     void OnMyButtonClick(object sender, EventArgs e) {
        ///     <para></para>
        ///     . . . myTextView.Text = "I clicked my button!";
        ///     <para></para>
        ///     }
        ///     <para></para>
        ///     <para></para>
        ///     protected override void OnCreate(Bundle bundle) {
        ///     <para></para>
        ///     . . . base.OnCreate(bundle);
        ///     <para></para>
        ///     <para></para>
        ///     . . . SetContentView(Resource.Layout.Main);
        ///     <para></para>
        ///     . . . Cheeseknife.Inject(this);
        ///     <para></para>
        ///     <para></para>
        ///     . . . myTextView.Text = "I was injected!";
        ///     <para></para>
        ///     }
        ///     <para></para>
        /// </summary>
        /// <param name="parent">Parent.</param>
        public static void Inject(Activity parent)
        {
            InjectView(parent, parent.Window.DecorView.RootView);
        }

        /// <summary>
        ///     Inject the specified parent and view, scanning all class
        ///     member fields and methods for injection attributions.
        ///     This method would normally be called to inject a fragment
        ///     or other arbitrary view container. eg:
        ///     <para></para>
        ///     <para></para>
        ///     Fragment Example Usage:
        ///     <para></para>
        ///     <para></para>
        ///     In your OnCreateView method ...
        ///     <para></para>
        ///     var view = inflater.Inflate(Resource.Layout.fragment, null);
        ///     <para></para>
        ///     Cheeseknife.Inject(this, view);
        ///     <para></para>
        ///     return view;
        ///     <para></para>
        ///     <para></para>
        ///     In your OnDestroyView method ...
        ///     <para></para>
        ///     Cheeseknife.Reset(this);
        ///     <para></para>
        /// </summary>
        /// <param name="parent">Parent.</param>
        /// <param name="view">View.</param>
        public static void Inject(object parent, View view)
        {
            InjectView(parent, view);
        }

        /// <summary>
        ///     Reset the specified parent fields to null, which is useful
        ///     within the OnDestroyView fragment lifecycle method, particularly
        ///     if you are using RetainInstance = true.
        /// </summary>
        /// <param name="parent">Parent.</param>
        public static void Reset(object parent)
        {
            if (!InternalCache.ContainsKey(parent.GetType())) return;

            GetOrCreate(parent.GetType())?.Items?.ToList().ForEach(x => x.ResetWidget(parent));
        }

        #endregion

        #region PRIVATE API

        /// <summary>
        ///     Gets the attributed methods inside the parent object with
        ///     the matching type of attribute.
        /// </summary>
        /// <returns>The attributed methods.</returns>
        /// <param name="attributeType">Attribute type.</param>
        /// <param name="parent">Parent.</param>
        private static IEnumerable<MethodInfo> GetAttributedMethods(Type attributeType, object parent)
        {
            return parent.GetType().GetMethods(InjectionBindingFlags).Where(x => x.IsDefined(attributeType));
        }

        /// <summary>
        ///     Injects a method by mapping the appropriate event handler to
        ///     the user's attributed receiving method.
        /// </summary>
        /// <param name="attributeType">Attribute Type.</param>
        /// <param name="method">Method.</param>
        /// <param name="parent">Parent.</param>
        /// <param name="view">View.</param>
        /// <param name="eventName">Event name.</param>
        private static void InjectMethod(Type attributeType, MethodInfo method, object parent, View view,
            string eventName)
        {
            var attribute = method.GetCustomAttribute(attributeType, false) as InjectView;

            if (attribute == null)
                return;

            var widget = view.FindViewById<View>(attribute.ResourceId);

            var eventInfo = widget.GetType().GetEvent(eventName);
            if (eventInfo == null)
                throw new CheeseknifeException(widget.GetType(), eventName);

            var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
            var eventParameterTypes = eventInfo.EventHandlerType.GetMethod(MethodNameInvoke).GetParameters()
                .Select(p => p.ParameterType).ToArray();

            if (methodParameterTypes.Length != eventParameterTypes.Length)
                throw new CheeseknifeException(eventParameterTypes);

            if (methodParameterTypes.Where((t, i) => t != eventParameterTypes[i]).Any())
                throw new CheeseknifeException(eventParameterTypes);

            var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, parent, method);
            eventInfo.AddEventHandler(widget, handler);
        }

        private static Dictionary<Type, string> GetInjectableEvents()
        {
            return new Dictionary<Type, string>
            {
                {typeof(InjectOnClick), "Click"},
                {typeof(InjectOnItemClick), "ItemClick"},
                {typeof(InjectOnTouch), "Touch"}
            };
        }

        [Preserve]
        private static void InjectionEventPreserver()
        {
            new View(null).Click += (s, e) => { };
            new View(null).Touch += (s, e) => { };
            new ListView(null).ItemClick += (s, e) => { };
        }

        private static CacheObject GetOrCreate(Type type)
        {
            CacheObject cached;
            if (InternalCache.ContainsKey(type))
            {
                while (true)
                    if (InternalCache.TryGetValue(type, out cached)) break;
                return cached;
            }

            cached = CreateCacheObject(type);

            while (true)
                if (InternalCache.TryAdd(type, cached)) break;

            return cached;
        }

        private static CacheObject CreateCacheObject(Type type)
        {
            var cached = new CacheObject();
            cached.AddFields(GetAttributedFields(InjectType, type));
            cached.AddProperties(GetAttributedProperties(InjectType, type));
            return cached;
        }

        private static CacheObject CreateCacheObject(IEnumerable<FieldInfo> fields,
            IEnumerable<PropertyInfo> properties)
        {
            var cached = new CacheObject();
            cached.AddFields(fields);
            cached.AddProperties(properties);
            return cached;
        }

        /// <summary>
        ///     Gets the attributed fields inside the parent object with
        ///     the matching type of attribute.
        /// </summary>
        /// <returns>The attributed fields.</returns>
        /// <param name="attributeType">Attribute type.</param>
        /// <param name="parent">Parent.</param>
        private static IEnumerable<FieldInfo> GetAttributedFields(Type attributeType, object parent)
        {
            return parent.GetType().GetFields(InjectionBindingFlags).Where(x => x.IsDefined(attributeType));
        }

        /// <summary>
        ///     Gets the attributed fields inside the parent object with
        ///     the matching type of attribute.
        /// </summary>
        /// <returns>The attributed fields.</returns>
        /// <param name="attributeType">Attribute type.</param>
        /// <param name="parent">Parent.</param>
        private static IEnumerable<FieldInfo> GetAttributedFields(Type attributeType, Type parent)
        {
            return parent.GetFields(InjectionBindingFlags).Where(x => x.IsDefined(attributeType));
        }

        /// <summary>
        ///     Gets the attributed properties inside the parent object with
        ///     the matching type of attribute.
        /// </summary>
        /// <returns>The attributed properties.</returns>
        /// <param name="attributeType">Attribute type.</param>
        /// <param name="parent">Parent.</param>
        private static IEnumerable<PropertyInfo> GetAttributedProperties(Type attributeType, object parent)
        {
            return parent.GetType().GetProperties(InjectionBindingFlags).Where(x => x.IsDefined(attributeType));
        }

        /// <summary>
        ///     Gets the attributed properties inside the parent object with
        ///     the matching type of attribute.
        /// </summary>
        /// <returns>The attributed properties.</returns>
        /// <param name="attributeType">Attribute type.</param>
        /// <param name="parent">Parent.</param>
        private static IEnumerable<PropertyInfo> GetAttributedProperties(Type attributeType, Type parent)
        {
            return parent.GetProperties(InjectionBindingFlags).Where(x => x.IsDefined(attributeType));
        }

        /// <summary>
        ///     Resolves an android view to a specific view type. This is
        ///     needed to allow custom Android view classes to resolve
        ///     correctly (eg, Com.Android.Volley.NetworkImageView etc).
        /// </summary>
        /// <returns>The android view.</returns>
        /// <param name="view">Parent view to resolve view from.</param>
        /// <param name="resourceId">Resource identifier.</param>
        /// <typeparam name="T">The required specific Android view type.</typeparam>
        private static T ResolveAndroidView<T>(View view, int resourceId) where T : View
        {
            return view?.FindViewById<T>(resourceId);
        }

        /// <summary>
        ///     Injects the parent class by iterating over all of its
        ///     fields, properties and methods, checking if they have
        ///     injection attributes. For any fields/props/methods that
        ///     have injection attributes do the following:
        ///     <para></para>
        ///     <para></para>
        ///     1. If it is a field/prop -> attempt to resolve the actual
        ///     Android widget in the given view and assign it as the
        ///     field value, effectively 'injecting' it.
        ///     <para></para>
        ///     <para></para>
        ///     2. If it is a method -> attempt to apply an event
        ///     handler of the related type to the widget identified
        ///     by the resource id specified in the attribute. Some
        ///     widget types are verified before applying the events.
        /// </summary>
        /// <param name="parent">Parent.</param>
        /// <param name="view">View.</param>
        private static void InjectView(object parent, View view)
        {
            GetInjectableEvents().ToList().ForEach(x =>
            {
                GetAttributedMethods(x.Key, parent).ToList()
                    .ForEach(xx => InjectMethod(x.Key, xx, parent, view, x.Value));
            });

            GetOrCreate(parent.GetType())?.Items?.ToList().ForEach(x => x.SetWidget(parent, view));
        }

        #endregion
    }
}