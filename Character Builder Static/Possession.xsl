<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:import href="Common.xsl"/>
<xsl:output method="html" omit-xml-declaration="yes" />
<xsl:template match="Possession | DisplayPossession">
<html>
	<head>
		<style type="text/css">
		       <xsl:call-template name="CSS" />	
.StackSize {
	font-weight: normal;
}
.Weight {
	font-weight: normal;
	font-family:Verdana, Arial, Helvetica, sans-serif;
	font-size:10pt;
}
.Price {
	text-align: right;
	font-weight: bold;
}
		</style>
	</head>
	<body>
		<table class="ItemTable" cellspacing="0" cellborder="0" width="100%"><tr><td class="Name"><xsl:value-of select="Name"/>
<xsl:if test="Count=1 and boolean(Item/SingleUnit/node())">
<span class="Count">(<xsl:value-of select="Item/SingleUnit" />)</span>
</xsl:if>
<xsl:if test="Count=1 and not(boolean(Item/SingleUnit/node())) and boolean(Item/Unit/node())">
		<span class="Count">
			(<xsl:value-of select="Count"/><xsl:text> </xsl:text>)
		</span>
</xsl:if>
<xsl:if test="Count>1">
		<span class="Count">
			(<xsl:value-of select="Count"/><xsl:if test="boolean(Item/Unit/node())"><xsl:text> </xsl:text><xsl:value-of select="Item/Unit" /></xsl:if>)
		</span>
</xsl:if>		
	</td><td align="right">
<xsl:if test="Weight>-1">
		<span class="Weight">
			<xsl:value-of select="Weight"/> lb
		</span>
	</xsl:if>
	</td></tr></table>
	<div class="Info">
		<xsl:apply-templates select="Info"/>
	</div>
	<div class="Description">
		<xsl:apply-templates select="Description"/>
	</div>
		<xsl:apply-templates select="Item | Magic/MagicProperty" />	
	</body>
</html>
</xsl:template>
<xsl:template match="StackSize" />
<xsl:template match="Item/Name" />
<xsl:template match="Weight" />
<xsl:template match="Price" />
<xsl:template match="Source" />
<xsl:template match="Item">
	<br/><br/><table class="ItemTable" cellspacing="0" cellborder="0" width="100%"><tr><td class="Name"><xsl:value-of select="Name"/>	
	</td><td align="right">
<xsl:if test="Weight>0">
		<span class="Weight">
			<xsl:value-of select="Weight"/> lb
		</span>
</xsl:if>
	</td></tr></table>
	<xsl:if test="boolean(Spell/node())"><xsl:choose>
			<xsl:when test="Spell/Level = '0'"><div class="Requirement">Scroll, common</div><div class="Info">Attack Bonus +5, Save DC 13</div></xsl:when>
			<xsl:when test="Spell/Level = '1'"><div class="Requirement">Scroll, common</div><div class="Info">Attack Bonus +5, Save DC 13</div></xsl:when>
			<xsl:when test="Spell/Level = '2'"><div class="Requirement">Scroll, uncommon</div><div class="Info">Attack Bonus +5, Save DC 13</div></xsl:when>
			<xsl:when test="Spell/Level = '3'"><div class="Requirement">Scroll, uncommon</div><div class="Info">Attack Bonus +7, Save DC 15</div></xsl:when>
			<xsl:when test="Spell/Level = '4'"><div class="Requirement">Scroll, rare</div><div class="Info">Attack Bonus +7, Save DC 15</div></xsl:when>
			<xsl:when test="Spell/Level = '5'"><div class="Requirement">Scroll, rare</div><div class="Info">Attack Bonus +9, Save DC 17</div></xsl:when>
			<xsl:when test="Spell/Level = '6'"><div class="Requirement">Scroll, very rare</div><div class="Info">Attack Bonus +9, Save DC 17</div></xsl:when>
			<xsl:when test="Spell/Level = '7'"><div class="Requirement">Scroll, very rare</div><div class="Info">Attack Bonus +10, Save DC 18</div></xsl:when>
			<xsl:when test="Spell/Level = '8'"><div class="Requirement">Scroll, very rare</div><div class="Info">Attack Bonus +10, Save DC 18</div></xsl:when>
			<xsl:when test="Spell/Level = '9'"><div class="Requirement">Scroll, legendary</div><div class="Info">Attack Bonus +11, Save DC 19</div></xsl:when>
			<xsl:otherwise><div class="Requirement">Scroll, varies</div></xsl:otherwise>
		</xsl:choose>
		
		<div class="Description">
			<p>A spell scroll bears the words of a single spell, written in a mystical cipher. If the spell is on your class's spell list, you can use an action to read the scroll and cast its spell without having to provide any of the spell's components. Otherwise, the scroll is unintelligible.</p>
			<p class="morelines">If the spell is on your class's spell list but of a higher level than you can normally cast, you must make an ability check using your spell casting ability to determine whether you cast it successfully. The DC equals 10 + the spell's level. On a failed check, the spell disappears from the scroll with no other effect.</p>
			<p class="moreline">Once the spell is cast, the words on the scroll fade, and the scroll itself crumbles to dust.</p>
		</div></xsl:if>		<div class="Description">
			<xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="Description" /></xsl:with-param>
			</xsl:call-template>
		</div>
	<xsl:apply-templates select="StealthDisadvantage | StrengthRequired | BaseAC | ACBonus"/>
	<xsl:apply-templates select="Keywords"/>
	<xsl:if test="boolean(Damage/node())" ><div class="Damage">
		<xsl:apply-templates select="Damage"/><xsl:text> </xsl:text><xsl:apply-templates select="DamageType"/> damage
	</div></xsl:if>
	<div class="Price"><xsl:comment></xsl:comment>
<xsl:if test="Price/pp>0"><xsl:value-of select="Price/pp"/> pp</xsl:if>
<xsl:if test="Price/gp>0"><xsl:if test="Price/pp>0">, </xsl:if><xsl:value-of select="Price/gp"/> gp</xsl:if>
<xsl:if test="Price/ep>0"><xsl:if test="Price/pp>0 or Price/gp>0">, </xsl:if><xsl:value-of select="Price/ep"/> ep</xsl:if>
<xsl:if test="Price/sp>0"><xsl:if test="Price/pp>0 or Price/gp>0 or Price/ep>0">, </xsl:if><xsl:value-of select="Price/sp"/> sp</xsl:if>
<xsl:if test="Price/cp>0"><xsl:if test="Price/pp>0 or Price/gp>0 or Price/ep>0 or Price/sp>0">, </xsl:if><xsl:value-of select="Price/cp"/> cp</xsl:if>
	</div>
	<xsl:apply-templates select="Spell"/>
</xsl:template>
<xsl:template match="MagicProperty">
	<br/><br/><div class="Name"><xsl:value-of select="Name"/></div>
		<xsl:if test="boolean(Requirement/node()) or (boolean(Rarity/node()) and Rarity != 'None') or count(AttunementFeatures/*[text()]) > 0" ><div class="Requirement"><xsl:if test="boolean(Requirement/node())"><xsl:apply-templates select="Requirement"/><xsl:if test="boolean(Rarity/node()) and Rarity != 'None' and not(contains(translate(Requirement, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), translate(Rarity, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))) and not(Rarity = 'VeryRare' and contains(translate(Requirement, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'very rare'))">, <xsl:if test="Rarity = 'VeryRare'">very rare</xsl:if><xsl:if test="Rarity != 'VeryRare'"><span style="text-transform:lowercase"><xsl:value-of select="Rarity"/></span></xsl:if></xsl:if></xsl:if><xsl:if test="boolean(Rarity/node()) and Rarity != 'None' and not(boolean(Requirement/node()))"><xsl:if test="Rarity = 'VeryRare'">Very rare</xsl:if><xsl:if test="Rarity != 'VeryRare'"><xsl:value-of select="Rarity"/></xsl:if></xsl:if><xsl:if test="count(AttunementFeatures/*[text()]) > 0 and not(contains(Requirement, 'requires attunement')) and not(boolean(Requirement/node()) or (boolean(Rarity/node()) and Rarity != 'None'))">(Requires attunement)</xsl:if><xsl:if test="count(AttunementFeatures/*[text()]) > 0 and not(contains(Requirement, 'requires attunement')) and (boolean(Requirement/node()) or (boolean(Rarity/node()) and Rarity != 'None'))"> (requires attunement)</xsl:if></div></xsl:if>
		<div class="Description">
			<xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="./Description" /></xsl:with-param>
			</xsl:call-template>
		</div>
		<xsl:apply-templates select="AttunementFeatures/* | CarryFeatures/* | EquipFeatures/* | OnUseFeatures/*"/>
		<div class="Source"><xsl:value-of select="Source"/></div>
</xsl:template>
<xsl:template match="Spell/Keywords">
	<div class="Keyword"><xsl:for-each select="Keyword[not(Name='somatic' or Name='verbal' or Name='illusion' or Name='abjuration' or Name='conjuration' or Name='divination' or Name='evocation' or Name='enchantment' or Name='necromancy' or Name='transmutation' or Name='cantrip' or Name='ritual')]|Range|Versatile|Save"><xsl:sort select="Name" /><xsl:apply-templates select="." /><xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each></div>
</xsl:template>
<xsl:template match="Level">
	<xsl:if test="text() > 0"><xsl:call-template name="FormatRanking"><xsl:with-param name="Value"><xsl:value-of select="text()"/></xsl:with-param></xsl:call-template>-level </xsl:if>
</xsl:template>
<xsl:template match="Spell" ><br/>
<div class="Name"><xsl:value-of select="Name"/></div>
	<div class="Spell"><xsl:apply-templates select="Level" /> <xsl:for-each select="Keywords/Keyword[(Name='abjuration' or Name='conjuration' or Name='divination' or Name='evocation' or Name='enchantment' or Name='necromancy' or Name='transmutation' or Name='illusion')]"><xsl:sort select="Name" /><xsl:value-of select="Name" /><xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each><xsl:if test="boolean(Keywords/Keyword[Name='cantrip']/node())" > cantrip</xsl:if><xsl:if test="boolean(Keywords/Keyword[Name='ritual']/node())" > (ritual)</xsl:if></div>
	<xsl:if test="boolean(differentAbility/node()) and not(differentAbility = 'None')" ><b>Spellcasting Ability: </b><xsl:apply-templates select="differentAbility"/><br/></xsl:if>
	<xsl:if test="boolean(RechargeModifier/node()) and not(RechargeModifier = 'Unmodified')" ><b>Spell Recharge: </b><xsl:apply-templates select="RechargeModifier"/><br/></xsl:if>
	<xsl:if test="boolean(Flavour/node())" ><div class="Flavour"><xsl:apply-templates select="Flavour"/></div></xsl:if>
	<xsl:if test="boolean(Info/node())" ><div class="Info"><xsl:apply-templates select="Info"/></div></xsl:if>
	<xsl:apply-templates select="Keywords|AdditionalKeywords"/>
	<span class="Bold">Casting Time: </span><xsl:value-of select="CastingTime"/><br/>
	<span class="Bold">Range: </span><xsl:value-of select="Range"/><br/>
	<span class="Bold">Components: </span><xsl:if test="boolean(Keywords/Keyword[Name='verbal']/node())" >V<xsl:if test="boolean(Keywords/Keyword[Name='somatic']/node()) or boolean(Keywords/Material/node())" >, </xsl:if></xsl:if><xsl:if test="boolean(Keywords/Keyword[Name='somatic']/node())" >S<xsl:if test="boolean(Keywords/Material/node())" >, </xsl:if></xsl:if><xsl:for-each select="Keywords/Material" >M (<xsl:value-of select="Components"/>)<xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each><br/>
	<span class="Bold">Duration: </span><xsl:value-of select="Duration"/><br/>
	<div class="Description">
		<xsl:call-template name="newline-to-paragraph">
			<xsl:with-param name="input"><xsl:copy-of select="./Description" /></xsl:with-param>
		</xsl:call-template>
	</div>
	<xsl:apply-templates select="Descriptions/*"/>
	<xsl:apply-templates select="Modifikations/*"/>
	<div class="Source"><xsl:value-of select="Source"/></div>
</xsl:template>
<xsl:template match="Spell/Keywords">
	<div class="Keyword"><xsl:for-each select="Keyword[not(Name='somatic' or Name='verbal' or Name='illusion' or Name='abjuration' or Name='conjuration' or Name='divination' or Name='evocation' or Name='enchantment' or Name='necromancy' or Name='transmutation' or Name='cantrip' or Name='ritual')]|Range|Versatile|Save"><xsl:sort select="Name" /><xsl:apply-templates select="." /><xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each></div>
</xsl:template>
</xsl:stylesheet>


