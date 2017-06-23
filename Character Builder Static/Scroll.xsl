<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:import href="Common.xsl"/>
<xsl:output method="html" omit-xml-declaration="yes" />
<xsl:template match="Item">
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
		</style>
	</head>
	<body>
		<table class="ItemTable" cellspacing="0" cellborder="0" width="100%"><tr><td class="Name"><xsl:value-of select="Name"/>
<xsl:if test="Count=1 and boolean(SingleUnit/node())">
<span class="Count">(<xsl:value-of select="Item/SingleUnit" />)</span>
</xsl:if>
<xsl:if test="Count=1 and not(boolean(SingleUnit/node())) and boolean(Unit/node())">
		<span class="Count">
			(<xsl:value-of select="Count"/><xsl:text> </xsl:text><xsl:value-of select="Item/Unit" />)
		</span>
</xsl:if>					
<xsl:if test="StackSize>1">
			<span class="StackSize">
				(<xsl:value-of select="StackSize"/><xsl:if test="boolean(Unit/node())"><xsl:text> </xsl:text><xsl:value-of select="Unit" /></xsl:if>)
			</span>
</xsl:if>		
		</td><td align="right">
<xsl:if test="Weight>0">
			<span class="Weight">
				<xsl:value-of select="Weight"/> lb
			</span>
</xsl:if>
		</td></tr></table>
		<xsl:choose>
			<xsl:when test="Spell/Level == '0'"><div class="Requirement">Scroll, common</div><div class="Info">Attack Bonus +5, Save DC 13</div></xsl:when>
			<xsl:when test="Spell/Level == '1'"><div class="Requirement">Scroll, common</div><div class="Info">Attack Bonus +5, Save DC 13</div></xsl:when>
			<xsl:when test="Spell/Level == '2'"><div class="Requirement">Scroll, uncommon</div><div class="Info">Attack Bonus +5, Save DC 13</div></xsl:when>
			<xsl:when test="Spell/Level == '3'"><div class="Requirement">Scroll, uncommon</div><div class="Info">Attack Bonus +7, Save DC 15</div></xsl:when>
			<xsl:when test="Spell/Level == '4'"><div class="Requirement">Scroll, rare</div><div class="Info">Attack Bonus +7, Save DC 15</div></xsl:when>
			<xsl:when test="Spell/Level == '5'"><div class="Requirement">Scroll, rare</div><div class="Info">Attack Bonus +9, Save DC 17</div></xsl:when>
			<xsl:when test="Spell/Level == '6'"><div class="Requirement">Scroll, very rare</div><div class="Info">Attack Bonus +9, Save DC 17</div></xsl:when>
			<xsl:when test="Spell/Level == '7'"><div class="Requirement">Scroll, very rare</div><div class="Info">Attack Bonus +10, Save DC 18</div></xsl:when>
			<xsl:when test="Spell/Level == '8'"><div class="Requirement">Scroll, very rare</div><div class="Info">Attack Bonus +10, Save DC 18</div></xsl:when>
			<xsl:when test="Spell/Level == '9'"><div class="Requirement">Scroll, legendary</div><div class="Info">Attack Bonus +11, Save DC 19</div></xsl:when>
			<xsl:otherwise><div class="Requirement">Scroll, varies</div></xsl:when>
		</xsl:choose>
		
		<div class="Description">
			<p>A spell scroll bears the words of a single spell, written in a mystical cipher. If the spell is on your class's spell list, you can use an action to read the scroll and cast its spell without having to provide any of the spell's components. Otherwise, the scroll is unintelligible.</p>
			<p class="morelines">If the spell is on your class's spell list but of a higher level than you can normally cast, you must make an ability check using your spell casting ability to determine whether you cast it successfully. The DC equals 10 + the spell's level. On a failed check, the spell disappears from the scroll with no other effect.</p>
			<p>Once the spell is cast, the words on the scroll fade, and the scroll itself crumbles to dust.</p>
		</div>
		<xsl:apply-templates select="StealthDisadvantage | StrengthRequired | BaseAC | ACBonus"/>
		<xsl:apply-templates select="Keywords"/>
		<xsl:if test="boolean(Damage/node())" ><div class="Damage">
			<xsl:apply-templates select="Damage"/><xsl:text> </xsl:text><xsl:apply-templates select="DamageType"/> damage
		</div></xsl:if>
		<div class="Source">Systems Reference Document v5.1</div>
		<br/><br/>
		<xsl:apply-templates select="Spell" />
	</body>
</html>
</xsl:template>
<xsl:template match="StackSize" />
<xsl:template match="Item/Name" />
<xsl:template match="Weight" />
<xsl:template match="Price" />
<xsl:template match="Source" />
<xsl:template match="Spell" />
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
<xsl:template match="Level">
	<xsl:if test="text() > 0"><xsl:call-template name="FormatRanking"><xsl:with-param name="Value"><xsl:value-of select="text()"/></xsl:with-param></xsl:call-template>-level </xsl:if>
</xsl:template>
</xsl:stylesheet>


