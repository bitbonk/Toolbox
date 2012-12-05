namespace AbstractBaseClass
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    // Place this attribute on any abstract class where you want to declare
    // a concrete version of that class at design time.
    [AttributeUsage(AttributeTargets.Class)]
    internal class ConcreteClassAttribute : Attribute
    {
        private readonly Type concreteType;

        public ConcreteClassAttribute(Type concreteType)
        {
            this.concreteType = concreteType;
        }

        public Type ConcreteType
        {
            get { return this.concreteType; }
        }
    }

    // Here is our type description provider.  This is the same provider
    // as ConcreteClassProvider except that it uses the ConcreteClassAttribute
    // to find the concrete class.
    internal class GeneralConcreteClassProvider : TypeDescriptionProvider
    {
        private Type abstractType;
        private Type concreteType;

        public GeneralConcreteClassProvider() : base(TypeDescriptor.GetProvider(typeof (Form)))
        {
        }

        // This method locates the abstract and concrete
        // types we should be returning.

        // If the designer tries to create an instance of AbstractForm, we override 
        // it here to create a concerete form instead.
        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            this.EnsureTypes(objectType);
            if (objectType == this.abstractType)
            {
                objectType = this.concreteType;
            }

            return base.CreateInstance(provider, objectType, argTypes, args);
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            this.EnsureTypes(objectType);
            if (objectType == this.abstractType)
            {
                return this.concreteType;
            }
            return base.GetReflectionType(objectType, instance);
        }

        private void EnsureTypes(Type objectType)
        {
            if (this.abstractType == null)
            {
                var searchType = objectType;
                while (this.abstractType == null && searchType != null && searchType != typeof (Object))
                {
                    foreach (
                        ConcreteClassAttribute cca in
                            searchType.GetCustomAttributes(typeof (ConcreteClassAttribute), false))
                    {
                        this.abstractType = searchType;
                        this.concreteType = cca.ConcreteType;
                        break;
                    }
                    searchType = searchType.BaseType;
                }

                if (this.abstractType == null)
                {
                    // If this happens, it means that someone added
                    // this provider to a class but did not add
                    // a ConcreteTypeAttribute
                    throw new InvalidOperationException(
                        string.Format("No ConcreteClassAttribute was found on {0} or any of its subtypes.", objectType));
                }
            }
        }
    }
}