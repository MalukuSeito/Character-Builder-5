<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:import href="Common.xsl"/>
<xsl:output method="html" omit-xml-declaration="yes" />
<xsl:template match="Background">
<html>
	<head>
		<style type="text/css"> 
			<xsl:call-template name="CSS" />
		</style>
	</head>
	<body>
		<div class="Name"><xsl:value-of select="Name"/></div>
		<xsl:if test="boolean(Flavour/node())" ><div class="Flavour"><xsl:apply-templates select="Flavour"/></div></xsl:if>
		<div class="Description">
			<xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="./Description" /></xsl:with-param>
			</xsl:call-template>
		</div>
		<xsl:apply-templates select="Descriptions/Description"/>
		<xsl:apply-templates select="Descriptions/TableDescription"/>
		<div class="Features">
			<xsl:apply-templates select="Features/*"/>
		</div>
		<xsl:apply-templates select="PersonalityTrait"/>
		<xsl:apply-templates select="Ideal"/>
		<xsl:apply-templates select="Bond"/>
		<xsl:apply-templates select="Flaw"/>
		<div class="Source"><xsl:value-of select="Source"/></div>
	</body>
</html>
</xsl:template>
<xsl:template match="Item/Name" />
<xsl:template match="Source" />
<xsl:template match="PersonalityTrait">
	<table class="Standalone" cellspacing="0" cellborder="0" width="100%"><tr><th> </th><th>Personality Trait</th></tr><xsl:apply-templates select="TableEntry"/></table>
</xsl:template>

<xsl:template match="Bond">
	<table class="Standalone" cellspacing="0" cellborder="0" width="100%"><tr><th> </th><th>Bond</th></tr><xsl:apply-templates  select="TableEntry"/></table>
</xsl:template>

<xsl:template match="Ideal">
	<table class="Standalone" cellspacing="0" cellborder="0" width="100%"><tr><th> </th><th>Ideal</th></tr><xsl:apply-templates  select="TableEntry"/></table>
</xsl:template>

<xsl:template match="Flaw">
	<table class="Standalone" cellspacing="0" cellborder="0" width="100%"><tr><th> </th><th>Flaw</th></tr><xsl:apply-templates select="TableEntry"/></table>
</xsl:template>
</xsl:stylesheet>


