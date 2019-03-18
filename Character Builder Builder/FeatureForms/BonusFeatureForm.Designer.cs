namespace Character_Builder_Builder.FeatureForms
{
    partial class BonusFeatureForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BonusFeatureForm));
            this.button1 = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Condition = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.allowOffhand = new System.Windows.Forms.CheckBox();
            this.BonusSize = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.profBonus = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.saveBonus = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.DamageText = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.Damage = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.Initiative = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.AC = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SaveDC = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.Attack = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.BaseAbility = new System.Windows.Forms.CheckedListBox();
            this.label19 = new System.Windows.Forms.Label();
            this.SaveBonusAbility = new System.Windows.Forms.CheckedListBox();
            this.label21 = new System.Windows.Forms.Label();
            this.offhandAbility = new System.Windows.Forms.CheckBox();
            this.label23 = new System.Windows.Forms.Label();
            this.Spell = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.basicFeature1 = new Character_Builder_Builder.FeatureForms.BasicFeature();
            this.label26 = new System.Windows.Forms.Label();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.SkillList = new Character_Builder_Builder.StringList();
            this.label18 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Skill = new System.Windows.Forms.TextBox();
            this.Passive = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.proficiencyOptions = new Character_Builder_Builder.StringList();
            this.label24 = new System.Windows.Forms.Label();
            this.attackOption = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BonusSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(2, 748);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(743, 25);
            this.button1.TabIndex = 18;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.AutoEllipsis = true;
            this.label20.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label20.Location = new System.Drawing.Point(2, 735);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(743, 13);
            this.label20.TabIndex = 45;
            this.label20.Text = "The bonus values have similar inputs, but must result in a numerical value.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(2, 280);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 13);
            this.label1.TabIndex = 56;
            this.label1.Text = "Condition for Bonus: (NCalc Expression)";
            // 
            // Condition
            // 
            this.Condition.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Condition.Location = new System.Drawing.Point(2, 293);
            this.Condition.Name = "Condition";
            this.Condition.Size = new System.Drawing.Size(743, 20);
            this.Condition.TabIndex = 55;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer1.Location = new System.Drawing.Point(2, 313);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer1.Panel2.Controls.Add(this.attackOption);
            this.splitContainer1.Panel2.Controls.Add(this.label26);
            this.splitContainer1.Size = new System.Drawing.Size(743, 318);
            this.splitContainer1.SplitterDistance = 368;
            this.splitContainer1.TabIndex = 54;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.allowOffhand);
            this.splitContainer2.Panel1.Controls.Add(this.BonusSize);
            this.splitContainer2.Panel1.Controls.Add(this.label25);
            this.splitContainer2.Panel1.Controls.Add(this.profBonus);
            this.splitContainer2.Panel1.Controls.Add(this.label22);
            this.splitContainer2.Panel1.Controls.Add(this.saveBonus);
            this.splitContainer2.Panel1.Controls.Add(this.label16);
            this.splitContainer2.Panel1.Controls.Add(this.DamageText);
            this.splitContainer2.Panel1.Controls.Add(this.label15);
            this.splitContainer2.Panel1.Controls.Add(this.Damage);
            this.splitContainer2.Panel1.Controls.Add(this.label14);
            this.splitContainer2.Panel1.Controls.Add(this.Initiative);
            this.splitContainer2.Panel1.Controls.Add(this.label13);
            this.splitContainer2.Panel1.Controls.Add(this.AC);
            this.splitContainer2.Panel1.Controls.Add(this.label12);
            this.splitContainer2.Panel1.Controls.Add(this.SaveDC);
            this.splitContainer2.Panel1.Controls.Add(this.label11);
            this.splitContainer2.Panel1.Controls.Add(this.Attack);
            this.splitContainer2.Panel1.Controls.Add(this.label10);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Panel2.Controls.Add(this.offhandAbility);
            this.splitContainer2.Panel2.Controls.Add(this.label23);
            this.splitContainer2.Panel2.Controls.Add(this.Spell);
            this.splitContainer2.Size = new System.Drawing.Size(368, 318);
            this.splitContainer2.SplitterDistance = 185;
            this.splitContainer2.TabIndex = 54;
            // 
            // allowOffhand
            // 
            this.allowOffhand.AutoSize = true;
            this.allowOffhand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.allowOffhand.Location = new System.Drawing.Point(0, 301);
            this.allowOffhand.Name = "allowOffhand";
            this.allowOffhand.Size = new System.Drawing.Size(185, 17);
            this.allowOffhand.TabIndex = 77;
            this.allowOffhand.Text = "Allows Offhand Weapon";
            this.allowOffhand.UseVisualStyleBackColor = true;
            // 
            // BonusSize
            // 
            this.BonusSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BonusSize.Location = new System.Drawing.Point(0, 277);
            this.BonusSize.Name = "BonusSize";
            this.BonusSize.Size = new System.Drawing.Size(185, 20);
            this.BonusSize.TabIndex = 76;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Dock = System.Windows.Forms.DockStyle.Top;
            this.label25.Location = new System.Drawing.Point(0, 264);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(152, 13);
            this.label25.TabIndex = 75;
            this.label25.Text = "Bonus to Size (Carry Capacity):";
            // 
            // profBonus
            // 
            this.profBonus.Dock = System.Windows.Forms.DockStyle.Top;
            this.profBonus.Location = new System.Drawing.Point(0, 244);
            this.profBonus.Name = "profBonus";
            this.profBonus.Size = new System.Drawing.Size(185, 20);
            this.profBonus.TabIndex = 74;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Dock = System.Windows.Forms.DockStyle.Top;
            this.label22.Location = new System.Drawing.Point(0, 231);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(177, 13);
            this.label22.TabIndex = 73;
            this.label22.Text = "Bonus to Proficiency bonus: (NCalc)";
            // 
            // saveBonus
            // 
            this.saveBonus.Dock = System.Windows.Forms.DockStyle.Top;
            this.saveBonus.Location = new System.Drawing.Point(0, 211);
            this.saveBonus.Name = "saveBonus";
            this.saveBonus.Size = new System.Drawing.Size(185, 20);
            this.saveBonus.TabIndex = 72;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Top;
            this.label16.Location = new System.Drawing.Point(0, 198);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(166, 13);
            this.label16.TabIndex = 71;
            this.label16.Text = "Bonus to selected Saves: (NCalc)";
            // 
            // DamageText
            // 
            this.DamageText.Dock = System.Windows.Forms.DockStyle.Top;
            this.DamageText.Location = new System.Drawing.Point(0, 178);
            this.DamageText.Name = "DamageText";
            this.DamageText.Size = new System.Drawing.Size(185, 20);
            this.DamageText.TabIndex = 70;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Top;
            this.label15.Location = new System.Drawing.Point(0, 165);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(174, 13);
            this.label15.TabIndex = 69;
            this.label15.Text = "Bonus to Damage (Displayed Text):";
            // 
            // Damage
            // 
            this.Damage.Dock = System.Windows.Forms.DockStyle.Top;
            this.Damage.Location = new System.Drawing.Point(0, 145);
            this.Damage.Name = "Damage";
            this.Damage.Size = new System.Drawing.Size(185, 20);
            this.Damage.TabIndex = 68;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Top;
            this.label14.Location = new System.Drawing.Point(0, 132);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(133, 13);
            this.label14.TabIndex = 67;
            this.label14.Text = "Bonus to Damage: (NCalc)";
            // 
            // Initiative
            // 
            this.Initiative.Dock = System.Windows.Forms.DockStyle.Top;
            this.Initiative.Location = new System.Drawing.Point(0, 112);
            this.Initiative.Name = "Initiative";
            this.Initiative.Size = new System.Drawing.Size(185, 20);
            this.Initiative.TabIndex = 66;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Top;
            this.label13.Location = new System.Drawing.Point(0, 99);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(132, 13);
            this.label13.TabIndex = 65;
            this.label13.Text = "Bonus to Initiative: (NCalc)";
            // 
            // AC
            // 
            this.AC.Dock = System.Windows.Forms.DockStyle.Top;
            this.AC.Location = new System.Drawing.Point(0, 79);
            this.AC.Name = "AC";
            this.AC.Size = new System.Drawing.Size(185, 20);
            this.AC.TabIndex = 64;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.Location = new System.Drawing.Point(0, 66);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(107, 13);
            this.label12.TabIndex = 63;
            this.label12.Text = "Bonus to AC: (NCalc)";
            // 
            // SaveDC
            // 
            this.SaveDC.Dock = System.Windows.Forms.DockStyle.Top;
            this.SaveDC.Location = new System.Drawing.Point(0, 46);
            this.SaveDC.Name = "SaveDC";
            this.SaveDC.Size = new System.Drawing.Size(185, 20);
            this.SaveDC.TabIndex = 62;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Location = new System.Drawing.Point(0, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(162, 13);
            this.label11.TabIndex = 61;
            this.label11.Text = "Bonus to Spell Save DC: (NCalc)";
            // 
            // Attack
            // 
            this.Attack.Dock = System.Windows.Forms.DockStyle.Top;
            this.Attack.Location = new System.Drawing.Point(0, 13);
            this.Attack.Name = "Attack";
            this.Attack.Size = new System.Drawing.Size(185, 20);
            this.Attack.TabIndex = 60;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Top;
            this.label10.Location = new System.Drawing.Point(0, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(144, 13);
            this.label10.TabIndex = 59;
            this.label10.Text = "Bonus to Attackrollls: (NCalc)";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 17);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.BaseAbility);
            this.splitContainer3.Panel1.Controls.Add(this.label19);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.SaveBonusAbility);
            this.splitContainer3.Panel2.Controls.Add(this.label21);
            this.splitContainer3.Size = new System.Drawing.Size(179, 267);
            this.splitContainer3.SplitterDistance = 128;
            this.splitContainer3.TabIndex = 80;
            // 
            // BaseAbility
            // 
            this.BaseAbility.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BaseAbility.FormattingEnabled = true;
            this.BaseAbility.Location = new System.Drawing.Point(0, 13);
            this.BaseAbility.Name = "BaseAbility";
            this.BaseAbility.ScrollAlwaysVisible = true;
            this.BaseAbility.Size = new System.Drawing.Size(179, 115);
            this.BaseAbility.TabIndex = 8;
            this.BaseAbility.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.BaseAbility_ItemCheck);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Dock = System.Windows.Forms.DockStyle.Top;
            this.label19.Location = new System.Drawing.Point(0, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(159, 13);
            this.label19.TabIndex = 7;
            this.label19.Text = "Attack and Damage Base Ability";
            // 
            // SaveBonusAbility
            // 
            this.SaveBonusAbility.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaveBonusAbility.FormattingEnabled = true;
            this.SaveBonusAbility.Location = new System.Drawing.Point(0, 13);
            this.SaveBonusAbility.Name = "SaveBonusAbility";
            this.SaveBonusAbility.ScrollAlwaysVisible = true;
            this.SaveBonusAbility.Size = new System.Drawing.Size(179, 122);
            this.SaveBonusAbility.TabIndex = 8;
            this.SaveBonusAbility.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.SaveBonusAbility_ItemCheck);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Dock = System.Windows.Forms.DockStyle.Top;
            this.label21.Location = new System.Drawing.Point(0, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(182, 13);
            this.label21.TabIndex = 7;
            this.label21.Text = "Saving Throws (applicable for bonus)";
            // 
            // offhandAbility
            // 
            this.offhandAbility.AutoSize = true;
            this.offhandAbility.Dock = System.Windows.Forms.DockStyle.Top;
            this.offhandAbility.Location = new System.Drawing.Point(0, 0);
            this.offhandAbility.Name = "offhandAbility";
            this.offhandAbility.Size = new System.Drawing.Size(179, 17);
            this.offhandAbility.TabIndex = 81;
            this.offhandAbility.Text = "Ability Mod to Offhand Damage";
            this.offhandAbility.UseVisualStyleBackColor = true;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label23.Location = new System.Drawing.Point(0, 284);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(102, 13);
            this.label23.TabIndex = 79;
            this.label23.Text = "Counts As Weapon:";
            // 
            // Spell
            // 
            this.Spell.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Spell.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.Spell.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Spell.FormattingEnabled = true;
            this.Spell.Location = new System.Drawing.Point(0, 297);
            this.Spell.Name = "Spell";
            this.Spell.Size = new System.Drawing.Size(179, 21);
            this.Spell.TabIndex = 78;
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(2, 631);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(743, 13);
            this.label2.TabIndex = 53;
            this.label2.Text = "Note: The condition results in a true value if the bonus should be applied and fa" +
    "lse otherwise";
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(2, 644);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label3.Size = new System.Drawing.Size(743, 13);
            this.label3.TabIndex = 52;
            this.label3.Text = "For Weapon Attacks and AC Bonus:";
            // 
            // label4
            // 
            this.label4.AutoEllipsis = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(2, 657);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label4.Size = new System.Drawing.Size(743, 13);
            this.label4.TabIndex = 51;
            this.label4.Text = "The following string values are known: Category (of the item), Name (of the item)" +
    ", Damageroll, Damagetype";
            // 
            // label5
            // 
            this.label5.AutoEllipsis = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.Location = new System.Drawing.Point(2, 670);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label5.Size = new System.Drawing.Size(743, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "The following number values are known: Str, Dex, Con, Int, Wis, Cha (Value) and S" +
    "trMod, DexMod, ConMod, IntMod, WisMod, ChaMod (Modifier)";
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Location = new System.Drawing.Point(2, 683);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label6.Size = new System.Drawing.Size(743, 13);
            this.label6.TabIndex = 49;
            this.label6.Text = "The following boolean values are known: All Keywords of the Item: Ranged, Melee, " +
    "Simple, Martial, Finesse, Light, Heavy, Armor (if armor is worn)";
            // 
            // label7
            // 
            this.label7.AutoEllipsis = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label7.Location = new System.Drawing.Point(2, 696);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.label7.Size = new System.Drawing.Size(743, 13);
            this.label7.TabIndex = 48;
            this.label7.Text = "In addition:  Unarmored, Two_Handed (wielding two-handed weapon), Offhand (wieldi" +
    "ng offhand weapon), FreeHand, Shield (wielding shield).";
            // 
            // label8
            // 
            this.label8.AutoEllipsis = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label8.Location = new System.Drawing.Point(2, 709);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.label8.Size = new System.Drawing.Size(743, 13);
            this.label8.TabIndex = 47;
            this.label8.Text = "For Spells only the name of the spell (as Name) and the following boolean values " +
    "are known: ";
            // 
            // label9
            // 
            this.label9.AutoEllipsis = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label9.Location = new System.Drawing.Point(2, 722);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.label9.Size = new System.Drawing.Size(743, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "Str, Dex, Con, Int, Wis, Cha (if the spellcasting ability is used), the internal " +
    "name of the spellcasting feature and all keywords of the spell.";
            // 
            // basicFeature1
            // 
            this.basicFeature1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.basicFeature1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicFeature1.Feature = null;
            this.basicFeature1.Location = new System.Drawing.Point(2, 2);
            this.basicFeature1.Name = "basicFeature1";
            this.basicFeature1.Padding = new System.Windows.Forms.Padding(2);
            this.basicFeature1.Size = new System.Drawing.Size(743, 278);
            this.basicFeature1.TabIndex = 57;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Dock = System.Windows.Forms.DockStyle.Top;
            this.label26.Location = new System.Drawing.Point(0, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(245, 13);
            this.label26.TabIndex = 58;
            this.label26.Text = "Weapon Attack Option Text: (empty for always on)";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 33);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.SkillList);
            this.splitContainer4.Panel1.Controls.Add(this.label18);
            this.splitContainer4.Panel1.Controls.Add(this.panel1);
            this.splitContainer4.Panel1.Controls.Add(this.label17);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.proficiencyOptions);
            this.splitContainer4.Panel2.Controls.Add(this.label24);
            this.splitContainer4.Size = new System.Drawing.Size(371, 285);
            this.splitContainer4.SplitterDistance = 164;
            this.splitContainer4.TabIndex = 61;
            // 
            // SkillList
            // 
            this.SkillList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SkillList.HistoryManager = null;
            this.SkillList.Items = null;
            this.SkillList.Location = new System.Drawing.Point(0, 46);
            this.SkillList.Name = "SkillList";
            this.SkillList.Size = new System.Drawing.Size(371, 118);
            this.SkillList.Suggestions = ((System.Collections.Generic.IEnumerable<string>)(resources.GetObject("SkillList.Suggestions")));
            this.SkillList.TabIndex = 11;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Dock = System.Windows.Forms.DockStyle.Top;
            this.label18.Location = new System.Drawing.Point(0, 33);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(164, 13);
            this.label18.TabIndex = 10;
            this.label18.Text = "Bonus to Skills (Applicable Skills):";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Skill);
            this.panel1.Controls.Add(this.Passive);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(371, 20);
            this.panel1.TabIndex = 9;
            // 
            // Skill
            // 
            this.Skill.Dock = System.Windows.Forms.DockStyle.Top;
            this.Skill.Location = new System.Drawing.Point(0, 0);
            this.Skill.Name = "Skill";
            this.Skill.Size = new System.Drawing.Size(201, 20);
            this.Skill.TabIndex = 4;
            // 
            // Passive
            // 
            this.Passive.AutoSize = true;
            this.Passive.Dock = System.Windows.Forms.DockStyle.Right;
            this.Passive.Location = new System.Drawing.Point(201, 0);
            this.Passive.Name = "Passive";
            this.Passive.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.Passive.Size = new System.Drawing.Size(170, 20);
            this.Passive.TabIndex = 3;
            this.Passive.Text = "Applies only to passive value";
            this.Passive.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Top;
            this.label17.Location = new System.Drawing.Point(0, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(115, 13);
            this.label17.TabIndex = 8;
            this.label17.Text = "Bonus to Skills (Value):";
            // 
            // proficiencyOptions
            // 
            this.proficiencyOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.proficiencyOptions.HistoryManager = null;
            this.proficiencyOptions.Items = null;
            this.proficiencyOptions.Location = new System.Drawing.Point(0, 13);
            this.proficiencyOptions.Name = "proficiencyOptions";
            this.proficiencyOptions.Size = new System.Drawing.Size(371, 104);
            this.proficiencyOptions.Suggestions = ((System.Collections.Generic.IEnumerable<string>)(resources.GetObject("proficiencyOptions.Suggestions")));
            this.proficiencyOptions.TabIndex = 12;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Dock = System.Windows.Forms.DockStyle.Top;
            this.label24.Location = new System.Drawing.Point(0, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(233, 13);
            this.label24.TabIndex = 11;
            this.label24.Text = "Proficient with this weapon when proficient with:";
            // 
            // attackOption
            // 
            this.attackOption.Dock = System.Windows.Forms.DockStyle.Top;
            this.attackOption.Location = new System.Drawing.Point(0, 13);
            this.attackOption.Name = "attackOption";
            this.attackOption.Size = new System.Drawing.Size(371, 20);
            this.attackOption.TabIndex = 60;
            // 
            // BonusFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(747, 775);
            this.Controls.Add(this.basicFeature1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Condition);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.button1);
            this.Name = "BonusFeatureForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bonus Feature";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BonusSize)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Condition;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox DamageText;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox Damage;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox Initiative;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox AC;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox SaveDC;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox Attack;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private BasicFeature basicFeature1;
        private System.Windows.Forms.TextBox saveBonus;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox profBonus;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.CheckedListBox BaseAbility;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckedListBox SaveBonusAbility;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox Spell;
        private System.Windows.Forms.NumericUpDown BonusSize;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox allowOffhand;
        private System.Windows.Forms.CheckBox offhandAbility;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private StringList SkillList;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox Skill;
        private System.Windows.Forms.CheckBox Passive;
        private System.Windows.Forms.Label label17;
        private StringList proficiencyOptions;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox attackOption;
        private System.Windows.Forms.Label label26;
    }
}