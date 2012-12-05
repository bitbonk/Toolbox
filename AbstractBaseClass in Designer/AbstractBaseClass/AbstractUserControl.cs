namespace AbstractBaseClass
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [TypeDescriptionProvider(typeof(ConcreteUserControlClassProvider))]
    public abstract partial class AbstractUserControl : UserControl
    {
        public AbstractUserControl()
        {
            this.InitializeComponent();
        }

        public abstract string AbstractProperty { get; set; }
    }

    // Here is a concrete version of AbstractUserControl that acts as a stand in
    internal class ConcreteUserControl : AbstractUserControl
    {
        public override string AbstractProperty { get; set; }
    }

    // Here is our type description provider.  All our provider needs to
    // do is return ConcreteUserControl as the reflection type.
    internal class ConcreteUserControlClassProvider : TypeDescriptionProvider
    {
        // Because we only want to augment the metadata for AbstractUserControl, instead of
        // completely replace it, we pass into the base class the current type
        // description provider.  This is the provider that normally handles
        // metadata for AbstractUserControl.  By doing this all we have to do is
        // override the areas we want to change.
        public ConcreteUserControlClassProvider()
            : base(TypeDescriptor.GetProvider(typeof(AbstractUserControl)))
        {
        }

        // Tell anyone who reflects on us that the concrete UserControl is the
        // UserControl to reflect against, not the abstract UserControl. This way, the
        // designer does not see an abstract class.

        // If the designer tries to create an instance of AbstractUserControl, we override 
        // it here to create a concerete UserControl instead.
        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(AbstractUserControl))
            {
                objectType = typeof(ConcreteUserControl);
            }

            return base.CreateInstance(provider, objectType, argTypes, args);
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(AbstractUserControl))
            {
                return typeof(ConcreteUserControl);
            }
            return base.GetReflectionType(objectType, instance);
        }
    }
}