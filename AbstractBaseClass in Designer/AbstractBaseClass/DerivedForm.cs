namespace AbstractBaseClass
{
    using System.ComponentModel;
    using System.Windows.Forms;

    internal class DerivedForm : AbstractForm
    {
        private readonly IContainer components;
        private Button button1;

        public DerivedForm()
        {
            // This call is required by the Windows Form Designer.
            this.InitializeComponent();
        }

        public override string AbstractProperty { get; set; }

        /// <summary>
        ///   Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        ///   Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Text = "button1";
            this.button1.Size = new System.Drawing.Size(208, 131);
            this.button1.TabIndex = 0;
            this.button1.Click += this.Button1OnClick;
            // 
            // DerivedForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.button1);
            this.Name = "DerivedForm";
            this.ResumeLayout(false);

        }

        private void Button1OnClick(object sender, System.EventArgs e)
        {

        }
    }
}