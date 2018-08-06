using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract;
using Int.Core.Application.Widget.Contract.Table.Adapter;
using Int.Core.Data.MVVM.Contract;
using Int.Core.Extensions;
using System.Diagnostics;

namespace Int.Core.Wrappers.Widget.CrossViewInjection
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CrossView : Attribute
    {
        public CrossView([CallerMemberName] string propertyName = null)
        {
            CrossViewName = propertyName;
        }

        public string CrossViewName { get; }
    }

    public abstract class CrossViewInjectorBase
    {
        private const string NullExceptionMessage = "{0} is null";
        private const string GetterIsForbidden = "Getting value from property is forbidden";
        private const string SetterIsForbidden = "Setting value to property is forbidden";

        protected CrossViewInjectorBase()
        {
        }

        protected CrossViewInjectorBase(IViewModelContainer vmContainer) : this()
        {
            ViewModelContainer = vmContainer;
            AssignCrossViewWrappersToVm();
        }

        protected CrossViewInjectorBase(ICrossCell crossCell) : this()
        {
            ViewModelContainer = crossCell;
            AssignCrossViewWrappersToVm();
        }

        protected object ViewModelContainer { get; }

        protected object ViewModel
        {
            get
            {
                var iBaseViewModel = (ViewModelContainer as IViewModelContainer)?.ModelView;
                var iCrossCellViewHolder = (ViewModelContainer as ICrossCell)?.CrossCellModel;

                return iBaseViewModel ?? iCrossCellViewHolder as object;
            }
        }

        protected abstract string GetStackTrace { get; }

        protected abstract IView GetWrapperForView(object view);
        protected abstract Type GetBaseType(Type type);

        protected void AssignCrossViewWrappersToVm()
        {
            // Reference for ViewModel saved for optimization
            // (otherwise it will be searched somwhere in service's dictionary).
            var vm = ViewModel;
            if (vm == null)
                throw new CrossViewInjectorException(string.Format(NullExceptionMessage, nameof(ViewModel)));

            if (ViewModelContainer == null)
                throw new CrossViewInjectorException(string.Format(NullExceptionMessage, nameof(ViewModelContainer)));

            var viewControllerProperties = FindCrossViewAttributedValues(ViewModelContainer?.GetType());
            var viewModelProperties = FindCrossViewAttributedValues(ViewModel?.GetType());

            foreach (var controllerProp in viewControllerProperties.Where(controllerProp =>
                !viewModelProperties.ContainsKey(controllerProp.Key)))
                ExceptionLogger.RaiseNonFatalException(new Exception(
                    $"{controllerProp.Key} cross view property declared in [{ViewModelContainer.GetType().Name}] was not found in [{vm.GetType().Name}]"));

            foreach (var vmProp in viewModelProperties.Where(
                vmProp => !viewControllerProperties.ContainsKey(vmProp.Key)))
                ExceptionLogger.RaiseNonFatalException(new Exception(
                    $"{vmProp.Key} cross view property declared in [{vm.GetType().Name}] was not found in [{ViewModelContainer.GetType().Name}]"));

            foreach (var item in viewModelProperties)
            {
                if (!viewControllerProperties.TryGetValue(item.Key, out var propertyWithValueToSet)) continue;
                if (item.Value.CanWrite)
                {
                    try
                    {
                        if (propertyWithValueToSet.CanRead)
                            item.Value.SetValue(ViewModel,
                                GetWrapperForView(propertyWithValueToSet?.GetValue(ViewModelContainer)));
                        else
                            ExceptionLogger.RaiseNonFatalException(
                                new CrossViewInjectorException(GetterIsForbidden));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                    ExceptionLogger.RaiseNonFatalException(
                        new CrossViewInjectorException(SetterIsForbidden));
            }
        }

        /// <summary>
        ///     Finds CrossView attributed properties.
        ///     Key - argument passed in CrossView attrubute.
        ///     Value - value of property.
        /// </summary>
        private Dictionary<string, PropertyInfo> FindCrossViewAttributedValues(Type type)
        {
            var result = new Dictionary<string, PropertyInfo>();

            if (type == null) return result;

            result = type.GetRuntimeProperties()
                         .Where(property => !property.GetCustomAttribute<CrossView>().IsNull())
                         .ToDictionary(
                             property => property.GetCustomAttribute<CrossView>()?.CrossViewName ?? string.Empty,
                             property => property) ?? result;

            var baseType = GetBaseType(type);
            if (baseType == null) return result;

            var parentAttributedFields = FindCrossViewAttributedValues(baseType);

            if ((parentAttributedFields?.Count ?? 0) > 0)
                foreach (var item in parentAttributedFields)
                    if (!result.ContainsKey(item.Key))
                        result.Add(item.Key, item.Value);

            return result;
        }
    }
}