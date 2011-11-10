using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Curiosity.Common.Mvc
{
    public class FormHandlerBus : IFormHandlerBus
    {
        private static IFormHandlerBus instance;
        private static readonly object lockObj = new object();

        private IFormHandlerFactory factory;
        private readonly IList<Type> supportedTypes = new List<Type>();

        public FormHandlerBus(IFormHandlerFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            this.factory = factory;
        }

        public static IFormHandlerBus Instance
        {
            get
            {
                InitializeDefaultBus();
                return instance;                
            }
            set { instance = value; }
        }

        public int NumberOfSupportedTypes
        {
            get { return supportedTypes.Count; }
        }

        public void AddFormHandlerType(Type formHandlerType)
        {
            if (formHandlerType == null)
            {
                throw new ArgumentNullException("formHandlerType");
            }
            if (formHandlerType.GetInterface(typeof(IFormHandler).Name) == null)
            {
                throw new ArgumentException(string.Format("Type {0} must implement the IFormHandler interface.", formHandlerType.Name));
            }
            if (!supportedTypes.Contains(formHandlerType))
            {
                supportedTypes.Add(formHandlerType);
            }
        }

        public void ClearFormHandlerTypes()
        {
            Debug.WriteLine("Clearing all form handler types.");
            supportedTypes.Clear();            
        }

        public bool ContainsFormHandler(Type formHandlerType)
        {
            return supportedTypes.Contains(formHandlerType);
        }

        public void SendForm(object form)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }
            var formType = form.GetType();
            foreach (var supportedType in GetFormHandlerTypeForFormType(formType))
            {
                supportedType.Handle(form);
            }
        }

        public void SetFormHandlerFactory(IFormHandlerFactory factory)
        {
            this.factory = factory;
        }

        public static bool IsValidFormHandler(Type type)
        {
            if (type == null || type.IsInterface || type.IsAbstract || type.IsNestedPrivate)
            {
                return false;
            }
            bool isFormHandler = type.GetInterface(typeof(IFormHandler).Name) != null;
            return isFormHandler;
        }

        private IEnumerable<IFormHandler> GetFormHandlerTypeForFormType(Type formType)
        {
            foreach (var handlerType in supportedTypes)
            {
                var handlerInstance = factory.CreateFormHandler(handlerType);
                if (handlerInstance.CanHandle(formType))
                {
                    yield return handlerInstance;
                }
            }
        }

        private static void InitializeDefaultBus()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        var factory = new DefaultFormHandlerFactory();
                        instance = new FormHandlerBus(factory);
                        AddAllFormHandlers();
                    }
                }
            }
        }

        private static void AddAllFormHandlers()
        {
            var formHandlers = FindAllFormHandlers();
            foreach (var formHandler in formHandlers)
            {
                Instance.AddFormHandlerType(formHandler);
            }
        }

        private static IEnumerable<Type> FindAllFormHandlers()
        {
            IEnumerable<Type> allFormHandlerTypes = Type.EmptyTypes;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types;                    
                }
                allFormHandlerTypes = allFormHandlerTypes.Concat(types);
            }
            var validHandlerTypes = allFormHandlerTypes.Where(t => IsValidFormHandler(t));
            return validHandlerTypes;
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return supportedTypes.GetEnumerator();            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
