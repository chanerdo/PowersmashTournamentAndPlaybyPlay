using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace PowersmashTournamentAndPlaybyPlay
{
    public partial class frmMain : Form
    {

        private MySqlConnection connDB = new MySqlConnection("datasource=unicsoftworks.com;port=3306;username=admin;password=powersmash123;");
        private DataTable tournament_data;
        private DataTable user_data;
        private DataTable reservation_data;
        private int player_single, player_double, player_mix, user1, user2;
        private int sp1, sp2, dp1, dp2, dp3, dp4, mp1, mp2, mp3, mp4;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            getTournamentData();
            getUserData();
            getReservationData();
            foreach (DataRow row in tournament_data.Rows)
            {
                cbxTournamentName.Items.Add(row.Field<string>(1));
            }
            dgvSinglePlayer.DataSource = user_data;
            dgvDoublePlayer.DataSource = user_data;
            dgvMixPlayer.DataSource = user_data;
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            cbxTournamentName.SelectedIndex = 0;
            cbxMatchType.SelectedIndex = 0;
            cbxSearchBySingleTour.SelectedIndex = 0;
            cbxSearchByDoubleTour.SelectedIndex = 0;
            cbxSearchByMixTour.SelectedIndex = 0;
            pbxSinglePlayer1_Click(this.pbxSinglePlayer1, e);
            pbxDoublePlayer1_Click(this.pbxDoublePlayer1, e);
            pbxMixPlayer1_Click(this.pbxMixPlayer1, e);
            for (int i = 0; i < dgvSinglePlayer.Columns.Count; i++)
            {
                dgvSinglePlayer.Columns[i].Visible = false;
                dgvMixPlayer.Columns[i].Visible = false;
                dgvDoublePlayer.Columns[i].Visible = false;
            }
            dgvSinglePlayer.Columns["id"].Visible = true;
            dgvSinglePlayer.Columns["team"].Visible = true;
            dgvSinglePlayer.Columns["email"].Visible = true;
            dgvSinglePlayer.Columns["firstname"].Visible = true;
            dgvSinglePlayer.Columns["lastname"].Visible = true;
            dgvSinglePlayer.Columns["gender"].Visible = true;
            dgvSinglePlayer.Columns["birthday"].Visible = true;
            dgvSinglePlayer.Columns["contact"].Visible = true;
            dgvSinglePlayer.Columns["address"].Visible = true;
            dgvDoublePlayer.Columns["id"].Visible = true;
            dgvDoublePlayer.Columns["team"].Visible = true;
            dgvDoublePlayer.Columns["email"].Visible = true;
            dgvDoublePlayer.Columns["firstname"].Visible = true;
            dgvDoublePlayer.Columns["lastname"].Visible = true;
            dgvDoublePlayer.Columns["gender"].Visible = true;
            dgvDoublePlayer.Columns["birthday"].Visible = true;
            dgvDoublePlayer.Columns["contact"].Visible = true;
            dgvDoublePlayer.Columns["address"].Visible = true;
            dgvMixPlayer.Columns["id"].Visible = true;
            dgvMixPlayer.Columns["team"].Visible = true;
            dgvMixPlayer.Columns["email"].Visible = true;
            dgvMixPlayer.Columns["firstname"].Visible = true;
            dgvMixPlayer.Columns["lastname"].Visible = true;
            dgvMixPlayer.Columns["gender"].Visible = true;
            dgvMixPlayer.Columns["birthday"].Visible = true;
            dgvMixPlayer.Columns["contact"].Visible = true;
            dgvMixPlayer.Columns["address"].Visible = true;
        }

        private void getTournamentData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.Tournament", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                tournament_data = new DataTable();
                sqlDataAdapter.Fill(tournament_data);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connDB.Close();
            }
        }

        private void getUserData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.user", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                user_data = new DataTable();
                sqlDataAdapter.Fill(user_data);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connDB.Close();
            }
        }

        private void getReservationData()
        {
            try
            {
                if (connDB.State == ConnectionState.Open) { connDB.Close(); }
                connDB.Open();
                MySqlCommand cmdDB = new MySqlCommand("SELECT * FROM powersmash.reservation", connDB);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(cmdDB);
                reservation_data = new DataTable();
                sqlDataAdapter.Fill(reservation_data);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connDB.Close();
            }
        }

        private void refreshDataGrid()
        {
            if(cbxMatchType.SelectedIndex == 0)
            {
                foreach (DataRow row in user_data.Rows)
                {
                    if (user1 != null || user2 != null)
                    {
                        if(row.Field<int>(0) == user1)
                        {
                            
                        }
                    }
                }
            }
        }

        private void cbxMatchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxMatchType.SelectedIndex == 0)
            {
                gbxSingle.Show();
                gbxDouble.Hide();
                gbxMix.Hide();
            }
            if (cbxMatchType.SelectedIndex == 1)
            {
                gbxDouble.Show();
                gbxSingle.Hide();
                gbxMix.Hide();
            }
            if (cbxMatchType.SelectedIndex == 2)
            {
                gbxMix.Show();
                gbxSingle.Hide();
                gbxDouble.Hide();
            }
        }

        private void dgvMixPlayer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dgvrow = dgvMixPlayer.Rows[e.RowIndex];
                int rowcount = 0;
                byte[] image = null;
                foreach (DataRow row in user_data.Rows)
                {
                    if (row.Field<int>(0) == int.Parse(dgvrow.Cells[0].Value.ToString()))
                    {
                        if (user_data.Rows[rowcount][42].ToString().Equals(""))
                        {
                            if (player_mix == 1) { pbxMixPlayer1.Image = Properties.Resources.profile; }
                            if (player_mix == 2) { pbxMixPlayer2.Image = Properties.Resources.profile; }
                            if (player_mix == 3) { pbxMixPlayer3.Image = Properties.Resources.profile; }
                            if (player_mix == 4) { pbxMixPlayer4.Image = Properties.Resources.profile; }
                        }
                        else
                        {
                            image = (byte[])(user_data.Rows[rowcount][42]);
                            if (player_mix == 1)
                            {
                                MemoryStream memstream = new MemoryStream(image);
                                pbxMixPlayer1.Image = Image.FromStream(memstream);
                                mp1 = row.Field<int>(0);
                            }
                            if (player_mix == 2)
                            {
                                MemoryStream memstream = new MemoryStream(image);
                                pbxMixPlayer2.Image = Image.FromStream(memstream);
                                mp2 = row.Field<int>(0);
                            }
                            if (player_mix == 3)
                            {
                                MemoryStream memstream = new MemoryStream(image);
                                pbxMixPlayer2.Image = Image.FromStream(memstream);
                                mp3 = row.Field<int>(0);
                            }
                            if (player_mix == 4)
                            {
                                MemoryStream memstream = new MemoryStream(image);
                                pbxMixPlayer4.Image = Image.FromStream(memstream);
                                mp4 = row.Field<int>(0);
                            }
                        }
                    }
                    rowcount++;
                }
            }
        }

        private void btnMixDone_Click(object sender, EventArgs e)
        {

        }

        private void pbxMixPlayer1_Click(object sender, EventArgs e)
        {
            pnlMixPlayer1.BackColor = Color.Black;
            pnlMixPlayer2.BackColor = Color.White;
            pnlMixPlayer3.BackColor = Color.White;
            pnlMixPlayer4.BackColor = Color.White;
            player_mix = 1;
        }

        private void pbxMixPlayer2_Click(object sender, EventArgs e)
        {
            pnlMixPlayer2.BackColor = Color.Black;
            pnlMixPlayer1.BackColor = Color.White;
            pnlMixPlayer3.BackColor = Color.White;
            pnlMixPlayer4.BackColor = Color.White;
            player_mix = 2;
        }

        private void pbxMixPlayer3_Click(object sender, EventArgs e)
        {
            pnlMixPlayer3.BackColor = Color.Black;
            pnlMixPlayer1.BackColor = Color.White;
            pnlMixPlayer2.BackColor = Color.White;
            pnlMixPlayer4.BackColor = Color.White;
            player_mix = 3;
        }

        private void pbxMixPlayer4_Click(object sender, EventArgs e)
        {
            pnlMixPlayer4.BackColor = Color.Black;
            pnlMixPlayer1.BackColor = Color.White;
            pnlMixPlayer2.BackColor = Color.White;
            pnlMixPlayer3.BackColor = Color.White;
            player_mix = 4;
        }

        private void dgvDoublePlayer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dgvrow = dgvDoublePlayer.Rows[e.RowIndex];
                int rowcount = 0;
                byte[] image = null;
                foreach (DataRow row in user_data.Rows)
                {
                    if (row.Field<int>(0) == int.Parse(dgvrow.Cells[0].Value.ToString()))
                    {
                        if (user_data.Rows[rowcount][42].ToString().Equals(""))
                        {
                            if (player_double == 1) { pbxDoublePlayer1.Image = Properties.Resources.profile; }
                            if (player_double == 2) { pbxDoublePlayer2.Image = Properties.Resources.profile; }
                            if (player_double == 3) { pbxDoublePlayer3.Image = Properties.Resources.profile; }
                            if (player_double == 4) { pbxDoublePlayer4.Image = Properties.Resources.profile; }
                        }
                        else
                        {
                            image = (byte[])(user_data.Rows[rowcount][42]);
                            if (player_double == 1)
                            {
                                MemoryStream memstream = new MemoryStream(image);
                                pbxDoublePlayer1.Image = Image.FromStream(memstream);
                                dp1 = row.Field<int>(0);
                            }
                            if (player_double == 2)
                            {
                                MemoryStream memstream = new MemoryStream(image);
                                pbxDoublePlayer2.Image = Image.FromStream(memstream);
                                dp2 = row.Field<int>(0);
                            }
                            if (player_double == 3)
                            {
                                MemoryStream memstream = new MemoryStream(image);
                                pbxDoublePlayer3.Image = Image.FromStream(memstream);
                                dp3 = row.Field<int>(0);
                            }
                            if (player_double == 4)
                            {
                                MemoryStream memstream = new MemoryStream(image);
                                pbxDoublePlayer4.Image = Image.FromStream(memstream);
                                dp4 = row.Field<int>(0);
                            }
                        }
                    }
                    rowcount++;
                }
            }
        }

        private void btnDoubleDone_Click(object sender, EventArgs e)
        {

        }

        private void pbxDoublePlayer1_Click(object sender, EventArgs e)
        {
            pnlDoublePlayer1.BackColor = Color.Black;
            pnlDoublePlayer2.BackColor = Color.White;
            pnlDoublePlayer3.BackColor = Color.White;
            pnlDoublePlayer4.BackColor = Color.White;
            player_double = 1;
        }

        private void pbxDoublePlayer2_Click(object sender, EventArgs e)
        {
            pnlDoublePlayer2.BackColor = Color.Black;
            pnlDoublePlayer1.BackColor = Color.White;
            pnlDoublePlayer3.BackColor = Color.White;
            pnlDoublePlayer4.BackColor = Color.White;
            player_double = 2;
        }

        private void pbxDoublePlayer3_Click(object sender, EventArgs e)
        {
            pnlDoublePlayer3.BackColor = Color.Black;
            pnlDoublePlayer1.BackColor = Color.White;
            pnlDoublePlayer2.BackColor = Color.White;
            pnlDoublePlayer4.BackColor = Color.White;
            player_double = 3;
        }

        private void pbxDoublePlayer4_Click(object sender, EventArgs e)
        {
            pnlDoublePlayer4.BackColor = Color.Black;
            pnlDoublePlayer1.BackColor = Color.White;
            pnlDoublePlayer2.BackColor = Color.White;
            pnlDoublePlayer3.BackColor = Color.White;
            player_double = 4;
        }

        private void pbxSinglePlayer1_Click(object sender, EventArgs e)
        {
            pnlSinglePlayer1.BackColor = Color.Black;
            pnlSinglePlayer2.BackColor = Color.White;
            player_single = 1;
        }

        private void pbxSinglePlayer2_Click(object sender, EventArgs e)
        {
            pnlSinglePlayer2.BackColor = Color.Black;
            pnlSinglePlayer1.BackColor = Color.White;
            player_single = 2;
        }

        private void dgvSinglePlayer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dgvrow = dgvSinglePlayer.Rows[e.RowIndex];
                int rowcount = 0;
                byte[] image = null;
                foreach (DataRow row in user_data.Rows)
                {
                    if (row.Field<int>(0) == int.Parse(dgvrow.Cells[0].Value.ToString()))
                    {
                        if(user_data.Rows[rowcount][42].ToString().Equals(""))
                        {
                            if (player_single == 1) { pbxSinglePlayer1.Image = Properties.Resources.profile; }
                            if (player_single == 2) { pbxSinglePlayer2.Image = Properties.Resources.profile; }
                        }
                        else
                        {
                            image = (byte[])(user_data.Rows[rowcount][42]);
                            if (player_single == 1)
                            {
                                MemoryStream memstream = new MemoryStream(image);
                                pbxSinglePlayer1.Image = Image.FromStream(memstream);
                                sp1 = row.Field<int>(0);
                            }
                            if (player_single == 2)
                            {
                                MemoryStream memstream = new MemoryStream(image);
                                pbxSinglePlayer2.Image = Image.FromStream(memstream);
                                sp2 = row.Field<int>(0);
                            }
                        }
                    }
                    rowcount++;
                }
            }
        }

        private void btnSingleDone_Click(object sender, EventArgs e)
        {

        }
    }
}
