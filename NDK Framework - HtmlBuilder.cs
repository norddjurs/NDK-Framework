using System;
using System.Collections.Generic;
using System.Text;

namespace NDK.Framework {

	#region HtmlBuilder class.
	/// <summary>
	/// Use this class to build simple HTML reports.
	/// </summary>
	public class HtmlBuilder {
		private StringBuilder html = new StringBuilder();

		#region Constructors.
		public HtmlBuilder() {
		} // HtmlBuilder
		#endregion

		#region Public methods.
		/// <summary>
		/// Appends a heading 1 to the HTML.
		/// </summary>
		/// <param name="title">The title.</param>
		public void AppendHeading1(String title) {
			if (title != null) {
				this.html.AppendFormat("<h1>{0}</h1>", title);
				this.html.AppendLine();
			}
		} // AppendHeading1

		  /// <summary>
		  /// Appends a heading 2 to the HTML.
		  /// </summary>
		  /// <param name="title">The title.</param>
		public void AppendHeading2(String title) {
			if (title != null) {
				this.html.AppendFormat("<h2>{0}</h2>", title);
				this.html.AppendLine();
			}
		} // AppendHeading2

		  /// <summary>
		  /// Appends a paragraph to the HTML.
		  /// </summary>
		  /// <param name="texts">The texts.</param>
		public void AppendParagraph(params String[] texts) {
			if (texts != null) {
				this.html.Append("<p>");
				for (Int32 textIndex = 0; textIndex < texts.Length; textIndex++) {
					this.html.Append(texts[textIndex].Replace(Environment.NewLine, "<br>"));
					if (textIndex < texts.Length - 1) {
						this.html.AppendLine("<br>");
					}
				}
				this.html.Append("</p>");
				this.html.AppendLine();
			}
		} // AppendParagraph

		/// <summary>
		/// Appends a paragraph to the HTML.
		/// </summary>
		/// <param name="texts">The bullit text</param>
		public void AppendBullits(params String[] texts) {
			if ((texts != null) && (texts.Length > 0)) {
				this.html.AppendLine("<ul>");
				foreach (String text in texts) {
					this.html.AppendFormat("<li>{0}</li>", text);
					this.html.AppendLine();
				}
				this.html.AppendLine("</ul>");
			}
		} // AppendBullits

		/// <summary>
		/// Appends a table to the HTML.
		/// The table is horizontal, and can be with a table header, table body and table footer.
		/// The table data are provided in a doule string list, the outher items are rows, and each row
		/// contains column strings. Be sure to add the same number of strings to each row.
		/// </summary>
		/// <param name="table">The table (string grid).</param>
		/// <param name="headCount">The number of rows that are headers.</param>
		/// <param name="footCount">The number of rows that are footers.</param>
		public void AppendHorizontalTable(List<List<String>> table, Int32 headCount, Int32 footCount) {
			try {
				if ((table != null) && (table.Count > 0)) {
					Int32 maxColumnCount = 0;
					String columnSpan = String.Empty;
					Int32 firstHeadRow = -1;
					Int32 lastHeadRow = -1;
					Int32 firstBodyRow = -1;
					Int32 lastBodyRow = -1;
					Int32 firstFootRow = -1;
					Int32 lastFootRow = -1;

					// Adjust the head count.
					if ((headCount < 0) || (headCount > table.Count)) {
						headCount = 0;
					} else {
						firstHeadRow = 0;
						lastHeadRow = headCount - 1;
					}

					// Adjust the foot count.
					if (footCount < 0) {
						footCount = 0;
					} else if ((footCount > headCount + table.Count)) {
						footCount = table.Count - headCount;
					}
					if (footCount > 0) {
						firstFootRow = table.Count - footCount;
						lastFootRow = table.Count - 1;
					}

					// Adjust the body count.
					if (table.Count - footCount > headCount) {
						firstBodyRow = headCount;
						lastBodyRow = table.Count - footCount - 1;
					}

					// Add table begin.
					this.html.AppendFormat("<table border=\"{0}\" cellpadding=\"{1}\" cellspacing=\"{2}\">", 0, 5, 0);
					this.html.AppendLine();

					for (Int32 rowIndex = 0; rowIndex < table.Count; rowIndex++) {
						// Add section begin (thead, tbody, tfoot).
						if (rowIndex == firstHeadRow) {
							// Add thead.
							this.html.AppendLine("<thead>");
						} else if (rowIndex == firstBodyRow) {
							// Add tbody.
							this.html.AppendLine("<tbody>");
						} else if (rowIndex == firstFootRow) {
							// Add tfoot.
							this.html.AppendLine("<tfoot>");
						}

						// Add table begin row (tr).
						this.html.AppendLine("<tr>");

						// Add columns.
						if (table[rowIndex] != null) {
							if (maxColumnCount < table[rowIndex].Count) {
								maxColumnCount = table[rowIndex].Count;
							}

							for (Int32 columnIndex = 0; columnIndex < table[rowIndex].Count; columnIndex++) {
								// Calculate column span.
								if ((columnIndex == table[rowIndex].Count - 1) && (columnIndex < maxColumnCount) && (maxColumnCount - columnIndex > 1)) {
									columnSpan = String.Format(" colspan=\"{0}\"", maxColumnCount - columnIndex);
								} else {
									columnSpan = String.Empty;
								}

								if (rowIndex < headCount) {
									// Add table header begin column (th).
									this.html.AppendFormat("<th{0}>", columnSpan);
									this.html.AppendLine();
								} else {
									// Add table begin column (td).
									this.html.AppendFormat("<td{0}>", columnSpan);
									this.html.AppendLine();
								}

								if (table[rowIndex][columnIndex] != null) {
									this.html.AppendLine(table[rowIndex][columnIndex]);
								} else {
									this.html.AppendLine("&nbsp;");
								}

								if (rowIndex < headCount) {
									// Add table header end column (th).
									this.html.AppendLine("</th>");
								} else {
									// Add table begin end (td).
									this.html.AppendLine("</td>");
								}
							}
						}

						// Add table end row (tr).
						this.html.AppendLine("</tr>");

						// Add section end (thead, tbody, tfoot).
						if (rowIndex == lastHeadRow) {
							// Add thead.
							this.html.AppendLine("</thead>");
						} else if (rowIndex == lastBodyRow) {
							// Add tbody.
							this.html.AppendLine("</tbody>");
						} else if (rowIndex == lastFootRow) {
							// Add tfoot.
							this.html.AppendLine("</tfoot>");
						}
					}

					// Add table end.
					this.html.AppendLine("</table>");
				}
			} catch {}
		} // AppendHorizontalTable

		/// <summary>
		/// Appends a table to the HTML.
		/// The table is vertical with two colums, the first colum is the table header.
		/// </summary>
		/// <param name="table">The table key values.</param>
		public void AppendVerticalTable(List<List<String>> table) {
			try {
				if ((table != null) && (table.Count > 0)) {
					// Add table begin.
					this.html.AppendFormat("<table border=\"{0}\" cellpadding=\"{1}\" cellspacing=\"{2}\">", 0, 5, 0);
					this.html.AppendLine();

					for (Int32 rowIndex = 0; rowIndex < table.Count; rowIndex++) {
						if (table[rowIndex].Count > 0) {
							this.html.AppendLine("<tr>");
							this.html.AppendLine("<th>");
							try {
								this.html.AppendLine(table[rowIndex][0]);
							} catch { }
							this.html.AppendLine("</th>");
							this.html.AppendLine("<td>");
							try {
								for (Int32 columnIndex = 1; columnIndex < table[rowIndex].Count; columnIndex++) {
									this.html.Append(table[rowIndex][columnIndex].Replace(Environment.NewLine, "<br>"));
									if (columnIndex < table[rowIndex].Count - 1) {
										this.html.Append("<br>");
									}
									this.html.AppendLine();
								}
							} catch { }
							this.html.AppendLine("</td>");
							this.html.AppendLine("</tr>");
						}
					}

					// Add table end.
					this.html.AppendLine("</table>");
				}
			} catch { }
		} // AppendVerticalTable

		public override string ToString() {
			// Create HTML document.
			StringBuilder html = new StringBuilder();
			html.AppendLine(@"<!DOCTYPE html>");
			html.AppendLine(@"<html>");
			html.AppendLine(@"	<head>");
			html.AppendLine(@"		<meta  content=""text /html; charset=UTF-8""  http-equiv=""content-type"">");
			html.AppendLine(@"		<title>Advis</title>");
			html.AppendLine(@"		<style>");
			html.AppendLine(@"			h2 {");
			html.AppendLine(@"				color: navy;");
			html.AppendLine(@"			} ");
			html.AppendLine(@"			thead, th {");
			html.AppendLine(@"				background-color: #72D2FF;");
			html.AppendLine(@"				text-align: left;");
			html.AppendLine(@"				vertical-align: top;");
			html.AppendLine(@"			}");
			html.AppendLine(@"			tbody {");
			html.AppendLine(@"				text-align: left;");
			html.AppendLine(@"				vertical-align: top;");
			html.AppendLine(@"			}");
			html.AppendLine(@"			tfoot {");
			html.AppendLine(@"				background-color: #EBF2FA;");
			html.AppendLine(@"				text-align: left;");
			html.AppendLine(@"				vertical-align: top;");
			html.AppendLine(@"			}");
			html.AppendLine(@"		</style>");
			html.AppendLine(@"	</head>");
			html.AppendLine(@"	<body>");
			html.Append(this.html);
			html.AppendLine(@"	</body> ");
			html.AppendLine(@"</html> ");

			// Return the HTML.
			return html.ToString();
		} // ToString
		#endregion

	} // HtmlBuilder
	#endregion

} // NDK.Framework