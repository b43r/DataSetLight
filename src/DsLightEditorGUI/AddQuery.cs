/*
 * DsLight
 * 
 * Copyright (c) 2014..2018 by Simon Baer
 * 
 * This program is free software; you can redistribute it and/or modify it under the terms
 * of the GNU General Public License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with this program;
 * If not, see http://www.gnu.org/licenses/.
 *
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using deceed.DsLight.EditorGUI.Model;

namespace deceed.DsLight.EditorGUI
{
    /// <summary>
    /// Dialog for adding a new query to an entity or editing an existing query.
    /// </summary>
    internal partial class AddQuery : Form
    {
        private int step = 1;
        private int lastStep = 2;

        private string preselectedSP = null;
        private bool ignoreChangeEvent = false;
        private string connectionString;
        private List<Query> existingQueries;
        private Query selectedQuery;

        /// <summary>
        /// Create a new instance of the dialog for editing an existing query.
        /// </summary>
        /// <param name="selectedQuery">the query to edit</param>
        /// <param name="connectionString">the connection string</param>
        /// <param name="existingQueries">list of existing queries in the entity</param>
        public AddQuery(Query selectedQuery, string connectionString, List<Query> existingQueries) 
            : this(connectionString, existingQueries)
        {
            this.selectedQuery = selectedQuery;
            txtName.Text = selectedQuery.Name;
            if (selectedQuery.ExecuteMethod == ExecuteMethod.Reader)
            {
                rbReader.Checked = true;
            }
            switch (selectedQuery.ExecuteMethod)
            {
                case ExecuteMethod.Reader:
                    rbReader.Checked = true;
                    break;
                case ExecuteMethod.Scalar:
                    rbScalar.Checked = true;
                    break;
                default:
                    rbNonQuery.Checked = true;
                    break;
            }

            if (selectedQuery.CommandType == System.Data.CommandType.Text)
            {
                rbSQL.Checked = true;
                txtSQL.Text = selectedQuery.CommandText;
            }
            else
            {
                rbStoredProc.Checked = true;
                preselectedSP = selectedQuery.CommandText;
            }

            // if we are editing an existing query, start with step 2
            step = 2;
            InitializeStep();
            SetControls();
        }

        /// <summary>
        /// Create a new instance of the dialog for adding a new query.
        /// </summary>
        /// <param name="connectionString">the connection string</param>
        /// <param name="existingQueries">list of existing queries in the entity</param>
        public AddQuery(string connectionString, List<Query> existingQueries)
        {
            this.connectionString = connectionString;
            this.existingQueries = existingQueries;
            InitializeComponent();
        }

        /// <summary>
        /// Gets the query name.
        /// </summary>
        public string QueryName
        {
            get { return txtName.Text; }
        }

        /// <summary>
        /// Gets the execute method.
        /// </summary>
        public ExecuteMethod ExecuteMethod
        {
            get
            {
                if (rbReader.Checked)
                {
                    return ExecuteMethod.Reader;
                }
                else if (rbScalar.Checked)
                {
                    return ExecuteMethod.Scalar;
                }
                else
                {
                    return ExecuteMethod.NonQuery;
                }
            }
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public CommandType CommandType
        {
            get
            {
                if (rbSQL.Checked)
                {
                    return System.Data.CommandType.Text;
                }
                else
                {
                    return System.Data.CommandType.StoredProcedure;
                }
            }
        }

        /// <summary>
        /// Gets the command text.
        /// </summary>
        public string CommandText
        {
            get
            {
                if (rbSQL.Checked)
                {
                    return txtSQL.Text;
                }
                else
                {
                    return Convert.ToString(cboSPs.SelectedItem);
                }
            }
        }

        /// <summary>
        /// Advance to the next step of the wizard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (step == lastStep)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                return;
            }
            else
            {
                step++;
                InitializeStep();
                SetControls();

                if (rbSQL.Checked)
                {
                    txtName.Focus();
                }
                else
                {
                    cboSPs.Focus();
                }
            }
        }

        /// <summary>
        /// Set controls within current step.
        /// </summary>
        private void InitializeStep()
        {
            if (step == 2)
            {
                txtName.Focus();
                txtName.SelectAll();

                if (rbStoredProc.Checked)
                {
                    gbName.Top = 104;
                    try
                    {
                        DB.Analyzer analyzer = new DB.Analyzer(connectionString);
                        ignoreChangeEvent = true;
                        cboSPs.DataSource = analyzer.GetSPNames();
                        if (!String.IsNullOrEmpty(preselectedSP))
                        {
                            cboSPs.SelectedIndex = cboSPs.FindString(preselectedSP);
                        }
                        else
                        {
                            cboSPs.SelectedIndex = -1;
                        }
                        ignoreChangeEvent = false;
                    }
                    catch (Exception ex)
                    {
                        cboSPs.SelectedIndex = -1;
                        cboSPs.Enabled = false;
                        MessageBox.Show(this, "An error occured while fetching stored procedures:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    gbName.Top = 12;
                }
            }
        }

        /// <summary>
        /// Go back to the previous step of the wizard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            step--;
            SetControls();
        }

        /// <summary>
        /// Set control visibility depending on current wizard step.
        /// </summary>
        private void SetControls()
        {
            bool duplicateName = existingQueries.Any(x => x.Name == txtName.Text && x != selectedQuery);

            btnPrevious.Enabled = step > 1;
            btnNext.Enabled = (step < lastStep) || 
                ((step == lastStep) &&
                 !String.IsNullOrEmpty(txtName.Text) &&
                 !duplicateName &&
                 ((rbSQL.Checked && !String.IsNullOrEmpty(txtSQL.Text)) ||
                  (rbStoredProc.Checked && cboSPs.SelectedIndex != -1))
                );
            gbType.Visible = (step == 1);
            gbSource.Visible = (step == 1);
            gbName.Visible = (step == 2);
            gbSQL.Visible = (step == 2) && (rbSQL.Checked);
            gbSP.Visible = (step == 2) && (rbStoredProc.Checked);

            lblDuplicateName.Visible = duplicateName;

            btnNext.Text = step == lastStep ? "Finish" : "Next >";
        }

        /// <summary>
        /// Update controls after the name has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            SetControls();
        }

        /// <summary>
        /// Update controls after the SQL query has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSQL_TextChanged(object sender, EventArgs e)
        {
            SetControls();
        }
        
        /// <summary>
        /// Update controls after the selected stored-procedure has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboSPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreChangeEvent)
            {
                if (String.IsNullOrEmpty(txtName.Text))
                {
                    txtName.Text = cboSPs.SelectedItem.ToString();
                }
                SetControls();
            }
        }

        /// <summary>
        /// Only allow letters, digits and the underscore character.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.KeyChar) && (e.KeyChar != 95) && (e.KeyChar != 8))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handle special key shortcuts in SQL query editor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSQL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.A))
            {
                // Ctrl-A = select all
                txtSQL.SelectAll();
                e.SuppressKeyPress = true;
            }
        }
    }
}
