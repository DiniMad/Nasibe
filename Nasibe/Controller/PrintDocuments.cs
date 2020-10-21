using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Globalization;
using System.IO;

namespace Nasibe.Controller
{
    class PrintDocuments
    {

        public static void AssignmentDocument(String[][] data, int gap, int documentStatus)
        {
            var systemDateTime = DateTime.Now;
            var persianDateTimem = new PersianCalendar();
            var persianDate = $"{persianDateTimem.GetYear(systemDateTime)}/{persianDateTimem.GetMonth(systemDateTime)}" +
                                 $"/{persianDateTimem.GetDayOfMonth(systemDateTime)}";
            Document doc = new Document();
            {
                var mainSection = doc.AddSection();
                mainSection.PageSetup.PageSize = PageSize.A5;
                var mainTitle = mainSection.AddParagraph();
                mainTitle.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                mainTitle.Text = "هیئت امنا بانو نسیبه";
                var style = new ParagraphStyle(doc) { Name = "MailTitle", CharacterFormat = { FontName = "Arial", FontSize = 30, Bold = true } };
                doc.Styles.Add(style);
                mainTitle.ApplyStyle(style.Name);
            }
            {
                var documentNumber = doc.LastSection.AddParagraph();
                documentNumber.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                if (documentStatus == 1)
                {
                    documentNumber.Text = $"{Properties.Settings.Default.Assignment} : شماره";
                    Properties.Settings.Default.Assignment++;
                }
                else if (documentStatus == 2)
                {
                    documentNumber.Text = $"{Properties.Settings.Default.All} : شماره";
                    Properties.Settings.Default.All++;
                }
                else if (documentStatus == 3)
                {
                    documentNumber.Text = $"{Properties.Settings.Default.ByPeople} : شماره";
                    Properties.Settings.Default.ByPeople++;
                }
                else if (documentStatus == 4)
                {
                    documentNumber.Text = $"{Properties.Settings.Default.Bought} : شماره";
                    Properties.Settings.Default.Bought++;
                }
                else if (documentStatus == 5)
                {
                    documentNumber.Text = $"{Properties.Settings.Default.SelectedItems} : شماره";
                    Properties.Settings.Default.SelectedItems++;
                }
                Properties.Settings.Default.Save();
                var style = new ParagraphStyle(doc) { Name = "DocumentNumber", CharacterFormat = { FontName = "Arial", FontSize = 10 } };
                doc.Styles.Add(style);
                documentNumber.ApplyStyle(style.Name);
            }
            {
                var date = doc.LastSection.AddParagraph();
                date.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                date.Text = $"{persianDate} : تاریخ";
                var style = new ParagraphStyle(doc) { Name = "Date", CharacterFormat = { FontName = "Arial", FontSize = 10 } };
                doc.Styles.Add(style);
                date.ApplyStyle(style.Name);
            }
            {
                var documentTitle = doc.LastSection.AddParagraph();
                documentTitle.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                if (documentStatus == 1)
                {
                    documentTitle.Text = "حواله انبار";
                }
                else if (documentStatus == 2)
                {
                    documentTitle.Text = "لیست همه اموال";
                }
                else if (documentStatus == 3)
                {
                    documentTitle.Text = "لیست اموال مردمی";
                }
                else if (documentStatus == 4)
                {
                    documentTitle.Text = "لیست اموال خریداری شده";
                }
                else if (documentStatus == 5)
                {
                    documentTitle.Text = "لیست اموال انتخاب شده";
                }
                var style = new ParagraphStyle(doc) { Name = "DocumentTitle", CharacterFormat = { FontName = "Arial", FontSize = 22, Bold = true } };
                doc.Styles.Add(style);
                documentTitle.ApplyStyle(style.Name);
            }
            {
                if (documentStatus == 1)
                {
                    var descriptionParagraph1 = doc.LastSection.AddParagraph();
                    descriptionParagraph1.Format.HorizontalAlignment = HorizontalAlignment.Right;
                    descriptionParagraph1.Text = "انبار                        اجناس مشروحه زیر به برادر";
                    var style = new ParagraphStyle(doc) { Name = "descriptionParagraph1", CharacterFormat = { FontName = "Arial", FontSize = 14 } };
                    doc.Styles.Add(style);
                    descriptionParagraph1.ApplyStyle(style.Name);
                }

            }
            {
                if (documentStatus == 1)
                {
                    var descriptionParagraph2 = doc.LastSection.AddParagraph();
                    descriptionParagraph2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                    string txt = ".نمانده                              تحویل و ذیلا رسید دریافت دارید";
                    descriptionParagraph2.Text = txt;
                    var style = new ParagraphStyle(doc)
                    {
                        Name = "descriptionParagraph2",
                        CharacterFormat = { FontName = "Arial", FontSize = 14 }
                    };
                    doc.Styles.Add(style);
                    descriptionParagraph2.ApplyStyle(style.Name);
                }
            }
            Section s = doc.LastSection;
            String[] Header = { "بها )ريال(", "نرخ )ريال(", "مقدار", "شرح", "شماره کالا" };
            if (documentStatus == 3)
            {
                Header = new[] { "مقدار", "شرح", "شماره کالا" };
            }
            Table table = s.AddTable(true);
            table.ResetCells(data.Length + 1 - gap, Header.Length);
            TableRow FRow = table.Rows[0];
            FRow.IsHeader = true;
            FRow.Height = 20;
            for (int i = 0; i < Header.Length; i++)
            {
                //Cell Alignment
                var p = FRow.Cells[i].AddParagraph();
                FRow.Cells[i].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
                if (i == 0 || i == 2)
                    FRow.Cells[i].Width = 80;
                if (i == 3)
                {
                    FRow.Cells[i].Width = 110;
                }

                p.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                //Fill Header data and Format the Cells
                var TR = p.AppendText(Header[i]);
                TR.CharacterFormat.FontName = "Arial";
                TR.CharacterFormat.FontSize = 10;
                TR.CharacterFormat.TextColor = System.Drawing.Color.Black;
                TR.CharacterFormat.Bold = true;
            }
            for (int r = 0; r < data.Length - gap; r++)
            {

                var dataRow = table.Rows[r + 1];
                //Row Height
                dataRow.Height = 15;
                //C Represents Column.
                for (int c = 0; c < data[r].Length; c++)
                {
                    //Cell Alignment
                    dataRow.Cells[c].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
                    //Fill Data in Rows
                    var p2 = dataRow.Cells[c].AddParagraph();
                    p2.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                    p2.Text = data[r][c];
                }
            }

            if (documentStatus == 1)
            {
                var subPragraph1 = doc.LastSection.AddParagraph();
                subPragraph1.Text = "اشیاء مشروحه بالا به اینجانب\t\t\tنمانده";
                subPragraph1.Format.HorizontalAlignment = HorizontalAlignment.Right;
                var subPragraph2 = doc.LastSection.AddParagraph();
                subPragraph2.Text = ".اشیاء مشروحه بالا تحویل و در کارت مربوط ثبت گردید";
                subPragraph2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                var subPragraph3 = doc.LastSection.AddParagraph();
                subPragraph3.Text = Environment.NewLine + "انبار دار هیئت امناء\t\t\tامضاء تحویل گیرنده";
                subPragraph3.Format.HorizontalAlignment = HorizontalAlignment.Center;
                var subTextStyle = new ParagraphStyle(doc)
                {
                    Name = "SubText",
                    CharacterFormat = { FontName = "Arial", FontSize = 14 }
                };
                var subTextStyle2 = new ParagraphStyle(doc)
                {
                    Name = "SubText2",
                    CharacterFormat = { FontName = "Arial", FontSize = 10 }
                };
                doc.Styles.Add(subTextStyle);
                doc.Styles.Add(subTextStyle2);
                subPragraph1.ApplyStyle(subTextStyle.Name);
                subPragraph2.ApplyStyle(subTextStyle.Name);
                subPragraph3.ApplyStyle(subTextStyle2.Name);
            }

            if (!Directory.Exists("Documents"))
                Directory.CreateDirectory("Documents");
            var documentPathName = "";
            if (documentStatus == 1)
            {
                documentPathName = $"Documents\\Assignment({ persianDateTimem.GetYear(systemDateTime)}-" +
                                          $"{ persianDateTimem.GetMonth(systemDateTime)}" + $"-" +
                                          $"{persianDateTimem.GetDayOfMonth(systemDateTime)})(" +
                                          $"{persianDateTimem.GetHour(systemDateTime)}-" +
                                          $"{persianDateTimem.GetMinute(systemDateTime)}-" +
                                          $"{persianDateTimem.GetSecond(systemDateTime)}).docx";
            }
            else if (documentStatus == 2)
            {
                documentPathName = $"Documents\\All({ persianDateTimem.GetYear(systemDateTime)}-" +
                                   $"{ persianDateTimem.GetMonth(systemDateTime)}" + $"-" +
                                   $"{persianDateTimem.GetDayOfMonth(systemDateTime)})(" +
                                   $"{persianDateTimem.GetHour(systemDateTime)}-" +
                                   $"{persianDateTimem.GetMinute(systemDateTime)}-" +
                                   $"{persianDateTimem.GetSecond(systemDateTime)}).docx";
            }
            else if (documentStatus == 3)
            {
                documentPathName = $"Documents\\ByPeople({ persianDateTimem.GetYear(systemDateTime)}-" +
                                   $"{ persianDateTimem.GetMonth(systemDateTime)}" + $"-" +
                                   $"{persianDateTimem.GetDayOfMonth(systemDateTime)})(" +
                                   $"{persianDateTimem.GetHour(systemDateTime)}-" +
                                   $"{persianDateTimem.GetMinute(systemDateTime)}-" +
                                   $"{persianDateTimem.GetSecond(systemDateTime)}).docx";
            }
            else if (documentStatus == 4)
            {
                documentPathName = $"Documents\\Bought({ persianDateTimem.GetYear(systemDateTime)}-" +
                                   $"{ persianDateTimem.GetMonth(systemDateTime)}" + $"-" +
                                   $"{persianDateTimem.GetDayOfMonth(systemDateTime)})(" +
                                   $"{persianDateTimem.GetHour(systemDateTime)}-" +
                                   $"{persianDateTimem.GetMinute(systemDateTime)}-" +
                                   $"{persianDateTimem.GetSecond(systemDateTime)}).docx";
            }
            else if (documentStatus == 5)
            {
                documentPathName = $"Documents\\Selected({ persianDateTimem.GetYear(systemDateTime)}-" +
                                   $"{ persianDateTimem.GetMonth(systemDateTime)}" + $"-" +
                                   $"{persianDateTimem.GetDayOfMonth(systemDateTime)})(" +
                                   $"{persianDateTimem.GetHour(systemDateTime)}-" +
                                   $"{persianDateTimem.GetMinute(systemDateTime)}-" +
                                   $"{persianDateTimem.GetSecond(systemDateTime)}).docx";
            }

            doc.SaveToFile(documentPathName);
            System.Diagnostics.Process.Start(documentPathName);
        }
        public static void RequestDocument(String[][] data, int gap)
        {
            var systemDateTime = DateTime.Now;
            var persianDateTimem = new PersianCalendar();
            var persianDate = $"{persianDateTimem.GetYear(systemDateTime)}/{persianDateTimem.GetMonth(systemDateTime)}" +
                              $"/{persianDateTimem.GetDayOfMonth(systemDateTime)}";
            Document doc = new Document();
            {
                var mainSection = doc.AddSection();
                mainSection.PageSetup.PageSize = PageSize.A5;
                var mainTitle = mainSection.AddParagraph();
                mainTitle.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                mainTitle.Text = "هیئت امنا بانو نسیبه";
                var style = new ParagraphStyle(doc) { Name = "MailTitle", CharacterFormat = { FontName = "Arial", FontSize = 30, Bold = true } };
                doc.Styles.Add(style);
                mainTitle.ApplyStyle(style.Name);
            }
            {
                var documentNumber = doc.LastSection.AddParagraph();
                documentNumber.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                documentNumber.Text = $"{Properties.Settings.Default.Request} : شماره";
                Properties.Settings.Default.Request++;
                Properties.Settings.Default.Save();
                var style = new ParagraphStyle(doc) { Name = "DocumentNumber", CharacterFormat = { FontName = "Arial", FontSize = 10 } };
                doc.Styles.Add(style);
                documentNumber.ApplyStyle(style.Name);
            }
            {
                var date = doc.LastSection.AddParagraph();
                date.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                date.Text = $"{persianDate} : تاریخ";
                var style = new ParagraphStyle(doc) { Name = "Date", CharacterFormat = { FontName = "Arial", FontSize = 10 } };
                doc.Styles.Add(style);
                date.ApplyStyle(style.Name);
            }
            {
                var documentTitle = doc.LastSection.AddParagraph();
                documentTitle.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                documentTitle.Text = "برگ درخواست خرید";
                var style = new ParagraphStyle(doc) { Name = "DocumentTitle", CharacterFormat = { FontName = "Arial", FontSize = 22, Bold = true } };
                doc.Styles.Add(style);
                documentTitle.ApplyStyle(style.Name);
            }
            {
                var descriptionParagraph1 = doc.LastSection.AddParagraph();
                descriptionParagraph1.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                descriptionParagraph1.Text = "خواهشمند است لوازم مشروحه ذیل را که مورد نیاز قسمت";
                var style = new ParagraphStyle(doc) { Name = "descriptionParagraph1", CharacterFormat = { FontName = "Arial", FontSize = 14 } };
                doc.Styles.Add(style);
                descriptionParagraph1.ApplyStyle(style.Name);
            }
            {
                var descriptionParagraph2 = doc.LastSection.AddParagraph();
                descriptionParagraph2.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                string txt = ".می باشد تهیه و تحویل نماید";
                descriptionParagraph2.Text = txt;
                var style = new ParagraphStyle(doc) { Name = "descriptionParagraph2", CharacterFormat = { FontName = "Arial", FontSize = 14 } };
                doc.Styles.Add(style);
                descriptionParagraph2.ApplyStyle(style.Name);

            }
            Spire.Doc.Section s = doc.LastSection;
            String[] Header = { "ملاحضات", "شرح", "تعداد", "شماره کالا" };
            Table table = s.AddTable(true);
            table.ResetCells(data.Length + 1 - gap, Header.Length);
            var FRow = table.Rows[0];
            FRow.IsHeader = true;
            FRow.Height = 20;
            for (int i = 0; i < Header.Length; i++)
            {
                //Cell Alignment
                var p = FRow.Cells[i].AddParagraph();
                FRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                if (i == 0 || i == 1)
                    FRow.Cells[i].Width = 120;
                p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                //Fill Header data and Format the Cells
                var TR = p.AppendText(Header[i]);
                TR.CharacterFormat.FontName = "Arial";
                TR.CharacterFormat.FontSize = 10;
                TR.CharacterFormat.TextColor = System.Drawing.Color.Black;
                TR.CharacterFormat.Bold = true;
            }
            for (int r = 0; r < data.Length - gap; r++)
            {

                var dataRow = table.Rows[r + 1];
                //Row Height
                dataRow.Height = 15;
                //C Represents Column.
                for (int c = 0; c < data[r].Length; c++)
                {
                    //Cell Alignment
                    dataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    //Fill Data in Rows
                    var p2 = dataRow.Cells[c].AddParagraph();
                    p2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                    p2.Text = data[r][c];
                }
            }
            var subPragraph1 = doc.LastSection.AddParagraph();
            subPragraph1.Text = Environment.NewLine + "درخواست کننده\t\t";
            subPragraph1.AppendText("کارپردازی\t\t");
            subPragraph1.AppendText("رئیس حسابداری");
            subPragraph1.Format.HorizontalAlignment = HorizontalAlignment.Center;
            var subTextStyle = new ParagraphStyle(doc) { Name = "SubText", CharacterFormat = { FontName = "Arial", FontSize = 14 } };
            doc.Styles.Add(subTextStyle);
            subPragraph1.ApplyStyle(subTextStyle.Name);
            if (!Directory.Exists($"Documents"))
                Directory.CreateDirectory($"Documents");
            var documentPathName = $"Documents\\Request({ persianDateTimem.GetYear(systemDateTime)}-" +
                                   $"{ persianDateTimem.GetMonth(systemDateTime)}" + $"-" +
                                   $"{persianDateTimem.GetDayOfMonth(systemDateTime)})(" +
                                   $"{persianDateTimem.GetHour(systemDateTime)}-" +
                                   $"{persianDateTimem.GetMinute(systemDateTime)}-" +
                                   $"{persianDateTimem.GetSecond(systemDateTime)}).docx";
            doc.SaveToFile(documentPathName);
            System.Diagnostics.Process.Start(documentPathName);
        }
        public static void ReceiptDocument(String[][] data, int gap)
        {
            var systemDateTime = DateTime.Now;
            var persianDateTimem = new PersianCalendar();
            var persianDate = $"{persianDateTimem.GetYear(systemDateTime)}/{persianDateTimem.GetMonth(systemDateTime)}" +
                              $"/{persianDateTimem.GetDayOfMonth(systemDateTime)}";
            Document doc = new Document();
            {
                var mainSection = doc.AddSection();
                mainSection.PageSetup.PageSize = PageSize.A5;
                mainSection.PageSetup.Orientation = PageOrientation.Landscape;
                var mainTitle = mainSection.AddParagraph();
                mainTitle.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                mainTitle.Text = "موسسه خریه نسیبه مرکز نگهداری معلولین و سالمندان";
                var style = new ParagraphStyle(doc) { Name = "MailTitle", CharacterFormat = { FontName = "Arial", FontSize = 25, Bold = true } };
                doc.Styles.Add(style);
                mainTitle.ApplyStyle(style.Name);
            }
            //{
            //    var documentTitelImage = doc.LastSection.AddParagraph();


            //    //Insert Image
            //    Image image = Image.FromFile(@"..\..\Images\InformationIcon.png");
            //    DocPicture picture = documentTitelImage.AppendPicture(image);

            //    //Set Image
            //    picture.Height = 40;
            //    picture.Width = 400;
            //}
            {
                var documentNumber = doc.LastSection.AddParagraph();
                documentNumber.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                documentNumber.Text = "تحت نظارت سازمان بهزیستی استان مرکزی-تفرش";
                Properties.Settings.Default.Request++;
                Properties.Settings.Default.Save();
                var style = new ParagraphStyle(doc) { Name = "SecendTitel", CharacterFormat = { FontName = "Arial", FontSize = 20, Bold = true } };
                doc.Styles.Add(style);
                documentNumber.ApplyStyle(style.Name);
            }
            {
                var documentNumber = doc.LastSection.AddParagraph();
                documentNumber.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                documentNumber.Text = "\"رسید مشارک مردمی\"";
                Properties.Settings.Default.Request++;
                Properties.Settings.Default.Save();
                var style = new ParagraphStyle(doc) { Name = "ThirdTitel", CharacterFormat = { FontName = "Arial", FontSize = 20, Bold = true } };
                doc.Styles.Add(style);
                documentNumber.ApplyStyle(style.Name);
            }
            {
                var date = doc.LastSection.AddParagraph();
                date.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                date.Text = $"{persianDate} : تاریخ";
                var style = new ParagraphStyle(doc) { Name = "Date", CharacterFormat = { FontName = "Arial", FontSize = 10 } };
                doc.Styles.Add(style);
                date.ApplyStyle(style.Name);
            }
            {
                var descriptionParagraph1 = doc.LastSection.AddParagraph();
                descriptionParagraph1.Format.HorizontalAlignment = HorizontalAlignment.Right;
                descriptionParagraph1.Text = "درصورت چک طی";
                descriptionParagraph1.AppendText("  ");
                descriptionParagraph1.AppendField("Servic", FieldType.FieldFormCheckBox);
                descriptionParagraph1.AppendText("خدماتی");
                descriptionParagraph1.AppendText("  ");
                descriptionParagraph1.AppendField("NonCash", FieldType.FieldFormCheckBox);
                descriptionParagraph1.AppendText("غیر نقدی");
                descriptionParagraph1.AppendText("  ");
                descriptionParagraph1.AppendField("Cash", FieldType.FieldFormCheckBox);
                descriptionParagraph1.AppendText($"مبلغ به عدد                    به حروف                 ریال به صورت نقدی");
                //   var style = new ParagraphStyle(doc) { Name = "descriptionParagraph1", CharacterFormat = { FontName = "Arial", FontSize = 14 } };
                //   doc.Styles.Add(style);
                //   descriptionParagraph1.ApplyStyle(style.Name);
            }
            {
                var descriptionParagraph2 = doc.LastSection.AddParagraph();
                descriptionParagraph2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                descriptionParagraph2.AppendField("privite", FieldType.FieldFormCheckBox);
                descriptionParagraph2.AppendText("خاص با شرح");
                descriptionParagraph2.AppendText("  ");
                descriptionParagraph2.AppendField("public", FieldType.FieldFormCheckBox);
                descriptionParagraph2.AppendText(" چند شماره                تاریخ                عهده جاری                نزد بانک                با نیت عام");

                //   var style = new ParagraphStyle(doc) { Name = "descriptionParagraph2", CharacterFormat = { FontName = "Arial", FontSize = 14 } };
                //   doc.Styles.Add(style);
                //   descriptionParagraph2.ApplyStyle(style.Name);

            }
            Section s = doc.LastSection;
            String[] Header = { "شماره رسیده وارده به انبار", "جمع کل ریال", "بهای واحد ريال", "مقدار", "واحد جنس", "درصورت غیر نقدی و خدماتی شرح کالا", "ردیف" };
            Table table = s.AddTable(true);
            table.ResetCells(data.Length + 1 - gap, Header.Length);
            var FRow = table.Rows[0];
            FRow.IsHeader = true;
            FRow.Height = 20;
            for (int i = 0; i < Header.Length; i++)
            {
                //Cell Alignment
                var p = FRow.Cells[i].AddParagraph();
                FRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                if (i == 0 || i == 1)
                    FRow.Cells[i].Width = 120;
                p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                //Fill Header data and Format the Cells
                var TR = p.AppendText(Header[i]);
                TR.CharacterFormat.FontName = "Arial";
                TR.CharacterFormat.FontSize = 10;
                TR.CharacterFormat.TextColor = System.Drawing.Color.Black;
                TR.CharacterFormat.Bold = true;
            }
            for (int r = 0; r < data.Length - gap; r++)
            {

                var dataRow = table.Rows[r + 1];
                //Row Height
                dataRow.Height = 15;
                //C Represents Column.
                for (int c = 0; c < data[r].Length; c++)
                {
                    //Cell Alignment
                    dataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    //Fill Data in Rows
                    var p2 = dataRow.Cells[c].AddParagraph();
                    p2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                    p2.Text = data[r][c];
                }
            }
            {
                var descriptionParagraph3 = doc.LastSection.AddParagraph();
                descriptionParagraph3.AppendField("Center", FieldType.FieldFormCheckBox);
                descriptionParagraph3.AppendText("مرکز");
                descriptionParagraph3.AppendText("\t");
                descriptionParagraph3.AppendField("Mojtama", FieldType.FieldFormCheckBox);
                descriptionParagraph3.AppendText("دارای کد شناسایی                 جهت کمک به مجتمع");
                descriptionParagraph3.AppendText("\t\t\t");
                descriptionParagraph3.AppendText("نیکوکار");
                descriptionParagraph3.AppendText("\t");
                descriptionParagraph3.AppendField("Men", FieldType.FieldFormCheckBox);
                descriptionParagraph3.AppendText("برادر");
                descriptionParagraph3.AppendText("\t");
                descriptionParagraph3.AppendField("Women", FieldType.FieldFormCheckBox);
                descriptionParagraph3.AppendText("توسط خواهر");
                descriptionParagraph3.Format.HorizontalAlignment = HorizontalAlignment.Right;
                //   var style = new ParagraphStyle(doc) { Name = "descriptionParagraph1", CharacterFormat = { FontName = "Arial", FontSize = 14 } };
                //   doc.Styles.Add(style);
                //   descriptionParagraph1.ApplyStyle(style.Name);
            }
            {
                var descriptionParagraph4 = doc.LastSection.AddParagraph();
                descriptionParagraph4.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                descriptionParagraph4.Text = " .از خداوند سبحان خیر دنیا و آخرت را برای شما آرزومندیم.هدیه گردید که بدینوسیله وصول آن را اعلام می گردد";
                //   var style = new ParagraphStyle(doc) { Name = "descriptionParagraph1", CharacterFormat = { FontName = "Arial", FontSize = 14 } };
                //   doc.Styles.Add(style);
                //   descriptionParagraph1.ApplyStyle(style.Name);
            }
            {
                var descriptionParagraph5 = doc.LastSection.AddParagraph();
                descriptionParagraph5.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Left;
                descriptionParagraph5.Text = "\t\t\t: نام ونام خانوادگی رئیس مرکز/مجتمع";
                var style = new ParagraphStyle(doc) { Name = "descriptionParagraph5", CharacterFormat = { FontSize = 15 } };
                doc.Styles.Add(style);
                descriptionParagraph5.ApplyStyle(style.Name);
            }
            {
                var descriptionParagraph6 = doc.LastSection.AddParagraph();
                descriptionParagraph6.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Left;
                descriptionParagraph6.Text = "\t\t\t:امضا";
                var style = new ParagraphStyle(doc) { Name = "descriptionParagraph6", CharacterFormat = { FontSize = 15 } };
                doc.Styles.Add(style);
                descriptionParagraph6.ApplyStyle(style.Name);
            }
            {
                var descriptionParagraph7 = doc.LastSection.AddParagraph();
                descriptionParagraph7.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Left;
                descriptionParagraph7.Text = "\t\t\t:مهر";
                var style = new ParagraphStyle(doc) { Name = "descriptionParagraph7", CharacterFormat = { FontSize = 15 } };
                doc.Styles.Add(style);
                descriptionParagraph7.ApplyStyle(style.Name);
            }
            if (!Directory.Exists($"Documents"))
                Directory.CreateDirectory($"Documents");
            var documentPathName = $"Documents\\Receipt({ persianDateTimem.GetYear(systemDateTime)}-" +
                                   $"{ persianDateTimem.GetMonth(systemDateTime)}" + $"-" +
                                   $"{persianDateTimem.GetDayOfMonth(systemDateTime)})(" +
                                   $"{persianDateTimem.GetHour(systemDateTime)}-" +
                                   $"{persianDateTimem.GetMinute(systemDateTime)}-" +
                                   $"{persianDateTimem.GetSecond(systemDateTime)}).docx";
            doc.SaveToFile(documentPathName);
            System.Diagnostics.Process.Start(documentPathName);
        }
    }
}
