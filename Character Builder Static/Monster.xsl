<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:import href="Common.xsl"/>
<xsl:output method="html" omit-xml-declaration="yes" />
<xsl:template match="Monster">
<html>
	<head>
		<meta http-equiv="X-UA-Compatible" content="IE=edge" /> 
		<style type="text/css"> 
			<xsl:call-template name="CSS" />
		</style>
	</head>
	<body>
		<div class="Name"><xsl:value-of select="Name"/></div>
		<div class="Small"><xsl:value-of select="Size"/><xsl:text> </xsl:text><xsl:apply-templates select="Keywords"/><xsl:if test="boolean(Alignment/node())">, <xsl:value-of select="Alignment"/></xsl:if></div>
		<xsl:if test="boolean(Flavour/node())" ><div class="Flavour"><xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="Flavour" /></xsl:with-param>
			</xsl:call-template></div></xsl:if>
    <xsl:if test="boolean(ImageData/node())">
      <xsl:apply-templates select="ImageData"/>
    </xsl:if>
    		<hr/>
		<span class="Bold">Armor Class</span><xsl:text> </xsl:text><xsl:value-of select="AC"/><xsl:if test="boolean(ACText/node())"><xsl:text> (</xsl:text><xsl:value-of select="ACText"/>)</xsl:if><br/>
		<span class="Bold">Hit Points</span><xsl:text> </xsl:text><xsl:value-of select="HP"/><xsl:if test="boolean(HPRoll/node())"><xsl:text> (</xsl:text><xsl:value-of select="HPRoll"/>)</xsl:if><br/>
		<span class="Bold">Speed</span><xsl:text> </xsl:text><xsl:apply-templates select="Speeds"/>
		<hr/>
		<div style="text-align:center">
			<div class="Ability">
				<div class="Bold">STR</div>
				<xsl:value-of select="Strength"/><xsl:text> (</xsl:text><xsl:value-of select="format-number(floor(Strength div 2) - 5, '+0;-0')"/>)
			</div>
			<div class="Ability">
				<div class="Bold">DEX</div>
				<xsl:value-of select="Dexterity"/><xsl:text> (</xsl:text><xsl:value-of select="format-number(floor(Dexterity div 2) - 5, '+0;-0')"/>)
			</div>
			<div class="Ability">
				<div class="Bold">CON</div>
				<xsl:value-of select="Constitution"/><xsl:text> (</xsl:text><xsl:value-of select="format-number(floor(Constitution div 2) - 5, '+0;-0')"/>)
			</div>
			<div class="Ability">
				<div class="Bold">INT</div>
				<xsl:value-of select="Intelligence"/><xsl:text> (</xsl:text><xsl:value-of select="format-number(floor(Intelligence div 2) - 5, '+0;-0')"/>)
			</div>
			<div class="Ability">
				<div class="Bold">WIS</div>
				<xsl:value-of select="Wisdom"/><xsl:text> (</xsl:text><xsl:value-of select="format-number(floor(Wisdom div 2) - 5, '+0;-0')"/>)
			</div>
			<div class="Ability">
				<div class="Bold">CHA</div>
				<xsl:value-of select="Charisma"/><xsl:text> (</xsl:text><xsl:value-of select="format-number(floor(Charisma div 2) - 5, '+0;-0')"/>)
			</div>
		</div>	
		<hr />
		<xsl:if test="boolean(SaveBonus/MonsterSaveBonus/node())"><span class="Bold">Saving Throws</span><xsl:text> </xsl:text><xsl:apply-templates select="SaveBonus"/><br/></xsl:if>
		<xsl:if test="boolean(SkillBonus/MonsterSkillBonus/node())"><span class="Bold">Skills</span><xsl:text> </xsl:text><xsl:apply-templates select="SkillBonus"/><br/></xsl:if>
		<xsl:if test="boolean(Vulnerablities/string/node())"><span class="Bold">Damage Vulnerabilities</span><xsl:text> </xsl:text><xsl:apply-templates select="Vulnerablities"/><br/></xsl:if>
		<xsl:if test="boolean(Resistances/string/node())"><span class="Bold">Damage Resistances</span><xsl:text> </xsl:text><xsl:apply-templates select="Resistances"/><br/></xsl:if>
		<xsl:if test="boolean(Immunities/string/node())"><span class="Bold">Damage Immunities</span><xsl:text> </xsl:text><xsl:apply-templates select="Immunities"/><br/></xsl:if>
		<xsl:if test="boolean(ConditionImmunities/string/node())"><span class="Bold">Condition Immunities</span><xsl:text> </xsl:text><xsl:apply-templates select="ConditionImmunities"/><br/></xsl:if>
		<span class="Bold">Senses</span><xsl:text> </xsl:text><xsl:if test="boolean(Senses/string/node())"><xsl:apply-templates select="Senses"/><xsl:text>, </xsl:text></xsl:if>passive Perception <xsl:choose><xsl:when test="boolean(SkillBonus/MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='perception']/node())"><xsl:value-of select="10 + floor(Wisdom div 2) - 5 + SkillBonus/MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='perception']/Bonus + PassivePerception"/></xsl:when><xsl:otherwise><xsl:value-of select="10 + floor(Wisdom div 2) - 5 + PassivePerception"/></xsl:otherwise></xsl:choose><br/>
		<span class="Bold">Languages</span><xsl:text> </xsl:text><xsl:choose><xsl:when test="boolean(Languages/string/node())"><xsl:apply-templates select="Languages"/></xsl:when><xsl:otherwise>&#8212;</xsl:otherwise></xsl:choose><br/>
		<span class="Bold">Challenge</span><xsl:text> </xsl:text><xsl:apply-templates select="CR"/><xsl:text> (</xsl:text><xsl:value-of select="XP"/> XP)<br/>	

		<div class="Description" style="margin-top:10px">
			<xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="./Description" /></xsl:with-param>
			</xsl:call-template>
		</div>
		<xsl:apply-templates select="Descriptions/*"/>
		<div class="Traits" style="margin-top:10px">
			<xsl:apply-templates select="Traits"/>
		</div>
		<xsl:if test="boolean(Actions/MonsterTrait/node()) or boolean(Actions/MonsterAction/node())">
			<div class="Underline">Actions</div>
			<div class="Actions">
				<xsl:apply-templates select="Actions"/>
			</div>
		</xsl:if>
		<xsl:if test="boolean(Reactions/MonsterTrait/node()) or boolean(Reactions/MonsterAction/node())">
			<div class="Underline">Reactions</div>
			<div class="Reactions">
				<xsl:apply-templates select="Reactions"/>
			</div>
		</xsl:if>
		<xsl:if test="boolean(LegendaryActions/MonsterTrait/node()) or boolean(LegendaryActions/MonsterAction/node())">
			<div class="Underline">Legendary Actions</div>
			<div class="LegendaryActions">
				<xsl:apply-templates select="LegendaryActions"/>
			</div>
		</xsl:if>
		<div class="Source"><xsl:value-of select="Source"/></div>
	</body>
</html>
</xsl:template>

<xsl:template name="newline-to-paragraph-titled">

	<xsl:param name="input" />
	<xsl:param name="title" />

    <xsl:variable name="output">

        <xsl:choose>
            <xsl:when test="contains($input, '&#10;')">
		    <xsl:element name="p"><xsl:if test="$title != ''"><span class="Bold"><xsl:value-of select="$title" />.</span><xsl:text> </xsl:text></xsl:if><xsl:copy-of select="substring-before($input, '&#10;')" /></xsl:element>
                <xsl:call-template name="newline-to-paragraph2">
                    <xsl:with-param name="input">
                        <xsl:copy-of select="substring-after($input, '&#10;')" />
                    </xsl:with-param>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:if test="$input != ''">
                    <xsl:element name="p"><xsl:if test="$title != ''"><span class="Bold"><xsl:value-of select="$title" />.</span><xsl:text> </xsl:text></xsl:if><xsl:copy-of select="$input" /></xsl:element>
                </xsl:if>
            </xsl:otherwise>
        </xsl:choose>

    </xsl:variable>

    <xsl:copy-of select="$output" />

</xsl:template>

<xsl:template match="CR[text()=0.25]">&#188;</xsl:template>
<xsl:template match="CR[text()=0.125]">&#8539;</xsl:template>
<xsl:template match="CR[text()=0.5]">&#189;</xsl:template>

<xsl:template match="Names/Names">
	<div class="Description Names"><span class="Bold"><xsl:apply-templates select="Title" />: </span>
		<xsl:for-each select="ListOfNames/string"><xsl:value-of select="."/><xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each></div>
</xsl:template>

<xsl:template match="Traits|Actions|LegendaryActions">
	<xsl:for-each select="MonsterTrait|MonsterAction"><xsl:apply-templates select="." /></xsl:for-each>
</xsl:template>

<xsl:template match="MonsterTrait">
	<div class="MonsterActionTrait"><xsl:call-template name="newline-to-paragraph-titled">
			<xsl:with-param name="input"><xsl:value-of select="Text" /></xsl:with-param>
			<xsl:with-param name="title"><xsl:value-of select="Name" /></xsl:with-param>
	</xsl:call-template></div>
</xsl:template>

<xsl:template match="MonsterAction">
	<div class="MonsterActionTrait"><xsl:call-template name="newline-to-paragraph-titled">
		<xsl:with-param name="input"><xsl:value-of select="Text" /></xsl:with-param>
		<xsl:with-param name="title"><xsl:value-of select="Name" /></xsl:with-param>
	</xsl:call-template><p class="morelines" style="padding-left: 8px"><i>Attack: <xsl:value-of select="format-number(AttackBonus, '+0;-0')" />, <xsl:value-of select="Damage" /> damage.</i></p></div>
</xsl:template>

<xsl:template match="Keywords">
	<xsl:for-each select="Keyword|Range|Versatile|Material|Save"><xsl:if test="position() = 2"><xsl:text> (</xsl:text></xsl:if><xsl:apply-templates select="." /><xsl:if test="position() != last() and position() > 1"><xsl:text>, </xsl:text></xsl:if><xsl:if test="position() = last() and position() > 1"><xsl:text>)</xsl:text></xsl:if></xsl:for-each>
</xsl:template>

<xsl:template match="Speeds|Senses|Languages|Resistances|Vulnerablities|Immunities|ConditionImmunities">
	<xsl:for-each select="string"><xsl:apply-templates select="." /><xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each>
</xsl:template>

<xsl:template match="SaveBonus|SkillBonus">
	<xsl:for-each select="MonsterSaveBonus|MonsterSkillBonus"><xsl:apply-templates select="." /><xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each>
</xsl:template>

<xsl:template match="MonsterSaveBonus[Ability='Strength']|MonsterSkillBonus[Ability='Strength']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='athletics']">
	<xsl:choose><xsl:when test="boolean(Skill/node())"><xsl:value-of select="Skill"/></xsl:when><xsl:otherwise>Str</xsl:otherwise></xsl:choose><xsl:text> </xsl:text><xsl:value-of select="format-number(floor(../../Strength div 2) - 5 + Bonus, '+0;-0')"/> <xsl:if test="boolean(Text/node())"><xsl:text> (</xsl:text><xsl:value-of select="Text"/>)</xsl:if>
</xsl:template>
<xsl:template match="MonsterSaveBonus[Ability='Dexterity']|MonsterSkillBonus[Ability='Dexterity']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='acrobatics']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='stealth']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='sleight of hand']">
	<xsl:choose><xsl:when test="boolean(Skill/node())"><xsl:value-of select="Skill"/></xsl:when><xsl:otherwise>Dex</xsl:otherwise></xsl:choose><xsl:text> </xsl:text><xsl:value-of select="format-number(floor(../../Dexterity div 2) - 5 + Bonus, '+0;-0')"/> <xsl:if test="boolean(Text/node())"><xsl:text> (</xsl:text><xsl:value-of select="Text"/>)</xsl:if>
</xsl:template>
<xsl:template match="MonsterSaveBonus[Ability='Constitution']|MonsterSkillBonus[Ability='Constitution']">
	<xsl:choose><xsl:when test="boolean(Skill/node())"><xsl:value-of select="Skill"/></xsl:when><xsl:otherwise>Con</xsl:otherwise></xsl:choose><xsl:text> </xsl:text><xsl:value-of select="format-number(floor(../../Constitution div 2) - 5 + Bonus, '+0;-0')"/> <xsl:if test="boolean(Text/node())"><xsl:text> (</xsl:text><xsl:value-of select="Text"/>)</xsl:if>
</xsl:template>
<xsl:template match="MonsterSaveBonus[Ability='Intelligence']|MonsterSkillBonus[Ability='Intelligence']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='arcana']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='history']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='nature']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='religion']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='investigation']">
	<xsl:choose><xsl:when test="boolean(Skill/node())"><xsl:value-of select="Skill"/></xsl:when><xsl:otherwise>Int</xsl:otherwise></xsl:choose><xsl:text> </xsl:text><xsl:value-of select="format-number(floor(../../Intelligence div 2) - 5 + Bonus, '+0;-0')"/> <xsl:if test="boolean(Text/node())"><xsl:text> (</xsl:text><xsl:value-of select="Text"/>)</xsl:if>
</xsl:template>
<xsl:template match="MonsterSaveBonus[Ability='Wisdom']|MonsterSkillBonus[Ability='Wisdom']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='animal handling']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='insight']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='medicine']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='survival']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='perception']">
	<xsl:choose><xsl:when test="boolean(Skill/node())"><xsl:value-of select="Skill"/></xsl:when><xsl:otherwise>Wis</xsl:otherwise></xsl:choose><xsl:text> </xsl:text><xsl:value-of select="format-number(floor(../../Wisdom div 2) - 5 + Bonus, '+0;-0')"/> <xsl:if test="boolean(Text/node())"><xsl:text> (</xsl:text><xsl:value-of select="Text"/>)</xsl:if>
</xsl:template>
<xsl:template match="MonsterSaveBonus[Ability='Charisma']|MonsterSkillBonus[Ability='Charisma']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='deception']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='intimidation']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='performance']|MonsterSkillBonus[translate(Skill, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='persuasion']">
	<xsl:choose><xsl:when test="boolean(Skill/node())"><xsl:value-of select="Skill"/></xsl:when><xsl:otherwise>Cha</xsl:otherwise></xsl:choose><xsl:text> </xsl:text><xsl:value-of select="format-number(floor(../../Charisma div 2) - 5 + Bonus, '+0;-0')"/> <xsl:if test="boolean(Text/node())"><xsl:text> (</xsl:text><xsl:value-of select="Text"/>)</xsl:if>
</xsl:template>
<xsl:template match="MonsterSaveBonus">
	<xsl:value-of select="Ability"/><xsl:text> </xsl:text><xsl:value-of select="format-number(Bonus, '+0;-0')"/><xsl:text> </xsl:text><xsl:if test="boolean(Text/node())"><xsl:text> (</xsl:text><xsl:value-of select="Text"/>)</xsl:if>
</xsl:template>
<xsl:template match="MonsterSkillBonus">
	<xsl:value-of select="Skill"/><xsl:text> </xsl:text><xsl:value-of select="format-number(Bonus, '+0;-0')"/><xsl:text> </xsl:text><xsl:if test="boolean(Text/node())"><xsl:text> (</xsl:text><xsl:value-of select="Text"/>)</xsl:if>
</xsl:template>


<xsl:template match="Keyword|Range|Versatile|Material|Save"><xsl:value-of select="Name" /></xsl:template>
</xsl:stylesheet>

