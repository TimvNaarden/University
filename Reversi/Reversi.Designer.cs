using System.Windows.Forms;

namespace Reversi
{
    partial class Reversi
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
		private Button NewGameButton;
		private Button HelpButton;
		private Label RedStones;
		private Label BlueStones;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private CheckBox SizeCheckBox6;
		private CheckBox SizeCheckBox8;
		private CheckBox SizeCheckBox10;
		private CheckBox SizeCheckBoxC;
		private CheckBox Level1CheckBox;
		private CheckBox Level2CheckBox;
		private CheckBox Level3CheckBox;
		private CheckBox MultiplayerCheckBox;
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reversi));
			this.NewGameButton = new System.Windows.Forms.Button();
			this.HelpButton = new System.Windows.Forms.Button();
			this.RedStones = new System.Windows.Forms.Label();
			this.BlueStones = new System.Windows.Forms.Label();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.SizeCheckBox6 = new System.Windows.Forms.CheckBox();
			this.SizeCheckBox8 = new System.Windows.Forms.CheckBox();
			this.SizeCheckBox10 = new System.Windows.Forms.CheckBox();
			this.SizeCheckBoxC = new System.Windows.Forms.CheckBox();
			this.Level1CheckBox = new System.Windows.Forms.CheckBox();
			this.Level2CheckBox = new System.Windows.Forms.CheckBox();
			this.Level3CheckBox = new System.Windows.Forms.CheckBox();
			this.MultiplayerCheckBox = new System.Windows.Forms.CheckBox();
			this.TurnLabel = new System.Windows.Forms.Label();
			this.OpponentLabel = new System.Windows.Forms.Label();
			this.SizeLabel = new System.Windows.Forms.Label();
			this.SizeTextBox = new System.Windows.Forms.TextBox();
			this.MovesPlayed = new System.Windows.Forms.Label();
			this.ShowCaptures = new System.Windows.Forms.CheckBox();
			this.ScoreLabel = new System.Windows.Forms.Label();
			this.RedWinsLabel = new System.Windows.Forms.Label();
			this.BlueWinsLabel = new System.Windows.Forms.Label();
			this.TieLabel = new System.Windows.Forms.Label();
			this.ExportLogButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// NewGameButton
			// 
			this.NewGameButton.BackColor = System.Drawing.SystemColors.ControlDark;
			this.NewGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.NewGameButton.Location = new System.Drawing.Point(11, 10);
			this.NewGameButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.NewGameButton.Name = "NewGameButton";
			this.NewGameButton.Size = new System.Drawing.Size(158, 49);
			this.NewGameButton.TabIndex = 0;
			this.NewGameButton.Text = "Nieuw Spel";
			this.NewGameButton.UseVisualStyleBackColor = false;
			// 
			// HelpButton
			// 
			this.HelpButton.BackColor = System.Drawing.SystemColors.ControlDark;
			this.HelpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.HelpButton.Location = new System.Drawing.Point(13, 74);
			this.HelpButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.HelpButton.Name = "HelpButton";
			this.HelpButton.Size = new System.Drawing.Size(156, 48);
			this.HelpButton.TabIndex = 1;
			this.HelpButton.Text = "Help";
			this.HelpButton.UseVisualStyleBackColor = false;
			// 
			// RedStones
			// 
			this.RedStones.AutoSize = true;
			this.RedStones.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.RedStones.ForeColor = System.Drawing.Color.Firebrick;
			this.RedStones.Location = new System.Drawing.Point(261, 24);
			this.RedStones.Name = "RedStones";
			this.RedStones.Size = new System.Drawing.Size(80, 20);
			this.RedStones.TabIndex = 2;
			this.RedStones.Text = "2 Stenen ";
			// 
			// BlueStones
			// 
			this.BlueStones.AutoSize = true;
			this.BlueStones.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.BlueStones.ForeColor = System.Drawing.Color.Blue;
			this.BlueStones.Location = new System.Drawing.Point(175, 24);
			this.BlueStones.Name = "BlueStones";
			this.BlueStones.Size = new System.Drawing.Size(80, 20);
			this.BlueStones.TabIndex = 3;
			this.BlueStones.Text = "2 Stenen ";
			// 
			// SizeCheckBox6
			// 
			this.SizeCheckBox6.AutoSize = true;
			this.SizeCheckBox6.Checked = true;
			this.SizeCheckBox6.CheckState = System.Windows.Forms.CheckState.Checked;
			this.SizeCheckBox6.Location = new System.Drawing.Point(651, 41);
			this.SizeCheckBox6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SizeCheckBox6.Name = "SizeCheckBox6";
			this.SizeCheckBox6.Size = new System.Drawing.Size(49, 20);
			this.SizeCheckBox6.TabIndex = 5;
			this.SizeCheckBox6.Text = "6x6";
			this.SizeCheckBox6.UseVisualStyleBackColor = true;
			// 
			// SizeCheckBox8
			// 
			this.SizeCheckBox8.AutoSize = true;
			this.SizeCheckBox8.Location = new System.Drawing.Point(651, 65);
			this.SizeCheckBox8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SizeCheckBox8.Name = "SizeCheckBox8";
			this.SizeCheckBox8.Size = new System.Drawing.Size(49, 20);
			this.SizeCheckBox8.TabIndex = 6;
			this.SizeCheckBox8.Text = "8x8";
			this.SizeCheckBox8.UseVisualStyleBackColor = true;
			// 
			// SizeCheckBox10
			// 
			this.SizeCheckBox10.AutoSize = true;
			this.SizeCheckBox10.Location = new System.Drawing.Point(651, 89);
			this.SizeCheckBox10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SizeCheckBox10.Name = "SizeCheckBox10";
			this.SizeCheckBox10.Size = new System.Drawing.Size(63, 20);
			this.SizeCheckBox10.TabIndex = 7;
			this.SizeCheckBox10.Text = "10x10";
			this.SizeCheckBox10.UseVisualStyleBackColor = true;
			// 
			// SizeCheckBoxC
			// 
			this.SizeCheckBoxC.AutoSize = true;
			this.SizeCheckBoxC.Location = new System.Drawing.Point(651, 113);
			this.SizeCheckBoxC.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SizeCheckBoxC.Name = "SizeCheckBoxC";
			this.SizeCheckBoxC.Size = new System.Drawing.Size(95, 20);
			this.SizeCheckBoxC.TabIndex = 8;
			this.SizeCheckBoxC.Text = "Aangepast";
			this.SizeCheckBoxC.UseVisualStyleBackColor = true;
			// 
			// Level1CheckBox
			// 
			this.Level1CheckBox.AutoSize = true;
			this.Level1CheckBox.Location = new System.Drawing.Point(508, 40);
			this.Level1CheckBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Level1CheckBox.Name = "Level1CheckBox";
			this.Level1CheckBox.Size = new System.Drawing.Size(107, 20);
			this.Level1CheckBox.TabIndex = 9;
			this.Level1CheckBox.Text = "Bob (level 1) ";
			this.Level1CheckBox.UseVisualStyleBackColor = true;
			// 
			// Level2CheckBox
			// 
			this.Level2CheckBox.AutoSize = true;
			this.Level2CheckBox.Location = new System.Drawing.Point(508, 64);
			this.Level2CheckBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Level2CheckBox.Name = "Level2CheckBox";
			this.Level2CheckBox.Size = new System.Drawing.Size(107, 20);
			this.Level2CheckBox.TabIndex = 10;
			this.Level2CheckBox.Text = "Luuk (level 2)";
			this.Level2CheckBox.UseVisualStyleBackColor = true;
			// 
			// Level3CheckBox
			// 
			this.Level3CheckBox.AutoSize = true;
			this.Level3CheckBox.Location = new System.Drawing.Point(508, 88);
			this.Level3CheckBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Level3CheckBox.Name = "Level3CheckBox";
			this.Level3CheckBox.Size = new System.Drawing.Size(102, 20);
			this.Level3CheckBox.TabIndex = 11;
			this.Level3CheckBox.Text = "Piet (level 3)";
			this.Level3CheckBox.UseVisualStyleBackColor = true;
			// 
			// MultiplayerCheckBox
			// 
			this.MultiplayerCheckBox.AutoSize = true;
			this.MultiplayerCheckBox.Checked = true;
			this.MultiplayerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.MultiplayerCheckBox.Location = new System.Drawing.Point(508, 112);
			this.MultiplayerCheckBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.MultiplayerCheckBox.Name = "MultiplayerCheckBox";
			this.MultiplayerCheckBox.Size = new System.Drawing.Size(111, 20);
			this.MultiplayerCheckBox.TabIndex = 12;
			this.MultiplayerCheckBox.Text = "Tegen Vriend";
			this.MultiplayerCheckBox.UseVisualStyleBackColor = true;
			// 
			// TurnLabel
			// 
			this.TurnLabel.AutoSize = true;
			this.TurnLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TurnLabel.ForeColor = System.Drawing.Color.Red;
			this.TurnLabel.Location = new System.Drawing.Point(207, 137);
			this.TurnLabel.Name = "TurnLabel";
			this.TurnLabel.Size = new System.Drawing.Size(272, 32);
			this.TurnLabel.TabIndex = 13;
			this.TurnLabel.Text = "Rode Speler aan zet";
			// 
			// OpponentLabel
			// 
			this.OpponentLabel.AutoSize = true;
			this.OpponentLabel.Location = new System.Drawing.Point(505, 19);
			this.OpponentLabel.Name = "OpponentLabel";
			this.OpponentLabel.Size = new System.Drawing.Size(88, 16);
			this.OpponentLabel.TabIndex = 14;
			this.OpponentLabel.Text = "Tegestander:";
			// 
			// SizeLabel
			// 
			this.SizeLabel.AutoSize = true;
			this.SizeLabel.Location = new System.Drawing.Point(648, 19);
			this.SizeLabel.Name = "SizeLabel";
			this.SizeLabel.Size = new System.Drawing.Size(86, 16);
			this.SizeLabel.TabIndex = 15;
			this.SizeLabel.Text = "Bord Grootte:";
			// 
			// SizeTextBox
			// 
			this.SizeTextBox.BackColor = System.Drawing.Color.Gainsboro;
			this.SizeTextBox.Location = new System.Drawing.Point(752, 113);
			this.SizeTextBox.Name = "SizeTextBox";
			this.SizeTextBox.Size = new System.Drawing.Size(64, 22);
			this.SizeTextBox.TabIndex = 16;
			this.SizeTextBox.Text = "12";
			// 
			// MovesPlayed
			// 
			this.MovesPlayed.AutoSize = true;
			this.MovesPlayed.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
			this.MovesPlayed.Location = new System.Drawing.Point(176, 59);
			this.MovesPlayed.Name = "MovesPlayed";
			this.MovesPlayed.Size = new System.Drawing.Size(102, 16);
			this.MovesPlayed.TabIndex = 17;
			this.MovesPlayed.Text = "0 zetten gedaan";
			// 
			// ShowCaptures
			// 
			this.ShowCaptures.AutoSize = true;
			this.ShowCaptures.Location = new System.Drawing.Point(169, 90);
			this.ShowCaptures.Name = "ShowCaptures";
			this.ShowCaptures.Size = new System.Drawing.Size(183, 20);
			this.ShowCaptures.TabIndex = 18;
			this.ShowCaptures.Text = "Toon aantal veroveringen";
			this.ShowCaptures.UseVisualStyleBackColor = true;
			// 
			// ScoreLabel
			// 
			this.ScoreLabel.AutoSize = true;
			this.ScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ScoreLabel.Location = new System.Drawing.Point(359, 19);
			this.ScoreLabel.Name = "ScoreLabel";
			this.ScoreLabel.Size = new System.Drawing.Size(120, 25);
			this.ScoreLabel.TabIndex = 19;
			this.ScoreLabel.Text = "Scoreboord:";
			// 
			// RedWinsLabel
			// 
			this.RedWinsLabel.AutoSize = true;
			this.RedWinsLabel.Location = new System.Drawing.Point(361, 45);
			this.RedWinsLabel.Name = "RedWinsLabel";
			this.RedWinsLabel.Size = new System.Drawing.Size(121, 16);
			this.RedWinsLabel.TabIndex = 20;
			this.RedWinsLabel.Text = "Rood Gewonnen: 0";
			// 
			// BlueWinsLabel
			// 
			this.BlueWinsLabel.AutoSize = true;
			this.BlueWinsLabel.Location = new System.Drawing.Point(361, 62);
			this.BlueWinsLabel.Name = "BlueWinsLabel";
			this.BlueWinsLabel.Size = new System.Drawing.Size(123, 16);
			this.BlueWinsLabel.TabIndex = 21;
			this.BlueWinsLabel.Text = "Blauw Gewonnen: 0";
			// 
			// TieLabel
			// 
			this.TieLabel.AutoSize = true;
			this.TieLabel.Location = new System.Drawing.Point(361, 78);
			this.TieLabel.Name = "TieLabel";
			this.TieLabel.Size = new System.Drawing.Size(80, 16);
			this.TieLabel.TabIndex = 22;
			this.TieLabel.Text = "Gelijkspel: 0";
			// 
			// ExportLogButton
			// 
			this.ExportLogButton.Location = new System.Drawing.Point(596, 758);
			this.ExportLogButton.Name = "ExportLogButton";
			this.ExportLogButton.Size = new System.Drawing.Size(210, 30);
			this.ExportLogButton.TabIndex = 23;
			this.ExportLogButton.Text = "Exporteer Spel Verslag";
			this.ExportLogButton.UseVisualStyleBackColor = true;
			this.ExportLogButton.Click += new System.EventHandler(this.ExportLogButtonClick);
			// 
			// Reversi
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(818, 800);
			this.Controls.Add(this.ExportLogButton);
			this.Controls.Add(this.TieLabel);
			this.Controls.Add(this.BlueWinsLabel);
			this.Controls.Add(this.RedWinsLabel);
			this.Controls.Add(this.ScoreLabel);
			this.Controls.Add(this.ShowCaptures);
			this.Controls.Add(this.MovesPlayed);
			this.Controls.Add(this.SizeTextBox);
			this.Controls.Add(this.SizeLabel);
			this.Controls.Add(this.OpponentLabel);
			this.Controls.Add(this.TurnLabel);
			this.Controls.Add(this.MultiplayerCheckBox);
			this.Controls.Add(this.Level3CheckBox);
			this.Controls.Add(this.Level2CheckBox);
			this.Controls.Add(this.Level1CheckBox);
			this.Controls.Add(this.SizeCheckBoxC);
			this.Controls.Add(this.SizeCheckBox10);
			this.Controls.Add(this.SizeCheckBox8);
			this.Controls.Add(this.SizeCheckBox6);
			this.Controls.Add(this.BlueStones);
			this.Controls.Add(this.RedStones);
			this.Controls.Add(this.HelpButton);
			this.Controls.Add(this.NewGameButton);
			this.ForeColor = System.Drawing.Color.Black;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "Reversi";
			this.Text = "Reversi";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Label TurnLabel;
		private Label OpponentLabel;
		private Label SizeLabel;
		private TextBox SizeTextBox;
		private Label MovesPlayed;
		private CheckBox ShowCaptures;
		private Label ScoreLabel;
		private Label RedWinsLabel;
		private Label BlueWinsLabel;
		private Label TieLabel;
		private Button ExportLogButton;
	}
}

