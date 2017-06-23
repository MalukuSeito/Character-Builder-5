<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="TableDescription">
	<div class="Header"><xsl:apply-templates select="Name" /> </div><xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="Text" /></xsl:with-param>
			</xsl:call-template>
	<table cellspacing="0" cellborder="0" width="100%"><tr><th><xsl:comment></xsl:comment></th><th><xsl:comment></xsl:comment><xsl:apply-templates select="TableName" /></th></tr><xsl:apply-templates select="Entries/TableEntry" /></table>
</xsl:template>
<xsl:template match="Descriptions/Description">
	<div class="Header"><xsl:apply-templates select="Name" /> </div><xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="Text" /></xsl:with-param>
			</xsl:call-template>
</xsl:template>

<xsl:template match="Descriptions/ListDescription">
	<div class="Header"><xsl:apply-templates select="Name" /> </div><xsl:call-template name="newline-to-paragraph">
				<xsl:with-param name="input"><xsl:copy-of select="Text" /></xsl:with-param>
			</xsl:call-template><xsl:apply-templates select="Names/Names"/>
</xsl:template>

<xsl:template match="Names/Names">
	<div class="List"><span class="Bold"><xsl:apply-templates select="Title" />: </span>
		<xsl:for-each select="ListOfNames/string"><xsl:value-of select="."/><xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each></div>
</xsl:template>



<xsl:template match="TableEntry"><tr><xsl:if test="MinRoll >= 0"><th valign="top"><xsl:comment></xsl:comment><xsl:value-of select="MinRoll"/><xsl:if test="not(MinRoll >= MaxRoll)">-<xsl:value-of select="MaxRoll"/></xsl:if></th></xsl:if><td><xsl:comment></xsl:comment><xsl:if test="boolean(Title/node())"><span class="Header"><xsl:apply-templates select="Title" />: </span></xsl:if><xsl:apply-templates select="Text" /></td></tr></xsl:template>
<xsl:template match="text()" name="insertBreaks">
   <xsl:param name="pText" select="."/>
   <xsl:choose>
     <xsl:when test="not(contains($pText, '&#xA;'))">
       <xsl:copy-of select="$pText"/>
     </xsl:when>
     <xsl:otherwise>
       <xsl:value-of select="substring-before($pText, '&#xA;')"/>
       <br />
       <xsl:call-template name="insertBreaks">
         <xsl:with-param name="pText" select="substring-after($pText, '&#xA;')"/>
       </xsl:call-template>
     </xsl:otherwise>
   </xsl:choose>
 </xsl:template>

 <xsl:template match="AbilityScoreFeature | BonusSpellKeywordChoiceFeature | ChoiceFeature | CollectionChoiceFeature | Feature | FreeItemAndGoldFeature | ItemChoiceConditionFeature | ItemChoiceFeature | HitPointsFeature | LanguageProficiencyFeature | LanguageChoiceFeature | OtherProficiencyFeature | SaveProficiencyFeature | SpeedFeature | SkillProficiencyChoiceFeature | SkillProficiencyFeature | SubRaceFeature | SubClassFeature | ToolProficiencyFeature | ToolKWProficiencyFeature | ToolProficiencyChoiceConditionFeature | BonusFeature | SpellcastingFeature | IncreaseSpellChoiceAmountFeature | ModifySpellChoiceFeature | SpellChoiceFeature | SpellSlotsFeature | BonusSpellPrepareFeature | BonusSpellFeature | ACFeature | AbilityScoreFeatFeature | ExtraAttackFeature | ResourceFeature | SpellModifyFeature | VisionFeature">
	 <xsl:if test="boolean(Name/node())" ><div class="Feature">
		<xsl:choose>
            		<xsl:when test="contains(Text, '&#10;')">
				<p><span class="Bold"><xsl:apply-templates select="Name" /><xsl:if test="Level > 1"> (Level <xsl:apply-templates select="Level" />)</xsl:if>: </span><xsl:if test="substring-before(Text, '&#10;') != ''"><xsl:copy-of select="substring-before(Text, '&#10;')" /></xsl:if></p>
				<xsl:call-template name="newline-to-paragraph2">
                    			<xsl:with-param name="input">
                        			<xsl:copy-of select="substring-after(Text, '&#10;')" />
                    			</xsl:with-param>
                		</xsl:call-template>
            		</xsl:when>
			<xsl:otherwise>
				<p><span class="Bold"><xsl:apply-templates select="Name" /><xsl:if test="Level > 1"> (Level <xsl:apply-templates select="Level" />)</xsl:if>: </span><xsl:copy-of select="Text" /></p>
			</xsl:otherwise>
	        </xsl:choose>
         </div></xsl:if>
</xsl:template>

<xsl:template match="MultiFeature">
	<xsl:if test="boolean(Name/node())" ><div class="Feature">
		<xsl:choose>
            		<xsl:when test="contains(Text, '&#10;')">
				<p><span class="Bold"><xsl:apply-templates select="Name" /><xsl:if test="Level > 1"> (Level <xsl:apply-templates select="Level" />)</xsl:if>: </span><xsl:if test="substring-before(Text, '&#10;') != ''"><xsl:copy-of select="substring-before(Text, '&#10;')" /></xsl:if></p>
				<xsl:call-template name="newline-to-paragraph2">
                    			<xsl:with-param name="input">
                        			<xsl:copy-of select="substring-after(Text, '&#10;')" />
                    			</xsl:with-param>
                		</xsl:call-template>
            		</xsl:when>
			<xsl:otherwise>
				<p><span class="Bold"><xsl:apply-templates select="Name" /><xsl:if test="Level > 1"> (Level <xsl:apply-templates select="Level" />)</xsl:if>: </span><xsl:copy-of select="Text" /></p>
			</xsl:otherwise>
	        </xsl:choose>
         </div></xsl:if>
	<xsl:apply-templates select="Features/*" />
</xsl:template>

<xsl:template match="Keywords|AdditionalKeywords">
	<div class="Keyword"><xsl:for-each select="Keyword|Range|Versatile|Material|Save"><xsl:sort select="Name" /><xsl:apply-templates select="." /><xsl:if test="position() != last()"><xsl:text>, </xsl:text></xsl:if></xsl:for-each></div>
</xsl:template>

<xsl:template match="Keyword"><span style="text-transform: capitalize;"><xsl:value-of select="Name" /></span></xsl:template>

<xsl:template match="Range"><span style="text-transform: capitalize;"><xsl:value-of select="Name" /></span> (<xsl:value-of select="Short" />/<xsl:value-of select="Long" />)</xsl:template>
<xsl:template match="Versatile"><span style="text-transform: capitalize;"><xsl:value-of select="Name" /></span> (<xsl:value-of select="Damage" />)</xsl:template>
<xsl:template match="Material"><span style="text-transform: capitalize;"><xsl:value-of select="Name" /></span> (<xsl:value-of select="Components" />)</xsl:template>
<xsl:template match="Save"><span style="text-transform: capitalize;"><xsl:value-of select="Name" /></span> (<xsl:apply-templates select="Throw" />)</xsl:template>
<xsl:template match="StealthDisadvantage"><xsl:if test="text() = 'true'"><b>Stealth: </b>Disadvantage<br/></xsl:if></xsl:template>
<xsl:template match="StrengthRequired"><xsl:if test="text() > 0"><b>Strength Required: </b><xsl:apply-templates select="text()" /> or -10 ft speed<br/></xsl:if></xsl:template>
<xsl:template match="BaseAC"><b>Armor Class: </b><xsl:apply-templates select="text()" /> (base)<br/></xsl:template>
<xsl:template match="ACBonus"><b>Armor Class: </b>+<xsl:apply-templates select="text()" /> (bonus)<br/></xsl:template>

<xsl:template match="Info">
	<xsl:if test="boolean(SaveDC/node())"><b>Save: DC </b><xsl:value-of select="SaveDC"/></xsl:if>
	<xsl:if test="not(boolean(SaveDC/node())) and AttackBonus>=0"><b>Attack: </b>1d20+<xsl:value-of select="AttackBonus"/></xsl:if>
	<xsl:if test="not(boolean(SaveDC/node())) and AttackBonus &lt; 0"><b>Attack: </b>1d20<xsl:value-of select="AttackBonus"/></xsl:if><br/>
	<b>Damage: </b><xsl:value-of select="Damage"/><xsl:text> </xsl:text><xsl:value-of select="DamageType"/><br/>
</xsl:template>

<xsl:template name="FormatRanking">
  <xsl:param name="Value" select="0" />

  <xsl:value-of select="$Value"/>

  <!-- a little parameter sanity check (integer > 0) -->
  <xsl:if test="
    translate($Value, '0123456789', '') = ''
    and
    $Value > 0
  ">
    <xsl:variable name="mod100" select="$Value mod 100" />
    <xsl:variable name="mod10"  select="$Value mod 10" />

    <xsl:choose>
      <xsl:when test="$mod100 = 11 or $mod100 = 12 or $mod100 = 13">
        <xsl:text>th</xsl:text>
      </xsl:when>
      <xsl:when test="$mod10 = 1">
        <xsl:text>st</xsl:text>
      </xsl:when>
      <xsl:when test="$mod10 = 2">
        <xsl:text>nd</xsl:text>
      </xsl:when>
      <xsl:when test="$mod10 = 3">
        <xsl:text>rd</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>th</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:if>
</xsl:template>

<xsl:template name="nl2p">

    <xsl:param name="input" />

    <xsl:variable name="output">
        <xsl:call-template name="newline-to-paragraph">
            <xsl:with-param name="input">
                <xsl:copy-of select="$input" />
            </xsl:with-param>
        </xsl:call-template>
    </xsl:variable>

    <xsl:copy-of select="$output" />

</xsl:template>

<!-- convert newline characters to <p></p> -->
<xsl:template name="newline-to-paragraph">

    <xsl:param name="input" />

    <xsl:variable name="output">

        <xsl:choose>
            <xsl:when test="contains($input, '&#10;')">
                    <xsl:element name="p"><xsl:copy-of select="substring-before($input, '&#10;')" /></xsl:element>
                <xsl:call-template name="newline-to-paragraph2">
                    <xsl:with-param name="input">
                        <xsl:copy-of select="substring-after($input, '&#10;')" />
                    </xsl:with-param>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:if test="$input != ''">
                    <xsl:element name="p"><xsl:copy-of select="$input" /></xsl:element>
                </xsl:if>
            </xsl:otherwise>
        </xsl:choose>

    </xsl:variable>

    <xsl:copy-of select="$output" />

</xsl:template>

<xsl:template name="newline-to-paragraph2">

    <xsl:param name="input" />

    <xsl:variable name="output">

        <xsl:choose>
            <xsl:when test="contains($input, '&#10;')">
		<xsl:element name="p"><xsl:attribute name="class">morelines</xsl:attribute><xsl:copy-of select="substring-before($input, '&#10;')" /></xsl:element>
                <xsl:call-template name="newline-to-paragraph2">
                    <xsl:with-param name="input">
                        <xsl:copy-of select="substring-after($input, '&#10;')" />
                    </xsl:with-param>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:if test="$input != ''">
                    <xsl:element name="p"><xsl:attribute name="class">morelines</xsl:attribute><xsl:copy-of select="$input" /></xsl:element>
                </xsl:if>
            </xsl:otherwise>
        </xsl:choose>

    </xsl:variable>

    <xsl:copy-of select="$output" />

</xsl:template>

<xsl:template name="CSS">
body,table,td,th {
	background-color:#FFF3D0;
	font-family:TeX Gyre Bonum, Verdana, Arial, Helvetica, sans-serif;
	font-size:10pt;
} 
.Source {
	font-style: italic;
	margin-top:10px;
}
.Name {
	color: #58170D;
	font-family:Andada SC, Verdana, Arial, Helvetica, sans-serif;
	font-weight: bold;
	font-size: 11pt;
}
.Header {
	font-weight: bold;
	margin-top:10px;
}
.Bold {
	font-weight: bold;
}
.Italic {
	font-style: italic;
}
.Features {
	margin-top:10px;
	}
.Feature {
	margin-top:6px;
}
.Flavour {
	padding-left:10px;
	padding-right:10px;
	font-style: italic;
	margin-bottom:10px;
}
table.Standalone {
	margin-top:10px;
}
p {
	margin:0px;
}
p.morelines {
	margin-top:3px;
}
.Properties {
	margin-top:10px;
}
.Names {
	margin-top:10px;
}	
img {
  display:block;
  width:90%;
  height:auto
}
</xsl:template>

  <xsl:template match="ImageData">
    <center style="margin-bottom:20px">
    <xsl:element name="img">
      <xsl:attribute name ="src">
        data:image/png;base64,<xsl:apply-templates/>
      </xsl:attribute>
    </xsl:element>
    </center>
  </xsl:template>

</xsl:stylesheet>

