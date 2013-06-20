<?xml version="1.0" encoding="utf-8"?><!-- DWXMLSource="PressInformationSheet.xml" -->
<!DOCTYPE xsl:stylesheet  [
	<!ENTITY nbsp   "&#160;">
	<!ENTITY copy   "&#169;">
	<!ENTITY reg    "&#174;">
	<!ENTITY trade  "&#8482;">
	<!ENTITY mdash  "&#8212;">
	<!ENTITY ldquo  "&#8220;">
	<!ENTITY rdquo  "&#8221;"> 
	<!ENTITY pound  "&#163;">
	<!ENTITY yen    "&#165;">
	<!ENTITY euro   "&#8364;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html" encoding="utf-8" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"/>
<xsl:template match="/">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title>Press Information Sheet</title>
<link href="PressInformationSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
	<div id="headerText">
    	<xsl:value-of select="Document/DocumentName" />
    </div>
    <hr />
    <div align="center">
    <div id="wrap">
    	<table width="250" border="1">
        <tr>
        	<td colspan="2"><xsl:value-of select="Document/GangDetails/Description" /> :</td>
        </tr>
        <tr>
        	<th>Field Name</th>
            <th>Field Value</th>
        </tr>
    	<xsl:for-each select="Document/GangDetails/Detail">
        <tr>
        	<td><xsl:value-of select="Label" />:</td>
            <td><xsl:value-of select="Text" /></td>
        </tr>
    	</xsl:for-each>
        </table>
        <table width="250" border="1">
        <tr>
        	<td colspan="2"><xsl:value-of select="Document/SheetQty/Description" /> :</td>
        </tr>
        <tr>
        	<th>Field Name</th>
            <th>Field Value</th>
        </tr>
        <xsl:for-each select="Document/SheetQty/Detail">
        <tr>
        	<td><xsl:value-of select="Label" />:</td>
            <td><xsl:value-of select="Text" /></td>
        </tr>
    	</xsl:for-each>
        </table>
        <table width="250" border="1">
        <tr>
        	<td colspan="3"><xsl:value-of select="Document/JobDetails/Description" /> :</td>
        </tr>
        <tr>
        	<th>Job Number</th>
            <th>Job Name</th>
            <th>Job Up-size</th>
        </tr>
        <xsl:for-each select="Document/JobDetails/Job">
        <tr>
        	<td><xsl:value-of select="JobNumber" /></td>
            <td><xsl:value-of select="JobName" /></td>
            <td><xsl:value-of select="JobUp" /></td>
        </tr>
    	</xsl:for-each>
        </table>
    </div>
    </div>
</body>
</html>

</xsl:template>
</xsl:stylesheet>