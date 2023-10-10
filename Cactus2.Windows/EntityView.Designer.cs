namespace Cactus2.Windows;

partial class EntityView
{
    /// <summary> 
    /// 必要なデザイナー変数です。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// 使用中のリソースをすべてクリーンアップします。
    /// </summary>
    /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region コンポーネント デザイナーで生成されたコード

    /// <summary> 
    /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
    /// コード エディターで変更しないでください。
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        Timer1 = new System.Windows.Forms.Timer(components);
        SuspendLayout();
        // 
        // Timer1
        // 
        Timer1.Tick += Timer1_Tick;
        // 
        // EntityView
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(0, 192, 0);
        Name = "EntityView";
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Timer Timer1;
}
