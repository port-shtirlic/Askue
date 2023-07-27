namespace Application
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
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.comboBoxServerSelect = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.BtnLoad = new DevExpress.XtraBars.BarButtonItem();
            this.btnExportExcel = new DevExpress.XtraBars.BarButtonItem();
            this.DateEnd = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.DateBegin = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.hasTe = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.hasEe = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.hasGvs = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.hasHvs = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.labelConnectionStatus = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupMain = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupBtnLoad = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupBtnExcel = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupDatesPeriod = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupResources1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupResources2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.measureUnitTree = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemTimeEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.barButtonGroup2 = new DevExpress.XtraBars.BarButtonGroup();
            this.barButtonGroup1 = new DevExpress.XtraBars.BarButtonGroup();
            this.ribbonPageCategory1 = new DevExpress.XtraBars.Ribbon.RibbonPageCategory();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measureUnitTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.comboBoxServerSelect,
            this.BtnLoad,
            this.btnExportExcel,
            this.DateEnd,
            this.DateBegin,
            this.hasTe,
            this.hasEe,
            this.hasGvs,
            this.hasHvs,
            this.labelConnectionStatus});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 35;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2,
            this.repositoryItemCheckEdit1,
            this.repositoryItemCheckEdit2,
            this.repositoryItemCheckEdit3,
            this.repositoryItemCheckEdit4,
            this.repositoryItemMemoEdit1});
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowMoreCommandsButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(1708, 158);
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            // 
            // comboBoxServerSelect
            // 
            this.comboBoxServerSelect.AllowRightClickInMenu = false;
            this.comboBoxServerSelect.Caption = "Сервер ";
            this.comboBoxServerSelect.Edit = this.repositoryItemComboBox1;
            this.comboBoxServerSelect.EditWidth = 100;
            this.comboBoxServerSelect.Id = 1;
            this.comboBoxServerSelect.Name = "comboBoxServerSelect";
            this.comboBoxServerSelect.EditValueChanged += new System.EventHandler(this.comboBoxServerSelect_EditValueChanged);
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // BtnLoad
            // 
            this.BtnLoad.AllowRightClickInMenu = false;
            this.BtnLoad.Caption = "Загрузить";
            this.BtnLoad.Enabled = false;
            this.BtnLoad.Id = 5;
            this.BtnLoad.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnLoad.ImageOptions.Image")));
            this.BtnLoad.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("BtnLoad.ImageOptions.LargeImage")));
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnLoad_ItemClick);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.AllowRightClickInMenu = false;
            this.btnExportExcel.Caption = "Выгрузить в эксель";
            this.btnExportExcel.Enabled = false;
            this.btnExportExcel.Id = 10;
            this.btnExportExcel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnExportExcel.ImageOptions.Image")));
            this.btnExportExcel.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnExportExcel.ImageOptions.LargeImage")));
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExportExcel_ItemClick);
            // 
            // DateEnd
            // 
            this.DateEnd.AllowRightClickInMenu = false;
            this.DateEnd.Caption = "Дата кон.";
            this.DateEnd.Edit = this.repositoryItemDateEdit1;
            this.DateEnd.EditWidth = 100;
            this.DateEnd.Id = 20;
            this.DateEnd.Name = "DateEnd";
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.False;
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // DateBegin
            // 
            this.DateBegin.AllowRightClickInMenu = false;
            this.DateBegin.Caption = "Дата нач.";
            this.DateBegin.Edit = this.repositoryItemDateEdit2;
            this.DateBegin.EditWidth = 100;
            this.DateBegin.Id = 21;
            this.DateBegin.Name = "DateBegin";
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            // 
            // hasTe
            // 
            this.hasTe.AllowRightClickInMenu = false;
            this.hasTe.Caption = "Показания ТЭ";
            this.hasTe.Edit = this.repositoryItemCheckEdit1;
            this.hasTe.EditValue = true;
            this.hasTe.Id = 28;
            this.hasTe.Name = "hasTe";
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // hasEe
            // 
            this.hasEe.AllowRightClickInMenu = false;
            this.hasEe.Caption = "Показания ЭЭ";
            this.hasEe.Edit = this.repositoryItemCheckEdit2;
            this.hasEe.EditValue = true;
            this.hasEe.Id = 30;
            this.hasEe.Name = "hasEe";
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // hasGvs
            // 
            this.hasGvs.AllowRightClickInMenu = false;
            this.hasGvs.Caption = "Показания ГВС";
            this.hasGvs.Edit = this.repositoryItemCheckEdit3;
            this.hasGvs.EditValue = true;
            this.hasGvs.Id = 31;
            this.hasGvs.Name = "hasGvs";
            // 
            // repositoryItemCheckEdit3
            // 
            this.repositoryItemCheckEdit3.AutoHeight = false;
            this.repositoryItemCheckEdit3.Name = "repositoryItemCheckEdit3";
            // 
            // hasHvs
            // 
            this.hasHvs.AllowRightClickInMenu = false;
            this.hasHvs.Caption = "Показания ХВС";
            this.hasHvs.Edit = this.repositoryItemCheckEdit4;
            this.hasHvs.EditValue = true;
            this.hasHvs.Id = 32;
            this.hasHvs.Name = "hasHvs";
            // 
            // repositoryItemCheckEdit4
            // 
            this.repositoryItemCheckEdit4.AutoHeight = false;
            this.repositoryItemCheckEdit4.Name = "repositoryItemCheckEdit4";
            // 
            // labelConnectionStatus
            // 
            this.labelConnectionStatus.AllowRightClickInMenu = false;
            this.labelConnectionStatus.Edit = this.repositoryItemMemoEdit1;
            this.labelConnectionStatus.EditHeight = 50;
            this.labelConnectionStatus.EditValue = "Выберите сервер";
            this.labelConnectionStatus.EditWidth = 150;
            this.labelConnectionStatus.Id = 34;
            this.labelConnectionStatus.Name = "labelConnectionStatus";
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.LinesCount = 2;
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            this.repositoryItemMemoEdit1.ReadOnly = true;
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupMain,
            this.ribbonPageGroupBtnLoad,
            this.ribbonPageGroupBtnExcel,
            this.ribbonPageGroupDatesPeriod,
            this.ribbonPageGroupResources1,
            this.ribbonPageGroupResources2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Загрузка";
            // 
            // ribbonPageGroupMain
            // 
            this.ribbonPageGroupMain.ItemLinks.Add(this.comboBoxServerSelect);
            this.ribbonPageGroupMain.ItemLinks.Add(this.labelConnectionStatus);
            this.ribbonPageGroupMain.Name = "ribbonPageGroupMain";
            // 
            // ribbonPageGroupBtnLoad
            // 
            this.ribbonPageGroupBtnLoad.ItemLinks.Add(this.BtnLoad);
            this.ribbonPageGroupBtnLoad.Name = "ribbonPageGroupBtnLoad";
            // 
            // ribbonPageGroupBtnExcel
            // 
            this.ribbonPageGroupBtnExcel.ItemLinks.Add(this.btnExportExcel);
            this.ribbonPageGroupBtnExcel.Name = "ribbonPageGroupBtnExcel";
            // 
            // ribbonPageGroupDatesPeriod
            // 
            this.ribbonPageGroupDatesPeriod.ItemLinks.Add(this.DateBegin);
            this.ribbonPageGroupDatesPeriod.ItemLinks.Add(this.DateEnd);
            this.ribbonPageGroupDatesPeriod.Name = "ribbonPageGroupDatesPeriod";
            // 
            // ribbonPageGroupResources1
            // 
            this.ribbonPageGroupResources1.ItemLinks.Add(this.hasGvs);
            this.ribbonPageGroupResources1.ItemLinks.Add(this.hasHvs);
            this.ribbonPageGroupResources1.Name = "ribbonPageGroupResources1";
            // 
            // ribbonPageGroupResources2
            // 
            this.ribbonPageGroupResources2.ItemLinks.Add(this.hasTe);
            this.ribbonPageGroupResources2.ItemLinks.Add(this.hasEe);
            this.ribbonPageGroupResources2.Name = "ribbonPageGroupResources2";
            // 
            // measureUnitTree
            // 
            this.measureUnitTree.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.measureUnitTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.measureUnitTree.Location = new System.Drawing.Point(0, 158);
            this.measureUnitTree.Name = "measureUnitTree";
            this.measureUnitTree.OptionsBehavior.Editable = false;
            this.measureUnitTree.OptionsView.CheckBoxStyle = DevExpress.XtraTreeList.DefaultNodeCheckBoxStyle.Check;
            this.measureUnitTree.Size = new System.Drawing.Size(348, 690);
            this.measureUnitTree.TabIndex = 1;
            this.measureUnitTree.BeforeCheckNode += new DevExpress.XtraTreeList.CheckNodeEventHandler(this.measureUnitTree_BeforeCheckNode);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "Дерево объектов";
            this.treeListColumn1.FieldName = "FullName";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // gridControl
            // 
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Location = new System.Drawing.Point(348, 158);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(1360, 690);
            this.gridControl.TabIndex = 2;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.ReadOnly = true;
            // 
            // repositoryItemTimeEdit1
            // 
            this.repositoryItemTimeEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
            // 
            // barButtonGroup2
            // 
            this.barButtonGroup2.AllowRightClickInMenu = false;
            this.barButtonGroup2.Caption = "barButtonGroup2";
            this.barButtonGroup2.Id = 15;
            this.barButtonGroup2.Name = "barButtonGroup2";
            // 
            // barButtonGroup1
            // 
            this.barButtonGroup1.AllowRightClickInMenu = false;
            this.barButtonGroup1.Caption = "barButtonGroup1";
            this.barButtonGroup1.Id = 12;
            this.barButtonGroup1.Name = "barButtonGroup1";
            // 
            // ribbonPageCategory1
            // 
            this.ribbonPageCategory1.Name = "ribbonPageCategory1";
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Name = "ribbonPage2";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1708, 848);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.measureUnitTree);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "MainForm";
            this.Ribbon = this.ribbonControl1;
            this.Text = "Отчет АСКУЭ";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measureUnitTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupMain;
        private DevExpress.XtraTreeList.TreeList measureUnitTree;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraBars.BarEditItem comboBoxServerSelect;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarButtonItem BtnLoad;
        private DevExpress.XtraBars.BarButtonItem btnExportExcel;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupBtnLoad;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupBtnExcel;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraBars.BarEditItem DateEnd;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupDatesPeriod;
        private DevExpress.XtraBars.BarEditItem DateBegin;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupResources1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupResources2;
        private DevExpress.XtraBars.BarEditItem hasTe;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraBars.BarEditItem hasEe;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraBars.BarEditItem hasGvs;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit3;
        private DevExpress.XtraBars.BarEditItem hasHvs;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit4;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repositoryItemTimeEdit1;
        private DevExpress.XtraBars.BarButtonGroup barButtonGroup2;
        private DevExpress.XtraBars.BarButtonGroup barButtonGroup1;
        private DevExpress.XtraBars.BarEditItem labelConnectionStatus;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraBars.Ribbon.RibbonPageCategory ribbonPageCategory1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
    }
}

