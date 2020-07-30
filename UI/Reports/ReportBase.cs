using System.Collections.Generic;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

using UI.Properties;

namespace Reports {
  public class ReportBase {
    protected Document document;
    protected Section section;
    protected Table table;

    public ReportBase() {      
      ReportSettings(PageFormat.A4, Orientation.Portrait);
    }

    public ReportBase(PageFormat pageFormat) {
      ReportSettings(pageFormat, Orientation.Portrait);
    }

    public ReportBase(Orientation orientation) {
      ReportSettings(PageFormat.A4, orientation);
    }

    public ReportBase(string title) {
      ReportSettings(PageFormat.A4, Orientation.Portrait, title);
    }

    public ReportBase(PageFormat pageFormat, Orientation orientation, string title = null) {
      ReportSettings(pageFormat, orientation, title);
    }

    public virtual void DefineStyles() {
      // Get the predefined style Normal.
      Style style = document.Styles["Normal"];

      /*
       * Because all styles are derived from Normal, the next line changes the 
       * font of the whole document. Or, more exactly, it changes the font of
       * all styles and paragraphs that do not redefine the font.
       */
      style.Font.Name = "Verdana";
      style.Font.Size = 10;

      // Create a new style called Table based on style Normal
      style = document.Styles.AddStyle("Table", "Normal");
      style.Font.Name = "Lucida Console";
      style.Font.Size = 9;
    }

    public virtual void CreatePage() {
      // Each MigraDoc document needs at least one section.
      this.section = AddSection(document.DefaultPageSetup.Orientation);

      //  Create header
      List<string> header = new List<string>() { document.Info.Title };
      Header(section, header);

      // Create footer
      Footer(section);
    }

    public virtual void CreatePage(Orientation orientation) {
      this.section = AddSection(orientation);
    }

    public virtual void Header(Section section, List<string> text, string image = null) {
      Style style = document.Styles[StyleNames.Header];
      style.ParagraphFormat.Font.Name = "Verdana";
      style.ParagraphFormat.Font.Size = 10;

      for (int i = 0; i < text.Count; i++) {
        Paragraph paragraph = section.Headers.Primary.AddParagraph();
        paragraph.AddFormattedText(text[i], TextFormat.Bold);
        paragraph.Format.Alignment = ParagraphAlignment.Center;
      }
    }

    public virtual void Footer(Section section, string text = null) {
      DefaultFooter(section);

      Row row = table.AddRow();
      row.Height = "0.25 in";
      row.VerticalAlignment = VerticalAlignment.Center;
      row.Borders.Top.Visible = true;

      Paragraph paragraph = row.Cells[0].AddParagraph();
      paragraph.Format.SpaceBefore = "0.01 in";
      paragraph.AddText(Resources.AppName);

      paragraph = row.Cells[1].AddParagraph();
      paragraph.Format.SpaceBefore = "0.01 in";
      if (!string.IsNullOrWhiteSpace(text)) {
        paragraph.AddText(text);
      }

      paragraph = row.Cells[2].AddParagraph();
      paragraph.Format.SpaceBefore = "0.01 in";
      AddPageNumber(paragraph);
    }

    public Section AddSection(Orientation orientation) {
      this.section = document.AddSection();
      section.PageSetup.Orientation = orientation;
      
      return this.section;
    }

    public Table AddTable(Section section) {
      this.table = section.AddTable();
      table.Style = "Table";

      return this.table;
    }

    protected void ReportSettings(PageFormat pageFormat, Orientation orientation, string title = null) {
      this.document = new Document();
      document.Info.Title = title;
      document.Info.Author = Resources.AppName;

      document.DefaultPageSetup.PageFormat = pageFormat;
      document.DefaultPageSetup.Orientation = orientation;
      document.DefaultPageSetup.LeftMargin = "0.625 in";
      document.DefaultPageSetup.RightMargin = "0.5 in";
      document.DefaultPageSetup.TopMargin = "1.25 in";
      document.DefaultPageSetup.BottomMargin = "1 in";
    }

    protected void DefaultFooter(Section section) {
      Unit[,] colSize = new Unit[2, 3] { { "4.5 cm", "9.1 cm", "4.5 cm" }, 
                                         { "6.7 cm", "13.4 cm", "6.7 cm" } };
      int index = (int)section.PageSetup.Orientation;

      this.table = section.Footers.Primary.AddTable();

      Column column = table.AddColumn(colSize[index, 0]);
      column.Format.Alignment = ParagraphAlignment.Left;

      column = table.AddColumn(colSize[index, 1]);
      column.Format.Alignment = ParagraphAlignment.Center;

      column = table.AddColumn(colSize[index, 2]);
      column.Format.Alignment = ParagraphAlignment.Right;
    }

    protected void AddPageNumber(Paragraph paragraph) {
      paragraph.AddText(Resources.Page);
      paragraph.AddPageField();
      paragraph.AddText("/");
      paragraph.AddNumPagesField();
    }
  }
}
