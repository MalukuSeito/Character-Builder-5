<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:import href="Common.xsl"/>
<xsl:output method="html" omit-xml-declaration="yes" />
<xsl:template match="ClassDefinition">
<html>
	<head>
		<style type="text/css"> 
			<xsl:call-template name="CSS" />
		</style>
	</head>
	<body>
		<div class="Name"><xsl:value-of select="Name"/></div>
		<xsl:if test="boolean(Flavour/node())" ><div class="Flavour"><xsl:call-template name="newline-to-paragraph"><xsl:with-param name="input"><xsl:copy-of select="Flavour" /></xsl:with-param></xsl:call-template></div></xsl:if>
    <xsl:if test="boolean(ImageData/node())">
      <xsl:apply-templates select="ImageData"/>
    </xsl:if>

		<div class="Description">
			<xsl:call-template name="newline-to-paragraph"><xsl:with-param name="input"><xsl:copy-of select="./Description" /></xsl:with-param></xsl:call-template>
		</div>
		<xsl:apply-templates select="Descriptions/Description"/>
		<xsl:apply-templates select="Descriptions/TableDescription"/>
		<div class="Features">
			<xsl:apply-templates select="Features/*"/>
		</div>
		<xsl:if test="boolean(FirstClassFeatures/*)"><div class="Features">
			<div class="Header">If this is your first class:</div>
			<xsl:apply-templates select="FirstClassFeatures/*"/>
		</div></xsl:if>
		<xsl:if test="boolean(MulticlassingFeatures/*)"><div class="Features">
			<div class="Header">If this is your second, third, ... class:</div>
			<xsl:apply-templates select="MulticlassingFeatures/*"/>
		</div></xsl:if>
		<div class="Source"><xsl:value-of select="Source"/></div>
	</body>
</html>
</xsl:template>
<xsl:template match="Item/Name" />
<xsl:template match="Source" />
</xsl:stylesheet>


