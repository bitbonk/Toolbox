namespace AbstractBaseClass
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [TypeDescriptionProvider(typeof (ConcreteClassProvider))]
    internal abstract partial class AbstractForm : Form
    {
        public AbstractForm()
        {
            this.InitializeComponent();
        }

        public abstract string AbstractProperty { get; set; }
    }

    // Here is a concrete version of AbstractForm that acts as a stand in
    internal class ConcreteForm : AbstractForm
    {
        public override string AbstractProperty { get; set; }
    }

    // Here is our type description provider.  All our provider needs to
    // do is return ConcreteForm as the reflection type.
    internal class ConcreteClassProvider : TypeDescriptionProvider
    {
        // Because we only want to augment the metadata for AbstractForm, instead of
        // completely replace it, we pass into the base class the current type
        // description provider.  This is the provider that normally handles
        // metadata for AbstractForm.  By doing this all we have to do is
        // override the areas we want to change.
        public ConcreteClassProvider() : base(TypeDescriptor.GetProvider(typeof (AbstractForm)))
        {
        }

        // Tell anyone who reflects on us that the concrete form is the
        // form to reflect against, not the abstract form. This way, the
        // designer does not see an abstract class.

        // If the designer tries to create an instance of AbstractForm, we override 
        // it here to create a concerete form instead.
        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof (AbstractForm))
            {
                objectType = typeof (ConcreteForm);
            }

            return base.CreateInstance(provider, objectType, argTypes, args);
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof (AbstractForm))
            {
                return typeof (ConcreteForm);
            }
            return base.GetReflectionType(objectType, instance);
        }
    }
}