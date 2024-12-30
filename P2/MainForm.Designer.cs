/* 
MIT License

Copyright (c) 2024 Maciej Aleksandrowicz

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace P2
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PreviewBox = new RichTextBox();
            MoveRight = new Button();
            MoveLeft = new Button();
            MoveUp = new Button();
            MoveDown = new Button();
            SimClockwise = new Button();
            SimMoveTo11 = new Button();
            SimMoveTo81 = new Button();
            SimMoveTo88 = new Button();
            SimMoveTo18 = new Button();
            loadRoundMap = new Button();
            panel1 = new Panel();
            loadNarrowPass = new Button();
            drawIntensions = new CheckBox();
            button1 = new Button();
            loadFullBorderMap = new Button();
            loadClockwiseMap = new Button();
            loadBorderMap = new Button();
            loadMazeMap = new Button();
            panel2 = new Panel();
            panel3 = new Panel();
            button2 = new Button();
            simAllClockwise = new Button();
            stopRepeat = new Button();
            repeat = new Button();
            simWaypoints = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // PreviewBox
            // 
            PreviewBox.BackColor = Color.Black;
            PreviewBox.Font = new Font("Cascadia Mono", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PreviewBox.ForeColor = Color.White;
            PreviewBox.Location = new Point(333, 134);
            PreviewBox.Name = "PreviewBox";
            PreviewBox.ReadOnly = true;
            PreviewBox.Size = new Size(403, 354);
            PreviewBox.TabIndex = 0;
            PreviewBox.Text = "Loading...";
            // 
            // MoveRight
            // 
            MoveRight.Location = new Point(154, 61);
            MoveRight.Name = "MoveRight";
            MoveRight.Size = new Size(58, 41);
            MoveRight.TabIndex = 1;
            MoveRight.Text = "Right";
            MoveRight.UseVisualStyleBackColor = true;
            MoveRight.Click += MoveRight_Click;
            // 
            // MoveLeft
            // 
            MoveLeft.Location = new Point(12, 61);
            MoveLeft.Name = "MoveLeft";
            MoveLeft.Size = new Size(55, 41);
            MoveLeft.TabIndex = 2;
            MoveLeft.Text = "Left";
            MoveLeft.UseVisualStyleBackColor = true;
            MoveLeft.Click += MoveLeft_Click;
            // 
            // MoveUp
            // 
            MoveUp.Location = new Point(82, 9);
            MoveUp.Name = "MoveUp";
            MoveUp.Size = new Size(61, 44);
            MoveUp.TabIndex = 3;
            MoveUp.Text = "Up";
            MoveUp.UseVisualStyleBackColor = true;
            MoveUp.Click += MoveUp_Click;
            // 
            // MoveDown
            // 
            MoveDown.Location = new Point(82, 61);
            MoveDown.Name = "MoveDown";
            MoveDown.Size = new Size(56, 42);
            MoveDown.TabIndex = 4;
            MoveDown.Text = "Down";
            MoveDown.UseVisualStyleBackColor = true;
            MoveDown.Click += MoveDown_Click;
            // 
            // SimClockwise
            // 
            SimClockwise.Location = new Point(45, 3);
            SimClockwise.Name = "SimClockwise";
            SimClockwise.Size = new Size(123, 38);
            SimClockwise.TabIndex = 5;
            SimClockwise.Text = "Sim Clockwise";
            SimClockwise.UseVisualStyleBackColor = true;
            SimClockwise.Click += SimClockwise_Click;
            // 
            // SimMoveTo11
            // 
            SimMoveTo11.Location = new Point(45, 47);
            SimMoveTo11.Name = "SimMoveTo11";
            SimMoveTo11.Size = new Size(123, 38);
            SimMoveTo11.TabIndex = 6;
            SimMoveTo11.Text = "Sim MoveTo(1,1)";
            SimMoveTo11.UseVisualStyleBackColor = true;
            SimMoveTo11.Click += SimMoveTo11_Click;
            // 
            // SimMoveTo81
            // 
            SimMoveTo81.Location = new Point(45, 91);
            SimMoveTo81.Name = "SimMoveTo81";
            SimMoveTo81.Size = new Size(123, 38);
            SimMoveTo81.TabIndex = 7;
            SimMoveTo81.Text = "Sim MoveTo(8,1)";
            SimMoveTo81.UseVisualStyleBackColor = true;
            SimMoveTo81.Click += SimMoveTo81_Click;
            // 
            // SimMoveTo88
            // 
            SimMoveTo88.Location = new Point(45, 135);
            SimMoveTo88.Name = "SimMoveTo88";
            SimMoveTo88.Size = new Size(123, 38);
            SimMoveTo88.TabIndex = 8;
            SimMoveTo88.Text = "Sim MoveTo(8,8)";
            SimMoveTo88.UseVisualStyleBackColor = true;
            SimMoveTo88.Click += SimMoveTo88_Click;
            // 
            // SimMoveTo18
            // 
            SimMoveTo18.Location = new Point(45, 179);
            SimMoveTo18.Name = "SimMoveTo18";
            SimMoveTo18.Size = new Size(123, 38);
            SimMoveTo18.TabIndex = 9;
            SimMoveTo18.Text = "Sim MoveTo(1,8)";
            SimMoveTo18.UseVisualStyleBackColor = true;
            SimMoveTo18.Click += SimMoveTo18_Click;
            // 
            // loadRoundMap
            // 
            loadRoundMap.Location = new Point(3, 3);
            loadRoundMap.Name = "loadRoundMap";
            loadRoundMap.Size = new Size(106, 49);
            loadRoundMap.TabIndex = 11;
            loadRoundMap.Text = "Load Round Map";
            loadRoundMap.UseVisualStyleBackColor = true;
            loadRoundMap.Click += loadRoundMap_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Bisque;
            panel1.Controls.Add(loadNarrowPass);
            panel1.Controls.Add(drawIntensions);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(loadFullBorderMap);
            panel1.Controls.Add(loadClockwiseMap);
            panel1.Controls.Add(loadBorderMap);
            panel1.Controls.Add(loadMazeMap);
            panel1.Controls.Add(loadRoundMap);
            panel1.Location = new Point(100, 269);
            panel1.Name = "panel1";
            panel1.Size = new Size(224, 275);
            panel1.TabIndex = 12;
            // 
            // loadNarrowPass
            // 
            loadNarrowPass.BackgroundImageLayout = ImageLayout.Stretch;
            loadNarrowPass.Location = new Point(3, 168);
            loadNarrowPass.Name = "loadNarrowPass";
            loadNarrowPass.Size = new Size(106, 49);
            loadNarrowPass.TabIndex = 18;
            loadNarrowPass.Text = " Load Narrow Pass";
            loadNarrowPass.UseVisualStyleBackColor = true;
            loadNarrowPass.Click += loadNarrowPass_Click;
            // 
            // drawIntensions
            // 
            drawIntensions.AutoSize = true;
            drawIntensions.Location = new Point(111, 253);
            drawIntensions.Name = "drawIntensions";
            drawIntensions.Size = new Size(110, 19);
            drawIntensions.TabIndex = 17;
            drawIntensions.Text = "Draw Intensions";
            drawIntensions.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.Location = new Point(115, 113);
            button1.Name = "button1";
            button1.Size = new Size(106, 49);
            button1.TabIndex = 16;
            button1.Text = " Load Clockwise";
            button1.UseVisualStyleBackColor = true;
            button1.Click += loadClockwise;
            // 
            // loadFullBorderMap
            // 
            loadFullBorderMap.BackgroundImageLayout = ImageLayout.Stretch;
            loadFullBorderMap.Location = new Point(3, 113);
            loadFullBorderMap.Name = "loadFullBorderMap";
            loadFullBorderMap.Size = new Size(106, 49);
            loadFullBorderMap.TabIndex = 15;
            loadFullBorderMap.Text = " Load Chaotic Clockwise";
            loadFullBorderMap.UseVisualStyleBackColor = true;
            loadFullBorderMap.Click += loadChaoticClockwise_Click;
            // 
            // loadClockwiseMap
            // 
            loadClockwiseMap.Location = new Point(115, 58);
            loadClockwiseMap.Name = "loadClockwiseMap";
            loadClockwiseMap.Size = new Size(106, 49);
            loadClockwiseMap.TabIndex = 14;
            loadClockwiseMap.Text = " Load Clockwise Map";
            loadClockwiseMap.UseVisualStyleBackColor = true;
            loadClockwiseMap.Click += loadClockwiseMap_Click;
            // 
            // loadBorderMap
            // 
            loadBorderMap.Location = new Point(3, 58);
            loadBorderMap.Name = "loadBorderMap";
            loadBorderMap.Size = new Size(106, 49);
            loadBorderMap.TabIndex = 13;
            loadBorderMap.Text = " Load Border Map";
            loadBorderMap.UseVisualStyleBackColor = true;
            loadBorderMap.Click += loadBorderMap_Click;
            // 
            // loadMazeMap
            // 
            loadMazeMap.Location = new Point(115, 3);
            loadMazeMap.Name = "loadMazeMap";
            loadMazeMap.Size = new Size(106, 49);
            loadMazeMap.TabIndex = 12;
            loadMazeMap.Text = "Load Maze Map";
            loadMazeMap.UseVisualStyleBackColor = true;
            loadMazeMap.Click += loadMazeMap_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.YellowGreen;
            panel2.Controls.Add(MoveUp);
            panel2.Controls.Add(MoveDown);
            panel2.Controls.Add(MoveLeft);
            panel2.Controls.Add(MoveRight);
            panel2.Location = new Point(100, 134);
            panel2.Name = "panel2";
            panel2.Size = new Size(224, 123);
            panel2.TabIndex = 13;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Gold;
            panel3.Controls.Add(simWaypoints);
            panel3.Controls.Add(button2);
            panel3.Controls.Add(simAllClockwise);
            panel3.Controls.Add(stopRepeat);
            panel3.Controls.Add(repeat);
            panel3.Controls.Add(SimMoveTo18);
            panel3.Controls.Add(SimClockwise);
            panel3.Controls.Add(SimMoveTo88);
            panel3.Controls.Add(SimMoveTo11);
            panel3.Controls.Add(SimMoveTo81);
            panel3.Location = new Point(742, 134);
            panel3.Name = "panel3";
            panel3.Size = new Size(205, 410);
            panel3.TabIndex = 14;
            // 
            // button2
            // 
            button2.Location = new Point(45, 267);
            button2.Name = "button2";
            button2.Size = new Size(123, 38);
            button2.TabIndex = 13;
            button2.Text = "Sim All Clockwise Lookup";
            button2.UseVisualStyleBackColor = true;
            button2.Click += simAllClockwiseLookup_Click;
            // 
            // simAllClockwise
            // 
            simAllClockwise.Location = new Point(45, 223);
            simAllClockwise.Name = "simAllClockwise";
            simAllClockwise.Size = new Size(123, 38);
            simAllClockwise.TabIndex = 12;
            simAllClockwise.Text = "Sim All Clockwise";
            simAllClockwise.UseVisualStyleBackColor = true;
            simAllClockwise.Click += simAllClockwise_Click;
            // 
            // stopRepeat
            // 
            stopRepeat.Location = new Point(144, 367);
            stopRepeat.Name = "stopRepeat";
            stopRepeat.Size = new Size(48, 40);
            stopRepeat.TabIndex = 11;
            stopRepeat.Text = "Stop";
            stopRepeat.UseVisualStyleBackColor = true;
            stopRepeat.Click += stopRepeat_Click;
            // 
            // repeat
            // 
            repeat.Location = new Point(13, 367);
            repeat.Name = "repeat";
            repeat.Size = new Size(123, 40);
            repeat.TabIndex = 10;
            repeat.Text = "Repeat";
            repeat.UseVisualStyleBackColor = true;
            repeat.Click += repeat_Click;
            // 
            // simWaypoints
            // 
            simWaypoints.Location = new Point(45, 308);
            simWaypoints.Name = "simWaypoints";
            simWaypoints.Size = new Size(123, 38);
            simWaypoints.TabIndex = 14;
            simWaypoints.Text = "Sim Waypoints";
            simWaypoints.UseVisualStyleBackColor = true;
            simWaypoints.Click += simWaypoints_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1060, 651);
            Controls.Add(PreviewBox);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(panel3);
            Name = "MainForm";
            Text = "Main Form";
            Load += MainForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox PreviewBox;
        private Button MoveRight;
        private Button MoveLeft;
        private Button MoveUp;
        private Button MoveDown;
        private Button SimClockwise;
        private Button SimMoveTo11;
        private Button SimMoveTo81;
        private Button SimMoveTo88;
        private Button SimMoveTo18;
        private Button loadRoundMap;
        private Panel panel1;
        private Button loadMazeMap;
        private Panel panel2;
        private Panel panel3;
        private Button loadBorderMap;
        private Button loadClockwiseMap;
        private Button repeat;
        private Button stopRepeat;
        private Button loadFullBorderMap;
        private Button simAllClockwise;
        private Button button1;
        private Button button2;
        private CheckBox drawIntensions;
        private Button loadNarrowPass;
        private Button simWaypoints;
    }
}
