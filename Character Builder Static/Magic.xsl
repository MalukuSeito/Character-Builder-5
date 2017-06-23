<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:import href="Common.xsl"/>
<xsl:output method="html" omit-xml-declaration="yes" />
<xsl:template match="MagicProperty">
<html>
	<head>
		<style type="text/css"> 
<xsl:call-template name="CSS" />
.Requirement {
	padding-left:10px;
	padding-right:10px;
	font-style: italic;
	margin-bottm:10px;
}
</style>
	</head>
	<body>
		<div class="Name"><xsl:value-of select="Name"/></div>
		<xsl:if test="boolean(Requirement/node()) or (boolean(Rarity/node()) and Rarity != 'None') or count(AttunementFeatures/*[text()]) > 0 or count(AttunedEquipFeatures/*[text()]) > 0 or count(AttunedOnUseFeatures/*[text()]) > 0" ><div class="Requirement"><xsl:if test="boolean(Requirement/node())"><xsl:apply-templates select="Requirement"/><xsl:if test="boolean(Rarity/node()) and Rarity != 'None' and not(contains(translate(Requirement, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), translate(Rarity, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))) and not(Rarity = 'VeryRare' and contains(translate(Requirement, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'very rare'))">, <xsl:if test="Rarity = 'VeryRare'">very rare</xsl:if><xsl:if test="Rarity != 'VeryRare'"><span style="text-transform:lowercase"><xsl:value-of select="Rarity"/></span></xsl:if></xsl:if></xsl:if><xsl:if test="boolean(Rarity/node()) and Rarity != 'None' and not(boolean(Requirement/node()))"><xsl:if test="Rarity = 'VeryRare'">Very rare</xsl:if><xsl:if test="Rarity != 'VeryRare'"><xsl:value-of select="Rarity"/></xsl:if></xsl:if><xsl:if test="(count(AttunementFeatures/*[text()]) > 0 or count(AttunedEquipFeatures/*[text()]) > 0 or count(AttunedOnUseFeatures/*[text()]) > 0) and not(contains(Requirement, 'requires attunement')) and not(boolean(Requirement/node()) or (boolean(Rarity/node()) and Rarity != 'None'))">(Requires attunement)</xsl:if><xsl:if test="(count(AttunementFeatures/*[text()]) > 0 or count(AttunedEquipFeatures/*[text()]) > 0 or count(AttunedOnUseFeatures/*[text()]) > 0) and not(contains(Requirement, 'requires attunement')) and (boolean(Requirement/node()) or (boolean(Rarity/node()) and Rarity != 'None'))"> (requires attunement)</xsl:if></div></xsl:if>
    <xsl:if test="boolean(ImageData/node())">
      <xsl:apply-templates select="ImageData"/>
    </xsl:if>
    <div class="Description">
			<xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="./Description" /></xsl:with-param>
			</xsl:call-template>
		</div>
		<xsl:apply-templates select="AttunementFeatures/* | CarryFeatures/* | EquipFeatures/* | OnUseFeatures/* | AttunedEquipFeatures/* | AttunedOnUseFeatures/*"/>
		<div class="Source"><xsl:value-of select="Source"/></div>
	</body>
</html>
</xsl:template>
</xsl:stylesheet>


