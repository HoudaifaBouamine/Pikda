using DevExpress.Data.Helpers;
using DevExpress.Utils.Layout;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Pikda.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pikda
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        List<AreaViewDto> AreaViewDtos = new List<AreaViewDto>();
        public MainForm()
        {
            InitializeComponent();
            InitializeAreasView();
            InitializeCardsTable();
        }

        void InitializeCardsTable()
        {
            CardsTable.Items.Add("wow");
            CardsTable.Items.Add("wow 2");
            var ctx = CardsTable.ContextMenu = new ContextMenu();
            ctx.MenuItems.Add("Add New Card", CardsTable_AddNewCard);

            void CardsTable_AddNewCard(object sender, EventArgs e)
            {
                var result = XtraInputBox.Show("Card Name", "Adding New Card",$"Card-{Guid.NewGuid().ToString().Substring(0,4)}");
                if(string.IsNullOrEmpty(result))
                {
                    MessageBox.Show("Can not create card without name", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (ListViewItem cardItem in CardsTable.Items)
                    if (cardItem.Text == result)
                    {
                        MessageBox.Show($"Card with the name {result} already exist !", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                CardsTable.Items.Add(result);
            }
        }
        void InitializeAreasView()
        {
            AreasViewGrid.DataSource = AreaViewDtos;
            AreaViewDtos.Add(new AreaViewDto
            {
                Prop = "FirstName",
                Value = "Value1"
            });
            AreaViewDtos.Add(new AreaViewDto
            {
                Prop = "LastName",
                Value = "Value2"
            });

            GridView view = AreasViewGrid.MainView as GridView;

            var propCol = view.Columns["Prop"];
            var valCol = view.Columns["Value"];

            var propsLookUp = new RepositoryItemLookUpEdit();
            propsLookUp.DataSource = new string[] { "FirstName", "LastName", "BirthDay" };
            AreasViewGrid.RepositoryItems.Add(propsLookUp);
            propCol.ColumnEdit = propsLookUp;
        }

    }
}
