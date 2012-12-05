namespace AbstractBaseClass
{
    using System.ComponentModel;
    using System.Windows.Forms;

    [TypeDescriptionProvider(typeof (GeneralConcreteClassProvider))]
    [ConcreteClass(typeof (GeneralConcreteForm))]
    internal abstract partial class GeneralAbstractForm : Form
    {
        public GeneralAbstractForm()
        {
            this.InitializeComponent();
        }

        public abstract string AbstractProperty { get; set; }
    }

    // Here is a concrete version of AbstractForm that acts as a stand in
    internal class GeneralConcreteForm : GeneralAbstractForm
    {
        public override string AbstractProperty { get; set; }
    }
}