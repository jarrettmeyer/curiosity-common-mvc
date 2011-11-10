using System;
using System.Collections.Generic;

namespace Curiosity.Common.Mvc
{
    public class FormHandlerFactory : List<Type>
    {
        private static volatile IFormHandlerFactory factory;
        private static readonly object lockObj = new object();

        public static IFormHandlerFactory Instance
        {
            get
            {
                InitializeDefaultFactory();
                return factory;
            }
            set { factory = value; }
        }

        public void AddFormHandler(Type type)
        {
            Instance.Add(type);
        }

        private static void InitializeDefaultFactory()
        {
            if (factory == null)
            {
                lock(lockObj)
                {
                    if (factory == null)
                    {
                        factory = new DefaultFormHandlerFactory();
                    }
                }
            }
        }
    }
}
