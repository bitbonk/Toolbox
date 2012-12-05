namespace AbstractBaseClass
{
    public partial class DerivedUserControl : AbstractUserControl
    {
        public DerivedUserControl()
        {
            this.InitializeComponent();
        }

        public override string AbstractProperty { get; set; }
    }
}