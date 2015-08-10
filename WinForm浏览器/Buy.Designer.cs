namespace MyWebBrowser
{
    partial class Buy
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblCost = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblRemainder = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "请投币……";
            // 
            // lblCost
            // 
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(47, 41);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(41, 12);
            this.lblCost.TabIndex = 1;
            this.lblCost.Text = "应投币";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(156, 41);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(41, 12);
            this.lblAmount.TabIndex = 2;
            this.lblAmount.Text = "已投币";
            // 
            // lblRemainder
            // 
            this.lblRemainder.AutoSize = true;
            this.lblRemainder.Location = new System.Drawing.Point(250, 41);
            this.lblRemainder.Name = "lblRemainder";
            this.lblRemainder.Size = new System.Drawing.Size(41, 12);
            this.lblRemainder.TabIndex = 3;
            this.lblRemainder.Text = "已投币";
            // 
            // Buy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lblRemainder);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblCost);
            this.Controls.Add(this.label1);
            this.Name = "Buy";
            this.Size = new System.Drawing.Size(544, 326);
            this.Load += new System.EventHandler(this.Buy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lblCost;
        public System.Windows.Forms.Label lblAmount;
        public System.Windows.Forms.Label lblRemainder;
    }
}
