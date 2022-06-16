<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
	<xsl:output method="xml" indent="yes"/>
	<xsl:template match="/">
		<Products>
			<xsl:for-each select="Root/products">
				<Product>
					<Name>
						<xsl:value-of select="title" />
					</Name>
					<Categories>
						<xsl:for-each select="categories">
							<xsl:value-of select="."/>
							<xsl:if test="position() != last()">
								<xsl:text>, </xsl:text>
							</xsl:if>
						</xsl:for-each>
					</Categories>
					<Twitter>
						<xsl:choose>
							<xsl:when test="not(twitter)">
								<xsl:value-of select="twitter"/>
							</xsl:when>
							<xsl:when test="contains(twitter,'@')">
								<xsl:value-of select="twitter"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat('@',twitter)" />
							</xsl:otherwise>
						</xsl:choose>
					</Twitter>
				</Product>
			</xsl:for-each>
		</Products>
	</xsl:template>
</xsl:stylesheet> 