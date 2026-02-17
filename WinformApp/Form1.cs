namespace WinformApp;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Button1_Click(object? sender, EventArgs e)
    {
        label1.Text = "This is update value";
    }
}
