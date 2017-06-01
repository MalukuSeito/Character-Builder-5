<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:import href="Common.xsl"/>
<xsl:output method="html" omit-xml-declaration="yes" />
<xsl:template match="DescriptionContainer">
<html>
	<head>
		<style type="text/css"> 
			<xsl:call-template name="CSS" />
		</style>
	</head>
	<body>
		<xsl:apply-templates select="Descriptions/*"/>
	</body>
</html>
</xsl:template>
</xsl:stylesheet>


