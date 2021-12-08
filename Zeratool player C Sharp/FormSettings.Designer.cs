
namespace Zeratool_player_C_Sharp
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageDirectShow = new System.Windows.Forms.TabPage();
            this.btnRebuildGraph = new System.Windows.Forms.Button();
            this.groupBoxFilters = new System.Windows.Forms.GroupBox();
            this.btnSaveFilters = new System.Windows.Forms.Button();
            this.tabControlFilters = new System.Windows.Forms.TabControl();
            this.tabPageSplitters = new System.Windows.Forms.TabPage();
            this.comboBoxSplittersMKV = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxSplittersMPG = new System.Windows.Forms.ComboBox();
            this.comboBoxSplittersTS = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxSplittersMP4 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxSplittersAVI = new System.Windows.Forms.ComboBox();
            this.comboBoxSplittersOther = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageVideo = new System.Windows.Forms.TabPage();
            this.comboBoxVideoRenderers = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxVideoDecoders = new System.Windows.Forms.ComboBox();
            this.tabPageAudio = new System.Windows.Forms.TabPage();
            this.comboBoxAudioRenderers = new System.Windows.Forms.ComboBox();
            this.comboBoxAudioDecoders = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gbGraphDracula = new System.Windows.Forms.GroupBox();
            this.rbGraphModeManual = new System.Windows.Forms.RadioButton();
            this.rbGraphModeIntellectual = new System.Windows.Forms.RadioButton();
            this.rbGraphModeAutomatic = new System.Windows.Forms.RadioButton();
            this.comboBoxPlayers = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControlSettings.SuspendLayout();
            this.tabPageDirectShow.SuspendLayout();
            this.groupBoxFilters.SuspendLayout();
            this.tabControlFilters.SuspendLayout();
            this.tabPageSplitters.SuspendLayout();
            this.tabPageVideo.SuspendLayout();
            this.tabPageAudio.SuspendLayout();
            this.gbGraphDracula.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlSettings.Controls.Add(this.tabPageDirectShow);
            this.tabControlSettings.Location = new System.Drawing.Point(0, 29);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(584, 348);
            this.tabControlSettings.TabIndex = 0;
            // 
            // tabPageDirectShow
            // 
            this.tabPageDirectShow.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDirectShow.Controls.Add(this.btnRebuildGraph);
            this.tabPageDirectShow.Controls.Add(this.groupBoxFilters);
            this.tabPageDirectShow.Controls.Add(this.gbGraphDracula);
            this.tabPageDirectShow.Location = new System.Drawing.Point(4, 22);
            this.tabPageDirectShow.Name = "tabPageDirectShow";
            this.tabPageDirectShow.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDirectShow.Size = new System.Drawing.Size(576, 322);
            this.tabPageDirectShow.TabIndex = 0;
            this.tabPageDirectShow.Text = "DirectShow";
            // 
            // btnRebuildGraph
            // 
            this.btnRebuildGraph.Location = new System.Drawing.Point(345, 19);
            this.btnRebuildGraph.Name = "btnRebuildGraph";
            this.btnRebuildGraph.Size = new System.Drawing.Size(102, 23);
            this.btnRebuildGraph.TabIndex = 3;
            this.btnRebuildGraph.Text = "Перерендерить";
            this.btnRebuildGraph.UseVisualStyleBackColor = true;
            this.btnRebuildGraph.Click += new System.EventHandler(this.btnRebuildGraph_Click);
            // 
            // groupBoxFilters
            // 
            this.groupBoxFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFilters.Controls.Add(this.btnSaveFilters);
            this.groupBoxFilters.Controls.Add(this.tabControlFilters);
            this.groupBoxFilters.Location = new System.Drawing.Point(10, 61);
            this.groupBoxFilters.Name = "groupBoxFilters";
            this.groupBoxFilters.Size = new System.Drawing.Size(558, 253);
            this.groupBoxFilters.TabIndex = 1;
            this.groupBoxFilters.TabStop = false;
            this.groupBoxFilters.Text = "Декодеры и фильтры";
            // 
            // btnSaveFilters
            // 
            this.btnSaveFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveFilters.Location = new System.Drawing.Point(467, 224);
            this.btnSaveFilters.Name = "btnSaveFilters";
            this.btnSaveFilters.Size = new System.Drawing.Size(75, 23);
            this.btnSaveFilters.TabIndex = 12;
            this.btnSaveFilters.Text = "Сохранить";
            this.btnSaveFilters.UseVisualStyleBackColor = true;
            this.btnSaveFilters.Click += new System.EventHandler(this.btnSaveFilters_Click);
            // 
            // tabControlFilters
            // 
            this.tabControlFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlFilters.Controls.Add(this.tabPageSplitters);
            this.tabControlFilters.Controls.Add(this.tabPageVideo);
            this.tabControlFilters.Controls.Add(this.tabPageAudio);
            this.tabControlFilters.Location = new System.Drawing.Point(8, 16);
            this.tabControlFilters.Name = "tabControlFilters";
            this.tabControlFilters.SelectedIndex = 0;
            this.tabControlFilters.Size = new System.Drawing.Size(544, 202);
            this.tabControlFilters.TabIndex = 0;
            // 
            // tabPageSplitters
            // 
            this.tabPageSplitters.AutoScroll = true;
            this.tabPageSplitters.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSplitters.Controls.Add(this.comboBoxSplittersMKV);
            this.tabPageSplitters.Controls.Add(this.label11);
            this.tabPageSplitters.Controls.Add(this.comboBoxSplittersMPG);
            this.tabPageSplitters.Controls.Add(this.comboBoxSplittersTS);
            this.tabPageSplitters.Controls.Add(this.label10);
            this.tabPageSplitters.Controls.Add(this.label9);
            this.tabPageSplitters.Controls.Add(this.comboBoxSplittersMP4);
            this.tabPageSplitters.Controls.Add(this.label4);
            this.tabPageSplitters.Controls.Add(this.label3);
            this.tabPageSplitters.Controls.Add(this.comboBoxSplittersAVI);
            this.tabPageSplitters.Controls.Add(this.comboBoxSplittersOther);
            this.tabPageSplitters.Controls.Add(this.label1);
            this.tabPageSplitters.Location = new System.Drawing.Point(4, 22);
            this.tabPageSplitters.Name = "tabPageSplitters";
            this.tabPageSplitters.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSplitters.Size = new System.Drawing.Size(536, 176);
            this.tabPageSplitters.TabIndex = 0;
            this.tabPageSplitters.Text = "Сплиттеры";
            // 
            // comboBoxSplittersMKV
            // 
            this.comboBoxSplittersMKV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSplittersMKV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSplittersMKV.FormattingEnabled = true;
            this.comboBoxSplittersMKV.Location = new System.Drawing.Point(117, 115);
            this.comboBoxSplittersMKV.Name = "comboBoxSplittersMKV";
            this.comboBoxSplittersMKV.Size = new System.Drawing.Size(413, 21);
            this.comboBoxSplittersMKV.TabIndex = 11;
            this.comboBoxSplittersMKV.SelectedIndexChanged += new System.EventHandler(this.comboBoxSplittersMKV_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 118);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Сплиттер MKV:";
            // 
            // comboBoxSplittersMPG
            // 
            this.comboBoxSplittersMPG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSplittersMPG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSplittersMPG.FormattingEnabled = true;
            this.comboBoxSplittersMPG.Location = new System.Drawing.Point(116, 34);
            this.comboBoxSplittersMPG.Name = "comboBoxSplittersMPG";
            this.comboBoxSplittersMPG.Size = new System.Drawing.Size(414, 21);
            this.comboBoxSplittersMPG.TabIndex = 9;
            this.comboBoxSplittersMPG.SelectedIndexChanged += new System.EventHandler(this.comboBoxSplittersMPG_SelectedIndexChanged);
            // 
            // comboBoxSplittersTS
            // 
            this.comboBoxSplittersTS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSplittersTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSplittersTS.FormattingEnabled = true;
            this.comboBoxSplittersTS.Location = new System.Drawing.Point(116, 61);
            this.comboBoxSplittersTS.Name = "comboBoxSplittersTS";
            this.comboBoxSplittersTS.Size = new System.Drawing.Size(414, 21);
            this.comboBoxSplittersTS.TabIndex = 8;
            this.comboBoxSplittersTS.SelectedIndexChanged += new System.EventHandler(this.comboBoxSplittersTS_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Сплиттер TS:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Сплиттер MPG:";
            // 
            // comboBoxSplittersMP4
            // 
            this.comboBoxSplittersMP4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSplittersMP4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSplittersMP4.FormattingEnabled = true;
            this.comboBoxSplittersMP4.Location = new System.Drawing.Point(116, 88);
            this.comboBoxSplittersMP4.Name = "comboBoxSplittersMP4";
            this.comboBoxSplittersMP4.Size = new System.Drawing.Size(414, 21);
            this.comboBoxSplittersMP4.TabIndex = 5;
            this.comboBoxSplittersMP4.SelectedIndexChanged += new System.EventHandler(this.comboBoxSplittersMP4_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Сплиттер MP4:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Сплиттер AVI:";
            // 
            // comboBoxSplittersAVI
            // 
            this.comboBoxSplittersAVI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSplittersAVI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSplittersAVI.FormattingEnabled = true;
            this.comboBoxSplittersAVI.Location = new System.Drawing.Point(116, 7);
            this.comboBoxSplittersAVI.Name = "comboBoxSplittersAVI";
            this.comboBoxSplittersAVI.Size = new System.Drawing.Size(414, 21);
            this.comboBoxSplittersAVI.TabIndex = 2;
            this.comboBoxSplittersAVI.SelectedIndexChanged += new System.EventHandler(this.comboBoxSplittersAVI_SelectedIndexChanged);
            // 
            // comboBoxSplittersOther
            // 
            this.comboBoxSplittersOther.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSplittersOther.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSplittersOther.FormattingEnabled = true;
            this.comboBoxSplittersOther.Location = new System.Drawing.Point(116, 143);
            this.comboBoxSplittersOther.Name = "comboBoxSplittersOther";
            this.comboBoxSplittersOther.Size = new System.Drawing.Size(414, 21);
            this.comboBoxSplittersOther.TabIndex = 1;
            this.comboBoxSplittersOther.SelectedIndexChanged += new System.EventHandler(this.comboBoxSplittersOther_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Другие сплиттеры:";
            // 
            // tabPageVideo
            // 
            this.tabPageVideo.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageVideo.Controls.Add(this.comboBoxVideoRenderers);
            this.tabPageVideo.Controls.Add(this.label6);
            this.tabPageVideo.Controls.Add(this.label5);
            this.tabPageVideo.Controls.Add(this.comboBoxVideoDecoders);
            this.tabPageVideo.Location = new System.Drawing.Point(4, 22);
            this.tabPageVideo.Name = "tabPageVideo";
            this.tabPageVideo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVideo.Size = new System.Drawing.Size(536, 176);
            this.tabPageVideo.TabIndex = 1;
            this.tabPageVideo.Text = "Видео";
            // 
            // comboBoxVideoRenderers
            // 
            this.comboBoxVideoRenderers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVideoRenderers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVideoRenderers.FormattingEnabled = true;
            this.comboBoxVideoRenderers.Location = new System.Drawing.Point(98, 34);
            this.comboBoxVideoRenderers.Name = "comboBoxVideoRenderers";
            this.comboBoxVideoRenderers.Size = new System.Drawing.Size(432, 21);
            this.comboBoxVideoRenderers.TabIndex = 3;
            this.comboBoxVideoRenderers.SelectedIndexChanged += new System.EventHandler(this.comboBoxVideoRenderers_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Рендерер видео:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Декодер видео:";
            // 
            // comboBoxVideoDecoders
            // 
            this.comboBoxVideoDecoders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVideoDecoders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVideoDecoders.FormattingEnabled = true;
            this.comboBoxVideoDecoders.Location = new System.Drawing.Point(98, 7);
            this.comboBoxVideoDecoders.Name = "comboBoxVideoDecoders";
            this.comboBoxVideoDecoders.Size = new System.Drawing.Size(432, 21);
            this.comboBoxVideoDecoders.TabIndex = 0;
            this.comboBoxVideoDecoders.SelectedIndexChanged += new System.EventHandler(this.comboBoxVideoDecoders_SelectedIndexChanged);
            // 
            // tabPageAudio
            // 
            this.tabPageAudio.AutoScroll = true;
            this.tabPageAudio.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAudio.Controls.Add(this.comboBoxAudioRenderers);
            this.tabPageAudio.Controls.Add(this.comboBoxAudioDecoders);
            this.tabPageAudio.Controls.Add(this.label8);
            this.tabPageAudio.Controls.Add(this.label7);
            this.tabPageAudio.Location = new System.Drawing.Point(4, 22);
            this.tabPageAudio.Name = "tabPageAudio";
            this.tabPageAudio.Size = new System.Drawing.Size(536, 176);
            this.tabPageAudio.TabIndex = 2;
            this.tabPageAudio.Text = "Аудио";
            // 
            // comboBoxAudioRenderers
            // 
            this.comboBoxAudioRenderers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAudioRenderers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAudioRenderers.FormattingEnabled = true;
            this.comboBoxAudioRenderers.Location = new System.Drawing.Point(98, 34);
            this.comboBoxAudioRenderers.Name = "comboBoxAudioRenderers";
            this.comboBoxAudioRenderers.Size = new System.Drawing.Size(432, 21);
            this.comboBoxAudioRenderers.TabIndex = 3;
            this.comboBoxAudioRenderers.SelectedIndexChanged += new System.EventHandler(this.comboBoxAudioRenderers_SelectedIndexChanged);
            // 
            // comboBoxAudioDecoders
            // 
            this.comboBoxAudioDecoders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAudioDecoders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAudioDecoders.FormattingEnabled = true;
            this.comboBoxAudioDecoders.Location = new System.Drawing.Point(98, 7);
            this.comboBoxAudioDecoders.Name = "comboBoxAudioDecoders";
            this.comboBoxAudioDecoders.Size = new System.Drawing.Size(432, 21);
            this.comboBoxAudioDecoders.TabIndex = 2;
            this.comboBoxAudioDecoders.SelectedIndexChanged += new System.EventHandler(this.comboBoxAudioDecoders_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Рендерер аудио:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Декодер аудио:";
            // 
            // gbGraphDracula
            // 
            this.gbGraphDracula.Controls.Add(this.rbGraphModeManual);
            this.gbGraphDracula.Controls.Add(this.rbGraphModeIntellectual);
            this.gbGraphDracula.Controls.Add(this.rbGraphModeAutomatic);
            this.gbGraphDracula.Location = new System.Drawing.Point(8, 6);
            this.gbGraphDracula.Name = "gbGraphDracula";
            this.gbGraphDracula.Size = new System.Drawing.Size(331, 49);
            this.gbGraphDracula.TabIndex = 0;
            this.gbGraphDracula.TabStop = false;
            this.gbGraphDracula.Text = "Граф Дракула";
            // 
            // rbGraphModeManual
            // 
            this.rbGraphModeManual.AutoSize = true;
            this.rbGraphModeManual.Location = new System.Drawing.Point(258, 19);
            this.rbGraphModeManual.Name = "rbGraphModeManual";
            this.rbGraphModeManual.Size = new System.Drawing.Size(60, 17);
            this.rbGraphModeManual.TabIndex = 2;
            this.rbGraphModeManual.TabStop = true;
            this.rbGraphModeManual.Text = "Ручной";
            this.rbGraphModeManual.UseVisualStyleBackColor = true;
            this.rbGraphModeManual.CheckedChanged += new System.EventHandler(this.rbGraphModeManual_CheckedChanged);
            // 
            // rbGraphModeIntellectual
            // 
            this.rbGraphModeIntellectual.AutoSize = true;
            this.rbGraphModeIntellectual.Location = new System.Drawing.Point(130, 19);
            this.rbGraphModeIntellectual.Name = "rbGraphModeIntellectual";
            this.rbGraphModeIntellectual.Size = new System.Drawing.Size(122, 17);
            this.rbGraphModeIntellectual.TabIndex = 1;
            this.rbGraphModeIntellectual.TabStop = true;
            this.rbGraphModeIntellectual.Text = "Интеллектуальный";
            this.rbGraphModeIntellectual.UseVisualStyleBackColor = true;
            this.rbGraphModeIntellectual.CheckedChanged += new System.EventHandler(this.rbGraphModeIntellectual_CheckedChanged);
            // 
            // rbGraphModeAutomatic
            // 
            this.rbGraphModeAutomatic.AutoSize = true;
            this.rbGraphModeAutomatic.Location = new System.Drawing.Point(15, 19);
            this.rbGraphModeAutomatic.Name = "rbGraphModeAutomatic";
            this.rbGraphModeAutomatic.Size = new System.Drawing.Size(109, 17);
            this.rbGraphModeAutomatic.TabIndex = 0;
            this.rbGraphModeAutomatic.TabStop = true;
            this.rbGraphModeAutomatic.Text = "Автоматический";
            this.rbGraphModeAutomatic.UseVisualStyleBackColor = true;
            this.rbGraphModeAutomatic.CheckedChanged += new System.EventHandler(this.rbGraphModeAutomatic_CheckedChanged);
            // 
            // comboBoxPlayers
            // 
            this.comboBoxPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPlayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlayers.FormattingEnabled = true;
            this.comboBoxPlayers.Location = new System.Drawing.Point(46, 2);
            this.comboBoxPlayers.Name = "comboBoxPlayers";
            this.comboBoxPlayers.Size = new System.Drawing.Size(534, 21);
            this.comboBoxPlayers.TabIndex = 1;
            this.comboBoxPlayers.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlayers_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Плеер:";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 377);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxPlayers);
            this.Controls.Add(this.tabControlSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.Text = "Настройки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageDirectShow.ResumeLayout(false);
            this.groupBoxFilters.ResumeLayout(false);
            this.tabControlFilters.ResumeLayout(false);
            this.tabPageSplitters.ResumeLayout(false);
            this.tabPageSplitters.PerformLayout();
            this.tabPageVideo.ResumeLayout(false);
            this.tabPageVideo.PerformLayout();
            this.tabPageAudio.ResumeLayout(false);
            this.tabPageAudio.PerformLayout();
            this.gbGraphDracula.ResumeLayout(false);
            this.gbGraphDracula.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageDirectShow;
        private System.Windows.Forms.ComboBox comboBoxPlayers;
        private System.Windows.Forms.GroupBox gbGraphDracula;
        private System.Windows.Forms.RadioButton rbGraphModeManual;
        private System.Windows.Forms.RadioButton rbGraphModeIntellectual;
        private System.Windows.Forms.RadioButton rbGraphModeAutomatic;
        private System.Windows.Forms.Button btnRebuildGraph;
        private System.Windows.Forms.GroupBox groupBoxFilters;
        private System.Windows.Forms.TabControl tabControlFilters;
        private System.Windows.Forms.TabPage tabPageSplitters;
        private System.Windows.Forms.ComboBox comboBoxSplittersOther;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPageVideo;
        private System.Windows.Forms.TabPage tabPageAudio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSplittersMP4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxSplittersAVI;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxVideoDecoders;
        private System.Windows.Forms.ComboBox comboBoxVideoRenderers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxAudioRenderers;
        private System.Windows.Forms.ComboBox comboBoxAudioDecoders;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxSplittersMPG;
        private System.Windows.Forms.ComboBox comboBoxSplittersTS;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxSplittersMKV;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnSaveFilters;
    }
}
