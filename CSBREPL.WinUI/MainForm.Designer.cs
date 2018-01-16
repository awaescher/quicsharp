namespace CSBREPL.WinUI
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.txtCode = new EasyScintilla.SimpleEditor();
			this.txtOut = new EasyScintilla.SimpleEditor();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtCode
			// 
			this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCode.Location = new System.Drawing.Point(0, 0);
			this.txtCode.Name = "txtCode";
			this.txtCode.Size = new System.Drawing.Size(946, 360);
			this.txtCode.Styler = null;
			this.txtCode.TabIndex = 0;
			this.txtCode.Text = resources.GetString("txtCode.Text");
			// 
			// txtOut
			// 
			this.txtOut.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtOut.Location = new System.Drawing.Point(0, 0);
			this.txtOut.Name = "txtOut";
			this.txtOut.Size = new System.Drawing.Size(946, 231);
			this.txtOut.Styler = null;
			this.txtOut.TabIndex = 0;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.txtCode);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.txtOut);
			this.splitContainer1.Size = new System.Drawing.Size(946, 595);
			this.splitContainer1.SplitterDistance = 360;
			this.splitContainer1.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(946, 595);
			this.Controls.Add(this.splitContainer1);
			this.KeyPreview = true;
			this.Name = "MainForm";
			this.Text = "CSBREPL";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private EasyScintilla.SimpleEditor txtCode;
		private EasyScintilla.SimpleEditor txtOut;
		private System.Windows.Forms.SplitContainer splitContainer1;
	}
}

