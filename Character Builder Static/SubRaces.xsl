<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:import href="Common.xsl"/>
<xsl:output method="html" omit-xml-declaration="yes" />
<xsl:template match="SubRace">
<html>
	<head>
		<style type="text/css"> 
			<xsl:call-template name="CSS" />
		</style>
	</head>
	<body>
		<div class="Name"><xsl:value-of select="Name"/> (<xsl:value-of select="ParentRace"/>)</div>
		<xsl:if test="boolean(Flavour/node())" ><div class="Flavour"><xsl:call-template name="newline-to-paragraph"><xsl:with-param name="input"><xsl:copy-of select="Flavour" /></xsl:with-param></xsl:call-template></div></xsl:if>
    <xsl:if test="boolean(ImageData/node())">
      <xsl:apply-templates select="ImageData"/>
    </xsl:if>
		<div class="Description">
			<xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="./Description" /></xsl:with-param>
			</xsl:call-template>
		</div>
		<xsl:apply-templates select="Descriptions/*"/>
		<div class="Features">
			<xsl:apply-templates select="Features/*"/>
		</div>
		<div class="Source"><xsl:value-of select="Source"/></div>
	</body>
</html>
</xsl:template>
<xsl:template match="Source" />
</xsl:stylesheet>


