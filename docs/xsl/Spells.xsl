<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:import href="Common.xsl"/>
<xsl:output method="html" omit-xml-declaration="yes" />
<xsl:template match="Spell">
<html>
	<head>
		<style type="text/css"> 
			<xsl:call-template name="CSS" />
.Spell {
	font-style: italic;
}
.Spell:first-letter {
    text-transform: uppercase;
}

</style>
	</head>
	<body>
		<div class="Name"><xsl:value-of select="Name"/></div>
		<div class="Spell"><xsl:apply-templates select="Level" /> <xsl:for-each select="Keywords/Keyword[(Name='abjuration' or Name='conjuration' or Name='divination' or Name='evocation' or Name='enchantment' or Name='necromancy' or Name='transmutation' or Name='illusion')]"><xsl:sort select="Name" /><xsl:value-of select="Name" /><xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each><xsl:if test="boolean(Keywords/Keyword[Name='cantrip']/node())" > cantrip</xsl:if><xsl:if test="boolean(Keywords/Keyword[Name='ritual']/node())" > (ritual)</xsl:if></div>
		<xsl:if test="boolean(differentAbility/node()) and not(differentAbility = 'None')" ><b>Spellcasting Ability: </b><xsl:apply-templates select="differentAbility"/><br/></xsl:if>
		<xsl:if test="boolean(RechargeModifier/node()) and not(RechargeModifier = 'Unmodified')" ><b>Spell Recharge: </b><xsl:apply-templates select="RechargeModifier"/><br/></xsl:if>
		<xsl:if test="boolean(Flavour/node())" ><div class="Flavour"><xsl:apply-templates select="Flavour"/></div></xsl:if>
		<xsl:if test="boolean(Info/node())" ><div class="Info"><xsl:apply-templates select="Info"/></div></xsl:if>
		<xsl:apply-templates select="Keywords|AdditionalKeywords"/>
		<span class="Bold">Casting Time: </span><xsl:value-of select="CastingTime"/><br/>
		<span class="Bold">Range: </span><xsl:value-of select="Range"/><br/>
		<span class="Bold">Components: </span><xsl:if test="boolean(Keywords/Keyword[Name='verbal']/node())" >V<xsl:if test="boolean(Keywords/Keyword[Name='somatic']/node()) or boolean(Keywords/Material/node()) or boolean(Keywords/Royalty/node())" >, </xsl:if></xsl:if><xsl:if test="boolean(Keywords/Keyword[Name='somatic']/node())" >S<xsl:if test="boolean(Keywords/Material/node()) or boolean(Keywords/Royalty/node())" >, </xsl:if></xsl:if><xsl:for-each select="Keywords/Material" >M (<xsl:value-of select="Components"/>)<xsl:if test="position() != last() or boolean(../Royalty/node())"><xsl:text>, </xsl:text></xsl:if></xsl:for-each><xsl:for-each select="Keywords/Royalty" >R (<xsl:value-of select="Price"/>)<xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each><br/>
		<span class="Bold">Duration: </span><xsl:value-of select="Duration"/><br/>
		<div class="Description">
			<xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="./Description" /></xsl:with-param>
			</xsl:call-template>
		</div>
		<xsl:apply-templates select="Descriptions/*"/>
		<xsl:apply-templates select="Modifikations/*"/>
		<div class="Source"><xsl:value-of select="Source"/></div>
	</body>
</html>
</xsl:template>
<xsl:template match="Source" />
<xsl:template match="Keywords">
	<div class="Keyword"><xsl:for-each select="Keyword[not(Name='somatic' or Name='verbal' or Name='illusion' or Name='abjuration' or Name='conjuration' or Name='divination' or Name='evocation' or Name='enchantment' or Name='necromancy' or Name='transmutation' or Name='cantrip' or Name='ritual')]|Range|Versatile|Save"><xsl:sort select="Name" /><xsl:apply-templates select="." /><xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each></div>
</xsl:template>
<xsl:template match="Spell/Level">
	<xsl:if test="text() > 0"><xsl:call-template name="FormatRanking"><xsl:with-param name="Value"><xsl:value-of select="text()"/></xsl:with-param></xsl:call-template>-level </xsl:if>
</xsl:template>
</xsl:stylesheet>


